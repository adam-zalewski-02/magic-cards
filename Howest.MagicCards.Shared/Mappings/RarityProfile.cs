using Howest.MagicCards.Shared.DTO.Artist;
using Howest.MagicCards.Shared.DTO.Rarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Mappings
{
    public class RarityProfile : Profile
    {
        public RarityProfile()
        {
            CreateMap<Rarity, RarityReadDTO>();
            CreateMap<Rarity, RarityDetailReadDTO>();
        }
    }
}
