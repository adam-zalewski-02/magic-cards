using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Howest.MagicCards.MinimalAPI.Model
{
    public class DeckCardsDTO
    {
        [BsonElement("_id")]
        public Int64 Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = null!;
        [BsonElement("colorid")]
        public string ImageUrl { get; set; } = null!;
        [BsonElement("image_url")]
        public string ColorId { get; set; } = null!;
        [BsonElement("manacost")]
        public string Manacost { get; set; } = null!;
        [BsonElement("converted_mana_cost")]
        public string ConvertedManaCost { get; set; } = null!;
        [BsonElement("quantity")]
        public int quantity { get; set; }
    }
}
