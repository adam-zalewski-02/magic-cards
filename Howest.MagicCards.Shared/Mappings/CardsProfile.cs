
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO.Card;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Howest.MagicCards.Shared.Mappings
{
    public class CardsProfile : Profile
    {
        public CardsProfile() 
        {
            CreateMap<Card, CardReadDTO>()
                .ForMember(dto => dto.ArtistName,
                            opt => opt.MapFrom(a => a.Artist.FullName))
                .ForMember(dto => dto.Set,
                            opt => opt.MapFrom(s => s.Set.SetName))
                .ForMember(dto => dto.Rarity,
                            opt => opt.MapFrom(r => r.Rarity.RarityName))
                .ForMember(dto => dto.ColorCodes, opt => opt.MapFrom(c => c.CardColor.Select(cc => cc.Colors.Code).ToList()));

            CreateMap<Card, CardDetailReadDTO>()
                 .ForMember(dto => dto.ArtistName,
                            opt => opt.MapFrom(a => a.Artist.FullName))
                .ForMember(dto => dto.Set,
                            opt => opt.MapFrom(s => s.Set.SetName))
                .ForMember(dto => dto.Rarity,
                            opt => opt.MapFrom(r => r.Rarity.RarityName))
                .ForMember(dto => dto.ColorCodes, opt => opt.MapFrom(c => c.CardColor.Select(cc => cc.Colors.Code).ToList()));
        }
    }
}
