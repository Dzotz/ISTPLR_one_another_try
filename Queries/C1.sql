Select C.Name, C.Address
From CoffeeShops C 
Where (C.OwnerID = X) And
Not Exists (Select Orders.OrderID From Orders Where CoffeeShopID=c.CoffeeShopID)