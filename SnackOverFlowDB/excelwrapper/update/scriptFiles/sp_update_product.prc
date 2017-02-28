USE [SnackOverflowDB]
GO
IF EXISTS(SELECT * FROM sys.objects WHERE type = 'P' AND  name = 'sp_update_product')
BEGIN
DROP PROCEDURE sp_update_product
Print '' print  ' *** dropping procedure sp_update_product'
End
GO

Print '' print  ' *** creating procedure sp_update_product'
GO
Create PROCEDURE sp_update_product
(
@old_PRODUCT_ID[INT],
@old_NAME[NVARCHAR](50),
@new_NAME[NVARCHAR](50),
@old_DESCRIPTION[NVARCHAR](200),
@new_DESCRIPTION[NVARCHAR](200),
@old_UNIT_PRICE[DECIMAL](10,2),
@new_UNIT_PRICE[DECIMAL](10,2),
@old_IMAGE_NAME[VARCHAR](50)=null,
@new_IMAGE_NAME[VARCHAR](50),
@old_ACTIVE[BIT],
@new_ACTIVE[BIT],
@old_UNIT_OF_MEASUREMENT[NVARCHAR](20),
@new_UNIT_OF_MEASUREMENT[NVARCHAR](20),
@old_DELIVERY_CHARGE_PER_UNIT[DECIMAL](5,2),
@new_DELIVERY_CHARGE_PER_UNIT[DECIMAL](5,2)
)
AS
BEGIN
UPDATE product
SET NAME = @new_NAME, DESCRIPTION = @new_DESCRIPTION, UNIT_PRICE = @new_UNIT_PRICE, IMAGE_NAME = @new_IMAGE_NAME, ACTIVE = @new_ACTIVE, UNIT_OF_MEASUREMENT = @new_UNIT_OF_MEASUREMENT, DELIVERY_CHARGE_PER_UNIT = @new_DELIVERY_CHARGE_PER_UNIT
WHERE (PRODUCT_ID = @old_PRODUCT_ID)
AND (NAME = @old_NAME)
AND (DESCRIPTION = @old_DESCRIPTION)
AND (UNIT_PRICE = @old_UNIT_PRICE)
AND (IMAGE_NAME = @old_IMAGE_NAME OR ISNULL(IMAGE_NAME, @old_IMAGE_NAME) IS NULL)
AND (ACTIVE = @old_ACTIVE)
AND (UNIT_OF_MEASUREMENT = @old_UNIT_OF_MEASUREMENT)
AND (DELIVERY_CHARGE_PER_UNIT = @old_DELIVERY_CHARGE_PER_UNIT)
END