SELECT Cashiers.Name 
FROM Cashiers
WHERE Cashiers.CoffeeShopID IN
(SELECT CoffeeShops.CoffeeShopID FROM CoffeeShops WHERE CoffeeShops.OwnerID = P);