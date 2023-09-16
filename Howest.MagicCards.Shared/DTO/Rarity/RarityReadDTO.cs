using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO.Rarity
{
    public record RarityReadDTO
    {
        public string Code { get; init; }
        public string RarityName { get; init; }
    }
}
