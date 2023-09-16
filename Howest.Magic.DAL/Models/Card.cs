using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Howest.MagicCards.DAL.Models
{
    public partial class Card
    {

        public Card()
        {
            CardColor = new HashSet<CardColors>();
        }
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public string ConvertedManaCost { get; set; }
        public string Type { get; set; }
        public string RarityCode { get; set; }
        public string SetCode { get; set; }
        public string Text { get; set; }
        public string Flavor { get; set; }
        public Int64 ArtistId { get; set; }
        public string Number { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string Layout { get; set; }
        public int? MultiverseId { get; set; }
        public string OriginalImageUrl { get; set; }
        public string Image { get; set; }
        public string OriginalText { get; set; }
        public string OriginalType { get; set; }
        public string MtgId { get; set; }
        public string Variations { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        
        public virtual Artist Artist { get; set; }
        public virtual Set Set { get; set; }
        public virtual Rarity Rarity { get; set; }
        public virtual ICollection<CardColors> CardColor { get; set; }
    }
}
