using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SIS.Domain.Interfaces;
using SISAPI.DTO;
using System.Net.Mime;

namespace SIS.API.Controllers
{
    [ApiController]
    [Route ("[controller]")]
#if ProducesConsumes
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
#endif
    public class TeacherCoordinationRoleInterestController : ControllerBase
    {
        private readonly ILogger<TeacherCoordinationRoleInterestController> _logger;
        private readonly ISISTeacherCoordinationRoleInterestRepository _repository;
        private readonly IMapper _mapper;

        public TeacherCoordinationRoleInterestController(ILogger<TeacherCoordinationRoleInterestController> logger, ISISTeacherCoordinationRoleInterestRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TeacherCoordinationRoleInterestDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public ActionResult<IEnumerable<CoordinationRoleDTO>> Get()
        {
            return Ok(_mapper.Map<List<CoordinationRoleDTO>>(_repository.TeacherCoordinationRoleInterests.Values.ToList()));
        }

    }
}
