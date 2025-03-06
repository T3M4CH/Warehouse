// See https://aka.ms/new-console-template for more information

using Warehouse.Containers;
using Warehouse.Products;

var foodBox = new Box<Food>(50.0);
var clothesBox = new Box<Clothes>(50.0);
var animalsPallet = new Pallet<Animals>(1000.0, 20.0);

animalsPallet.AddProduct(new Animals("Snake", 50.0, "1241x15125"));
animalsPallet.AddProduct(new Animals("BigDawg", 30.0, "0x1241ad12"));

clothesBox.AddProduct(new Clothes("GucciGlasses", 0.2, "M"));
clothesBox.AddProduct(new Clothes("T-Shirt Stone Island", 2.0, "XXXL"));

foodBox.AddProduct(new Food("Snacks", 0.5, new DateTime(2025, 3, 10)));
foodBox.AddProduct(new Food("GodCheese", 0.2, new DateTime(2025, 3, 15)));
foodBox.AddProduct(new Food("ExpiredFood", 0.2, new DateTime(2025, 2, 1)));

var warehouse = new Warehouse.Controller.Warehouse();
warehouse.AddContainer(animalsPallet);
warehouse.AddContainer(clothesBox);
warehouse.AddContainer(foodBox);

warehouse.SortContainersByWeight();

Console.WriteLine("Clothes < 2kg " + string.Join(',', warehouse.FilterProducts(product => product is Clothes { Weight: <= 2 }).Select(pr => pr.Name)));
Console.WriteLine("Not expired food " + string.Join(',', warehouse.FilterProducts(product => product is Food food && food.ExpireDate > DateTime.Now).Select(pr => pr.Name)));