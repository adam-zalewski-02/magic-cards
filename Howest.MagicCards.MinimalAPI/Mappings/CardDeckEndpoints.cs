using Howest.MagicCards.MinimalAPI.Model;
using Microsoft.AspNetCore.Server.HttpSys;

namespace Howest.MagicCards.MinimalAPI.Mappings
{
    public static class CardDeckEndpoints
    {
        public static void MapCardDeckEndpoints(this WebApplication app)
        {
            app.MapGet("/api/deckcards", (CardDeckRepository cardDeckRepository) =>
            {
                return (cardDeckRepository.GetAllCards() is IEnumerable<DeckCardsDTO> cards)
                         ? Results.Ok(cards)
                         : Results.NotFound("No cards found");
            }).WithTags("Get all cards");


            app.MapPost($"api/deckcards", (CardDeckRepository cardDeckRepository, DeckCardsDTO newCard) =>
            {
                DeckCardsDTO? card = cardDeckRepository.GetCardById(newCard.Id);
                if (card is not null)
                {
                    newCard.quantity = card.quantity + 1;
                    cardDeckRepository.UpdateCard(newCard, newCard.Id);
                    return Results.Ok($"Card already existed, added one to quantity of card {newCard.Id}");
                }

                cardDeckRepository.AddCard(newCard);
                return Results.Created($"/api/deckcards/{newCard?.Id}", newCard);
            }
            ).Accepts<DeckCardsDTO>("application/json");

            app.MapPut($"api/deckcards/{{id}}", (CardDeckRepository cardDeckRepository, DeckCardsDTO updatedCard, Int64 id) =>
            {
                DeckCardsDTO? card = cardDeckRepository.GetCardById(id);
                if (card is null)
                {
                    return Results.NotFound($"No card with id {id} found");
                }
                cardDeckRepository.UpdateCard(updatedCard, id);
                return Results.Ok(updatedCard);
            }
            ).Accepts<DeckCardsDTO>("application/json");

            app.MapDelete($"api/deckcards/{{id}}", (CardDeckRepository cardDeckRepository, Int64 id) =>
            {
                return cardDeckRepository.DeleteCard(id)
                    ? Results.Ok($"Card with id {id} is deleted from the deck")
                    : Results.NotFound($"No card found with id {id}");
            }
            );

            app.MapDelete($"api/deckcards", (CardDeckRepository cardDeckRepository) =>
            {
                return cardDeckRepository.DeleteAllCards()
                    ? Results.Ok($"Deleted all cards from deck")
                    : Results.NotFound($"No cards found in deck / something went wrong");
            }
           );


        }

        public static void AddCardDeckServices(this IServiceCollection services)
        {
            services.AddSingleton<CardDeckRepository>();
        }
    }
}
