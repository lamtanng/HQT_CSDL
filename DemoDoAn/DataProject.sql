create database TRUNGTAMHOCLAPTRINH
use TRUNGTAMHOCLAPTRINH
go

--DROP TABLE TRUYENTIN
--DROP TABLE HOCVAO
--DROP TABLE BANGDIEMDANH
--DROP TABLE PHUTRACH
--DROP TABLE DANHSACHNHOM
--DROP TABLE NHOMHOC
--DROP TABLE THUTRONGTUAN
--DROP TABLE NGAYHOC
--DROP TABLE THONGBAO
--DROP TABLE HOCVIEN
--DROP TABLE CAHOC
--DROP TABLE PHONGHOC
--DROP TABLE LOPHOC
--DROP TABLE KHOAHOC
--DROP TABLE GIAOVIEN
--DROP TABLE TAIKHOAN

--DROP VIEW V_DANHSACHNHOMHOC
--DROP VIEW V_HOCPHIDATHU
--DROP VIEW V_LICHHOCHOCVIEN
--DROP VIEW V_LICHHOCTRUNGTAM
--DROP VIEW V_LIENLAC

--DROP TRIGGER TG_KiemTraTrungSDT
--DROP TRIGGER TG_KiemTraTrungLop
--DROP TRIGGER TG_KiemTraDieuKienCapChungChi
--DROP TRIGGER TG_TaoIDTangTuDong

--DROP FUNCTION uf_AutoGenerateIDByParamenters
--DROP FUNCTION uf_AutoGenerateID


----------------------------------------------Funtion tạo ID tự động------------------------------------------------------


-------------------Kiểm tra đăng nhập--------------------
CREATE OR ALTER FUNCTION func_kiemTraDangNhap(@username varchar(20), @pass varchar(20))
RETURNs TABLE
	AS RETURN 
		SELECT TenDangNhap, QuyenNguoiDung
		FROM TAIKHOAN
		WHERE TenDangNhap = @username and MatKhau = @pass


SELECT * FROM GIAOVIEN WHERE TenDangNhap = 'ACC011'

SELECT dbo.TAIKHOAN.TenDangNhap, dbo.HOCVIEN.MaHocVien, dbo.GIAOVIEN.MaGiaoVien
FROM   dbo.GIAOVIEN INNER JOIN
             dbo.TAIKHOAN ON dbo.GIAOVIEN.TenDangNhap = dbo.TAIKHOAN.TenDangNhap INNER JOIN
             dbo.HOCVIEN ON dbo.TAIKHOAN.TenDangNhap = dbo.HOCVIEN.TenDangNhap
go


-- Function gán giá trị SoLuongChoNgoi trong nhóm học


-- Function Tự tạo ID có tham số truyền vào lần lượt là ID cuối cùng trong table, mẫu ID (vd: 'kh', 'nv'), độ dài ID
CREATE or ALTER FUNCTION uf_AutoGenerateIDByParamenters (@lastID varchar(10), @prefix varchar(MAX), @size int)
RETURNS varchar(10)
AS
BEGIN
	DECLARE @nextID varchar(10)
	IF(@lastID = '' or @lastID IS NULL)
		SET @lastID = @prefix + REPLICATE(0, @size - LEN(@prefix))	--kh000
	DECLARE @num_nextID int -- varchar sẽ đúng hơn là int
	SET @lastID = TRIM(@lastID) --lastID = kh000
	SET @num_nextID = REPLACE(@lastID, @prefix, '') --num = 000
	SET @num_nextID += 1 --num = 1
	DECLARE @new_size int --Kích thước của dãy số, ví dụ kh001, thì new_size = 3
	SET @new_size = @size - LEN(@prefix) --new_size = 3
	SET @nextID = @prefix + RIGHT((REPLICATE(0, @new_size) + CONVERT(VARCHAR(MAX), @num_nextID)), @new_size)

	RETURN @nextID
END
GO


-- Function Tự tạo ID với tham số truyền vào là tên Bảng cần tạo (VD: Customer, Employee)
CREATE OR ALTER FUNCTION uf_AutoGenerateID (@name varchar(MAX))
RETURNS varchar(20)
BEGIN
	DECLARE @lastID varchar(10), @prefix varchar(MAX),  @num int, @nextID varchar(10)

	SET @lastID = 
	CASE @name
		WHEN 'TAIKHOAN' THEN (SELECT TOP 1 TAIKHOAN.TenDangNhap FROM TAIKHOAN ORDER BY TAIKHOAN.TenDangNhap DESC)
		WHEN 'GIAOVIEN' THEN (SELECT TOP 1 GIAOVIEN.MaGiaoVien FROM GIAOVIEN ORDER BY GIAOVIEN.MaGiaoVien DESC)
		WHEN 'HOCVIEN'	THEN (SELECT TOP 1 HOCVIEN.MaHocVien FROM HOCVIEN ORDER BY HOCVIEN.MaHocVien DESC)
		WHEN 'THONGBAO' THEN (SELECT TOP 1 THONGBAO.MaThongBao FROM THONGBAO ORDER BY THONGBAO.MaThongBao DESC)
	END;
	
	SET @prefix =
	CASE @name
		WHEN 'TAIKHOAN' THEN 'ACC'
		WHEN 'GIAOVIEN' THEN 'GV'
		WHEN 'HOCVIEN'	THEN 'HV'
		WHEN 'THONGBAO' THEN 'TB'
	END;

	SET @num = 3 + LEN(TRIM(@prefix))

	SET @nextID = dbo.uf_AutoGenerateIDByParamenters(@lastID, @prefix, @num)


	RETURN @nextID
END
GO


-- Trigger tao ID tăng tự động --
CREATE TRIGGER TG_TaoIDTangTuDong ON NHOMHOC
AFTER Insert
AS
BEGIN
	DECLARE @maNhom varchar(20);
	DECLARE @maLop varchar(20);
	DECLARE @sizeOfNum int = 2; --(+2: là 2 số sau kí tự prefix)

	SELECT @maLop = i.MaLopHoc, @maNhom = i.MaNhomHoc FROM inserted i

	BEGIN
		DECLARE @lastID varchar(20);
		DECLARE @prefix varchar(MAX) = @maLop; --gán prefix là kí tự của maLop
		PRINT @prefix;

		--lấy số lượng kí tự trong mã nhóm 
		DECLARE @size INT = CONVERT(INT, LEN(TRIM(@prefix)) + @sizeOfNum);
		PRINT @size;

		--Lấy ra ID nhóm học gần nhất của lớp đó và có mã nhóm khác với cái mới thêm vào
		SELECT TOP 1 @lastID = NHOMHOC.MaNhomHoc FROM NHOMHOC 
		WHERE NHOMHOC.MaLopHoc = @maLop and NHOMHOC.MaNhomHoc != @maNhom 
		ORDER BY NHOMHOC.MaNhomHoc DESC;

		--gọi hàm tạo ID mới cho Nhóm học
		DECLARE @nextID varchar(20) = dbo.uf_AutoGenerateIDByParamenters(@lastID, @prefix, @size) ;
		--update ID mới cho nhóm học
		UPDATE NHOMHOC SET NHOMHOC.MaNhomHoc = @nextID WHERE NHOMHOC.MaNhomHoc = @maNhom and NHOMHOC.MaLopHoc = @maLop
	END
	
END
go




----------------------------------------------Tạo các bảng----------------------------------------------------------------




--Bảng Tài khoản
CREATE TABLE TAIKHOAN(
	TenDangNhap varchar(20) CONSTRAINT TenDangNhap DEFAULT dbo.uf_AutoGenerateID('TAIKHOAN'),
	MatKhau varchar(20) NOT NULL,
	QuyenNguoiDung varchar(20) NOT NULL,
	CONSTRAINT PK_TAIKHOAN PRIMARY KEY (TenDangNhap)
)
go

--Bảng Giáo viên
CREATE TABLE GIAOVIEN(
	MaGiaoVien varchar(20) CONSTRAINT MaGiaoVien DEFAULT dbo.uf_AutoGenerateID('GIAOVIEN'),
	HoTen nvarchar(50) NOT NULL,
	NgaySinh DATE CHECK(DATEDIFF(year, NgaySinh, GETDATE()) >= 18),
	GioiTinh nvarchar(20) NOT NULL, 
	DiaChi nvarchar(100) NOT NULL,
	SoDienThoai varchar(20) CHECK(len(SoDienThoai) = 10),
	Email varchar(50) NOT NULL,
	TenDangNhap varchar(20) CONSTRAINT FK_GIAOVIEN_TAIKHOAN FOREIGN KEY REFERENCES TAIKHOAN(TenDangNhap) ON DELETE SET NULL ON UPDATE CASCADE,
	CONSTRAINT TK_GV UNIQUE (TenDangNhap),
	CONSTRAINT PK_GIAOVIEN PRIMARY KEY (MaGiaoVien)
)
go


--Bảng Khóa học
CREATE TABLE KHOAHOC(
	MaKhoaHoc varchar(20) constraint PK_KHOAHOC PRIMARY KEY,
	TenKhoaHoc nvarchar(50) NOT NULL
)
go

--Bảng Lớp học
CREATE TABLE LOPHOC(
	MaLopHoc varchar(20) constraint PK_LOPHOC PRIMARY KEY,
	MaKhoaHoc varchar(20) constraint FK_LOPHOC_KHOAHOC FOREIGN KEY REFERENCES KHOAHOC(MaKhoaHoc) ON DELETE SET NULL ON UPDATE CASCADE,
	TenLopHoc nvarchar(50) NOT NULL,
	TongSoBuoiHoc INT CHECK(TongSoBuoiHoc > 0),
	HocPhi REAL CHECK(HocPhi >= 0)
)
go

--Bảng Phòng học
CREATE TABLE PHONGHOC(
	MaPhongHoc varchar(20) constraint PK_PHONGHOC PRIMARY KEY,
	SoLuongChoNgoi INT CHECK(SoLuongChoNgoi > 0)
)	
go


--Bảng Ca học
CREATE TABLE CAHOC(
	Ca INT constraint PK_CAHOC PRIMARY KEY,
	GioBatDau varchar(10) NOT NULL,
	GioKetThuc varchar(10) NOT NULL
)	
go

