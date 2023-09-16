using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class CardExtensions
    {

        public static IQueryable<Card> toFilteredListGraphQL(this IQueryable<Card> cards, string power, string toughness)
        {
            if (string.IsNullOrEmpty(power) && string.IsNullOrEmpty(toughness))
            {
                return cards;
            }
            else if (string.IsNullOrEmpty(power))
            {
                return cards.Where(c => c.Toughness == toughness);
            }
            else if (string.IsNullOrEmpty(toughness))
            {
                return cards.Where(c => c.Power == power);
            }
            return cards.Where(c => c.Toughness == toughness & c.Power == power);
        }

        public static IQueryable<Card> toFilteredList(this IQueryable<Card> cards, string searchArtist, string searchCardName, string searchCardText, string searchType, string searchSet, string searchRarity)
        {
            cards = cards.SearchArtist(searchArtist)
            .SearchCardName(searchCardName)
            .SearchCardText(searchCardText)
            .SearchSet(searchSet)
            .SearchCardRarity(searchRarity)
            .SearchCardType(searchType);

            return cards;
        }

        public static IQueryable<Card> SearchArtist(this IQueryable<Card> cards, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return cards;
            }
            else
            {
                string lowerCaseTerm = searchTerm.Trim().ToLower();
                return cards
                    .Where(c => c.Artist.FullName.ToLower().Contains(lowerCaseTerm));
            }
        }

        public static IQueryable<Card> SearchSet(this IQueryable<Card> cards, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return cards;
            }
            else
            {
                string lowerCaseTerm = searchTerm.Trim().ToLower();
                return cards
                    .Where(c => c.Set.SetName.ToLower().Contains(lowerCaseTerm));
            }
        }

        public static IQueryable<Card> SearchCardName(this IQueryable<Card> cards, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return cards;
            }
            else
            {
                string lowerCaseTerm = searchTerm.Trim().ToLower();
                return cards
                    .Where(c => c.Name.ToLower().Contains(lowerCaseTerm));
            }
        }

        public static IQueryable<Card> SearchCardText(this IQueryable<Card> cards, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return cards;
            }
            else
            {
                string lowerCaseTerm = searchTerm.Trim().ToLower();
                return cards
                    .Where(c => (c.Text != null && c.OriginalText != null) && (c.Text.ToLower().Contains(lowerCaseTerm)
                                || c.OriginalText.ToLower().Contains(lowerCaseTerm)));
            }
        }

        public static IQueryable<Card> SearchCardRarity(this IQueryable<Card> cards, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return cards;
            }
            else
            {
                string lowerCaseTerm = searchTerm.Trim().ToLower();
                return cards
                    .Where(c => c.Rarity.RarityName.ToLower() == lowerCaseTerm);
            }
        }

        public static IQueryable<Card> SearchCardType(this IQueryable<Card> cards, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return cards;
            }
            else
            {
                string lowerCaseTerm = searchTerm.Trim().ToLower();
                return cards
                    .Where(c => c.Type.ToLower().Contains(lowerCaseTerm));
            }
        }

        public static IQueryable<Card> Sort(this IQueryable<Card> cards, string orderByQueryString)
        {
            if (string.IsNullOrEmpty(orderByQueryString))
            {
                return cards;
            }

            string[] orderParameters = orderByQueryString.Trim().Split(',');
            PropertyInfo[] propertyInfos = typeof(Card).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            StringBuilder orderQueryBuilder = new StringBuilder();

            foreach (string param in orderParameters)
            {
                if (!string.IsNullOrWhiteSpace(param))
                {
                    string propertyFromQueryName = param.Split(" ")[0];
                    PropertyInfo objectProperty = propertyInfos
                                     .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                    if (objectProperty is not null)
                    {
                        string direction = param.EndsWith(" desc", StringComparison.InvariantCultureIgnoreCase) ? "descending" : "ascending";
                        orderQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
                    }
                }
            }

            string orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return cards.OrderBy(c => c.Name);
            }

            return cards.OrderBy(orderQuery);
        }
    }
}
