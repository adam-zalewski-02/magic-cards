using Howest.MagicCards.Shared.DTO.Card;
using Howest.MagicCards.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Howest.MagicCards.WebAPI.Controllers
{

    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private const string _key = "allCards";
        private readonly ICardRepository _cardRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public CardsController(ICardRepository cardRepository, IMapper mapper, IMemoryCache memoryCache)
        {
            _cardRepo = cardRepository;
            _mapper = mapper;
            _cache = memoryCache;
        }

        private async Task<IEnumerable<Card>>GetCardsAsync()
        {
            if (!_cache.TryGetValue(_key, out IEnumerable<Card> cachedResult))
            {
                cachedResult = await _cardRepo.GetAllCards().ToListAsync();
                                                      

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _cache.Set(_key, cachedResult, cacheOptions);

            }
            return cachedResult;
        }

        [HttpGet, MapToApiVersion("1.1")]
        public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards([FromQuery] CardFilter filter, [FromServices] IConfiguration config)
        {
            IEnumerable<Card> cachedResult = await GetCardsAsync();
            filter.MaxPageSize = int.Parse(config["MaxPageSize"]);
            filter.PageSize = int.Parse(config["PageSize"]);

            return (cachedResult.AsQueryable() is IQueryable<Card> allCards)
                   ? Ok(new PagedResponse<IEnumerable<CardReadDTO>>(
                       allCards
                       .toFilteredList
                       (
                           filter.ArtistName ?? string.Empty,
                           filter.CardName ?? string.Empty,
                           filter.CardText ?? string.Empty,
                           filter.CardType ?? string.Empty,
                           filter.Set ?? string.Empty,
                           filter.CardRarity ?? string.Empty
                       )
                       .ToPagedList(filter.PageNumber, filter.PageSize)
                       .ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider),
                                    

                    filter.PageNumber,
                    filter.PageSize)
                   {
                       TotalRecords = allCards
                       .toFilteredList
                       (
                           filter.ArtistName ?? string.Empty,
                           filter.CardName ?? string.Empty,
                           filter.CardText ?? string.Empty,
                           filter.CardType ?? string.Empty,
                           filter.Set ?? string.Empty,
                           filter.CardRarity ?? string.Empty
                       )
                       .Count()
                   })

                    : NotFound(new Response<CardReadDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = "No cards found "
                    }
                    );
        }

        [HttpGet, MapToApiVersion("1.5")]
        public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCardsSort([FromQuery] CardFilter filter, [FromServices] IConfiguration config)
        {
            IEnumerable<Card> cachedResult = await GetCardsAsync();
            filter.MaxPageSize = int.Parse(config["MaxPageSize"]);
            filter.PageSize = int.Parse(config["PageSize"]);

            return (cachedResult.AsQueryable() is IQueryable<Card> allCards)
                   ? Ok(new PagedResponse<IEnumerable<CardReadDTO>>(
                       allCards
                       .toFilteredList
                       (
                           filter.ArtistName ?? string.Empty,
                           filter.CardName ?? string.Empty,
                           filter.CardText ?? string.Empty,
                           filter.CardType ?? string.Empty,
                           filter.Set ?? string.Empty,
                           filter.CardRarity ?? string.Empty
                       )
                       .Sort(filter.OrderBy ?? string.Empty)
                       .ToPagedList(filter.PageNumber, filter.PageSize)
                       .ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider),


                    filter.PageNumber,
                    filter.PageSize)
                   {
                       TotalRecords = allCards
                       .toFilteredList
                       (
                           filter.ArtistName ?? string.Empty,
                           filter.CardName ?? string.Empty,
                           filter.CardText ?? string.Empty,
                           filter.CardType ?? string.Empty,
                           filter.Set ?? string.Empty,
                           filter.CardRarity ?? string.Empty
                       )
                       .Count()
                   })

                    : NotFound(new Response<CardReadDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = "No cards found "
                    }
                    );
        }

        [HttpGet("{id:int}", Name ="GetCardById"), MapToApiVersion("1.5")]
        public async Task<ActionResult<Response<CardDetailReadDTO>>> GetCardById(int id)
        {
            return (await _cardRepo.GetCardByIdAsync(id) is Card foundCard)
                    ? Ok(_mapper.Map<CardDetailReadDTO>(foundCard))
                    : NotFound(new Response<CardReadDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = $"No card found with id {id}"
                    }
                    );

        }
    }
}
