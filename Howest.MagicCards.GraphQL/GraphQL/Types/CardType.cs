using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace Howest.MagicCards.GraphQL.GraphQL.Types
{
    public class CardType : ObjectGraphType<Card>
    {
        public CardType(IArtistRepository artistRepository)
        {
            Name = "Card";

            Field(c => c.Id, type: typeof(IdGraphType))
                .Description("Id of the card");

            Field(c => c.Name, type: typeof(StringGraphType))
                .Description("Name of the card");

            Field(c => c.ManaCost, type: typeof(StringGraphType))
                .Description("The manacost of the card");

            Field(c => c.Set.SetName, type: typeof(StringGraphType))
                .Name("set")
                .Description("Set the card is apart of");

            Field(c => c.Rarity.RarityName, type: typeof(StringGraphType))
                .Name("rarity")
                .Description("The rarity of the card");

            Field(c => c.Power, type: typeof(StringGraphType))
                .Description("The power value of the card");

            Field(c => c.Toughness, type: typeof(StringGraphType))
                .Description("The toughness of the card");

            Field(c => c.OriginalImageUrl, type: typeof(StringGraphType))
                .Description("The image url of the card");

            Field<ArtistType>("Artist", resolve: context => artistRepository.GetArtistByIdAsync((Convert.ToInt32(context.Source.ArtistId))));

        }
    }
}
