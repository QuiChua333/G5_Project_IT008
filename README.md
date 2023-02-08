# Quản lí rạp chiếu phim
<img src="./ImageApp/FatFilmFoo.png" width="800"/>

## Mô tả 

Đại dịch covid đã hoành hành suốt gần ba năm khiến cho cuộc sống con người trở nên vô cùng khó khăn. Giờ đây là giai đoạn phục hồi kinh tế, con người bước vào cuộc sống mới với sự bận rộn kèm theo đó áp lực công việc cũng ngày càng lớn. Chúng kéo theo nhu cầu giải trí của con người cũng ngày càng tăng lên, trong đó xem phim là hình thức được nhiều người lựa chọn. Điều này là một tiềm năng phát triển rất lớn và được nhiều công ty khai thác. Do vậy mà các rạp chiếu phim cũng hoạt động trở lại và năng suất hơn bao giờ hết.

Một vấn đề lớn được đặt ra là việc quản lý các rạp phim sao cho hiệu quả, chính xác, tránh được rủi ro không nên có. Việc quản lý một rạp phim từ mặt hàng, nhân viên, cho tới lợi nhuận đã không thể thực hiện bằng tay. Quá nhiều sai số làm các chủ rạp phim gặp rất nhiều khó khăn. 
Hiểu được điều này, nhóm 5 quyết định xây dựng một ứng dụng hỗ trợ các rạp chiếu phim trong việc quản lý, tận dụng những công nghệ tiên tiến để phát triển, mục tiêu hướng đến chính là nâng cao trải nghiệm người dùng về cả giao diện lẫn tốc độ xử lý, kèm theo đó là những tính năng mở rộng phù hợp với thực tiễn.



### Mục đích ứng dụng

#### Hiện trạng và yêu cầu thực tế
* Mặc dù trên thị trường phần mềm đã xuất hiện nhiều phần mềm quản lý, nhưng nhận thấy chúng đều phức tạp, cầu kì, không thân thiện với người dùng. 

#### Mục đích 

*	Tìm hiểu được ngôn ngữ C#, các công nghệ để hoàn thành được đồ án.
*	Tạo ra được ứng dụng có giao diện đẹp, thân thiện với người sử dụng.
*	Ứng dụng chạy ổn định trong thời gian dài, các chức năng thực hiện nhanh chóng.

#### Yêu cầu
* Đáp ứng những tính năng tiêu chuẩn cần có trên những ứng dụng quản lý rạp chiếu phim hiện có trên thị trường. Mở rộng và phát triển những tính năng mới hỗ trợ tối đa cho người dùng, khắc phục những hạn chế và yếu kém của hệ thống quản lý hiện nay.
* Báo cáo, thống kê, thêm, cập nhật dữ liệu, phải diễn ra nhanh chóng, chính xác.
* Dễ dàng tra cứu, tìm kiếm các thông tin.
* Dễ dàng cập nhật và lựa chọn lên lịch chiếu phim phải phù hợp, chính xác hạn chế thấp nhất sai sót để nâng cao chất lượng phục vụ của rạp.
* Giao diện thân thiện, dễ sử dụng, bố cục hợp lý, hài hoà về màu sắc và mang tính đồng bộ cao, phân quyền cho người dùng thông qua tài khoản.
* Ứng dụng phải tương thích với đa số các hệ điều hành phổ biến hiện nay như Window Vista SP1, Window 8.1, Window 10, ...Đặc biệt, ứng dụng trong quá trình sử dụng phải hoạt động ổn định, tránh những trường hợp xảy ra lỗi xung đột với hệ thống gây ra khó chịu cho người dùng trong quá trình sử dụng, tệ hơn là ảnh hưởng trực tiếp đến khách hàng của rạp phim. Việc mở rộng, nâng cấp ứng dụng về sau phải dễ dàng khi người dùng có nhu cầu.

#### Người dùng 

* Chủ rạp phim: vai trò quản lí
* Nhân viên
### Công nghệ 

* Hệ thống API: WPF - Mô hình MVVM
* IDE: Visual Studio 2022 (C#/.Net)
* Database: SQL Server, Excel Sheet
* Công cụ quản lý: Git, GitHub
* Công cụ thiết kế: Figma



## Tác giả 

* [Lê Quang Nhân](https://www.facebook.com/profile.php?id=100040989546712) - 21522402

* [Nguyễn Văn Phát](https://www.facebook.com/profile.php?id=100009796787588) - 21522448

* [Huỳnh Ngọc Quí](https://www.facebook.com/quichua333) - 21520417

* [Phan Trọng Tính](https://www.facebook.com/profile.php?id=100073316952962) - 21522683

 * Sinh viên khoa Công nghệ Phần mềm, trường Đại học Công nghệ Thông tin, Đại học Quốc gia thành phố Hồ Chí Minh 

## Giảng viên hướng dẫn 

* Thầy Nguyễn Tấn Toàn, giảng viên Khoa Công Nghệ Phần Mềm, trường Đại học Công nghệ Thông tin, Đại học Quốc gia Thành phố Hồ Chí Minh 

## Hướng dẫn cài đặt và kết nối cơ sở dữ liệu

* Cài đặt [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/).

* Cài đặt [SQL Sever 2022](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

* Download [CinemaManagementDatabase.sql](https://drive.google.com/file/d/13NLEl_wsCyYTGFiyjYWBwTJ-r2a8jC7K/view?usp=sharing)

* Download phền mềm tại Github: https://github.com/QuiChua333/G5_Project_IT008.git

* Cài đặt SQL Server và copy tên máy chủ
<img src="./ImageApp/ConnectString.jpg" width="400"/>

* Mở file CinemaManagementProjectg.sln và kết nối phần mềm với Database vừa tạo bằng cách sửa chuỗi Connection String (phần bôi đen) trong file App.config
<img src="./ImageApp/ChangeString.jpg"/>

* Khởi tạo Database bằng cách query script chứa trong file CinemaManagementDatabase.sql

## Phản hồi 

Tạo phản hồi ở mục Issues, mỗi phản hồi của bạn sẽ giúp chúng tôi cải thiện ứng dụng tốt hơn. Cảm ơn vì sự giúp đỡ! 
