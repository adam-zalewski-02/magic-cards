using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Models
{
    public partial class CardColors
    {
        public Int64 CardId { get; set; }

        public Int64 ColorId { get; set; }

        public virtual Colors Colors { get; set; }
        public virtual Card Card { get; set; }
    }
}
