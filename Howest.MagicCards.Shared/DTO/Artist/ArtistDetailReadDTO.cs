using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO.Artist
{
    public record ArtistDetailReadDTO
    {
        public Int64 Id { get; init; }
        public string FullName { get; init; }
    }
}
