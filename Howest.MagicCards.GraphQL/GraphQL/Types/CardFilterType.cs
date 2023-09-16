using GraphQL.Types;
using Howest.MagicCards.Shared.Filters;

namespace Howest.MagicCards.GraphQL.GraphQL.Types
{
    public class CardFilterType : InputObjectGraphType<CardFilterGraphQL>
    {
        public CardFilterType() 
        {
            Name = "FilterCard";

            Field(c => c.Power, type: typeof(StringGraphType))
               .Name("power");

            Field(c => c.Toughness, type: typeof(StringGraphType))
                .Name("toughness");

        }
    }
}
