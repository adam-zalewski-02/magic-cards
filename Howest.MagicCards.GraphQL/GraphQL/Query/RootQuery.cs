using GraphQL.Types;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.Extensions.Options;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.GraphQL.GraphQL.Types;
using GraphQL;
using Howest.MagicCards.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.GraphQL.GraphQL.Query
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(ICardRepository cardRepository, IArtistRepository artistRepository, IOptions<PaginationFilter> pagingFilter) 
        {
            Name = "Query";
            PaginationFilter pagingOptions = pagingFilter.Value;

            #region Cards
            Field<ListGraphType<CardType>>(
                "Cards",
                Description = "Get all cards",
                arguments: new QueryArguments
                {
                    new  QueryArgument<IntGraphType> { Name = "page", DefaultValue = pagingOptions.PageNumber},
                    new  QueryArgument<CardFilterType> { Name = "filter"}
                },
                resolve: context =>
                {
                    int page = context.GetArgument<int>("page");
                    CardFilterGraphQL cardFilter = context.GetArgument<CardFilterGraphQL>("filter") ?? new CardFilterGraphQL();

                    return cardRepository.GetAllCards()
                                        .toFilteredListGraphQL(
                                                    cardFilter.Power,
                                                    cardFilter.Toughness)
                                        .ToPagedList(page, pagingOptions.PageSize)
                                        .ToList();
                }
             );

            #endregion

            #region Artists
            Field<ListGraphType<ArtistType>>(
                "Artists",
                Description = "Get all artists",
                arguments: new QueryArguments
                {
                    new QueryArgument<CardFilterType> { Name = "filter" },
                    new QueryArgument<IntGraphType> { Name = "limit", DefaultValue = int.MaxValue}
                },
                resolve: context =>
                {
                    int page = context.GetArgument<int>("page");
                    int limit = context.GetArgument<int>("limit");

                    return artistRepository
                                .GetAllArtists()
                                .Take(limit)
                                .ToList();
                }
             );

            Field<ArtistType>(
                "Artist",
                Description = "Get artist by id",
                arguments: new QueryArguments
                {
                    new  QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }
                },
                resolve: context =>
                {
                    int id = context.GetArgument<int>("id");

                    return artistRepository.GetArtistByIdAsync(id);
                }
            );
            #endregion
        }
    }
}
