using Howest.MagicCards.Shared.DTO.Rarity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Howest.MagicCards.WebAPI.controllers
{
    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SetController : ControllerBase
    {
        private readonly ISetRepository _setRepo;
        private readonly IMapper _mapper;

        public SetController(ISetRepository setRepo, IMapper mapper)
        {
            _setRepo = setRepo;
            _mapper = mapper;
        }


        [HttpGet, MapToApiVersion("1.1"), MapToApiVersion("1.5")]
        public async Task<ActionResult<IEnumerable<SetReadDetailDTO>>> GetSets([FromServices] IConfiguration config)
        {

            return (_setRepo.GetAllSets() is IQueryable<Set> allSets)
                   ? Ok(await allSets.ProjectTo<SetReadDetailDTO>(_mapper.ConfigurationProvider).ToListAsync())
            : NotFound(new Response<SetReadDetailDTO>()
            {
                Succeeded = false,
                Errors = new string[] { "404" },
                Message = "No sets found"
            }
            );
        }
    }
}
