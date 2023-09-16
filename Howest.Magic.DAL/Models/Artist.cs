using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Howest.MagicCards.DAL.Models
{
    public partial class Artist
    {
        public Int64 Id { get; set; }
        public string FullName { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
    }
}
