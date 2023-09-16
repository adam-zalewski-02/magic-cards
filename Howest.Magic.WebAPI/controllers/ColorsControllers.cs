using Howest.MagicCards.Shared.DTO.Color;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.WebAPI.controllers
{
    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorRepository _colorRepo;
        private readonly IMapper _mapper;

        public ColorsController(IColorRepository colorRepo, IMapper mapper)
        {
            _colorRepo = colorRepo;
            _mapper = mapper;
        }


        [HttpGet, MapToApiVersion("1.1"), MapToApiVersion("1.5")]
        public async Task<ActionResult<IAsyncEnumerable<ColorReadDTO>>> GetColors([FromServices] IConfiguration config)
        {

            return (_colorRepo.GetAllColors() is IQueryable<Colors> allColors)
                   ? Ok(await allColors.ProjectTo<ColorReadDTO>(_mapper.ConfigurationProvider).ToListAsync())
                    : NotFound(new Response<ColorReadDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = "No colors found"
                    }
            );
        }
    }
}