--Bảng Nhóm học
CREATE TABLE NHOMHOC(
	MaNhomHoc varchar(20) constraint PK_NHOMHOC PRIMARY KEY Default '',
	MaLopHoc varchar(20) constraint FK_NHOMHOC_LOPHOC FOREIGN KEY REFERENCES LOPHOC(MaLopHoc) ON DELETE SET NULL ON UPDATE CASCADE,
	MaGiaoVien varchar(20) constraint FK_NHOMHOC_GIAOVIEN FOREIGN KEY REFERENCES GIAOVIEN(MaGiaoVien) ON DELETE SET NULL ON UPDATE CASCADE,
	MaPhongHoc varchar(20) constraint FK_NHOMHOC_PHONGHOC FOREIGN KEY REFERENCES PHONGHOC(MaPhongHoc) ON DELETE SET NULL ON UPDATE CASCADE,
	Ca INT constraint FK_NHOMHOC_CAHOC FOREIGN KEY REFERENCES CAHOC(Ca) ON DELETE SET NULL ON UPDATE CASCADE,
	SoLuongHocVienToiThieu INT CHECK(SoLuongHocVienToiThieu > 0), 
	SoLuongHocVienToiDa INT CHECK(SoLuongHocVienToiDa > 0), --trigger
	NgayBatDau DATE NOT NULL,
	NgayKetThuc DATE NOT NULL, --trigger
	TongHocVien int NOT NULL DEFAULT 0,
	TrangThaiMoDangKy BIT NOT NULL DEFAULT 0,
)	
go

--Bảng Học viên
CREATE TABLE HOCVIEN(
	MaHocVien varchar(20) CONSTRAINT MaHocVien DEFAULT dbo.uf_AutoGenerateID('HOCVIEN'),
	TenHocVien nvarchar(50) NOT NULL,
	NgaySinh DATE NOT NULL,
	GioiTinh nvarchar(20) NOT NULL, 
	DiaChi nvarchar(100) NOT NULL,
	SoDienThoai varchar(20) CHECK(len(SoDienThoai) = 10),
	CCCD varchar(50) CHECK(len(CCCD) = 12),
	TenDangNhap varchar(20) CONSTRAINT FK_HOCVIEN_TAIKHOAN FOREIGN KEY REFERENCES TAIKHOAN(TenDangNhap) ON DELETE SET NULL ON UPDATE CASCADE,
	CONSTRAINT TK_HV UNIQUE (TenDangNhap),
	CONSTRAINT PK_HOCVIEN PRIMARY KEY (MaHocVien)
)
go

--Bảng Thông báo
CREATE TABLE THONGBAO(
	MaThongBao varchar(20) CONSTRAINT MaThongBao DEFAULT dbo.uf_AutoGenerateID('THONGBAO'),
	MaGiaoVien varchar(20) CONSTRAINT FK_THONGBAO_GIAOVIEN FOREIGN KEY REFERENCES GIAOVIEN(MaGiaoVien) ON DELETE SET NULL ON UPDATE CASCADE,
	TieuDe nvarchar(50),
	NoiDung nvarchar(255) NOT NULL,
	CONSTRAINT PK_THONGBAO PRIMARY KEY (MaThongBao)
)
go

--Bảng Ngày học
CREATE TABLE NGAYHOC(
	NgayHoc DATE constraint PK_NGAYHOC PRIMARY KEY,
)
go

--Bảng Thú trong tuần
CREATE TABLE THUTRONGTUAN(
	ThuTrongTuan nvarchar(20) constraint PK_THUTRONGTUAN PRIMARY KEY,
)
go

--Bảng Phụ trách
CREATE TABLE PHUTRACH(
	MaGiaoVien varchar(20) constraint FK_PHUTRACH_GIAOVIEN FOREIGN KEY REFERENCES GIAOVIEN(MaGiaoVien) ON DELETE CASCADE ON UPDATE CASCADE,
	MaKhoaHoc varchar(20) constraint FK_PHUTRACH_KHOAHOC FOREIGN KEY REFERENCES KHOAHOC(MaKhoaHoc) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT PK_PHUTRACH PRIMARY KEY (MaGiaoVien, MaKhoaHoc)
)
go

--Bảng Học vào
CREATE TABLE HOCVAO(
	MaNhomHoc varchar(20) constraint FK_HOCVAO_NHOMHOC FOREIGN KEY REFERENCES NHOMHOC(MaNhomHoc) ON DELETE CASCADE ON UPDATE CASCADE,
	ThuTrongTuan nvarchar(20) constraint FK_HOCVAO_THUTRONGTUAN FOREIGN KEY REFERENCES THUTRONGTUAN(ThuTrongTuan) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT PK_HOCVAO PRIMARY KEY (MaNhomHoc, ThuTrongTuan)
)
go

--Bảng Danh sách nhóm
CREATE TABLE DANHSACHNHOM(
	MaNhomHoc varchar(20) constraint FK_DANHSACHNHOM_NHOMHOC FOREIGN KEY REFERENCES NHOMHOC(MaNhomHoc) ON DELETE CASCADE,
	MaHocVien varchar(20) constraint FK_DANHSACHNHOM_HOCVIEN FOREIGN KEY REFERENCES HOCVIEN(MaHocVien) ON DELETE CASCADE,
	DiemLyThuyet REAL CHECK(DiemLyThuyet between 0 and 10) DEFAULT 0,
	DiemThucHanh REAL CHECK(DiemThucHanh between 0 and 10) DEFAULT 0,
	TrangThaiThanhToan BIT NOT NULL DEFAULT 0,
	TrangThaiCapChungChi BIT NOT NULL DEFAULT 0,
	CONSTRAINT PK_DANHSACHNHOM PRIMARY KEY (MaHocVien, MaNhomHoc)
)
go

--Bảng Truyền tin
CREATE TABLE TRUYENTIN(
	MaThongBao varchar(20) constraint FK_TRUYENTIN_THONGBAO FOREIGN KEY REFERENCES THONGBAO(MaThongBao) ON DELETE CASCADE,
	MaNhomHoc varchar(20) constraint FK_TRUYENTIN_NHOMHOC FOREIGN KEY REFERENCES NHOMHOC(MaNhomHoc) ON DELETE CASCADE,
	CONSTRAINT PK_TRUYENTIN PRIMARY KEY (MaThongBao, MaNhomHoc)
)
go

--Bảng Bảng điểm danh
CREATE TABLE BANGDIEMDANH(
	NgayHoc DATE constraint FK_BANGDIEMDANH_NGAYHOC FOREIGN KEY REFERENCES NGAYHOC(NgayHoc) ON DELETE CASCADE ,
	MaNhomHoc varchar(20) constraint FK_BANGDIEMDANH_NHOMHOC FOREIGN KEY REFERENCES NHOMHOC(MaNhomHoc) ON DELETE CASCADE ,
	MaHocVien varchar(20) constraint FK_BANGDIEMDANH_HOCVIEN FOREIGN KEY REFERENCES HOCVIEN(MaHocVien) ON DELETE CASCADE,
	HienDien BIT NOT NULL DEFAULT 0,
	CONSTRAINT PK_BANGDIEMDANH PRIMARY KEY (NgayHoc, MaNhomHoc, MaHocVien)
)
go

----------------------------------------------------Nhập liệu----------------------------------------------------------

--TAIKHOAN
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passhocvien', '1' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passhocvien', '1' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passhocvien', '1' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passhocvien', '1' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passhocvien', '1' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passhocvien', '1' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passhocvien', '1' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passhocvien', '1' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passhocvien', '1' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passhocvien', '1' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passgiaovien', '2' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passgiaovien', '2' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passgiaovien', '2' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passgiaovien', '2' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passgiaovien', '2' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passgiaovien', '2' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passgiaovien', '2' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passgiaovien', '2' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passgiaovien', '2' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passgiaovien', '2' )
INSERT INTO TAIKHOAN(MatKhau, QuyenNguoiDung) VALUES 
('passadmin', '0' )
GO

--GIAOVIEN
INSERT INTO GIAOVIEN(HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap) VALUES
(N'Nguyễn Thị Hoa', N'Nam', '1998-03-17', N'Hồ Chí Minh', '0987654321', 'gvhoa@gmail.com', 'ACC011')
INSERT INTO GIAOVIEN(HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap) VALUES
(N'Trần Văn Hải', N'Nam', '1997-06-05', N'Bến Tre', '0912345678', 'gvhai@gmail.com', 'ACC012')
INSERT INTO GIAOVIEN(HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap) VALUES
(N'Lê Thị Thanh Hà', N'Nữ', '1995-11-11', N'Đồng Tháp', '0936714148', 'gvha@gmail.com', 'ACC013')
INSERT INTO GIAOVIEN(HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap) VALUES
(N'Phạm Văn Nam', N'Nam', '1996-08-23', N'Hồ Chí Minh', '0923272864', 'gvnam@gmail.com', 'ACC014')
INSERT INTO GIAOVIEN(HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap) VALUES
(N'Hoàng Thị Hồng Nhung', N'Nữ', '1994-01-29', N'Hồ Chí Minh', '0975428093', 'gvnhung@gmail.com', 'ACC015')
INSERT INTO GIAOVIEN(HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap) VALUES
(N'Đặng Thanh Tuấn', N'Nam', '1993-04-19', N'An Giang', '0893378460', 'gvtuan@gmail.com', 'ACC016')
INSERT INTO GIAOVIEN(HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap) VALUES
(N'Võ Thị Ngọc Diễm', N'Nữ', '1992-07-08', N'Hồ Chí Minh', '0946603030', 'gvdiem@gmail.com', 'ACC017')
INSERT INTO GIAOVIEN(HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap) VALUES
(N'Bùi Văn Phương', N'Nam', '1991-09-26', N'An Giang', '0937723847', 'gvphuong@gmail.com', 'ACC018')
INSERT INTO GIAOVIEN(HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap) VALUES
(N'Mai Thị Thu Hà', N'Nữ', '1990-12-14', N'Bình Dương', '0917972667', 'gvhathu@gmail.com', 'ACC019')
INSERT INTO GIAOVIEN(HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap) VALUES
(N'Ngô Đình Trung', N'Nam', '1989-03-04', N'Gia Lai', '0943834283', 'gvtrung@gmail.com', 'ACC020')
GO

--KHOAHOC
Insert into KHOAHOC (MaKhoaHoc, TenKhoaHoc) Values
('CSS', N'Lập trình CSS'),
('CPP', N'Lập trình C++'),
('HTML', N'Lập trình HTML'),
('JAVA', N'Lập trình JAVA'),
('JS', N'Lập trình JAVASCRIPT')
GO

