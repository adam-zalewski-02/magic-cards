using Howest.MagicCards.Shared.DTO.Color;
using Howest.MagicCards.Shared.DTO.Rarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Mappings
{
    public class ColorsProfile : Profile
    {
        public ColorsProfile()
        {
            CreateMap<Colors, ColorReadDTO>();
        }
    }
}
