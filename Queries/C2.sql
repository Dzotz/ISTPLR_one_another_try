Select O.OrderID 
From Orders O
Where O.Date=Convert(date, X) AND Exists (Select P.PositionID From Positions p Where P.OrderID = O.OrderID AND Exists(Select Products.Price From Products Where P.ProductID=Products.ProductID And Products.Price>20));