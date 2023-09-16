using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Howest.MagicCards.DAL.Models
{
    public partial class AdvSearchFilter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Set { get; set; }
        public string Rarity { get; set; }
        public string Artist { get; set; }
        public string Sort { get; set; }
    }
}
