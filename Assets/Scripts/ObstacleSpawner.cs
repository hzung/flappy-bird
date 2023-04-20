using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

	[SerializeField] private string pipePattern;
	[SerializeField] private float waitTime;
	[SerializeField] private GameObject[] obstaclePrefabs;
	private Dictionary<string, GameObject> obstaclePrefabsDict;
	private Dictionary<string, string> easyObstaclePairs;
	private Dictionary<string, string> hardObstaclePairs;
	private float tempTime;
	private string[] pipeLevels;
	private int pipeLevelIndex = 0;
	private string spawnedPrefabName = string.Empty;
	private GameObject spawnedPrefab;
	private string pipeLevel;
	
	private const string Easy = "E";
	private const string Hard = "H";

	void Start(){
		tempTime = waitTime - Time.deltaTime;
		pipeLevels = pipePattern.Split('-');
		
		// Cache the obstacle prefab to a dict for faster access
		obstaclePrefabsDict = new Dictionary<string, GameObject>();
		foreach (var obstaclePrefab in obstaclePrefabs)
		{
			var prefabName = obstaclePrefab.name;
			if (!obstaclePrefabsDict.ContainsKey(prefabName))
			{
				obstaclePrefabsDict.Add(prefabName, obstaclePrefab);
			}
		}
		
		// Setup easy pair pipes
		easyObstaclePairs = new Dictionary<string, string>()
		{
			{"ObstacleContainer", "ObstacleContainer2"},
			{"ObstacleContainer2", "ObstacleContainer"},
			{"ObstacleContainer3", "ObstacleContainer4"},
			{"ObstacleContainer4", "ObstacleContainer3"},
		};
		
		// Setup hard pair pipes
		hardObstaclePairs = new Dictionary<string, string>()
		{
			{"ObstacleContainer", "ObstacleContainer4"},
			{"ObstacleContainer2", "ObstacleContainer4"},
			{"ObstacleContainer3", "ObstacleContainer"},
			{"ObstacleContainer4", "ObstacleContainer"},
		};
	}

	void LateUpdate () {
		if(GameManager.Instance.GameState()){
			tempTime += Time.deltaTime;
			if(tempTime > waitTime){
				// Wait for some time, create an obstacle, then set wait time to 0 and start again
				tempTime = 0;
				
				// First time, spawn a random prefab
				if (string.IsNullOrEmpty(spawnedPrefabName))
				{
					spawnedPrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
				}
				else
				{
					// Second time, spawn a prefab base on the pattern level
					switch (pipeLevel)
					{
						case Easy:
							spawnedPrefab = obstaclePrefabsDict[easyObstaclePairs[spawnedPrefabName]];
							break;
						case Hard:
							spawnedPrefab = obstaclePrefabsDict[hardObstaclePairs[spawnedPrefabName]];
							break;
					}
				}
				spawnedPrefabName = spawnedPrefab.name;
				Instantiate(spawnedPrefab, transform.position, transform.rotation);
				
				// Get the next spawn level
				pipeLevel = pipeLevels[pipeLevelIndex];
				pipeLevelIndex += 1;
				pipeLevelIndex %= pipeLevels.Length;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.transform.parent != null){
			Destroy(col.gameObject.transform.parent.gameObject);
		}else{
			Destroy(col.gameObject);
		}
	}
}