--LOPHOC
INSERT INTO LOPHOC(MaLopHoc, MaKhoaHoc, TenLopHoc, TongSoBuoiHoc, HocPhi) VALUES 
('CSS_CB','CSS', N'Lập trình CSS cơ bản', 30, 500000),
('CSS_NC','CSS', N'Lập trình CSS nâng cao', 30, 800000),
('HTML_CB','HTML', N'Lập trình HTML cơ bản', 30, 500000),
('HTML_NC', 'HTML', N'Lập trình HTML nâng cao', 30, 800000),
('JAVA_CB','JAVA', N'Lập trình JAVA cơ bản', 35, 650000),
('JAVA_NC','JAVA', N'Lập trình JAVA nâng cao', 40, 950000),
('CPP_CB','CPP', N'Lập trình C++ cơ bản', 25, 600000),
('CPP_NC','CPP', N'Lập trình C++ nâng cao', 40, 900000),
('JS_CB','JS', N'Lập trình JS cơ bản', 35, 600000),
('JS_NC','JS', N'Lập trình JS nâng cao', 46, 850000)
GO

--PHONGHOC
INSERT INTO PHONGHOC(MaPhongHoc, SoLuongChoNgoi) VALUES 
('P01', 30),
('P02', 35),
('P03', 40),
('P04', 30),
('P05', 30)
GO

--CAHOC
INSERT INTO CAHOC(Ca, GioBatDau, GioKetThuc) VALUES 
(1, '9', '11'),
(2, '15', '17'),
(3, '17', '19'),
(4, '19', '21')
GO

--NHOMHOC
INSERT INTO NHOMHOC(MaLopHoc, MaGiaoVien, MaPhongHoc, Ca, SoLuongHocVienToiThieu, SoLuongHocVienToiDa, NgayBatDau, NgayKetThuc, TrangThaiMoDangKy) VALUES
('CPP_CB', 'GV001', 'P01', 1, 2, 30, '2023-01-03', '2023-03-03', 1)
INSERT INTO NHOMHOC(MaLopHoc, MaGiaoVien, MaPhongHoc, Ca, SoLuongHocVienToiThieu, SoLuongHocVienToiDa, NgayBatDau, NgayKetThuc, TrangThaiMoDangKy) VALUES
('CPP_CB', 'GV002', 'P02', 1, 2, 35, '2023-01-03', '2023-03-03', 1)
INSERT INTO NHOMHOC(MaLopHoc, MaGiaoVien, MaPhongHoc, Ca, SoLuongHocVienToiThieu, SoLuongHocVienToiDa, NgayBatDau, NgayKetThuc, TrangThaiMoDangKy) VALUES
('CPP_NC', 'GV003', 'P03', 2, 2, 40, '2023-01-03', '2023-03-03', 1)
INSERT INTO NHOMHOC(MaLopHoc, MaGiaoVien, MaPhongHoc, Ca, SoLuongHocVienToiThieu, SoLuongHocVienToiDa, NgayBatDau, NgayKetThuc, TrangThaiMoDangKy) VALUES
('JS_CB', 'GV005', 'P05', 3, 2, 30, '2023-01-03', '2023-03-03', 1)
INSERT INTO NHOMHOC(MaLopHoc, MaGiaoVien, MaPhongHoc, Ca, SoLuongHocVienToiThieu, SoLuongHocVienToiDa, NgayBatDau, NgayKetThuc, TrangThaiMoDangKy) VALUES
('JS_CB', 'GV005', 'P05', 3, 2, 30, '2023-01-03', '2023-03-03', 1)
GO

--HOCVIEN
INSERT INTO HOCVIEN(TenHocVien, GioiTinh, NgaySinh, DiaChi, SoDienThoai, CCCD, TenDangNhap) VALUES
(N'Nguyễn Thị Hoa', N'Nam', '1998-03-17', N'Hồ Chí Minh', '0987654321', '458209041923', 'ACC001')
INSERT INTO HOCVIEN(TenHocVien, GioiTinh, NgaySinh, DiaChi, SoDienThoai, CCCD, TenDangNhap) VALUES
(N'Trần Văn Hải', N'Nam', '1997-06-05', N'Bến Tre', '0912345678', '458209041923', 'ACC002')
INSERT INTO HOCVIEN(TenHocVien, GioiTinh, NgaySinh, DiaChi, SoDienThoai, CCCD, TenDangNhap) VALUES
(N'Lê Thị Thanh Hà', N'Nữ', '1995-11-11', N'Đồng Tháp', '0936714148', '458209041923', 'ACC003')
INSERT INTO HOCVIEN(TenHocVien, GioiTinh, NgaySinh, DiaChi, SoDienThoai, CCCD, TenDangNhap) VALUES
(N'Phạm Văn Nam', N'Nam', '1996-08-23', N'Hồ Chí Minh', '0923272864', '458209041923', 'ACC004')
INSERT INTO HOCVIEN(TenHocVien, GioiTinh, NgaySinh, DiaChi, SoDienThoai, CCCD, TenDangNhap) VALUES
(N'Hoàng Thị Hồng Nhung', N'Nữ', '1994-01-29', N'Hồ Chí Minh', '0975428093', '458209041923', 'ACC005')
INSERT INTO HOCVIEN(TenHocVien, GioiTinh, NgaySinh, DiaChi, SoDienThoai, CCCD, TenDangNhap) VALUES
(N'Đặng Thanh Tuấn', N'Nam', '1993-04-19', N'An Giang', '0893378460', '458209041923', 'ACC006')
INSERT INTO HOCVIEN(TenHocVien, GioiTinh, NgaySinh, DiaChi, SoDienThoai, CCCD, TenDangNhap) VALUES
(N'Võ Thị Ngọc Diễm', N'Nữ', '1992-07-08', N'Hồ Chí Minh', '0946603030', '458209041923', 'ACC007')
INSERT INTO HOCVIEN(TenHocVien, GioiTinh, NgaySinh, DiaChi, SoDienThoai, CCCD, TenDangNhap) VALUES
(N'Bùi Văn Phương', N'Nam', '1991-09-26', N'An Giang', '0937723847', '458209041923', 'ACC008')
INSERT INTO HOCVIEN(TenHocVien, GioiTinh, NgaySinh, DiaChi, SoDienThoai, CCCD, TenDangNhap) VALUES
(N'Mai Thị Thu Hà', N'Nữ', '1990-12-14', N'Bình Dương', '0917972667', '458209041923', 'ACC009')
INSERT INTO HOCVIEN(TenHocVien, GioiTinh, NgaySinh, DiaChi, SoDienThoai, CCCD, TenDangNhap) VALUES
(N'Ngô Đình Trung', N'Nam', '1989-03-04', N'Gia Lai', '0943834283', '458209041923', 'ACC0010')
GO

--NGAYHOC
INSERT INTO NGAYHOC(NgayHoc) VALUES
('2023-01-03')
GO

--THUTRONGTUAN
INSERT INTO THUTRONGTUAN(ThuTrongTuan) VALUES
('2'), ('3'), ('4'), ('5'), ('6'), ('7')
GO

--PHUTRACH
INSERT INTO PHUTRACH(MaGiaoVien, MaKhoaHoc) VALUES
('GV001', 'CPP'),
('GV002', 'CPP'),
('GV003', 'CPP'),
('GV005', 'JS')
GO

--HOCVAO
INSERT INTO HOCVAO(MaNhomHoc, ThuTrongTuan) VALUES
('CPP_CB01', '2'),
('CPP_CB01', '4'),
('CPP_CB01', '6'),
('CPP_CB02', '2'),
('CPP_CB02', '4'),
('CPP_CB02', '6'),
('JS_CB01', '3'),
('JS_CB01', '5'),
('JS_CB01', '7')
GO

--DANHSACHNHOM
INSERT INTO DANHSACHNHOM(MaNhomHoc, MaHocVien) VALUES
('CPP_CB01', 'HV001'),
('CPP_CB01', 'HV002'),
('CPP_CB01', 'HV003'),
('CPP_CB02', 'HV004'),
('CPP_CB02', 'HV005'),
('CPP_CB02', 'HV006'),
('CPP_CB02', 'HV007'),
('CPP_NC01', 'HV001'),
('CPP_NC01', 'HV003'),
('CPP_NC01', 'HV005'),
('JS_CB01', 'HV001'),
('JS_CB01', 'HV009'),
('JS_CB01', 'HV008')
GO



--------------------------------------------------------Tạo view---------------------------------------------------------



--view LICHHOC cua trung tam
CREATE VIEW V_LICHHOCTRUNGTAM AS
SELECT dbo.NHOMHOC.MaNhomHoc, dbo.LOPHOC.TenLopHoc, dbo.CAHOC.Ca, dbo.PHONGHOC.MaPhongHoc, dbo.HOCVAO.ThuTrongTuan, dbo.GIAOVIEN.MaGiaoVien, dbo.GIAOVIEN.HoTen
FROM     dbo.CAHOC INNER JOIN
                  dbo.NHOMHOC ON dbo.CAHOC.Ca = dbo.NHOMHOC.Ca INNER JOIN
                  dbo.GIAOVIEN ON dbo.NHOMHOC.MaGiaoVien = dbo.GIAOVIEN.MaGiaoVien INNER JOIN
                  dbo.PHONGHOC ON dbo.NHOMHOC.MaPhongHoc = dbo.PHONGHOC.MaPhongHoc INNER JOIN
                  dbo.LOPHOC ON dbo.NHOMHOC.MaLopHoc = dbo.LOPHOC.MaLopHoc INNER JOIN
                  dbo.HOCVAO  ON dbo.NHOMHOC.MaNhomHoc = dbo.HOCVAO.MaNhomHoc
GO

--view Lich hoc cua hoc vien
CREATE VIEW V_LICHHOCHOCVIEN AS
SELECT dbo.HOCVIEN.MaHocVien, dbo.HOCVIEN.TenHocVien, dbo.DANHSACHNHOM.MaNhomHoc, dbo.NHOMHOC.MaLopHoc, dbo.LOPHOC.TenLopHoc, dbo.PHONGHOC.MaPhongHoc, dbo.CAHOC.Ca, dbo.HOCVAO.ThuTrongTuan, 
                  dbo.GIAOVIEN.MaGiaoVien, dbo.GIAOVIEN.HoTen
FROM     dbo.HOCVIEN INNER JOIN
                  dbo.DANHSACHNHOM ON dbo.HOCVIEN.MaHocVien = dbo.DANHSACHNHOM.MaHocVien INNER JOIN
                  dbo.NHOMHOC ON dbo.DANHSACHNHOM.MaNhomHoc = dbo.NHOMHOC.MaNhomHoc INNER JOIN
                  dbo.PHONGHOC ON dbo.NHOMHOC.MaPhongHoc = dbo.PHONGHOC.MaPhongHoc INNER JOIN
                  dbo.CAHOC ON dbo.NHOMHOC.Ca = dbo.CAHOC.Ca INNER JOIN
                  dbo.HOCVAO ON dbo.NHOMHOC.MaNhomHoc = dbo.HOCVAO.MaNhomHoc INNER JOIN
                  dbo.GIAOVIEN ON dbo.NHOMHOC.MaGiaoVien = dbo.GIAOVIEN.MaGiaoVien INNER JOIN
                  dbo.LOPHOC ON dbo.NHOMHOC.MaLopHoc = dbo.LOPHOC.MaLopHoc
