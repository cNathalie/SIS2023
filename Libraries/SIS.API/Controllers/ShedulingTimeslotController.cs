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
    public class ShedulingTimeslotController : ControllerBase
    {
        private readonly ILogger<ShedulingTimeslotController> _logger;
        private readonly ISISShedulingTimeslotRepository _repository;
        private readonly IMapper _mapper;

        public ShedulingTimeslotController(ILogger<ShedulingTimeslotController> logger, ISISShedulingTimeslotRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ShedulingTimeslotDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public ActionResult<IEnumerable<ShedulingTimeslotDTO>> Get()
        {
            return Ok(_mapper.Map<List<ShedulingTimeslotDTO>>(_repository.ShedulingTimeslots.Values.ToList()));
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
            var timeslotToDelete = _repository.ShedulingTimeslots.Values.FirstOrDefault(ts => ts.SchedulingTimeslotId == id);
            if (timeslotToDelete == null)
            {
                return NotFound();
            }

            _repository.Delete(timeslotToDelete);
            return NoContent();
        }


        [HttpPut]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public IActionResult Put([Required] int id, [FromBody][Required] ShedulingTimeslotDTO dto)
        {
            //If DTO comes back with default values or is null
            if (!IsValid(dto))
            {
                return BadRequest("Invalid request data.");
            }

            //Get instance by id
            var timeslotToUpdate = _repository.ShedulingTimeslots.Values.FirstOrDefault(ts => ts.SchedulingTimeslotId == id);

            //If instance does not exist
            if (timeslotToUpdate == null)
            {
                return NotFound();
            }

            //else:  UPDATE
            _repository.Update(timeslotToUpdate, _mapper.Map<ShedulingTimeslot>(dto));
            return Ok($"Timeslot with id:{id} has succesfully been updated.");

        }

        [HttpPost]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public IActionResult Post([FromBody][Required] ShedulingTimeslotDTO dto)
        {
            //If DTO comes back with default values or is null
            if (!IsValid(dto))
            {
                return BadRequest("Invalid request data.");
            }

            var timeslotToCreate = _mapper.Map<ShedulingTimeslot>(dto);

            //else INSERT
            var efTimeslotId = _repository.Insert(timeslotToCreate);
            dto.SchedulingTimeslotId = efTimeslotId;
            return CreatedAtAction(nameof(Get), new { id = efTimeslotId }, dto);
        }

        private bool IsValid(ShedulingTimeslotDTO dto)
        {
            if (dto == null 
                || dto.Name == "string"
                || dto.Description == "string"
                || dto.StartTime == default
                || dto.StopTime == default
                )
            { return false; }

            return true;
        }

    }
}
