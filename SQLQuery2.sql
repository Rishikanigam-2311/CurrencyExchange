-- =======================================================
-- Create Stored Procedure Template for Azure SQL Database
-- =======================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      <Author, , Rishika>
-- Create Date: <Create Date, 03.02.2025, >
-- Description: <Description, , SP for updating currency table with latest values from Fixer API>
-- =============================================

CREATE PROCEDURE SP_CurrencyUpdates
    @CurrencyCode VARCHAR(3),  
    @Date DATETIME,            
    @Rate DECIMAL(18, 6)      
AS
BEGIN
    INSERT INTO CurrencyUpdates (CurrencyCode, Date, Rate)
    VALUES (@CurrencyCode, @Date, @Rate);

END;


