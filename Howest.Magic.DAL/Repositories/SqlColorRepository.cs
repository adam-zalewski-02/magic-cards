using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlColorRepository : IColorRepository
    {
        private readonly MyCardContext _db;

        public SqlColorRepository(MyCardContext myCardDBContext)
        {
            _db = myCardDBContext;
        }

        public IQueryable<Colors> GetAllColors()
        {
            IQueryable<Colors> colors = _db.Colors
                .Select(c => c);
            return colors;
        }
    }
}
