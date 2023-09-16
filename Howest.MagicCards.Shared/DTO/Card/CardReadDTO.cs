using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO.Card
{
    public record CardReadDTO
    {
        public Int64 Id { get; init; }
        public string Name { get; init; }
        public string Type { get; init; }
        public string Rarity { get; init; }
        public string Set { get; init; }
        public string Text { get; init; }
        public string ArtistName { get; init; }
        public List<string> ColorCodes { get; init; }
        public string OriginalImageUrl { get; init; }
        public string Image { get; init; }

    }
}
