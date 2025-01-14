using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class Product
    {
        [BsonId]
        public int _id_ { get; set; }

        [BsonElement("Brand")]
        public string Brand { get; set; }

        [BsonElement("Model")]
        public string Model { get; set; }

        [BsonElement("Year")]
        public int Year { get; set; }

        [BsonElement("SalePrice")]
        public int SalePrice { get; set; }

        [BsonElement("onSale")]
        public bool onSale { get; set; }

        [BsonElement("engineSize")]
        public int engineSize { get; set; }

        [BsonElement("CategoryId")]

        public int CategoryId { get; set; }

    }
}
