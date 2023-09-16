using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace Howest.MagicCards.DAL.Models
{
    public partial class Colors
    {

        public Colors()
        {
            CardColor = new HashSet<CardColors>();
        }
        public Int64 Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CardColors> CardColor { get; set; }
    }
}
