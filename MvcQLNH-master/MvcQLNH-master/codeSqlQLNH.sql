-- Xóa cơ sở dữ liệu nếu đã tồn tại
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'SqlMvcQLNH')
BEGIN
    DROP DATABASE SqlMvcQLNH;
END
GO

-- Tạo cơ sở dữ liệu mới
CREATE DATABASE SqlMvcQLNH;
GO

USE SqlMvcQLNH;
GO

-- Bảng tbAccount: Lưu thông tin tài khoản
CREATE TABLE tbAccount (
    AccountID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    AccountType NVARCHAR(20) NOT NULL
);

-- Bảng tbUserInfo: Lưu thông tin nhân viên
CREATE TABLE tbUserInfo (
    UserInfoID INT IDENTITY(1,1) PRIMARY KEY,
    AccountID INT,
    FullName NVARCHAR(100) NOT NULL,
    BirthDay DATE NOT NULL,
    Sex INT NOT NULL,
    Email NVARCHAR(100),
    PhoneNumber NVARCHAR(20),
    FOREIGN KEY (AccountID) REFERENCES tbAccount(AccountID) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Bảng tbDMFood: Lưu danh mục thức ăn
CREATE TABLE tbDMFood (
    DMFoodID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL
);

-- Bảng tbFood: Lưu thông tin món ăn
CREATE TABLE tbFood (
    FoodID INT IDENTITY(1,1) PRIMARY KEY,
    FoodName NVARCHAR(100) NOT NULL,
    DMFoodID INT,
    Price INT NOT NULL,
    FOREIGN KEY (DMFoodID) REFERENCES tbDMFood(DMFoodID) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Bảng tbDSTable: Lưu thông tin bàn
CREATE TABLE tbDSTable (
    TableID INT IDENTITY(1,1) PRIMARY KEY,
    TableName NVARCHAR(50) NOT NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Trống', 'Có người'))
);

-- Bảng tbBillHistory: Lưu thông tin hóa đơn đã thanh toán
CREATE TABLE tbBillHistory (
    BillID INT IDENTITY(1,1) PRIMARY KEY,
    TableID INT,
    UserInfoID INT,
    TableName NVARCHAR(50),
    BillDate DATE NOT NULL,
    TotalAmount INT NOT NULL,
    FOREIGN KEY (TableID) REFERENCES tbDSTable(TableID) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (UserInfoID) REFERENCES tbUserInfo(UserInfoID) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Bảng tbBillDetails: Lưu chi tiết hóa đơn (số lượng món ăn và tổng giá tiền)
CREATE TABLE tbBillDetails (
    BillDetailID INT IDENTITY(1,1) PRIMARY KEY,
    BillID INT,
    FoodID INT,
    Quantity INT NOT NULL,
    TotalPrice DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (BillID) REFERENCES tbBillHistory(BillID) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (FoodID) REFERENCES tbFood(FoodID) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Bảng tbRevenu: Lưu thông tin doanh thu
CREATE TABLE tbRevenu (
    RevenuID INT IDENTITY(1,1) PRIMARY KEY,
    BillID INT,
    RevenuDate DATE NOT NULL,
    RevenuAmount INT NOT NULL,
    FOREIGN KEY (BillID) REFERENCES tbBillHistory(BillID) ON DELETE CASCADE ON UPDATE CASCADE
);


DROP TABLE tbDSTable ;
ALTER TABLE tbDSTable
DROP CONSTRAINT CHECK_Status;

ALTER TABLE tbDSTable
DROP COLUMN Status;

--Xóa ràng buộc của trường status trong bảng dstable
ALTER TABLE tbDSTable
DROP CONSTRAINT CK__tbDSTable__Statu__4222D4EF;




INSERT INTO tbAccount (Username, Password, AccountType) VALUES
('LAT', '123', '1'),
('admin', '1', '0'),
('NVT', '123', '1'),
('NST', '123', '1'),
('HTP', '123', '1');


INSERT INTO tbUserInfo (AccountID, FullName, BirthDay, Sex, Email, PhoneNumber) VALUES
(1, 'Lê Anh Tú', '2003-10-16', 1, 'lat@gmail.com', '19001610'),
(2, 'Quản Trị Viên', '2024-06-06', 0, 'admin@gmail.com', '19001000'),
(3, 'Nguyễn Văn Trường', '2003-01-01', 1, 'nvt@gmail.com', '12341234'),
(4, 'Nguyễn Sơn Tùng', '2003-06-01', 1, 'nst@gmail.com', '12345678'),
(5, 'Huỳnh Thịnh Phát', '2003-05-28', 1, 'htp@gmail.com', '09050506');


INSERT INTO tbDSTable (TableName, Status) VALUES
('Bàn 1', 'Trống'),
('Bàn 2', 'Trống'),
('Bàn 3', 'Trống'),
('Bàn 4', 'Trống'),
('Bàn 5', 'Trống'),
('Bàn 6', 'Trống'),
('Bàn 7', 'Có người'),
('Bàn 8', 'Trống'),
('Bàn 9', 'Có người'),
('Bàn 10', 'Trống'),
('Bàn 11', 'Trống'),
('Bàn 12', 'Trống'),
('Bàn 13', 'Có người'),
('Bàn 14', 'Trống'),
('Bàn 15', 'Trống'),
('Bàn 16', 'Có người'),
('Bàn 17', 'Trống'),
('Bàn 18', 'Trống'),
('Bàn 19', 'Trống'),
('Bàn 20', 'Trống');


INSERT INTO tbDMFood (CategoryName) VALUES
('Đồ Uống'),
('Rau'),
('Đồ Khai Vị'),
('Lẩu'),
('Hải Sản'),
('Món Tươi Sống'),
('Súp');

INSERT INTO tbFood (FoodName, DMFoodID, Price) VALUES
('Heniken', 1, 20000),
('Tiger', 1, 18000),
('Bia Quy Nhơn', 1, 7000),
('Bia Sài Gòn', 1, 15000),
('Cocacola', 1, 18000),
('PepSi', 1, 18000),
('7Up', 1, 17000),
('Nước lọc', 1, 6000),
('Rau Muống', 2, 2000),
('Rau Mùng Tơi', 2, 2000),
('Rau Cải Thảo', 2, 3000),
('Hoa chuối', 2, 3000),
('Rau Cần', 2, 2000),
('Phồng Tôm', 3, 6000),
('Đậu Phộng', 3, 6000),
('Khoai Tây Chiên', 3, 10000),
('Salat mắm cải', 3, 12000),
('Salat cổ hủ dừa', 3, 15000),
('Lẩu Thái', 4, 150000),
('Lẩu Tứ Xuyên', 4, 170000),
('Lẩu Kim Chi', 4, 150000),
('Lẩu Nấm', 4, 150000),
('Lẩu Riêu Cua', 4, 160000),
('Lẩu Không cay', 4, 130000),
('Lẩu Măng Chua', 4, 150000),
('Cá tuyết đút lò kiểu Thái', 5, 99000),
('Bóng cá hầm hải sâm', 5, 99000),
('Bóng cá chiên tôm', 5, 99000),
('Mỳ xào hải sản', 5, 50000),
('Miến xào hải sản', 5, 50000),
('Măng tây xào sò điệp', 5, 40000),
('Cơm chiên hải sản', 5, 35000),
('Bào ngư nướng', 5, 199000),
('Hàu hướng mỡ hành', 5, 35000),
('Cá Mú hấp xã', 5, 69000),
('Tôm chiên xù', 5, 49000),
('Cá Bớp xào', 5, 49000),
('Ba chỉ bò Mỹ', 6, 99000),
('Ba chỉ bò cuộn nấm kim châm', 6, 99000),
('Cổ bò Mỹ', 6, 99000),
('Bắp bò ta', 6, 99000),
('Lưỡi bò', 6, 35000),
('Tim lợn', 6, 35000),
('Sườn sụn lợn', 6, 35000),
('Thăn lợn', 6, 99000),
('Gà đùi', 6, 49000),
('Cánh gà', 6, 49000),
('Bạch tuộc', 6, 79000),
('Tôm viên', 6, 79000),
('Há cảo', 6, 45000),
('Thanh cua', 6, 49000),
('Mực ống', 6, 49000),
('Súp cua gà xé', 7, 49000),
('Súp măng tây', 7, 49000),
('Súp thập cẩm', 7, 49000),
('Súp hải sâm đông cô', 7, 49000),
('Súp tổ yến', 7, 49000),
('Súp sò điệp', 7, 49000),
('Súp bào ngư vi cá', 7, 99000),
('Súp cua rong biển', 7, 69000);