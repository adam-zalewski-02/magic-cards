using Howest.MagicCards.Shared.DTO.Card;
using Microsoft.AspNetCore.Components;
using System.Net.NetworkInformation;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using Microsoft.JSInterop;
using Howest.MagicCards.Shared.DTO.deck;
using System.Text;
using System.Net;
using Howest.MagicCards.MinimalAPI.Model;
using Howest.MagicCards.DAL.Models;
using DnsClient.Internal;
using Microsoft.Extensions.ObjectPool;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Specialized;
using System.Net.Http;

namespace Howest.MagicCards.Web.Pages
{
    public partial class Cards
    {
        private readonly string searchStorageKey = "searchFilter";

        private string title = "Magic The Gathering: Deck Builder";
        private string message = string.Empty;
        private int totalQuantity = 0;
        private string hoveredImageUrl;

        [Parameter]
        public int pageNumber { get; set; } = 1;

        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IHttpClientFactory? HttpClientFactory { get; set; }
        [Inject]
        public ProtectedLocalStorage storage { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }


        private AdvSearchFilter? advSearchFilter;

        private IEnumerable<CardReadDTO>? _cards = null;
        private IEnumerable<DeckReadDTO>? _deck = null;

        private readonly JsonSerializerOptions jsonOptions;

        public Cards()
        {
            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LocalStorageInit();
                await LoadDeckList();
                await LoadCardList();

                StateHasChanged();
            }
        }

        private async Task LocalStorageInit()
        {
            // Retrieve the AdvSearchFilter value from local storage or create an empty one if none is found
            string advSearchFilterValue = await JSRuntime.InvokeAsync<string>("localStorage.getItem", searchStorageKey);
            if (!string.IsNullOrEmpty(advSearchFilterValue))
            {
                advSearchFilter = JsonSerializer.Deserialize<AdvSearchFilter>(advSearchFilterValue);
            }
            else
            {
                advSearchFilter = new AdvSearchFilter();
                await JSRuntime.InvokeVoidAsync("localStorage.setItem", searchStorageKey, JsonSerializer.Serialize(advSearchFilter));
            }
        }

        public async Task LoadCardList()
        {   
            HttpClient? httpClient = HttpClientFactory?.CreateClient("CardsAPIv1.5");

            HttpResponseMessage response = await httpClient.GetAsync($"cards?" +
                                                                            $"pageNumber={pageNumber}&" +
                                                                            $"artistname={advSearchFilter?.Artist ?? string.Empty}&" +
                                                                            $"cardname={advSearchFilter?.Name ?? string.Empty}&" +
                                                                            $"cardtext={advSearchFilter?.Text ?? string.Empty}&" +
                                                                            $"set={advSearchFilter?.Set ?? string.Empty}&" +
                                                                            $"cardrarity={advSearchFilter?.Rarity ?? string.Empty}&" +
                                                                            $"cardType={advSearchFilter?.Type ?? string.Empty}&" +
                                                                            $"orderby={advSearchFilter?.Sort ?? string.Empty}");

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                PagedResponse<IEnumerable<CardReadDTO>>? result =
                        JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardReadDTO>>>(apiResponse, jsonOptions);
                _cards = result?.Data;
            }
            else
            {
                _cards = new List<CardReadDTO>();
                message = $"Error: {response.ReasonPhrase}";
            }

