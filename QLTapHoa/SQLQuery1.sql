use master
if exists (select * from sysdatabases where name = 'CNPM')
	drop database CNPM
go
create database CNPM
go
use CNPM
go
--tạo bảng
CREATE TABLE Admin
(
  idAdmin VARCHAR(10) NOT NULL,-- Mã admin
  username NVARCHAR(50) NOT NULL, -- Họ tên 
  phone CHAR(10) NOT NULL, -- SĐT 
  gender NVARCHAR(5) NOT NULL, -- Giới tính 
  email NVARCHAR(30) NOT NULL, -- Email
  PRIMARY KEY (idAdmin)
);

CREATE TABLE Provider
(
  idProvider VARCHAR(10) NOT NULL,-- Mã nhà cung cấp 
  nameProvider NVARCHAR(30) NOT NULL, -- tên nhà cung cấp
  PRIMARY KEY (idProvider),
);

CREATE TABLE Product
(
  idProduct VARCHAR(10) NOT NULL,-- Mã sản phẩm
  nameProduct NVARCHAR(30) NOT NULL, -- Tên sản phẩm
  category NVARCHAR(20) NOT NULL, -- Loại sản phẩm 
  price INT NOT NULL, -- Giá từng sản phẩm 
  quantity INT NOT NULL, -- Số lượng hiện có 
  imageProduct VARCHAR(50), -- Hình ảnh sản phẩm
  status NVARCHAR(30) NOT NULL, -- Tình trạng sử dụng 
  inventory NVARCHAR(30), -- Kho ?? 
  idProvider VARCHAR(10) NOT NULL,
  PRIMARY KEY (idProduct),
  FOREIGN KEY (idProvider) REFERENCES Provider(idProvider)
);

CREATE TABLE Orders
(
  idOrder VARCHAR(10) NOT NULL,-- Mã đơn hàng
  idProduct VARCHAR(10) NOT NULL, -- mã sản phẩm
  date DATE NOT NULL, --ngày lập đơn
  quantity INT NOT NULL, -- số lượng 
  status NVARCHAR(30) NOT NULL, -- tình trạng đơn hàng
  addressDelivery NVARCHAR(50) NOT NULL, -- địa chỉ giao hàng
  price INT NOT NULL,  -- tổng giá tiền
  PRIMARY KEY (idOrder, idProduct, date),
  FOREIGN KEY (idProduct) REFERENCES Product(idProduct)
);

CREATE TABLE Invoice
(
  idInvoice VARCHAR(10) NOT NULL,-- Mã hóa đơn
  idOrder VARCHAR(10) NOT NULL,-- Mã đơn hàng
  date DATE NOT NULL, -- ngày lập hóa đơn
  status NVARCHAR(30) NOT NULL, --tình trạng hóa đơn
  price INT NOT NULL,  -- tổng giá tiền,
  PRIMARY KEY (idInvoice, idOrder)
);

CREATE TABLE ExportTicket
(
  idExport VARCHAR(10) NOT NULL,-- Mã phiếu xuất hàng
  idProduct VARCHAR(10) NOT NULL, -- mã sản phâm
  quantity int,
  date DATE NOT NULL,  -- ngày lập phiếu xuất hàng
  addressDelivery NVARCHAR(50) NOT NULL, -- địa chỉ giao hàng
  price INT NOT NULL, -- đơn giá của sản phẩm
  totalPrice int not null,--tổng tiền của sản phẩm
  PRIMARY KEY (idExport, idProduct),
  FOREIGN KEY (idProduct) REFERENCES Product(idProduct)
);

CREATE TABLE ImportTicket
(
  idImport VARCHAR(10) NOT NULL,-- Mã phiếu nhập hàng
  idProduct VARCHAR(10) NOT NULL, -- mã sản phẩm 
  date DATE NOT NULL, -- ngày lập phiếu nhập hàng
  quantity INT NOT NULL, -- số lượng sản phẩm
  addressDelivery NVARCHAR(50) NOT NULL, -- địa chỉ nhận 
  price INT NOT NULL, -- đơn giá của sản phẩm
  totalPrice int not null,--tổng tiền
  idProvider VARCHAR(10) NOT NULL, -- mã nhà cung cấp
  PRIMARY KEY (idImport, idProduct),
  FOREIGN KEY (idProduct) REFERENCES Product(idProduct),
  FOREIGN KEY (idProvider) REFERENCES Provider(idProvider)
);