GO

--view thong ke nhom hoc
CREATE VIEW V_DANHSACHNHOMHOC AS
SELECT dbo.KHOAHOC.MaKhoaHoc, dbo.KHOAHOC.TenKhoaHoc, dbo.LOPHOC.MaLopHoc, dbo.LOPHOC.TenLopHoc, dbo.NHOMHOC.MaNhomHoc, dbo.LOPHOC.HocPhi
FROM     dbo.KHOAHOC INNER JOIN
                  dbo.LOPHOC ON dbo.KHOAHOC.MaKhoaHoc = dbo.LOPHOC.MaKhoaHoc INNER JOIN
                  dbo.NHOMHOC ON dbo.LOPHOC.MaLopHoc = dbo.NHOMHOC.MaLopHoc
GO

--view thong ke hoc phi da thu
CREATE VIEW V_HOCPHIDATHU AS
SELECT dbo.HOCVIEN.MaHocVien, dbo.HOCVIEN.TenHocVien, dbo.DANHSACHNHOM.MaNhomHoc, dbo.LOPHOC.MaLopHoc, dbo.LOPHOC.TenLopHoc, dbo.LOPHOC.HocPhi, dbo.DANHSACHNHOM.TrangThaiThanhToan
FROM     dbo.HOCVIEN INNER JOIN
                  dbo.DANHSACHNHOM ON dbo.HOCVIEN.MaHocVien = dbo.DANHSACHNHOM.MaHocVien INNER JOIN
                  dbo.NHOMHOC ON dbo.DANHSACHNHOM.MaNhomHoc = dbo.NHOMHOC.MaNhomHoc INNER JOIN
                  dbo.LOPHOC ON dbo.NHOMHOC.MaLopHoc = dbo.LOPHOC.MaLopHoc
GO

--view thông tin liên lạc giáo viên và học viên:
CREATE VIEW dbo.V_LIENLAC AS
SELECT	CONCAT(dbo.HOCVIEN.MaHocVien, dbo.GIAOVIEN.MaGiaoVien) AS Ma,
		CONCAT(dbo.HOCVIEN.TenHocVien, dbo.GIAOVIEN.HoTen) AS HoTen,
		CONCAT(dbo.HOCVIEN.SoDienThoai, dbo.GIAOVIEN.SoDienThoai) AS SoDienThoai
FROM     dbo.HOCVIEN FULL JOIN dbo.GIAOVIEN on HOCVIEN.MaHocVien = GIAOVIEN.MaGiaoVien
GO

-- views thong tin HS và GV
CREATE VIEW DBO.V_GIOITHIEU AS
SELECT MaGiaoVien AS Ma, HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, NULL AS CCCD, TenDangNhap
FROM GIAOVIEN
UNION
SELECT MaHocVien AS Ma, TenHocVien AS HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, NULL AS Email, CCCD, TenDangNhap
FROM HOCVIEN;




----------------------------------------------------Tạo trigger-------------------------------------------------------




--Kiểm tra trùng sdt:
CREATE TRIGGER TG_KiemTraTrungSDT
ON dbo.V_GIOITHIEU
INSTEAD OF INSERT, UPDATE
AS
BEGIN
    DECLARE @count INT = 0
	-- kiem tra xem sodienthoai trong bang view co duoc su dung hay chua
    SELECT @count = COUNT(*)
    FROM dbo.V_GIOITHIEU
    WHERE dbo.V_GIOITHIEU.SoDienThoai IN (SELECT inserted.SoDienThoai FROM inserted)

    IF (@count > 0)  
		BEGIN
			PRINT N'Số điện thoại này đã được sử dụng.';
			print @count
			ROLLBACK TRAN;
			
		END
	ELSE
	BEGIN
		DECLARE @ma varchar(20);
		DECLARE @sdt varchar(20);
		DECLARE @COUNTIDHV INT = 0;
		DECLARE @COUNTIDGV INT =0;
		SELECT @ma = i.Ma, @sdt = i.SoDienThoai FROM inserted i
		SELECT @COUNTIDHV = COUNT(*) FROM HOCVIEN WHERE @ma = HOCVIEN.MaHocVien
		SELECT @COUNTIDGV = COUNT(*) FROM GIAOVIEN WHERE @ma = GIAOVIEN.MaGiaoVien
		-- Kiem tra xem doi tuong duoc them vao da co trong bang HocVien chua
		IF(@COUNTIDHV > 0)
			BEGIN
				UPDATE HOCVIEN Set SoDienThoai = @sdt WHERE HOCVIEN.MaHocVien = @ma
			END
		-- Kiem tra xem doi tuong duoc them vao da co trong bang GiaoVien chua
		ELSE IF(@COUNTIDGV > 0)
			BEGIN
				UPDATE GIAOVIEN Set SoDienThoai = @sdt WHERE GIAOVIEN.MaGiaoVien = @ma
			END
		-- Kiem tra ma duoc them vao de xem ma do la HV hay GV de them vao bang phu hop
		ELSE IF (@ma like 'GV%')
			BEGIN
				INSERT INTO GIAOVIEN(GIAOVIEN.MaGiaoVien, GIAOVIEN.HoTen, GIAOVIEN.GioiTinh, GIAOVIEN.NgaySinh, GIAOVIEN.DiaChi, GIAOVIEN.SoDienThoai, GIAOVIEN.Email, GIAOVIEN.TenDangNhap)
				SELECT inserted.Ma, inserted.HoTen, inserted.GioiTinh, inserted.NgaySinh, inserted.DiaChi, inserted.SoDienThoai, inserted.Email, inserted.TenDangNhap FROM inserted
			END
		ELSE IF (@ma like 'HV%')
			BEGIN
				INSERT INTO HOCVIEN(HOCVIEN.MaHocVien, HOCVIEN.TenHocVien, HOCVIEN.GioiTinh, HOCVIEN.NgaySinh, HOCVIEN.DiaChi, HOCVIEN.SoDienThoai, HOCVIEN.CCCD, HOCVIEN.TenDangNhap)
				SELECT inserted.Ma, inserted.HoTen, inserted.GioiTinh, inserted.NgaySinh, inserted.DiaChi, inserted.SoDienThoai, inserted.CCCD, inserted.TenDangNhap FROM inserted
			END
	END
END
GO	



---------------------------------------- Trigger lien quan den bang NHOMHOC va DANHSACHNHOM -------------------


--Trigger kiểm tra học viên có đăng ký 2 nhóm cùng 1 lớp không?
drop trigger TG_KiemTraTrungLop
CREATE OR ALTER TRIGGER TG_KiemTraTrungLop ON DANHSACHNHOM
AFTER INSERT
AS
BEGIN
	
	DECLARE @SoHocVienToiDa int;
	DECLARE @SiSoHienTai int;
	DECLARE @MaNhomHocMoi varchar(20);
	Declare @MaHocVien varchar(20)

	SELECT @SiSoHienTai = NHOMHOC.TongHocVien, @SoHocVienToiDa = NHOMHOC.SoLuongHocVienToiDa, @MaNhomHocMoi=i.MaNhomHoc, @MaHocVien = i.MaHocVien
	FROM NHOMHOC, inserted i
	WHERE NHOMHOC.MaNhomHoc = i.MaNhomHoc

	PRINT (@MaHocVien)
	PRINT (@SiSoHienTai)
	print (@SoHocVienToiDa)
	PRINT (@MaNhomHocMoi)

	IF  EXISTS (	--Lấy dữ liệu MaHocVien, MaNhomHoc và MaLopHoc từ dữ liệu vừa được thêm vào
				SELECT * 
				FROM (Select i.MaHocVien, i.MaNhomHoc, LOPHOC.MaLopHoc 
						FROM inserted i join NHOMHOC on i.MaNhomHoc = NHOMHOC.MaNhomHoc
								join LOPHOC on NHOMHOC.MaLopHoc = LOPHOC.MaLopHoc)q
				WHERE EXISTS (--Lấy dữ liệu MaHocVien, MaNhomHoc và MaLopHoc trong hệ thống để đối chiếu với dữ liệu mới được thêm vào
							SELECT *
							FROM (SELECT DANHSACHNHOM.MaHocVien, LOPHOC.MaLopHoc, NHOMHOC.MaNhomHoc
								FROM DANHSACHNHOM join NHOMHOC on DANHSACHNHOM.MaNhomHoc = NHOMHOC.MaNhomHoc
											join LOPHOC on NHOMHOC.MaLopHoc =  LOPHOC.MaLopHoc)p
						--Kiểm tra xem hệ thống đã có lưu trữ bộ nào chứa MaHocVien trùng với MaHocVien được thêm vào,
						--MaLopHoc trùng với MaLopHoc được thêm vào và MaNhomHoc khác với MaNhomHoc được thêm vào
							WHERE p.MaHocVien = q.MaHocVien and p.MaLopHoc = q.MaLopHoc and p.MaNhomHoc <> q.MaNhomHoc))
		BEGIN
		--Nếu có thì tiến hành báo lỗi và hoàn tác
			RAISERROR('HỌC VIÊN ĐÃ THAM GIA LỚP HỌC NÀY!',16,1);
			ROLLBACK;
		END

	ELSE 
		BEGIN
			print @SiSoHienTai
			IF(@SiSoHienTai >= @SoHocVienToiDa)
				BEGIN
					--Nếu có thì tiến hành báo lỗi và hoàn tác
					RAISERROR('NHÓM ĐÃ ĐỦ SỐ LƯỢNG SINH VIÊN!',16,1);
					ROLLBACK;
				END
			ELSE
				BEGIN
					
					DECLARE @demTrung int = 0;
					select @demTrung = count(*) from dbo.uf_DemLichTrung(@MaHocVien, @MaNhomHocMoi)
					Print @demTrung;
					IF(@demTrung > 0)
						BEGIN
							RAISERROR('BỊ TRÙNG LỊCH HỌC!',16,1);
							ROLLBACK;
						END
					ELSE
						BEGIN
							pRINT '+tongHV'
							UPDATE NHOMHOC SET TongHocVien=TongHocVien+1 WHERE NHOMHOC.MaNhomHoc =@MaNhomHocMoi;
						END
				END
		END
END
go

