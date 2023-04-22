CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO

-- Food
-- Table
-- Food Category
-- Account
-- Bill
-- Bill Infor

CREATE TABLE FoodCategory
(
	ID INT IDENTITY PRIMARY KEY,
	Name NVARCHAR (100) NOT NULL DEFAULT N'Chưa đặt tên',
)
GO

CREATE TABLE Food
(
	ID INT IDENTITY PRIMARY KEY,
	Name NVARCHAR (100) NOT NULL DEFAULT N'Chưa đặt tên',
	IDCategory INT NOT NULL FOREIGN KEY REFERENCES dbo.FoodCategory (ID),
	Price FLOAT NOT NULL DEFAULT 0
)
GO

CREATE TABLE TableFood
(
	ID INT IDENTITY PRIMARY KEY,
	Name NVARCHAR (100) NOT NULL DEFAULT N'Bàn chưa có tên',
	Stat NVARCHAR (100) NOT NULL DEFAULT N'Trống' -- Trống | Có người
)
GO

CREATE TABLE Bill
(
	ID INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE (),
	DateCheckOut DATE,
	IDTable INT NOT NULL FOREIGN KEY REFERENCES dbo.TableFood (ID),
	Stat INT NOT NULL DEFAULT 0, -- 1: đã thanh toán | 0: chưa thanh toán
	Discount INT DEFAULT 0
)
GO

-- Sửa tên cột
--EXEC sp_rename 'dbo.Bill.Status','Stat';

CREATE TABLE BillInfor
(
	ID INT IDENTITY PRIMARY KEY,
	IDBill INT NOT NULL FOREIGN KEY REFERENCES dbo.Bill (ID),
	IDFood INT NOT NULL FOREIGN KEY REFERENCES dbo.Food (ID),
	Count INT NOT NULL DEFAULT 0
)
GO

CREATE TABLE Account
(
	UserName NVARCHAR (100) NOT NULL PRIMARY KEY,
	DisplayName NVARCHAR (100) NOT NULL DEFAULT 'HTT',
	Password NVARCHAR (100) NOT NULL DEFAULT 0,
	Type INT NOT NULL DEFAULT 0 -- 1: admin | 0: staff
)
GO

-- Thêm dữ liệu vào bảng Account
INSERT INTO dbo.Account (UserName, DisplayName, Password, Type) VALUES (N'htt',	N'Huynh Tan Thanh', N'admin', 1)
INSERT INTO dbo.Account (UserName, DisplayName, Password, Type) VALUES (N'staff',	N'Nhan vien',			N'staff', 0)
GO

CREATE PROC USP_GetAccountByUserName
	@userName NVARCHAR (100)
AS
	SELECT *
	FROM dbo.Account
	WHERE UserName = @userName
	RETURN 1
GO

--EXEC USP_GetAccountByUserName N'htt'

CREATE PROC USP_Login
	@userName NVARCHAR (100),
	@password NVARCHAR (100)
AS
	SELECT *
	FROM dbo.Account
	WHERE UserName = @userName AND Password = @password
GO

--EXEC USP_Login 'htt', 'admin'

-- Thêm dữ liệu tự động vào bảng TableFood
DECLARE @i SMALLINT = 1
DECLARE @temp NVARCHAR (100)
WHILE @i <= 10
BEGIN
	SET @temp = N'Bàn ' + CAST (@i AS NVARCHAR(100))
	INSERT INTO dbo.TableFood (Name) VALUES (@temp)
	SET @i += 1
END
GO

--DROP TABLE dbo.Food
--DROP TABLE dbo.FoodCategory
--DROP TABLE dbo.BillInfor
--DROP TABLE dbo.Bill
--DROP TABLE dbo.TableFood

CREATE PROC USP_GetTableList
AS
	SELECT *
	FROM dbo.TableFood
GO

--EXEC USP_GetTableList

UPDATE dbo.TableFood SET Stat = N'Có người' WHERE ID = 5

