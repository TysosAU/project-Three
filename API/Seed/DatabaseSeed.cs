using MongoDB.Bson;
using MongoDB.Driver;
using API.Models;

namespace API.Seed
{
    public static class DatabaseSeed
    {
        public static async Task Seed(IMongoDatabase database)
        {
            var productCollection = database.GetCollection<Product>("Products");
            var UserCollection = database.GetCollection<User>("Users");

            var productCount = await productCollection.CountDocumentsAsync(FilterDefinition<Product>.Empty);
            if (productCount == 0)
            {
                var products = new[]
                {
                new Product { _id_ = 1, CategoryId = 1, Brand = "Toyota", Model = "Camry", Year = 2011, SalePrice = 27656, onSale = true, engineSize = 2000 },
                new Product { _id_ = 2, CategoryId = 1, Brand = "Ford", Model = "Kuga", Year = 2011, SalePrice = 35000, onSale = true, engineSize = 2000 },
                new Product { _id_ = 3, CategoryId = 1, Brand = "Mercedes", Model = "E200", Year = 6123, SalePrice = 33000, onSale = true, engineSize = 2000 },
                new Product { _id_ = 4, CategoryId = 1, Brand = "Hyundai", Model = "Veloster", Year = 6145, SalePrice = 125000, onSale = true, engineSize = 2000 },
                new Product { _id_ = 5, CategoryId = 1, Brand = "BMW", Model = "340r", Year = 2012, SalePrice = 6000, onSale = true, engineSize = 1000 },
                new Product { _id_ = 6, CategoryId = 1, Brand = "Hyundai", Model = "Veloster", Year = 2012, SalePrice = 95000, onSale = true, engineSize = 2000 },
                new Product { _id_ = 7, CategoryId = 1, Brand = "Ford", Model = "Mondeo", Year = 2012, SalePrice = 65000, onSale = true, engineSize = 4000 },
                new Product { _id_ = 8, CategoryId = 1, Brand = "Toyota", Model = "Corolla", Year = 2013, SalePrice = 65000, onSale = true, engineSize = 3000 },
                new Product { _id_ = 9, CategoryId = 1, Brand = "Mercedes", Model = "E200", Year = 2014, SalePrice = 17000, onSale = true, engineSize = 2000 },
                new Product { _id_ = 10, CategoryId = 2, Brand = "BMW", Model = "330i", Year = 2015, SalePrice = 9900, onSale = true, engineSize = 1600 },
                new Product { _id_ = 11, CategoryId = 2, Brand = "Hyundai", Model = "Getz", Year = 2016, SalePrice = 10, onSale = false, engineSize = 1800 },
                new Product { _id_ = 12, CategoryId = 2, Brand = "Toyota", Model = "Land", Year = 2017, SalePrice = 6800, onSale = true, engineSize = 1200 },
                new Product { _id_ = 13, CategoryId = 2, Brand = "BMW", Model = "M series", Year = 2018, SalePrice = 125000, onSale = true, engineSize = 2400 },
                new Product { _id_ = 14, CategoryId = 2, Brand = "Mercedes", Model = "D150", Year = 2019, SalePrice = 550, onSale = true, engineSize = 2000 },
                new Product { _id_ = 15, CategoryId = 2, Brand = "Hyundai", Model = "Veloster", Year = 2022, SalePrice = 22000, onSale = false, engineSize = 1000 },
                new Product { _id_ = 16, CategoryId = 2, Brand = "BMW", Model = "7 series", Year = 2011, SalePrice = 95000, onSale = true, engineSize = 2000 },
                new Product { _id_ = 17, CategoryId = 2, Brand = "Tata", Model = "Gulli", Year = 2012, SalePrice = 1700, onSale = true, engineSize = 2000 },
                new Product { _id_ = 18, CategoryId = 3, Brand = "Hyundai", Model = "Leong", Year = 2013, SalePrice = 28000, onSale = true, engineSize = 2600 },
                new Product { _id_ = 19, CategoryId = 3, Brand = "Tata", Model = "125", Year = 2014, SalePrice = 28000, onSale = true, engineSize = 2000 },
                new Product { _id_ = 20, CategoryId = 3, Brand = "BMW", Model = "6 series", Year = 2015, SalePrice = 28000, onSale = false, engineSize = 1000 },
                new Product { _id_ = 21, CategoryId = 3, Brand = "Tata", Model = "Furs", Year = 2016, SalePrice = 28000, onSale = false, engineSize = 2000 },
                new Product { _id_ = 22, CategoryId = 3, Brand = "Mercedes", Model = "B series", Year = 2018, SalePrice = 28000, onSale = true, engineSize = 2700 },
                new Product { _id_ = 23, CategoryId = 3, Brand = "Toyota", Model = "Prad0", Year = 2019, SalePrice = 28000, onSale = true, engineSize = 2800 },
                new Product { _id_ = 24, CategoryId = 4, Brand = "BMW", Model = "330i", Year = 2020, SalePrice = 24000, onSale = true, engineSize = 2900 },
                new Product { _id_ = 25, CategoryId = 5, Brand = "Ford", Model = "selbi", Year = 2021, SalePrice = 35000, onSale = true, engineSize = 3000 },
                new Product { _id_ = 26, CategoryId = 5, Brand = "Ford", Model = "Falcon", Year = 1999, SalePrice = 12400, onSale = true, engineSize = 2000 },
                new Product { _id_ = 27, CategoryId = 5, Brand = "Tata", Model = "ES", Year = 2011, SalePrice = 13999, onSale = true, engineSize = 2000 },
                new Product { _id_ = 28, CategoryId = 5, Brand = "Hyundai", Model = "Veloster", Year = 2012, SalePrice = 12455, onSale = true, engineSize = 3000 },
                new Product { _id_ = 29, CategoryId = 5, Brand = "BMW", Model = "330", Year = 2013, SalePrice = 99999, onSale = true, engineSize = 3000 },
                new Product { _id_ = 30, CategoryId = 5, Brand = "Ford", Model = "Kuga", Year = 2014, SalePrice = 12555, onSale = true, engineSize = 2000 },
                new Product { _id_ = 31, CategoryId = 5, Brand = "Ford", Model = "Mondeo", Year = 2015, SalePrice = 12555, onSale = true, engineSize = 3000 },
                new Product { _id_ = 32, CategoryId = 5, Brand = "Ford", Model = "Mondeo", Year = 2016, SalePrice = 102222, onSale = true, engineSize = 2000 },
                new Product { _id_ = 33, CategoryId = 5, Brand = "Toyota", Model = "Camry", Year = 2018, SalePrice = 12555, onSale = true, engineSize = 2000 },
                };

                await productCollection.InsertManyAsync(products);
            }

            var UserCount = await UserCollection.CountDocumentsAsync(FilterDefinition<User>.Empty);
            if (UserCount == 0)
            {
                var users = new[]
                {
                new User { Id = ObjectId.GenerateNewId(), Username = "SampleAdministratorUser", Email = "Administrator@admin.com", Password = BCrypt.Net.BCrypt.HashPassword("AdminPassword123Cool"), Role = "admin"},
                new User { Id = ObjectId.GenerateNewId(), Username = "SampleDefaultUser", Email = "user@user.com", Password = BCrypt.Net.BCrypt.HashPassword("SampleUserPassword"), Role = "user"},
                };

                await UserCollection.InsertManyAsync(users);
            }
        }
    }
}
