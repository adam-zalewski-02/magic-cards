using GraphQL;
using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace Howest.MagicCards.GraphQL.GraphQL.Types
{
    public class ArtistType : ObjectGraphType<Artist>
    {
        public ArtistType(ICardRepository cardRepository) 
        {
            Name = "Artist";

            Field(a => a.Id, type: typeof(IdGraphType))
                .Description("Id of the artist");

            Field(a => a.FullName, type: typeof(StringGraphType))
                .Description("The full name of the artist");

            Field<ListGraphType<CardType>>
                (
                    "Cards",
                    arguments: new QueryArguments
                    {
                        new QueryArgument<IntGraphType> { Name = "limit", DefaultValue = int.MaxValue }
                    },
                    resolve: context =>
                    {
                        int limit = context.GetArgument<int>("limit");

                        return cardRepository.GetCardsByArtistId((int)context.Source.Id).Take(limit);
                    }
                );
        }
    }
}