-- Thêm dữ liệu vào bảng FoodCategory
INSERT INTO dbo.FoodCategory (Name) VALUES (N'Hải sản')
INSERT INTO dbo.FoodCategory (Name) VALUES (N'Nông sản')
INSERT INTO dbo.FoodCategory (Name) VALUES (N'Lâm sản')
INSERT INTO dbo.FoodCategory (Name) VALUES (N'Đặc sản')
INSERT INTO dbo.FoodCategory (Name) VALUES (N'Nước')

--UPDATE dbo.FoodCategory SET Name = N'Đặc sản' WHERE ID = 4

-- Thêm dữ liệu vào bảng FoodCategory
INSERT INTO dbo.Food (Name, IDCategory, Price) VALUES (N'Mực một nắng nướng sa tế', 1, 120000)
INSERT INTO dbo.Food (Name, IDCategory, Price) VALUES (N'Nghêu hấp xả', 1, 50000)
INSERT INTO dbo.Food (Name, IDCategory, Price) VALUES (N'Dú dê nướng sữa', 2, 60000)
INSERT INTO dbo.Food (Name, IDCategory, Price) VALUES (N'Heo rừng nướng muối ớt', 3, 75000)
INSERT INTO dbo.Food (Name, IDCategory, Price) VALUES (N'Cơm chiên mushi', 4, 99999)
INSERT INTO dbo.Food (Name, IDCategory, Price) VALUES (N'7Up', 5, 15000)
INSERT INTO dbo.Food (Name, IDCategory, Price) VALUES (N'Cafe', 5, 12000)

--UPDATE dbo.Food SET Name = N'Cơm chiên Dương Châu' WHERE ID = 5

-- Thêm dữ liệu vào bảng Bill
INSERT INTO dbo.Bill (DateCheckOut, IDTable, Stat) VALUES (NULL, 1, 0)
INSERT INTO dbo.Bill (DateCheckOut, IDTable, Stat) VALUES (NULL, 2, 0)
INSERT INTO dbo.Bill (DateCheckOut, IDTable, Stat) VALUES (GETDATE (), 2, 1)

-- Thêm dữ liệu vào bảng BillInfor
INSERT INTO dbo.BillInfor (IDBill, IDFood, Count) VALUES (1, 1, 2)
INSERT INTO dbo.BillInfor (IDBill, IDFood, Count) VALUES (1, 3, 4)
INSERT INTO dbo.BillInfor (IDBill, IDFood, Count) VALUES (1, 5, 1)
INSERT INTO dbo.BillInfor (IDBill, IDFood, Count) VALUES (2, 1, 2)
INSERT INTO dbo.BillInfor (IDBill, IDFood, Count) VALUES (2, 6, 2)
INSERT INTO dbo.BillInfor (IDBill, IDFood, Count) VALUES (3, 5, 2)
GO

CREATE PROC USP_InsertBill
	@IDTable INT,
	@Discount INT
AS
	INSERT INTO dbo.Bill (IDTable, Discount) VALUES (@IDTable, @Discount)
GO

CREATE PROC USP_InsertBillInfor
	@IDBill INT,
	@IDFood INT,
	@Count INT
AS
	DECLARE @foodCount INT = 1
	DECLARE @isExistBillInfor INT

	SELECT @isExistBillInfor = ID, @foodCount = A.Count
	FROM dbo.BillInfor A
	WHERE IDBill = @IDBill AND IDFood = @IDFood

	IF @isExistBillInfor > 0
	BEGIN
		DECLARE @newCount INT = @foodCount + @Count
		IF @newCount > 0
			UPDATE dbo.BillInfor SET Count = @foodCount + @Count WHERE IDFood = @IDFood
		
		ELSE
			DELETE dbo.BillInfor WHERE IDBill = @IDBill AND IDFood = @IDFood
		UPDATE dbo.BillInfor SET Count = @foodCount + @Count WHERE IDBill = @IDBill AND IDFood = @IDFood
	END

	ELSE
	BEGIN
		INSERT INTO dbo.BillInfor (IDBill, IDFood, Count) VALUES (@IDBill, @IDFood, @Count)
	END
