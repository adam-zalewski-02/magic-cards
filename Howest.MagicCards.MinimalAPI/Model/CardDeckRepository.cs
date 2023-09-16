using MongoDB.Bson;
using MongoDB.Driver;

namespace Howest.MagicCards.MinimalAPI.Model
{
    public class CardDeckRepository : ICardDeckRepository
    {
        private readonly IMongoCollection<DeckCardsDTO> _cards;

        public CardDeckRepository(IConfiguration config)
        {
            MongoDBSettings settings = config.GetSection("MongoDBSettings").Get<MongoDBSettings>();
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
            _cards = database.GetCollection<DeckCardsDTO>(settings.CollectionName);
        }

        public IEnumerable<DeckCardsDTO> GetAllCards()
        {
            return _cards.Find(_ => true).ToList();
        }

        public DeckCardsDTO GetCardById(Int64 id)
        {
            return _cards.Find(c => c.Id == id).FirstOrDefault();
        }

        public void AddCard(DeckCardsDTO newCard)
        {
            _cards.InsertOne(newCard);
        }

        public void UpdateCard(DeckCardsDTO cardToUpdate, Int64 id)
        {
            _cards.ReplaceOne(c => c.Id == id, cardToUpdate);
        }

        public bool DeleteCard(Int64 id)
        {
            FilterDefinition<DeckCardsDTO> filter = Builders<DeckCardsDTO>.Filter.Eq(c => c.Id, id);
            DeleteResult result = _cards.DeleteOne(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public bool DeleteAllCards()
        {
            DeleteResult result = _cards.DeleteMany(Builders<DeckCardsDTO>.Filter.Empty);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
