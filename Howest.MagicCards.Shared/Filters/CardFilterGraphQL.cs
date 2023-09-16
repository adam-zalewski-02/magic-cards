using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilterGraphQL : CardFilter
    {
        public string Power { get; set; }

        public string Toughness { get; set; }
    }
}
