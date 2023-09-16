using Howest.MagicCards.Shared.DTO.Artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Mappings
{
    public class ArtistsProfile : Profile
    {
        public ArtistsProfile() 
        {
            CreateMap<Artist, ArtistReadDTO>();
            CreateMap<Artist, ArtistDetailReadDTO>();
        }
    }
}
