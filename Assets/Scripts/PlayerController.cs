﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private float thrust, minTiltSmooth, maxTiltSmooth, hoverDistance, hoverSpeed, jumpPower, jumpDuration, fallDuration;
	[SerializeField] private AnimationCurve jumpCurve;
	[SerializeField] private AnimationCurve fallCurve;
	private static readonly int GrayscaleAmount = Shader.PropertyToID("_GrayscaleAmount");
	private bool start;
	private float timer, tiltSmooth, y;
	private Rigidbody2D playerRigid;
	private Quaternion downRotation, upRotation;
	private Material playerMaterial;
	private Sequence jumpSequence;

	void Start () {
		tiltSmooth = maxTiltSmooth;
		playerRigid = GetComponent<Rigidbody2D> ();
		downRotation = Quaternion.Euler (0, 0, -90);
		upRotation = Quaternion.Euler (0, 0, 35);
		playerMaterial = spriteRenderer.material;
	}

	void Update () {
		if (!start) {
			// Hover the player before starting the game
			timer += Time.deltaTime;
			y = hoverDistance * Mathf.Sin (hoverSpeed * timer);
			transform.localPosition = new Vector3 (0, y, 0);
		} else {
			// Rotate downward while falling
			transform.rotation = Quaternion.Lerp (transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
		}
		// Limit the rotation that can occur to the player
		transform.rotation = new Quaternion (transform.rotation.x, transform.rotation.y, Mathf.Clamp (transform.rotation.z, downRotation.z, upRotation.z), transform.rotation.w);
	}

	void LateUpdate () {
		if (GameManager.Instance.GameState ()) {
			if (Input.GetMouseButtonDown (0)) {
				if(!start){
					// This code checks the first tap. After first tap the tutorial image is removed and game starts
					start = true;
					GameManager.Instance.GetReady ();
					GetComponent<Animator>().speed = 2;
				}
				playerRigid.gravityScale = 1f;
				tiltSmooth = minTiltSmooth;
				transform.rotation = upRotation;
				playerRigid.velocity = Vector2.zero;
				// Push the player upwards
//				playerRigid.AddForce (Vector2.up * thrust);
				jumpSequence?.Kill();
				jumpSequence = DOTween.Sequence();
				jumpSequence.Append(transform.DOLocalMoveY(transform.localPosition.y + jumpPower, jumpDuration).SetEase(jumpCurve));
				jumpSequence.Append(transform.DOLocalMoveY(-0.9f, fallDuration).SetEase(fallCurve));
				SoundManager.Instance.PlayTheAudio("Flap");
			}
		}
		if (playerRigid.velocity.y < -1f) {
			// Increase gravity so that downward motion is faster than upward motion
			tiltSmooth = maxTiltSmooth;
			playerRigid.gravityScale = 2f;
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.transform.CompareTag ("Score")) {
			Destroy (col.gameObject);
			GameManager.Instance.UpdateScore ();
		} else if (col.transform.CompareTag ("Obstacle")) {
			// Destroy the Obstacles after they reach a certain area on the screen
			foreach (Transform child in col.transform.parent.transform) {
				child.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			}
			KillPlayer ();
		}
		
		if (col.transform.CompareTag ("Ground")) {
			playerRigid.simulated = false;
			KillPlayer ();
			transform.rotation = downRotation;
			// Grayscale player
			GrayScalePlayer();
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.transform.CompareTag ("Ground")) {
			playerRigid.simulated = false;
			KillPlayer ();
			transform.rotation = downRotation;
			// Grayscale player
			GrayScalePlayer();
		}
	}

	private void GrayScalePlayer()
	{
		float grayScaleAmount = 0, duration = 0.3f;
		DOTween.To(() => grayScaleAmount, x => grayScaleAmount = x, 1, duration)
			.OnUpdate(() => {
				playerMaterial.SetFloat(GrayscaleAmount, grayScaleAmount);
			});
	}

	public void KillPlayer () {
		GameManager.Instance.EndGame ();
		playerRigid.velocity = Vector2.zero;
		// Stop the flapping animation
		GetComponent<Animator> ().enabled = false;
	}

}