DECLARE @c int = 0;
select @c = count(*) from dbo.uf_DemLichTrung('HV002', 'CPP_CB01')
print @c

select HOCVAO.*, NHOMHOC.Ca from HOCVAO join NHOMHOC on HOCVAO.MaNhomHoc = NHOMHOC.MaNhomHoc 
select * from DANHSACHNHOM
delete DANHSACHNHOM where MaHocVien = 'HV002' and MaNhomHoc = 'CPP_CB01'
insert DANHSACHNHOM(MaHocVien, MaNhomHoc) VALUES ('HV002', 'CPP_CB01');
select count(*) from dbo.uf_DemLichTrung('HV002', 'CPP_CB01')

SELECT *
        FROM uf_KhoaHocCuaHocVien('HV002') kh
        INNER JOIN uf_NhomHocHocVienMuonChuyenDen('CPP_CB01') nh
        ON kh.Ca = nh.Ca AND kh.ThuTrongTuan = nh.ThuTrongTuan

--Trigger kiểm tra số lượng học viên của lớp khi xóa học viên khỏi danh sách
CREATE OR ALTER TRIGGER TG_KiemTraSoLuongHV ON DANHSACHNHOM
FOR DELETE
AS
BEGIN
	Declare @MaNhomHocBiXoa varchar(20)
	Select @MaNhomHocBiXoa = d.MaNhomHoc
	FROM NHOMHOC, deleted d
	WHERE NHOMHOC.MaNhomHoc = d.MaNhomHoc
	PRINT (@MaNhomHocBiXoa)
	UPDATE NHOMHOC SET TongHocVien=TongHocVien-1 where NHOMHOC.MaNhomHoc = @MaNhomHocBiXoa
END
go


--Trigger kiểm tra điều kiện cấp chứng chỉ:
CREATE TRIGGER TG_KiemTraDieuKienCapChungChi
ON DANHSACHNHOM
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @MaNhomHoc varchar(20);
    DECLARE @MaHocVien varchar(20);
    DECLARE @DiemLyThuyet REAL;
    DECLARE @DiemThucHanh REAL;
	DECLARE @DiemQuaMon REAL = 5;

    SELECT @MaNhomHoc = i.MaNhomHoc, @MaHocVien = i.MaHocVien, @DiemLyThuyet = i.DiemLyThuyet, @DiemThucHanh = i.DiemThucHanh
    FROM inserted i;

    IF @DiemLyThuyet >= @DiemQuaMon AND @DiemThucHanh >= @DiemQuaMon
    BEGIN
        UPDATE DANHSACHNHOM
        SET TrangThaiCapChungChi = 1
        WHERE MaNhomHoc = @MaNhomHoc AND MaHocVien = @MaHocVien;
    END

	  ELSE
    BEGIN
        UPDATE DANHSACHNHOM
        SET TrangThaiCapChungChi = 0
        WHERE MaNhomHoc = @MaNhomHoc AND MaHocVien = @MaHocVien;
    END
END;
go


-------------------------------------------- CHỈNH SỬA BẢNG ---------------------------------------------------


--Chỉnh sửa bảng NHOMHOC
Alter table NHOMHOC Add constraint df_value Default '' for MaNhomHoc
alter table NHOMHOC add TongHocVien int default 0
Alter table NHOMHOC Drop constraint DF__NHOMHOC__TrangTh__36470DEF
Alter table NHOMHOC Add constraint DF__NHOMHOC__TrangTh__36470DEF default 1 for TrangThaiMoDangKy
go

--Chỉnh sửa bảng THONGBAO
Alter table THONGBAO
Add NgayGui date DEFAULT GETDATE()
Alter table THONGBAO
Add GioGui time DEFAULT GETDATE()
go

update NHOMHOC
set TongHocVien = (
	select TongHocVienNhom
	from (select dbo.DANHSACHNHOM.MaNhomHoc, COUNT(MaHocVien) as TongHocVienNhom
			from DANHSACHNHOM
			group by MaNhomHoc) as t
	where NHOMHOC.MaNhomHoc = t.MaNhomHoc
)
go



----------------------------------------------FUNCTION MỚI-----------------------------------------------



--Tìm số buổi học dựa trên lớp học
CREATE OR ALTER FUNCTION uf_TimTongSoBuoiHoc (@maLop varchar(20))
RETURNS int
AS
BEGIN
	
	DECLARE @TongSoBuoiHoc int

	SELECT @TongSoBuoiHoc = LOPHOC.TongSoBuoiHoc 
	FROM LOPHOC
	WHERE LOPHOC.MaLopHoc = @maLop

	RETURN @TongSoBuoiHoc

END
go

Select dbo.uf_TimTongSoBuoiHoc('CSS_CB')
go

--Tính ngày kết thúc khóa học dựa trên ngày khai giảng và số buổi học của khóa đó
CREATE OR ALTER FUNCTION uf_TinhNgayKetThuc
(
    @ngayNhapHoc date,
	@maLop varchar(20)
)
RETURNS date
AS
BEGIN
	DECLARE @soBuoiHoc int;

	Select @soBuoiHoc = dbo.uf_TimTongSoBuoiHoc(@maLop)

    DECLARE @ngayKetThuc date;
    DECLARE @soNgay int;
    
    DECLARE @soTuan int = @soBuoiHoc / 3.0;
    
    DECLARE @soBuoiDu int = @soBuoiHoc % 3;
    SET @soNgay = @soTuan * 7;
    
    IF @soBuoiDu = 0
        SET @soNgay = @soNgay - 3;
    ELSE IF @soBuoiDu = 2
        SET @soNgay = @soNgay + 2;

    SET @ngayKetThuc = DATEADD(day, @soNgay, @ngayNhapHoc);
    
    RETURN @ngayKetThuc;
END;
go

SELECT dbo.uf_TinhNgayKetThuc('2023-11-8', 'CSS_CB') AS NgayKetThuc;
go

--Tìm số học viên tối đa của nhóm học dựa trên số chỗ ngồi của phòng học
CREATE OR ALTER FUNCTION uf_TimSoHocVienToiDa (@maPhong varchar(20))
RETURNS int
AS
BEGIN
	
	DECLARE @SoHocVienToiDa int

	SELECT @SoHocVienToiDa = PHONGHOC.SoLuongChoNgoi 
	FROM PHONGHOC
	WHERE PHONGHOC.MaPhongHoc = @maPhong

	RETURN @SoHocVienToiDa

END
go

Select dbo.uf_TimSoHocVienToiDa('P03')
go



-----------------------------------------------------CHỨC NĂNG XẾP LỊCH HỌC--------------------------------------------

--Function tạo bảng mới dựa trên ngày học
create or alter function uf_TaoBang (@thu1 int, @thu2 int, @thu3 int)
returns @container table (thu int)
As
begin
	insert into @container values (@thu1)
	insert into @container values (@thu2)
	insert into @container values (@thu3)
	return;
end
go

--Tìm GV đang có lịch trống dựa vào ngày và ca học đã chọn trước
create or alter procedure proc_GV_LichTrong(@thu1 int, @thu2 int, @thu3 int, @ca int)
as
begin
	select MaGiaoVien, HoTen
	from GIAOVIEN
	where not exists (Select * 
					from (select * from dbo.uf_LocTrungLich(@thu1, @thu2, @thu3, @ca))DS
					where GIAOVIEN.MaGiaoVien = DS.MaGiaoVien) 
end
go

exec dbo.proc_GV_LichTrong @thu1 = 2, @thu2 = 4, @thu3 = 6, @ca = 1
go

--Tìm phòng học đang có lịch trống dựa vào ngày và ca học đã chọn trước
create or alter procedure proc_PH_LichTrong(@thu1 int, @thu2 int, @thu3 int, @ca int)
as
begin
	select MaPhongHoc
	from PHONGHOC
	where not exists (Select * 
					from (select * from dbo.uf_LocTrungLich(@thu1, @thu2, @thu3, @ca))DS
					where PHONGHOC.MaPhongHoc = DS.MaPhongHoc) 
end
go

exec dbo.proc_PH_LichTrong @thu1 = 3, @thu2 = 5, @thu3 = 7, @ca = 1
go

--Kiểm tra GV/phòng học có đang có lịch vào ngày và ca học được chọn trước hay không
create or alter function uf_LocTrungLich(@thu1 int, @thu2 int, @thu3 int, @ca int)
returns @container table (MaGiaoVien varchar(20), MaPhongHoc varchar(20))
as
begin
	insert into @container
	select MaGiaoVien, MaPhongHoc
	from (Select thu from dbo.uf_TaoBang(@thu1, @thu2, @thu3))p join
		(SELECT dbo.NHOMHOC.MaNhomHoc, dbo.NHOMHOC.MaGiaoVien, dbo.GIAOVIEN.HoTen, dbo.NHOMHOC.MaPhongHoc, dbo.HOCVAO.ThuTrongTuan, dbo.NHOMHOC.Ca
		FROM dbo.HOCVAO INNER JOIN
			 dbo.NHOMHOC ON dbo.HOCVAO.MaNhomHoc = dbo.NHOMHOC.MaNhomHoc INNER JOIN
			 dbo.GIAOVIEN ON dbo.NHOMHOC.MaGiaoVien = dbo.GIAOVIEN.MaGiaoVien
		WHERE Ca = @ca)q on p.thu = q.ThuTrongTuan
	group by MaNhomHoc, MaGiaoVien, MaPhongHoc
	return;
end


-----------------PROCEDURES-----------------
---------------------------------------------------------TAI KHOAN----------------------------------------------------
-------------Them Tai Khoan------------------

CREATE or ALTER PROCEDURE proc_ThemTaiKhoan
	@MatKhau varchar(20),
	@QuyenNguoiDung varchar(20)
AS
BEGIN
	Begin Try
		INSERT INTO dbo.TAIKHOAN VALUES(default, @MatKhau, @QuyenNguoiDung)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemTaiKhoan @MatKhau = 'passhocvien', @QuyenNguoiDung ='1'
Select * from TAIKHOAN
go


--------------Xoa Tai Khoan------------------
CREATE OR ALTER PROC proc_XoaTaiKhoan
	@TenDangNhap varchar(20)
AS
BEGIN
	Begin try
		Delete from TAIKHOAN
		where TenDangNhap = @TenDangNhap
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaTaiKhoan @TenDangNhap = 'ACC022'
Select * from TAIKHOAN
go

--------------Cap nhat Tai Khoan--------------
CREATE or ALTER PROC proc_SuaTaiKhoan
	@TenDangNhap varchar(20),
	@MatKhau varchar(20)