            if (_cards != null && _cards.Count() == 0)
            {
                message = "No cards found";
            }
        }


        public async Task LoadDeckList()
        {
            HttpClient? httpClient = HttpClientFactory?.CreateClient("DeckMinimalAPI");

            HttpResponseMessage response = await httpClient.GetAsync($"deckcards");

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                List<DeckReadDTO>? result = JsonSerializer.Deserialize<List<DeckReadDTO>>(apiResponse, jsonOptions);
                foreach (DeckReadDTO card in result)
                {
                    totalQuantity += card.quantity; 
                }
                _deck = result;
            }
            else
            {
                _deck = new List<DeckReadDTO>();
                message = $"Error: {response.ReasonPhrase}";
            }

        }


        public async void ReloadCardList()
        {
            _cards = null;
            await LoadCardList();
        }

        public async Task ReloadDeckList()
        {
            totalQuantity = 0;
            _deck = null;
            await LoadDeckList();
        }

        private void loadDetailCard(Int64 cardId)
        {
            navigationManager.NavigateTo($"/cards/{cardId}");
        }

        public async void AddCardToDeck(CardReadDTO cardSelected)
        {
            if (totalQuantity < 60)
            {
                DeckReadDTO card = await ConvertCardToDeckCard(cardSelected);

                HttpClient httpClient = HttpClientFactory.CreateClient("DeckMinimalAPI");

                HttpContent content = new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync($"deckcards", content);

                if (!response.IsSuccessStatusCode)
                {
                    message = "something went wrong";
                }

                await ReloadDeckList();
                StateHasChanged();
            } else
            {
                message = "deck cannot contain more than 60 cards";
            }
        }

        public async void AddToCardQuantity(DeckReadDTO card)
        {
            if (totalQuantity < 60)
            {
                HttpClient httpClient = HttpClientFactory.CreateClient("DeckMinimalAPI");

                HttpContent content = new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync($"deckcards", content);

                if (!response.IsSuccessStatusCode)
                {
                    message = "something went wrong";
                }

                await ReloadDeckList();
                StateHasChanged();
            }
            else
            {
                message = "deck cannot contain more than 60 cards";
            }
        }

        public async Task<DeckReadDTO> ConvertCardToDeckCard(CardReadDTO card)
        {
            HttpClient? httpClient = HttpClientFactory?.CreateClient("CardsAPIv1.5");

            HttpResponseMessage response = await httpClient.GetAsync($"cards/{card.Id}");

            string apiResponse = await response.Content.ReadAsStringAsync();

            CardDetailReadDTO? result = JsonSerializer.Deserialize<CardDetailReadDTO>(apiResponse, jsonOptions);

            //converting all colors in colorcCode list to one string alphabetically
            List<string> colorCodeList = card.ColorCodes;
            colorCodeList.Sort();
            string sortedColorCode = string.Concat(colorCodeList);

            DeckReadDTO deckCard = new DeckReadDTO
            {
                Id = card.Id,
                Name = card.Name,
                ImageUrl = card.OriginalImageUrl,
                ColorId = sortedColorCode.ToLower(),
                Manacost = result.ManaCost,
                ConvertedManaCost = result.ConvertedManaCost,
                quantity = 1,
            };

            return deckCard;

        }

        public async void DeleteCardFromDeck(DeckReadDTO card)
        {
            HttpClient? httpClient = HttpClientFactory?.CreateClient("DeckMinimalAPI");

            if (card.quantity > 1)
            {
                DeckReadDTO putCard = new DeckReadDTO
                {
                    Id = card.Id,
                    Name = card.Name,
                    ImageUrl = card.ImageUrl,
                    ColorId = card.ColorId,
                    ConvertedManaCost = card.ConvertedManaCost,
                    Manacost = card.Manacost,
                    quantity = card.quantity - 1
                };

                HttpContent content = new StringContent(JsonSerializer.Serialize(putCard), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"deckcards/{card.Id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    message = "something went wrong";
                }


            }
            else
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"deckcards/{card.Id}");

                if (!response.IsSuccessStatusCode)
                {
                    message = "something went wrong";
                }
            }

            await ReloadDeckList();
            StateHasChanged();
        }

        public async Task DeleteAllCardsFromDeck()
        {
            HttpClient? httpClient = HttpClientFactory?.CreateClient("DeckMinimalAPI");

            HttpResponseMessage response = await httpClient.DeleteAsync($"deckcards");

            if (!response.IsSuccessStatusCode)
            {
                message = "something went wrong";
            }

            await ReloadDeckList();
            StateHasChanged();
        }

        public char[] getManaSymbols(string manacost)
        {
            if (!string.IsNullOrEmpty(manacost))
            {
                char[] onlyLetters = manacost.Where(Char.IsLetter).ToArray();
                return onlyLetters;
            }

            char[] allSymbols = new char[0];
            return allSymbols;
        }

        public int getManaNumber(string manacost)
        {
            if (!string.IsNullOrEmpty(manacost))
            {
                string numericPart = new string(manacost.Where(char.IsDigit).ToArray());
                int manaNumber;
                if (int.TryParse(numericPart, out manaNumber))
                {
                    return manaNumber;
                }
            }
            return -1;
        }



        void ShowCardImage(string imageUrl, string elementId)
        {
            hoveredImageUrl = imageUrl;
            JSRuntime.InvokeVoidAsync("addClass", elementId, "searchCardPreview");
        }

        void HideCardImage(string elementId)
        {
            hoveredImageUrl = null;
            JSRuntime.InvokeVoidAsync("removeClass", elementId, "searchCardPreview");
        }
    }
}
