using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlArtistRepository : IArtistRepository
    {
        private readonly MyCardContext _db;

        public SqlArtistRepository(MyCardContext myCardContext)
        {
            _db = myCardContext;
        }

        public IQueryable<Artist> GetAllArtists()
        {
            IQueryable<Artist> allArtists = _db.Artists
                .Select(a => a);

            return allArtists;
        }

        public async Task<Artist?> GetArtistByIdAsync(int id)
        {
            Artist? singleArtist = await _db.Artists
                .SingleOrDefaultAsync(a => a.Id == id);
            return singleArtist;
        }
    }
}
