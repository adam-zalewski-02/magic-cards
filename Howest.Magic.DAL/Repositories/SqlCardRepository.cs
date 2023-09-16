using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlCardRepository : ICardRepository
    {
        private readonly MyCardContext _db;

        public SqlCardRepository(MyCardContext myCardContext) 
        {
            _db = myCardContext;
        }

        public IQueryable<Card> GetAllCards()
        {
            IQueryable<Card> allCards = _db.Cards               
                                            .Include(c => c.Artist)
                                            .Include(c => c.Set)
                                            .Include(c => c.Rarity)
                                            .Include(c => c.CardColor)
                                                .ThenInclude(c => c.Colors)
                                            .Select(c => c);
            return allCards;
        }

        public IQueryable<Card> GetCardsByArtistId(int artistId)
        {
            IQueryable<Card> cards = _db.Cards
                .Where(a => a.ArtistId == artistId)
                .Select(c => c);

            return cards;
        }

        public async Task<Card?> GetCardByIdAsync(int cardId)
        {
            Card? card = await _db.Cards
                                            .Include(c => c.Artist)
                                            .Include(c => c.Set)
                                            .Include(c => c.Rarity)
                                            .Include(c => c.CardColor)
                                                .ThenInclude(c => c.Colors)
                                            .SingleOrDefaultAsync(c => c.Id == cardId);

            return card;
        }
    }
}
