using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlRarityRepository : IRarityRepository
    {
        private readonly MyCardContext _db;

        public SqlRarityRepository(MyCardContext myCardDBContext)
        {
            _db = myCardDBContext;
        }

        public IQueryable<Rarity> GetAllRarities()
        {
            IQueryable<Rarity> rarities = _db.Rarities
                .Select(r => r);
            return rarities;
        }
        public async Task<Rarity?> GetRarityByIdAsync(int id)
        {
            Rarity? singleRarity = await _db.Rarities
                .SingleOrDefaultAsync(r => r.Id == id);
            return singleRarity;

        }
    }
}