-------------------------------------------------------------------
--INSERT dữ liệu
-------------------------------------------------------------------
--Bảng Admin
INSERT INTO Admin (idAdmin, username, phone, gender, email)
VALUES ('ad01', 'admin', '123456789', N'nam', 'admin@gmail.com');

--Bảng provider
INSERT INTO Provider (idProvider, nameProvider)
VALUES ('PRV01', N'Công ty Cổ phần Tràng An');

INSERT INTO Provider (idProvider, nameProvider)
VALUES ('PRV02', N'Công ty TNHH IWater');

INSERT INTO Provider (idProvider, nameProvider)
VALUES ('PRV03', N'Công ty TNHH Top Xanh');

--Bảng Product

INSERT INTO Product (idProduct, nameProduct, category, price, quantity, imageProduct, status, inventory, idProvider)
VALUES
    ('SP01', N'Bimbim Lay''s', N'Thức ăn', 10000, 20, NULL, N'còn hàng', N'kho quận 10', 'PRV01'),
    ('SP02', N'Coca-Cola', N'Đồ uống', 15000, 40, NULL, N'còn hàng', N'kho quận 10', 'PRV02'),
    ('SP03', N'Bánh ngọt', N'Thức ăn', 25000, 15, NULL, N'còn hàng', N'kho quận 10', 'PRV01'),
    ('SP04', N'Súng nước', N'Đồ chơi', 20000, 0, NULL, N'hết hàng', NULL, 'PRV03');

-- Bảng Orders
INSERT INTO Orders (idOrder, idProduct, date, quantity, status, addressDelivery, price)
VALUES ('OD01', 'SP01', '2023-3-20', 5, N'đã giao', '46 SVH', 50000);

INSERT INTO Orders (idOrder, idProduct, date, quantity, status, addressDelivery, price)
VALUES ('OD01', 'SP03', '2023-3-20', 2, N'đã giao', '46 SVH', 50000);

INSERT INTO Orders (idOrder, idProduct, date, quantity, status, addressDelivery, price)
VALUES ('OD01', 'SP02', '2023-3-20', 2, N'đã giao', '46 SVH', 30000);

INSERT INTO Orders (idOrder, idProduct, date, quantity, status, addressDelivery, price)
VALUES ('OD02', 'SP04', '2023-3-27', 1, N'đã giao', '12 TC', 20000);

--Bảng ImportTicket

INSERT INTO ImportTicket (idImport, idProduct, date, quantity, addressDelivery, price, totalPrice, idProvider)
VALUES
    ('PNH01', 'SP01', '2023-4-4', 50, N'kho quận 10', 8000, 400000, 'PRV01'),
    ('PNH02', 'SP03', '2023-5-5', 40, N'kho quận 11', 23000, 920000, 'PRV01'),
    ('PN02', 'SP02', '2023-5-6', 50, N'kho quận 12', 12000, 600000, 'PRV02'),
    ('PN03', 'SP04', '2023-6-7', 10, N'kho quận 13', 18000, 180000, 'PRV03');

--Bảng ExportTicket

INSERT INTO ExportTicket (idExport, idProduct, quantity, date, addressDelivery, price, totalprice)
VALUES
    ('PXH01', 'SP01', 20, '2023-4-1', N'cửa hàng', 8000, 160000),
    ('PXH01', 'SP03', 10, '2023-4-1', N'cửa hàng', 23000, 230000),
    ('PXH02', 'SP02', 30, '2023-4-10', N'cửa hàng', 12000, 360000);



--Bảng Invoice
INSERT INTO Invoice (idInvoice, idOrder, date, status, price)
VALUES
    ('HD01', 'OD01', '2023-03-23', N'đã thanh toán', 130000),
    ('HD02', 'OD02', '2023-03-27', N'đã thanh toán', 20000);


