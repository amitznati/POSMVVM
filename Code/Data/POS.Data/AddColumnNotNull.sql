ALTER TABLE [Cashier] ADD Name INT NULL
GO
UPDATE [Cashier] SET Name = 4
GO
ALTER TABLE [Cashier] ALTER COLUMN Name INT NOT NULL
GO 