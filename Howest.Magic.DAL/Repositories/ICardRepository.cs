using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        IQueryable<Card> GetAllCards();

        IQueryable<Card> GetCardsByArtistId(int artistId);

        Task<Card?> GetCardByIdAsync(int cardId);

    }
}
