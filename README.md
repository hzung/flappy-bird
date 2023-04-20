> Sau khi làm bài xong, đánh giá ưu nhược điểm của thiết kế, tổ chức project. Đề
xuất cách tổ chức project của bạn. Đánh giá, đề xuất và các ghi chú bổ sung khác nếu có xin gửi kèm trong thư mục project dạng file Markdown (.md)

# Đánh giá ưu điểm
* Cách tổ chức các folder level 1: Animation, Fonts, Materials, ... vừa đủ cho tính chất của dự án
* Project kéo về, tải đúng version Unity là chạy được luôn. Rất thuận tiện để ứng viên setup làm bài test.
* Source code trong sáng, dễ hiểu, comment tương đối đầy đủ.
* Code chạy mượt, độ hoàn thiện cao.

# Một số điểm có thể cải thiện
* Sử dụng cơ chế Object Pooling ở ObstacleSpawner để đảm bảo tối ưu performance
* Đối với các function rỗng ví dụ như MainMenu.RateGame thì hoặc là thêm TODO hoặc là xóa
* Các string đang sử dụng so sánh hay làm tham số trong game trong game nên để ra một class constants riêng, hoặc dồn khai báo hết lên đầu của class.
* Nên tách riêng các AnimatorController bên trong thư mục Animation ra một folder đặt tên là Animators. Thư mục Animation chỉ chứa các animations. Đổi tên thư mục Animation thành Animations
* Tên các Asset không nên chứa dấu cách. Ví dụ: "Animation/Game Over.controller", "Sprites/Flappy Bird.png"
* Các Asset thừa không sử dụng trong game thì nên xóa. Hiện tại trong thư mục Fonts và Sprites đều có các asset thừa
* Setup size của Texture bên trong Sprites vừa đủ với kích thước sử dụng. Không nên để mặc định 2048.
* Đối với các thành phần UI ở bên trong canvas, khi muốn thay đổi size, không nên thay đổi scale. Ví dụ nút rate: Canvas/Rate. Để đảm bảo performance cũng như đảm bảo thuận lợi cho các thao tác với UI về sau nếu có. Ví dụ sử dụng grid layout group hoặc horizontal/vertical layout group.

# Flappy-Bird-Unity

Flappy Bird is a simple Game made for Android and IOS. This Unity project hopes to create a clone of this game.

># Disclaimer
>
>This project was made purely for learning purposes. All the Sprites and Audio Clips used have been downloaded from the Internet. I do not own any of them.

# Screenshots

![Main Menu](https://i.imgur.com/UzddAbX.png "Main Menu")


|Game Screen Day|Game Screen Night|
|--|--|
|![Game Screen 1](https://i.imgur.com/aLIkuU0.png "Game Screen")  | ![Game Screen 2](https://i.imgur.com/mB4aE3r.png "Game Screen") |

![Main Game](https://i.imgur.com/dW9ZMhc.png "Game")

|Game Over without Medal|Game Over with Medal|
|--|--|
|![Game Over Screen](https://i.imgur.com/rS2X3Rg.png "Game Over")  | ![Game Over Screen](https://i.imgur.com/h4Q9imU.png "Game Over") |