GO

CREATE TRIGGER UTG_UpdateBillInfor
ON dbo.BillInfor FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @IDBill INT

	SELECT @IDBill = IDBill FROM INSERTED

	DECLARE @IDTable INT

	SELECT @IDTable = IDTable FROM dbo.Bill WHERE ID = @IDBill AND Stat = 0

	DECLARE @count INT = (SELECT COUNT (*) FROM dbo.BillInfor WHERE IDBill = @IDBill)

	IF @count > 0
	BEGIN
		UPDATE dbo.TableFood SET Stat = N'Có người' WHERE ID = @IDTable
	END

	ELSE
	BEGIN
		UPDATE dbo.TableFood SET Stat = N'Trống' WHERE ID = @IDTable
	END

END
GO

--CREATE TRIGGER UTG_UpdateTable
--ON dbo.TableFood FOR UPDATE
--AS
--BEGIN
--	DECLARE @IDTable INT
--	DECLARE @status NVARCHAR (50)
--	SELECT @IDTable = ID, @status = Stat FROM INSERTED

--	DECLARE @IDBill INT
--	SELECT @IDBill = ID FROM dbo.Bill WHERE IDTable = @IDTable AND Stat = 0

--	DECLARE @countBillInfor INT
--	SELECT @countBillInfor = COUNT (*) FROM dbo.BillInfor WHERE idBill = @IDBill

--	IF @countBillInfor > 0 AND @status <> N'Có người'
--	BEGIN
--		UPDATE dbo.TableFood SET Stat = N'Có người' WHERE ID = @IDTable
--	END
--	ELSE IF @countBillInfor < 0 AND @status <> N'Trống'
--	BEGIN
--		UPDATE dbo.TableFood SET Stat = N'Trống' WHERE ID = @IDTable
--	END
--END
--GO

CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
	DECLARE @IDBill INT
	SELECT @IDBill = ID FROM INSERTED

	DECLARE @IDTable INT
	SELECT @IDTable = IDTable FROM dbo.Bill WHERE ID = @IDBill

	DECLARE @count INT = 0
	SELECT @count = COUNT (*) FROM dbo.Bill WHERE IDTable = @IDTable AND Stat = 0

	IF @count = 0
	BEGIN
		UPDATE dbo.TableFood SET Stat = N'Trống' WHERE ID = @IDTable
	END
END
GO

--ALTER TABLE dbo.Bill ADD Discount INT DEFAULT 0
--UPDATE dbo.Bill SET Discount = 0

CREATE PROC USP_SwitchTable
	@IDTable1 INT,
	@IDTable2 INT
AS
BEGIN
	DECLARE @IDFirstBill INT
	DECLARE @IDSecondBill INT

	DECLARE @isFirstTableEmpty INT = 1
	DECLARE @isSecondTableEmpty INT = 1

	SELECT @IDFirstBill = ID FROM dbo.Bill WHERE IDTable = @IDTable1 AND Stat = 0
	SELECT @IDSecondBill = ID FROM dbo.Bill WHERE IDTable = @IDTable2 AND Stat = 0

	IF @IDFirstBill IS NULL
	BEGIN
		INSERT INTO dbo.Bill (DateCheckOut, IDTable, Stat) VALUES (NULL, @IDTable1, 0)
		SELECT @IDFirstBill = MAX (ID) FROM dbo.Bill WHERE IDTable = @IDTable1 AND Stat = 0

		SET @isFirstTableEmpty = 1
	END

	SELECT @isFirstTableEmpty = COUNT (*) FROM dbo.BillInfor WHERE IDBill = @IDFirstBill

	IF @IDSecondBill IS NULL
	BEGIN
		INSERT INTO dbo.Bill (DateCheckOut, IDTable, Stat) VALUES (NULL, @IDTable2, 0)
		SELECT @IDFirstBill = MAX (ID) FROM dbo.Bill WHERE IDTable = @IDTable2 AND Stat = 0

		SET @isSecondTableEmpty = 1
	END

	SELECT @isSecondTableEmpty = COUNT (*) FROM dbo.BillInfor WHERE IDBill = @IDSecondBill
	
	SELECT ID INTO IDBillInforTable FROM dbo.BillInfor WHERE IDBill = @IDSecondBill

	UPDATE  dbo.BillInfor SET IDBill = @IDSecondBill WHERE IDBill = @IDFirstBill
	UPDATE dbo.BillInfor SET IDBill = @IDFirstBill WHERE ID IN (SELECT * FROM dbo.IDBillInforTable)

	DROP TABLE dbo.IDBillInforTable

	IF @isFirstTableEmpty = 0
	BEGIN
		UPDATE dbo.TableFood SET Stat = N'Trống' WHERE ID = @IDTable2
	END

	IF @isSecondTableEmpty = 0
	BEGIN
		UPDATE dbo.TableFood SET Stat = N'Trống' WHERE ID = @IDTable1
	END