AS
BEGIN
	Begin try
		UPDATE TAIKHOAN
		SET MatKhau = @MatKhau
		WHERE TenDangNhap = @TenDangNhap
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	End catch
END
GO

Exec dbo.proc_SuaTaiKhoan @TenDangNhap = 'ACC021', @MatKhau = 'passadmin'
Select * from TAIKHOAN
go



---------------GIAO VIEN---------------------
-------------Them Giao Vien------------------
CREATE or ALTER PROCEDURE proc_ThemGiaoVien
	@HoTen nvarchar(50),
	@NgaySinh DATE,
	@GioiTinh nvarchar(20),
	@DiaChi nvarchar(100),
	@SoDienThoai varchar(20),
	@Email varchar(50),
	@TenDangNhap varchar(20)
AS
BEGIN
	Begin Try
		Declare @QuyenNguoiDung varchar(20)
		SELECT @QuyenNguoiDung = QuyenNguoiDung
		FROM TAIKHOAN

		IF (@QuyenNguoiDung = '2')
			BEGIN
				INSERT INTO dbo.GIAOVIEN VALUES(default, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @Email, @TenDangNhap)
			END
		ELSE	
			BEGIN
				RAISERROR(N'ĐÂY KHÔNG PHẢI TÀI KHOẢN GIÁO VIÊN',16,1);
			END
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemGiaoVien @HoTen = N'Ngô Đình Trung', @NgaySinh = '1989-03-04', @GioiTinh  = N'Nam', @DiaChi = N'Gia Lai', @SoDienThoai = '0943834283', @Email = 'gvtrung@gmail.com', @TenDangNhap = 'ACC022'
Select * from GIAOVIEN
go

--------------Xoa Giao Vien------------------
CREATE OR ALTER PROC proc_XoaGiaoVien
	@MaGiaoVien varchar(20)
AS
BEGIN
	Begin try
		Delete from GIAOVIEN
		where MaGiaoVien = @MaGiaoVien
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaGiaoVien @MaGiaoVien = 'GV011'
Select * from GIAOVIEN
go

------------------Cap nhat Thong Tin Giao Vien------------------
CREATE or ALTER PROC proc_SuaThongTinGiaoVien
	@MaGiaoVien varchar(20),
	@HoTen nvarchar(50),
	@NgaySinh DATE,
	@GioiTinh nvarchar(20),
	@DiaChi nvarchar(100),
	@SoDienThoai varchar(20),
	@Email varchar(50)
AS
BEGIN
	Begin try
		UPDATE GIAOVIEN
		SET HoTen = @HoTen, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, DiaChi = @DiaChi, SoDienThoai = @SoDienThoai, Email = @Email
		WHERE MaGiaoVien = @MaGiaoVien
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	End catch
END
GO

Exec dbo.proc_SuaThongTinGiaoVien @MaGiaoVien = 'GV011', @HoTen = N'Nguyễn Tấn Lâm', @NgaySinh = '1989-03-04', @GioiTinh  = N'Nam', @DiaChi = N'Gia Lai', @SoDienThoai = '0943834283', @Email = 'gvtrung@gmail.com'
Select * from GIAOVIEN
go


---------------KHOA HOC---------------------
-------------Them Khoa Hoc------------------
CREATE or ALTER PROCEDURE proc_ThemKhoaHoc
	@MaKhoaHoc varchar(20),
	@TenKhoaHoc nvarchar(50)
AS
BEGIN
	Begin Try
		INSERT INTO dbo.KHOAHOC VALUES(@MaKhoaHoc, @TenKhoaHoc)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemKhoaHoc @MaKhoaHoc = 'CS', @TenKhoaHoc = N'Lập trình C#'
Select * from KHOAHOC
go

--------------Xoa Khoa Hoc------------------
CREATE OR ALTER PROC proc_XoaKhoaHoc
	@MaKhoaHoc varchar(20)
AS
BEGIN
	Begin try
		Delete from KHOAHOC
		where MaKhoaHoc = @MaKhoaHoc
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaKhoaHoc @MaKhoaHoc = 'CS'
Select * from KHOAHOC
go

--------------Cap nhat Khoa Hoc--------------
CREATE or ALTER PROC proc_SuaKhoaHoc
	@MaKhoaHoc varchar(20),
	@TenKhoaHoc nvarchar(50)
AS
BEGIN
	Begin try
		UPDATE KHOAHOC
		SET TenKhoaHoc = @TenKhoaHoc
		WHERE MaKhoaHoc = @MaKhoaHoc
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	End catch
END
GO

Exec dbo.proc_SuaKhoaHoc @MaKhoaHoc = 'CS', @TenKhoaHoc = N'Lập trình C#'
Select * from KHOAHOC
go

---------------LOP HOC---------------------
-------------Them Lop Hoc------------------
CREATE or ALTER PROCEDURE proc_ThemLopHoc
	@MaLopHoc varchar(20),
	@MaKhoaHoc varchar(20),
	@TenLopHoc nvarchar(50),
	@TongSoBuoiHoc INT,
	@HocPhi REAL
AS
BEGIN
	Begin Try
		INSERT INTO dbo.LOPHOC VALUES(@MaLopHoc, @MaKhoaHoc, @TenLopHoc, @TongSoBuoiHoc, @HocPhi)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemLopHoc @MaLopHoc = 'CS_CB', @MaKhoaHoc = 'CS', @TenLopHoc = N'Lập trình C# cơ bản', @TongSoBuoiHoc = 30, @HocPhi = 500000
Select * from LOPHOC
go

--------------Xoa Lop Hoc------------------
CREATE OR ALTER PROC proc_XoaLopHoc
	@MaLopHoc varchar(20)
AS
BEGIN
	Begin try
		Delete from LOPHOC
		where MaLopHoc = @MaLopHoc
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaLopHoc @MaLopHoc = 'CS_CB'
Select * from LOPHOC
go

--------------Cap nhat Lop Hoc--------------
CREATE or ALTER PROC proc_SuaLopHoc
	@MaLopHoc varchar(20),
	@TenLopHoc nvarchar(50),
	@TongSoBuoiHoc INT,
	@HocPhi REAL
AS
BEGIN
	Begin try
		UPDATE LOPHOC
		SET TenLopHoc = @TenLopHoc, TongSoBuoiHoc = @TongSoBuoiHoc, HocPhi = @HocPhi
		WHERE MaLopHoc = @MaLopHoc
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	End catch
END
GO

Exec dbo.proc_SuaLopHoc @MaLopHoc = 'CS_CB', @TenLopHoc = N'Lập trình C# cơ bản', @TongSoBuoiHoc = 45, @HocPhi  = 750000
Select * from LOPHOC
go

---------------PHONG HOC---------------------
-------------Them Phong Hoc------------------
CREATE or ALTER PROCEDURE proc_ThemPhongHoc
	@MaPhongHoc varchar(20),
	@SoLuongChoNgoi INT
AS
BEGIN
	Begin Try
		INSERT INTO dbo.PHONGHOC VALUES(@MaPhongHoc, @SoLuongChoNgoi)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemPhongHoc @MaPhongHoc = 'P06', @SoLuongChoNgoi = 45
Select * from PHONGHOC
go

--------------Xoa Phong Hoc------------------
CREATE OR ALTER PROC proc_XoaPhongHoc
	@MaPhongHoc varchar(20)
AS
BEGIN
	Begin try
		Delete from PHONGHOC
		where MaPhongHoc = @MaPhongHoc
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaPhongHoc @MaPhongHoc = 'P06'
Select * from PHONGHOC
go

--------------Cap nhat Phong Hoc--------------
CREATE or ALTER PROC proc_SuaPhongHoc
	@MaPhongHoc varchar(20),
	@SoLuongChoNgoi INT
AS
BEGIN
	Begin try
		UPDATE PHONGHOC
		SET SoLuongChoNgoi = @SoLuongChoNgoi
		WHERE MaPhongHoc = @MaPhongHoc
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	End catch
END
GO

Exec dbo.proc_SuaPhongHoc @MaPhongHoc = 'P05', @SoLuongChoNgoi = 45
Select * from PHONGHOC
go

---------------CA HOC---------------------
-------------Them Ca Hoc------------------
CREATE or ALTER PROCEDURE proc_ThemCaHoc
	@Ca INT,
	@GioBatDau varchar(10),
	@GioKetThuc varchar(10)
AS
BEGIN
	Begin Try
		INSERT INTO dbo.CAHOC VALUES(@Ca, @GioBatDau, @GioKetThuc)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemCaHoc @Ca = 5, @GioBatDau = '7', @GioKetThuc = '9'
Select * from CAHOC
go

--------------Xoa Ca Hoc------------------
CREATE OR ALTER PROC proc_XoaCaHoc
	@Ca INT
AS
BEGIN
	Begin try
		Delete from CAHOC
		where Ca = @Ca
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaCaHoc @Ca = 5
Select * from CAHOC
go

--------------Cap nhat Ca Hoc--------------
CREATE or ALTER PROC proc_SuaCaHoc
	@Ca INT,
	@GioBatDau varchar(10),
	@GioKetThuc varchar(10)
AS
BEGIN
	Begin try
		UPDATE CAHOC
		SET GioBatDau = @GioBatDau, GioKetThuc = @GioKetThuc
		WHERE Ca = @Ca
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	End catch
END
GO

Exec dbo.proc_SuaCaHoc @Ca = 1, @GioBatDau = '9', @GioKetThuc = '11'
Select * from CAHOC
go

---------------NHOM HOC---------------------
-------------Them Nhom Hoc------------------
CREATE or ALTER PROCEDURE proc_ThemNhomHoc
	@MaLopHoc varchar(20),
	@MaGiaoVien varchar(20),
	@MaPhongHoc varchar(20),
	@Ca INT,
	@SoLuongHocVienToiThieu INT,
	@SoLuongHocVienToiDa INT,
	@NgayBatDau DATE,
	@NgayKetThuc DATE
AS
BEGIN
	Begin Try
		INSERT INTO dbo.NHOMHOC VALUES(default, @MaLopHoc, @MaGiaoVien, @MaPhongHoc, @Ca, @SoLuongHocVienToiThieu, @SoLuongHocVienToiDa, @NgayBatDau, @NgayKetThuc, default, default)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemNhomHoc @MaLopHoc = 'CPP_NC', @MaGiaoVien = 'GV007', @MaPhongHoc = 'P04', @Ca = 3, @SoLuongHocVienToiThieu = 2, @SoLuongHocVienToiDa = 20, @NgayBatDau = '2023-11-06', @NgayKetThuc = '2023-12-01'
Select * from NHOMHOC
go

--------------Xoa Nhom Hoc------------------
CREATE OR ALTER PROC proc_XoaNhomHoc
	@MaNhomHoc varchar(20)
