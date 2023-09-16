namespace Howest.MagicCards.MinimalAPI.Model
{
    public interface ICardDeckRepository
    {
        void AddCard(DeckCardsDTO newCard);
        bool DeleteCard(Int64 id);
        bool DeleteAllCards();
        IEnumerable<DeckCardsDTO> GetAllCards();
        DeckCardsDTO? GetCardById(Int64 id);
        void UpdateCard(DeckCardsDTO updatedCard, Int64 id);
    }
}
