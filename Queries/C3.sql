Select C.PhoneNumber
From Cashiers C 
Where C.Name = X AND Exists (Select Co.CoffeeShopID From CoffeeShops Co Where Co.CoffeeShopID=C.CoffeeShopID And (Select Count(OrderID) From Orders Where Orders.CoffeeShopID=Co.CoffeeShopID)>2);