AS
BEGIN
	Begin try
		Delete from NHOMHOC
		where MaNhomHoc = @MaNhomHoc
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaNhomHoc @MaNhomHoc = 'JS_CB02'
Select * from NHOMHOC
go

----------------Cap nhat Nhom Hoc--------------
--CREATE or ALTER PROC proc_SuaNhomHoc
--	@MaNhomHoc varchar(20),
--	@TenHocVien nvarchar(50),
--	@NgaySinh DATE,
--	@GioiTinh nvarchar(20),
--	@DiaChi nvarchar(100),
--	@SoDienThoai varchar(20),
--	@CCCD varchar(50)
--AS
--BEGIN
--	Begin try
--		UPDATE HOCVIEN
--		SET TenHocVien = @TenHocVien, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, DiaChi = @DiaChi, SoDienThoai = @SoDienThoai, CCCD = @CCCD
--		WHERE MaHocVien = @MaHocVien
--	End try
--	Begin catch
--		declare @mess varchar(max)
--		set @mess=ERROR_MESSAGE()
--		Raiserror(@mess, 16, 1)
--	End catch
--END
--GO

--Exec dbo.proc_SuaThongTinHocVien @MaHocVien = 'HV011', @TenHocVien = N'Nguyễn Tấn Lâm', @NgaySinh = '1989-03-04', @GioiTinh  = N'Nam', @DiaChi = N'Gia Lai', @SoDienThoai = '0943834283', @CCCD = '458209041923'
--Select * from GIAOVIEN
go

---------------HOC VIEN---------------------
-------------Them Hoc Vien------------------
CREATE or ALTER PROCEDURE proc_ThemHocVien
	@TenHocVien nvarchar(50),
	@NgaySinh DATE,
	@GioiTinh nvarchar(20),
	@DiaChi nvarchar(100),
	@SoDienThoai varchar(20),
	@CCCD varchar(50),
	@TenDangNhap varchar(20)
AS
BEGIN
	Begin Try
		Declare @QuyenNguoiDung varchar(20)
		SELECT @QuyenNguoiDung = QuyenNguoiDung
		FROM TAIKHOAN

		IF (@QuyenNguoiDung = '1')
			BEGIN
				INSERT INTO dbo.HOCVIEN VALUES(default, @TenHocVien, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @CCCD, @TenDangNhap)
			END
		ELSE	
			BEGIN
				RAISERROR(N'ĐÂY KHÔNG PHẢI TÀI KHOẢN HỌC VIÊN',16,1);
			END
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemHocVien @TenHocVien = N'Ngô Đình Trung', @NgaySinh = '1989-03-04', @GioiTinh  = N'Nam', @DiaChi = N'Gia Lai', @SoDienThoai = '0943834283', @CCCD = '458209041923', @TenDangNhap = 'ACC023'
Select * from HOCVIEN
go

--------------Xoa Hoc Vien------------------
CREATE OR ALTER PROC proc_XoaHocVien
	@MaHocVien varchar(20)
AS
BEGIN
	Begin try
		Delete from HOCVIEN
		where MaHocVien = @MaHocVien
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaHocVien @MaHocVien = 'HV010'
Select * from HOCVIEN
go

--------------Cap nhat Thong Tin Hoc Vien--------------
CREATE or ALTER PROC proc_SuaThongTinHocVien
	@MaHocVien varchar(20),
	@TenHocVien nvarchar(50),
	@NgaySinh DATE,
	@GioiTinh nvarchar(20),
	@DiaChi nvarchar(100),
	@SoDienThoai varchar(20),
	@CCCD varchar(50)
AS
BEGIN
	Begin try
		UPDATE HOCVIEN
		SET TenHocVien = @TenHocVien, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, DiaChi = @DiaChi, SoDienThoai = @SoDienThoai, CCCD = @CCCD
		WHERE MaHocVien = @MaHocVien
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	End catch
END
GO

Exec dbo.proc_SuaThongTinHocVien @MaHocVien = 'HV011', @TenHocVien = N'Nguyễn Tấn Lâm', @NgaySinh = '1989-03-04', @GioiTinh  = N'Nam', @DiaChi = N'Gia Lai', @SoDienThoai = '0943834283', @CCCD = '458209041923'
Select * from GIAOVIEN
go

---------------THONG BAO---------------------
-------------Them Thong Bao------------------
CREATE or ALTER PROCEDURE proc_ThemThongBao
	@MaGiaoVien varchar(20),
	@TieuDe nvarchar(50),
	@NoiDung nvarchar(255)
AS
BEGIN
	Begin Try
		INSERT INTO dbo.THONGBAO VALUES(default, @MaGiaoVien, @TieuDe, @NoiDung, default, default)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemThongBao @MaGiaoVien = 'GV001', @TieuDe = N'Hello', @NoiDung = N'Hello, I am GV001.'
Select * from THONGBAO
go

--------------Xoa Thong Bao------------------
CREATE OR ALTER PROC proc_XoaThongBao
	@MaThongBao varchar(20)
AS
BEGIN
	Begin try
		Delete from THONGBAO
		where MaThongBao = @MaThongBao
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaThongBao @MaThongBao = 'TB001'
Select * from THONGBAO
go

--------------Cap nhat Thong Bao--------------
CREATE or ALTER PROC proc_SuaThongBao
	@MaThongBao varchar(20),
	@TieuDe nvarchar(50),
	@NoiDung nvarchar(255)
AS
BEGIN
	Begin try
		UPDATE THONGBAO
		SET TieuDe = @TieuDe, NoiDung = @NoiDung
		WHERE MaThongBao = @MaThongBao
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	End catch
END
GO

Exec dbo.proc_SuaThongBao @MaThongBao = 'TB001', @TieuDe = 'HELLO', @NoiDung = 'Hello, I am GV001'
Select * from THONGBAO
go

---------------NGAY HOC---------------------
-------------Them Ngay Hoc------------------
CREATE or ALTER PROCEDURE proc_ThemNgayHoc
	@NgayHoc DATE
AS
BEGIN
	Begin Try
		INSERT INTO dbo.NGAYHOC VALUES(@NgayHoc)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemNgayHoc @NgayHoc = '2023-11-01'
Select * from NGAYHOC
go

--------------Xoa Phong Hoc------------------
CREATE OR ALTER PROC proc_XoaNgayHoc
	@NgayHoc DATE
AS
BEGIN
	Begin try
		Delete from NGAYHOC
		where NgayHoc = @NgayHoc
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaNgayHoc @NgayHoc = '2023-11-01'
Select * from NGAYHOC
go

--------------Cap nhat Ngay Hoc--------------

---------------THU TRONG TUAN---------------------
-------------Them Thu Trong Tuan------------------
CREATE or ALTER PROCEDURE proc_ThemThuTrongTuan
	@ThuTrongTuan nvarchar(20)
AS
BEGIN
	Begin Try
		INSERT INTO dbo.THUTRONGTUAN VALUES(@ThuTrongTuan)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemThuTrongTuan @ThuTrongTuan = 'CN'
Select * from THUTRONGTUAN
go

--------------Xoa Thu Trong Tuan------------------
CREATE OR ALTER PROC proc_XoaThuTrongTuan
	@ThuTrongTuan nvarchar(20)
AS
BEGIN
	Begin try
		Delete from THUTRONGTUAN
		where ThuTrongTuan = @ThuTrongTuan
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaThuTrongTuan @ThuTrongTuan = 'CN'
Select * from THUTRONGTUAN
go

--------------Cap nhat Thu Hoc--------------


---------------HOC VAO---------------------
-------------Them Hoc Vao------------------
CREATE or ALTER PROCEDURE proc_ThemHocVao
	@MaNhomHoc varchar(20),
	@ThuTrongTuan nvarchar(20)
AS
BEGIN
	Begin Try
		INSERT INTO dbo.HOCVAO VALUES(@MaNhomHoc, @ThuTrongTuan)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemHocVao @MaNhomHoc = 'CPP_NC02', @ThuTrongTuan = '2'
Select * from HOCVAO
go

--------------Xoa Hoc Vao------------------
CREATE OR ALTER PROC proc_XoaHocVao
	@MaNhomHoc varchar(20)
AS
BEGIN
	Begin try
		Delete from HOCVAO
		where MaNhomHoc = @MaNhomHoc
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaHocVao @MaNhomHoc = 'CPP_NC02'
Select * from HOCVAO
go

--------------Cap nhat Hoc Vao--------------

---------------DANH SACH NHOM---------------------
-------------Them Danh Sach Nhom------------------
CREATE or ALTER PROCEDURE proc_ThemDanhSachNhom
	@MaNhomHoc varchar(20),
	@MaHocVien varchar(20)
AS
BEGIN
	Begin Try
		INSERT INTO dbo.DANHSACHNHOM VALUES(@MaNhomHoc, @MaHocVien, default, default, default, default)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemDanhSachNhom @MaNhomHoc = 'JS_CB03', @MaHocVien = 'HV005'
Select * from DANHSACHNHOM
go

--------------Xoa Danh Sach Nhom------------------
CREATE OR ALTER PROC proc_XoaDanhSachNhom
	@MaHocVien varchar(20),
	@MaNhomHoc varchar(20)
AS
BEGIN
	Begin try
		Delete from DANHSACHNHOM
		where MaHocVien = @MaHocVien and MaNhomHoc = @MaNhomHoc
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaDanhSachNhom @MaHocVien = 'HV005', @MaNhomHoc = 'JS_CB03'
Select * from DANHSACHNHOM
go

--------------Cap nhat Danh Sach Nhom--------------
CREATE or ALTER PROC proc_SuaDanhSachNhom
	@MaHocVien varchar(20),
	@MaNhomHoc varchar(20),
	@DiemLyThuyet REAL,
	@DiemThucHanh REAL,
	@TrangThaiThanhToan BIT
AS
BEGIN
	Begin try
		UPDATE DANHSACHNHOM
		SET DiemLyThuyet = @DiemLyThuyet, DiemThucHanh = @DiemThucHanh, TrangThaiThanhToan = @TrangThaiThanhToan
		WHERE MaHocVien = @MaHocVien and MaNhomHoc = @MaNhomHoc
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	End catch
END
GO

Exec dbo.proc_SuaDanhSachNhom @MaHocVien = 'HV001', @MaNhomHoc = 'CPP_CB01', @DiemLyThuyet = 10, @DiemThucHanh  = 10, @TrangThaiThanhToan = 1
Select * from DANHSACHNHOM
go

