USE [SnackOverflowDB]
GO
IF EXISTS(SELECT * FROM sys.objects WHERE type = 'P' AND  name = 'sp_create_category')
BEGIN
DROP PROCEDURE sp_create_category
Print '' print  ' *** dropping procedure sp_create_category'
End
GO

Print '' print  ' *** creating procedure sp_create_category'
GO
Create PROCEDURE sp_create_category
(
@CATEGORY_ID[NVARCHAR](200)
)
AS
BEGIN
INSERT INTO CATEGORY (CATEGORY_ID)
VALUES
(@CATEGORY_ID)
END
