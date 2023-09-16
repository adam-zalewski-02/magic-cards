using Howest.MagicCards.Shared.DTO.Card;
using Howest.MagicCards.Shared.DTO.Color;
using Howest.MagicCards.Shared.DTO.Rarity;
using Howest.MagicCards.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Howest.MagicCards.WebAPI.controllers
{
    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RarityController : ControllerBase
    {
        private readonly IRarityRepository _rarityRepo;
        private readonly IMapper _mapper;

        public RarityController(IRarityRepository rarityRepo, IMapper mapper)
        {
            _rarityRepo = rarityRepo;
            _mapper = mapper;
        }

        [HttpGet, MapToApiVersion("1.1"), MapToApiVersion("1.5")]
        public async Task<ActionResult<IEnumerable<RarityDetailReadDTO>>> GetRarities([FromServices] IConfiguration config)
        {

            return (_rarityRepo.GetAllRarities() is IQueryable<Rarity> allRarities)
                   ? Ok(await allRarities.ProjectTo<RarityDetailReadDTO>(_mapper.ConfigurationProvider).ToListAsync())
                    : NotFound(new Response<RarityDetailReadDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = "No rarities found"
                    }
            );
        }



        [HttpGet("{id:int}", Name = "GetRarityById"), MapToApiVersion("1.1"), MapToApiVersion("1.5")]
        public async Task<ActionResult<RarityReadDTO>> GetRarity(int id)
        {
            return (await _rarityRepo.GetRarityByIdAsync(id) is Rarity foundRarity)
                    ? Ok(_mapper.Map<RarityReadDTO>(foundRarity))
                    : NotFound(new Response<RarityReadDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = $"No rarity found with id {id}"
                    }
                    );
        }
    }
}
