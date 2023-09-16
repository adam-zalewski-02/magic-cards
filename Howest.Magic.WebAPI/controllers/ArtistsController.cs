using Howest.MagicCards.Shared.DTO.Artist;
using Howest.MagicCards.Shared.DTO.Rarity;
using Howest.MagicCards.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistRepository _artistRepo;
        private readonly IMapper _mapper;

        public ArtistsController(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepo = artistRepository;
            _mapper = mapper;
        }


        [HttpGet, MapToApiVersion("1.1"), MapToApiVersion("1.5")]
        public ActionResult<PagedResponse<IEnumerable<ArtistDetailReadDTO>>> GetArtists([FromQuery] PaginationFilter filter, [FromServices] IConfiguration config)
        {
            filter.MaxPageSize = int.Parse(config["maxPageSize"]);

            return (_artistRepo.GetAllArtists() is IQueryable<Artist> allArtists)
                   ? Ok(new PagedResponse<IEnumerable<ArtistDetailReadDTO>>(
                       allArtists.ToPagedList(filter.PageNumber, filter.PageSize)
                       .ProjectTo<ArtistDetailReadDTO>(_mapper.ConfigurationProvider),
                                    

                    filter.PageNumber,
                    filter.PageSize)
                   {
                       TotalRecords = allArtists.Count()
                   })

                   : NotFound(new Response<ArtistDetailReadDTO>()
                   {
                       Succeeded = false,
                       Errors = new string[] { "404" },
                       Message = "No artists found"
                   }
                   );
        }

        [HttpGet("{id:int}", Name = "GetArtistById"), MapToApiVersion("1.1"), MapToApiVersion("1.5")]
        public async Task<ActionResult<ArtistReadDTO>> GetArtist(int id)
        {
            return (await _artistRepo.GetArtistByIdAsync(id) is Artist foundArtist)
                    ? Ok(_mapper.Map<ArtistReadDTO>(foundArtist))
                    : NotFound(new Response<RarityReadDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = $"No artist found with id {id}"
                    }
                    );
        }
    }
}
