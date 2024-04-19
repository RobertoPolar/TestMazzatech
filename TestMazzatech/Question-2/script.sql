CREATE TABLE Trades (
    TradeID INT IDENTITY(1,1) PRIMARY KEY,
    Value DECIMAL(18, 2),
    ClientSector VARCHAR(10)
);

CREATE TABLE CategorizedTrades (
    TradeID INT,
    Category VARCHAR(20),
    CONSTRAINT FK_CategorizedTrades_Trades FOREIGN KEY (TradeID) REFERENCES Trades(TradeID)
);
go

CREATE OR ALTER PROCEDURE CategorizeTrades
AS
BEGIN
    DELETE FROM CategorizedTrades;

    INSERT INTO CategorizedTrades (TradeID, Category)
    SELECT TradeID,
           CASE 
               WHEN Value < 1000000 AND ClientSector = 'Public' THEN 'LOWRISK'
               WHEN Value >= 1000000 AND ClientSector = 'Public' THEN 'MEDIUMRISK'
               WHEN Value >= 1000000 AND ClientSector = 'Private' THEN 'HIGHRISK'
               ELSE NULL
           END AS Category
    FROM Trades;
END;
GO

INSERT INTO Trades (Value, ClientSector) VALUES (2000000, 'Private');
INSERT INTO Trades (Value, ClientSector) VALUES (400000, 'Public');
INSERT INTO Trades (Value, ClientSector) VALUES (500000, 'Public');
INSERT INTO Trades (Value, ClientSector) VALUES (3000000, 'Public');

EXEC CategorizeTrades;

SELECT * FROM CategorizedTrades;