---------------TRUYEN TIN---------------------
-------------Them Truyen Tin------------------
CREATE or ALTER PROCEDURE proc_ThemTruyenTin
	@MaThongBao varchar(20),
	@MaNhomHoc varchar(20)
AS
BEGIN
	Begin Try
		INSERT INTO dbo.TRUYENTIN VALUES(@MaThongBao, @MaNhomHoc)
	End try
	Begin catch
		declare @mess varchar(max)
		set @mess=ERROR_MESSAGE()
		Raiserror(@mess, 16, 1)
	end catch
END
GO

Exec dbo.proc_ThemTruyenTin @MaThongBao = 'TB002', @MaNhomHoc = 'CPP_CB01'
Select * from THONGBAO
Select * from TRUYENTIN
go

--------------Xoa Truyen Tin------------------
CREATE OR ALTER PROC proc_XoaTruyenTin
	@MaThongBao varchar(20),
	@MaNhomHoc varchar(20)
AS
BEGIN
	Begin try
		Delete from TRUYENTIN
		where MaNhomHoc = @MaNhomHoc and MaThongBao = @MaThongBao
	End try
	Begin catch
		print N'Không xóa được'
		print ERROR_MESSAGE()
		Rollback Tran delete_Empl
	End catch
END
GO

Exec dbo.proc_XoaTruyenTin @MaThongBao = 'TB002', @MaNhomHoc = 'CPP_CB01'
Select * from TRUYENTIN
go

--------------Cap nhat Truyen Tin--------------

--------------GỌI DANH SÁCH NHÓM HỌC--------------
CREATE or ALTER PROC proc_LayDanhSachNhom
AS
BEGIN
	SELECT dbo.NHOMHOC.*, dbo.LOPHOC.TenLopHoc, dbo.GIAOVIEN.HoTen as TenGiaoVien
	FROM     dbo.LOPHOC INNER JOIN
                  dbo.NHOMHOC ON dbo.LOPHOC.MaLopHoc = dbo.NHOMHOC.MaLopHoc INNER JOIN
                  dbo.GIAOVIEN ON dbo.NHOMHOC.MaGiaoVien = dbo.GIAOVIEN.MaGiaoVien
END
GO

SELECT TOP 1 * FROM THONGBAO ORDER BY MaThongBao DESC

SELECT * FROM NHOMHOC
SELECT * FROM HOCVAO
SELECT * FROM THONGBAO
SELECT * FROM TRUYENTIN
go

--Lấy thông báo mới nhất được gửi đến học viên
CREATE OR ALTER PROC proc_LayThongBaoMoiNhat 
	@MaHocVien varchar(20)
AS
BEGIN
	SELECT TOP 1 * 
	FROM (SELECT dbo.THONGBAO.MaThongBao, dbo.THONGBAO.MaGiaoVien, dbo.GIAOVIEN.HoTen, dbo.TRUYENTIN.MaNhomHoc, dbo.DANHSACHNHOM.MaHocVien, dbo.THONGBAO.TieuDe, dbo.THONGBAO.NoiDung, dbo.THONGBAO.NgayGui, dbo.THONGBAO.GioGui
		FROM   dbo.DANHSACHNHOM INNER JOIN
			   dbo.NHOMHOC ON dbo.DANHSACHNHOM.MaNhomHoc = dbo.NHOMHOC.MaNhomHoc INNER JOIN
			   dbo.GIAOVIEN INNER JOIN
			   dbo.THONGBAO ON dbo.GIAOVIEN.MaGiaoVien = dbo.THONGBAO.MaGiaoVien INNER JOIN
			   dbo.TRUYENTIN ON dbo.THONGBAO.MaThongBao = dbo.TRUYENTIN.MaThongBao ON dbo.NHOMHOC.MaGiaoVien = dbo.GIAOVIEN.MaGiaoVien AND dbo.NHOMHOC.MaNhomHoc = dbo.TRUYENTIN.MaNhomHoc
		WHERE MaHocVien = @MaHocVien)q
	ORDER BY MaThongBao DESC
END
go

EXEC dbo.proc_LayThongBaoMoiNhat @MaHocVien = 'HV002'
go


----------------- function tim  manhomhoc, cahoc, va thuhoc va hoc vien muon chuyen den

CREATE OR ALTER FUNCTION uf_NhomHocHocVienMuonChuyenDen(@MaNhomHoc varchar(100))
RETURNs TABLE
	AS RETURN 
		SELECT dbo.NHOMHOC.MaNhomHoc, dbo.HOCVAO.ThuTrongTuan, dbo.NHOMHOC.Ca
			FROM dbo.NHOMHOC INNER JOIN dbo.HOCVAO ON dbo.NHOMHOC.MaNhomHoc = dbo.HOCVAO.MaNhomHoc
				WHERE NHOMHOC.MaNhomHoc = @MaNhomHoc
GO

CREATE OR ALTER FUNCTION uf_KhoaHocCuaHocVien(@MaHocVien varchar(100))
RETURNs TABLE
AS RETURN 
			SELECT        dbo.DANHSACHNHOM.MaNhomHoc, dbo.HOCVAO.ThuTrongTuan, dbo.NHOMHOC.Ca
			FROM            dbo.DANHSACHNHOM INNER JOIN
									 dbo.NHOMHOC ON dbo.DANHSACHNHOM.MaNhomHoc = dbo.NHOMHOC.MaNhomHoc INNER JOIN
									 dbo.HOCVIEN ON dbo.DANHSACHNHOM.MaHocVien = dbo.HOCVIEN.MaHocVien INNER JOIN
									 dbo.HOCVAO ON dbo.NHOMHOC.MaNhomHoc = dbo.HOCVAO.MaNhomHoc
										WHERE HOCVIEN.MaHocVien =@MaHocVien
GO



------------------------------trigger kiểm tra trùng lịch học hay không ---------------------------------
drop trigger TG_KiemTraTrungNhomHocKhiChuyenNhom
CREATE OR ALTER TRIGGER TG_KiemTraTrungNhomHocKhiChuyenNhom
ON DANHSACHNHOM
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MaNhomHoc varchar(20), @MaHocVien varchar(20), @DiemLyThuyet REAL, @DiemThucHanh REAL, @TrangThaiThanhToan BIT, @TrangThaiCapChungChi BIT;

    -- Lấy giá trị từ bảng INSERTED
    SELECT @MaNhomHoc = MaNhomHoc, @MaHocVien = MaHocVien, @DiemLyThuyet = DiemLyThuyet,
           @DiemThucHanh = DiemThucHanh, @TrangThaiThanhToan = TrangThaiThanhToan, @TrangThaiCapChungChi = TrangThaiCapChungChi
    FROM INSERTED;

    -- Kiểm tra sự trùng lịch học sử dụng hai function
    IF EXISTS (
        SELECT 1
        FROM uf_KhoaHocCuaHocVien(@MaHocVien) kh
        INNER JOIN uf_NhomHocHocVienMuonChuyenDen(@MaNhomHoc) nh
        ON kh.Ca = nh.Ca AND kh.ThuTrongTuan = nh.ThuTrongTuan
    )
    BEGIN
        -- Nếu có trùng lịch học, thông báo lỗi hoặc thực hiện xử lý khác nếu cần
        RAISERROR('Không thể chèn DanhSachNhom vì trùng lịch học', 16, 1);
    END
    ELSE
    BEGIN
        -- Nếu không có trùng lịch học, thực hiện INSERT vào bảng DANHSACHNHOM
        INSERT INTO DANHSACHNHOM (MaNhomHoc, MaHocVien, DiemLyThuyet, DiemThucHanh, TrangThaiThanhToan, TrangThaiCapChungChi)
        VALUES (@MaNhomHoc, @MaHocVien, @DiemLyThuyet, @DiemThucHanh, @TrangThaiThanhToan, @TrangThaiCapChungChi);
    END
END


--------------------------------------------------------TẾT-------------------------------------------------

-- TEST LỊCH HỌC
INSERT INTO DANHSACHNHOM(MaNhomHoc, MaHocVien) VALUES
('JS_NC01', 'HV002');

-- TEST TRÙNG LỚP
INSERT INTO DANHSACHNHOM(MaNhomHoc, MaHocVien) VALUES
('JS_NC01', 'HV001');
INSERT INTO DANHSACHNHOM(MaNhomHoc, MaHocVien) VALUES
('CPP_CB01', 'HV002');
INSERT INTO DANHSACHNHOM(MaNhomHoc, MaHocVien) VALUES
('JS_NC01', 'HV003');


-- TỔNG HỌC VIÊN TRONG NHÓM HỌC CHUYỂN VỀ 0
UPDATE NHOMHOC SET TongHocVien = 0 WHERE MaNhomHoc = 'JS_NC01'
UPDATE NHOMHOC SET TongHocVien = 0 WHERE MaNhomHoc = 'CPP_CB01'
UPDATE NHOMHOC SET TongHocVien = 0 WHERE MaNhomHoc = 'CPP_NC01'
UPDATE NHOMHOC SET TongHocVien = 0 WHERE MaNhomHoc = 'JS_CB0'	
------------------------------------------------------------------------------
delete from DANHSACHNHOM where MaHocVien = 'HV001' and MaNhomHoc = 'JS_NC01'



drop trigger TG_KiemTraSoLuongHV
drop trigger TG_KiemTraDieuKienCapChungChi

select *from NHOMHOC 

select *from HOCVAO
SELECT *FROM DANHSACHNHOM

SELECT name, 4
FROM sys.triggers
WHERE parent_id = OBJECT_ID('DANHSACHNHOM');




SELECT name, execution_sequence 
FROM sys.triggers
WHERE parent_id = OBJECT_ID('DANHSACHNHOM')
ORDER BY execution_sequence ;
GO

CREATE OR ALTER FUNCTION uf_DemLichTrung(@MaHocVien varchar(20), @MaNhomHoc varchar(20))
RETURNS TABLE
AS RETURN
		SELECT nh.MaNhomHoc, nh.ThuTrongTuan, nh.Ca
        FROM uf_KhoaHocCuaHocVien(@MaHocVien) kh
        INNER JOIN uf_NhomHocHocVienMuonChuyenDen(@MaNhomHoc) nh
        ON kh.Ca = nh.Ca AND kh.ThuTrongTuan = nh.ThuTrongTuan
GO
DECLARE @c int = 0;
select @c = count(*) from dbo.uf_DemLichTrung('HV002', 'CPP_CB01')
print @c
SELECT *
        FROM uf_KhoaHocCuaHocVien('HV002') kh
        INNER JOIN uf_NhomHocHocVienMuonChuyenDen('CPP_CB01') nh
        ON kh.Ca = nh.Ca AND kh.ThuTrongTuan = nh.ThuTrongTuan