SELECT AVG(Price)
FROM Products
WHERE ProductID	IN (SELECT ProductID FROM Positions WHERE PositionID IN (SELECT PositionID FROM Orders WHERE Orders.Date = CONVERT(date, X) AND Orders.CashierID = Y)); 