using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlSetRepository : ISetRepository
    {
        private readonly MyCardContext _db;

        public SqlSetRepository(MyCardContext myCardContext)
        {
            _db = myCardContext;
        }

        public IQueryable<Set> GetAllSets()
        {
            IQueryable<Set> allSets = _db.Sets
                                            .Select(s => s);
            return allSets;
        }
    }
}