END
GO

CREATE FUNCTION USF_GetTotalPriceByIDBill(@IDBill INT)
RETURNS TABLE
AS
RETURN
(
	SELECT (D.Price * C.Count) - ((D.Price * C.Count) / 100 * B.Discount) AS ThanhTien
	FROM dbo.TableFood A, dbo.Bill B, dbo.BillInfor C, dbo.Food D
	WHERE B.Stat = 1 AND A.ID = B.IDTable AND B.ID = C.IDBill AND C.IDFood = D.ID AND C.IDBill = @IDBill
);
GO

CREATE FUNCTION USF_GetSumTotalPrice(@IDBill INT)
RETURNS FLOAT
AS
BEGIN
	RETURN (SELECT SUM (A.ThanhTien) FROM dbo.USF_GetTotalPriceByIDBill(@IDBill) A)
END
GO

CREATE PROC USP_GetListBillByDate
	@dateCheckIn DATE,
	@dateCheckOut DATE
AS
BEGIN
	SELECT A.Name AS [Tên bàn], DateCheckIn AS [Ngày đặt], DateCheckOut AS [Ngày thanh toán], Discount AS [Giảm giá], dbo.USF_GetSumTotalPrice(B.ID) AS [Tổng tiền]
	FROM dbo.TableFood A, dbo.Bill B
	WHERE DateCheckIn >= @dateCheckIn AND DateCheckOut <= @dateCheckOut AND B.Stat = 1 AND A.ID = B.IDTable
END
GO

CREATE PROC USP_UpdateAccount
	@UserName NVARCHAR (100),
	@DisplayName NVARCHAR (100),
	@Password NVARCHAR (100),
	@newPassword NVARCHAR (100)
AS
BEGIN
	DECLARE @isRightPasswrod INT = (SELECT COUNT (*) FROM dbo.Account WHERE UserName = @UserName AND Password = @Password)

	IF @isRightPasswrod > 0
	BEGIN
		IF @newPassword IS NULL OR @newPassword = ''
		BEGIN
			UPDATE dbo.Account SET DisplayName = @DisplayName WHERE UserName = @UserName
		END
		ELSE
		BEGIN
			UPDATE dbo.Account SET DisplayName = @DisplayName, Password = @newPassword WHERE UserName = @UserName AND Password = @Password
		END
	END
END
GO

CREATE TRIGGER UTG_DeleteBillInfor
ON dbo.BillInfor FOR DELETE
AS
BEGIN
	DECLARE @IDBillInfor INT
	DECLARE @IDBill INT
	SELECT @IDBillInfor = ID, @IDBill = DELETED.IDBill FROM DELETED

	DECLARE @IDTable INT
	SELECT @IDTable = IDTable FROM dbo.Bill WHERE ID = @IDBill

	DECLARE @count INT = 0
	SELECT @count = COUNT (*) FROM dbo.BillInfor A, dbo.Bill B WHERE A.IDBill = B.ID AND B.ID = @IDBill AND B.Stat = 0
		
	IF @count = 0
	BEGIN
	UPDATE dbo.TableFood SET Stat = N'Trống' WHERE ID = @IDTable
	END
