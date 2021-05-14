Select Orders.OrderID, Orders.Date
From Orders Where OrderID in (Select OrderId From Positions where ProductID in (Select ProductID From Products Where Price = X));