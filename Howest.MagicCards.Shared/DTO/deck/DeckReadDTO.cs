using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO.deck
{
    public record DeckReadDTO
    {
        public Int64 Id { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string ColorId { get; init; } 
        public string Manacost { get; init; }
        public string ConvertedManaCost { get; init; }
        public int quantity { get; init; }
    }
}
