CREATE OR ALTER PROCEDURE P_CreatePayment
    @UserId uniqueidentifier,
    @Amount DECIMAL(19,4)
AS
BEGIN
	DECLARE @CurrentBalance decimal(19,4);
    IF @Amount = 0 
		THROW 50000, 'The @Amount parameter cannot be zero', 16;
	
	SELECT TOP 1 @CurrentBalance = Balance FROM dbo.Wallets WHERE UserId = @UserId;
	IF @CurrentBalance is NULL
		THROW 50000, 'Wallet not found', 16;
    IF @Amount < 0 and (@CurrentBalance + @Amount) < 0
		THROW 50000, 'Insufficient funds for debit', 16;
    
    UPDATE dbo.Wallets 
    SET Balance = Balance + @Amount
	WHERE UserId = @UserId;
    SELECT 1 AS Result;
END;