END
GO

CREATE FUNCTION USF_ConvertToUnsign(@strInput NVARCHAR (4000))
RETURNS NVARCHAR(4000)
AS
BEGIN
	IF @strInput IS NULL
		RETURN @strInput

	IF @strInput = ''
		RETURN @strInput

	DECLARE @RT NVARCHAR (4000)
	DECLARE @SIGN_CHARS NCHAR (136)
	DECLARE @UNSIGN_CHARS NCHAR (136)
	
	SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' + NCHAR (272) + NCHAR (208)
	SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
	
	DECLARE @COUNTER INT = 1
	DECLARE @COUNTER1 INT

	WHILE (@COUNTER <= LEN (@strInput))
	BEGIN
		SET @COUNTER1 = 1

			WHILE (@COUNTER1 <= LEN (@SIGN_CHARS) + 1)
			BEGIN
				IF UNICODE (SUBSTRING (@SIGN_CHARS, @COUNTER1, 1)) = UNICODE (SUBSTRING (@strInput, @COUNTER, 1))
				BEGIN
					IF @COUNTER = 1
						SET @strInput = SUBSTRING (@UNSIGN_CHARS, @COUNTER1, 1) + SUBSTRING (@strInput, @COUNTER+1, LEN (@strInput) - 1)
					ELSE
						SET @strInput = SUBSTRING (@strInput, 1, @COUNTER - 1) + SUBSTRING (@UNSIGN_CHARS, @COUNTER1, 1) + SUBSTRING (@strInput, @COUNTER + 1, LEN (@strInput) - @COUNTER)
						BREAK
				END
				SET @COUNTER1 = @COUNTER1 + 1
			END

		SET @COUNTER = @COUNTER + 1
	END

	SET @strInput = REPLACE (@strInput, ' ', '-')
	
	RETURN @strInput
END
GO

CREATE PROC USP_GetListBillByDateAndPage
	@dateCheckIn DATE,
	@dateCheckOut DATE,
	@page INT
AS
BEGIN
	DECLARE @pageRows INT = 8
	DECLARE @selectRows INT = @pageRows
	DECLARE @exceptRows INT = (@page - 1) * @pageRows

	; WITH BillShow (ID, [Tên bàn], [Ngày đặt], [Ngày thanh toán], [Giảm giá], [Tổng tiền])
	AS
	(
		SELECT B.ID AS [ID], A.Name AS [Tên bàn], DateCheckIn AS [Ngày đặt], DateCheckOut AS [Ngày thanh toán], Discount AS [Giảm giá], dbo.USF_GetSumTotalPrice(B.ID) AS [Tổng tiền]
		FROM dbo.TableFood A, dbo.Bill B
		WHERE DateCheckIn >= @dateCheckIn AND DateCheckOut <= @dateCheckOut AND B.Stat = 1 AND A.ID = B.IDTable
	)

	SELECT TOP (@selectRows) *
	FROM BillShow
	WHERE ID NOT IN (SELECT TOP (@exceptRows) ID FROM BillShow)
END
GO

CREATE PROC USP_GetAmountBillByDate
	@dateCheckIn DATE,
	@dateCheckOut DATE
AS
BEGIN
	SET DATEFORMAT dmy
	SELECT COUNT (*)
	FROM dbo.TableFood A, dbo.Bill B
	WHERE DateCheckIn >= @dateCheckIn AND DateCheckOut <= @dateCheckOut AND B.Stat = 1 AND A.ID = B.IDTable
END
GO

													--    /\    --
													--   //\\   --
													--  //  \\  --
													-- // ^^ \\ --
													-- \\ ~~ // --
													--  \\  //  --
													--   \\//   --
													--    \/    --

SELECT * FROM dbo.Bill

SELECT * FROM dbo.BillInfor

SELECT * FROM dbo.TableFood

SELECT * FROM dbo.Food

SELECT * FROM dbo.FoodCategory

SELECT * FROM dbo.Account