using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SIS.Domain;
using SIS.Domain.Interfaces;
using SISAPI.DTO;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace SIS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
#if ProducesConsumes
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
#endif
    public class PeriodController : ControllerBase
    {
        private readonly ILogger<PeriodController> _logger;
        private readonly ISISPeriodRepository _repository;
        private readonly IMapper _mapper;

        public PeriodController(ILogger<PeriodController> logger, ISISPeriodRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PeriodDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public ActionResult<IEnumerable<PeriodDTO>> Get()
        {
            return Ok(_mapper.Map<List<PeriodDTO>>(_repository.Periods.Values.ToList()));
        }


        [HttpDelete]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public ActionResult Delete([Required] int id)
        {
            var periodToDelete = _repository.Periods.Values.FirstOrDefault(p => p.PeriodId == id);
            if (periodToDelete == null)
            {
                return NotFound();
            }

            _repository.Delete(periodToDelete);
            return NoContent();
        }

        [HttpPut]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public IActionResult Put([Required] int id, [FromBody][Required] PeriodDTO dto)
        {
            //If DTO comes back with default values or is null
            if (!IsValid(dto))
            {
                return BadRequest("Invalid request data.");
            }

            //Get instance by id
            var coordinationRoleToUpdate = _repository.Periods.Values.FirstOrDefault(p => p.PeriodId == id);

            //If instance does not exist
            if (coordinationRoleToUpdate == null)
            {
                return NotFound();
            }

            //else:  UPDATE
            _repository.Update(coordinationRoleToUpdate, _mapper.Map<Period>(dto));
            return Ok($"Period with id:{id} has succesfully been updated.");

        }

        [HttpPost]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public IActionResult Post([FromBody][Required] PeriodDTO dto)
        {
            //If DTO comes back with default values or is null
            if (!IsValid(dto))
            {
                return BadRequest("Invalid request data.");
            }

            var periodToCreate = _mapper.Map<Period>(dto);

            //else INSERT
            var efPeriodId = _repository.Insert(periodToCreate);
            dto.PeriodId = efPeriodId;
            return CreatedAtAction(nameof(Get), new { id = efPeriodId }, dto);
        }

        private bool IsValid(PeriodDTO dto)
        {
            if (dto == null ||
                (dto.Name == "string"))
            { return false; }

            return true;
        }

    }
}
