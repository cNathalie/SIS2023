using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SIS.Domain;
using SIS.Domain.Interfaces;
using SISAPI.DTO;
using System.ComponentModel.DataAnnotations;
using System.Data;
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


        [HttpDelete]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public ActionResult Delete([Required] int id)
        {
            var roleInterestToDelete = _repository.TeacherCoordinationRoleInterests.Values.FirstOrDefault(ri => ri.TeacherCoordinationRoleInterestId == id);
            if (roleInterestToDelete == null)
            {
                return NotFound();
            }

            _repository.Delete(roleInterestToDelete);
            return NoContent();
        }


        [HttpPut]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public IActionResult Put([Required] int id, [FromBody][Required] TeacherCoordinationRoleInterestDTO dto)
        {
            //If DTO comes back with default values or is null
            if (!IsValid(dto))
            {
                return BadRequest("Invalid request data.");
            }

            //Get instance by id
            var roleInterestToUpdate = _repository.TeacherCoordinationRoleInterests.Values.FirstOrDefault(ri => ri.TeacherCoordinationRoleInterestId == id);

            //If instance does not exist
            if (roleInterestToUpdate == null)
            {
                return NotFound();
            }

            //else:  UPDATE
            _repository.Update(roleInterestToUpdate, _mapper.Map<TeacherCoordinationRoleInterest>(dto));
            return Ok($"Teacher CoordinationRole Interest with id:{id} has succesfully been updated.");

        }

        [HttpPost]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public IActionResult Post([FromBody][Required] TeacherCoordinationRoleInterestDTO dto)
        {
            //If DTO comes back with default values or is null
            if (!IsValid(dto))
            {
                return BadRequest("Invalid request data.");
            }

            var roleInterestToCreate = _mapper.Map<TeacherCoordinationRoleInterest>(dto);

            //else INSERT
            var efRoleInterestId = _repository.Insert(roleInterestToCreate);
            dto.TeacherCoordinationRoleInterestId = efRoleInterestId;
            return CreatedAtAction(nameof(Get), new { id = efRoleInterestId }, dto);
        }

        private bool IsValid(TeacherCoordinationRoleInterestDTO dto)
        {
            var dateTimeNow = new DateTime();
            DateOnly dateNow = DateOnly.FromDateTime(dateTimeNow);

            if (dto == null
                || dto.AcademicYearStart == dateNow
                || dto.AcademicYearStop == dateNow
                || dto.TeacherFirstName == "string"
                || dto.TeacherLastName == "string"
                || dto.TeacherPreference == "string"
                || dto.CoordinationRole == "string"
                )
            { return false; }

            return true;
        }

    }
}
