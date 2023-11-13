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
    [Route ("[controller]")]
#if ProducesConsumes
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
#endif
    public class TeacherLocationInterestController : ControllerBase
    {
        private readonly ILogger<TeacherLocationInterestController> _logger;
        private readonly ISISTeacherLocationInterestRepository _repository;
        private readonly IMapper _mapper;

        public TeacherLocationInterestController(ILogger<TeacherLocationInterestController> logger, ISISTeacherLocationInterestRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TeacherLocationInterestDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public ActionResult<IEnumerable<TeacherLocationInterestDTO>> Get()
        {
            return Ok(_mapper.Map<List<TeacherLocationInterestDTO>>(_repository.TeacherLocationInterests.Values.ToList()));
        }


        [HttpDelete(Name = "DeleteTeacherLocationInterest")]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public ActionResult Delete([Required] int id)
        {
            var teacherLocationInterestToDelete = _repository.TeacherLocationInterests.Values
                                                                .Where(tli => tli.TeacherLocationInterestId == id)
                                                                .FirstOrDefault();
            if (teacherLocationInterestToDelete == null)
            {
                return NotFound();
            }

            _repository.Delete(teacherLocationInterestToDelete);
            return NoContent();
        }

        [HttpPut]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public IActionResult Put([Required] int id, [FromBody][Required] TeacherLocationInterestDTO dto)
        {
            //If DTO comes back with default values or is null
            if (!IsValid(dto))
            {
                return BadRequest("Invalid request data.");
            }

            //Get instance by id
            var locationInterestToUpdate = _repository.TeacherLocationInterests.Values.FirstOrDefault(li => li.TeacherLocationInterestId == id);

            //If instance does not exist
            if (locationInterestToUpdate == null)
            {
                return NotFound();
            }

            //else:  UPDATE
            _repository.Update(locationInterestToUpdate, _mapper.Map<TeacherLocationInterest>(dto));
            return Ok($"Teacher Location Interest with id:{id} has succesfully been updated.");

        }


        [HttpPost]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public IActionResult Post([FromBody][Required] TeacherLocationInterestDTO dto)
        {
            //If DTO comes back with default values or is null
            if (!IsValid(dto))
            {
                return BadRequest("Invalid request data.");
            }

            var locationInterestToCreate = _mapper.Map<TeacherLocationInterest>(dto);

            //else INSERT
            var efLocationInterestId = _repository.Insert(locationInterestToCreate);
            dto.TeacherLocationInterestId = efLocationInterestId;
            return CreatedAtAction(nameof(Get), new { id = efLocationInterestId }, dto);
        }

        private bool IsValid(TeacherLocationInterestDTO dto)
        {
            // Api sets default date to today, so to check if the dto is send back without changing all the 
            // default values we need to check for today's date

            var dateTimeNow = new DateTime();
            DateOnly dateNow = DateOnly.FromDateTime(dateTimeNow);

            if (dto == null ||
                (dto.AcademicYearStart == dateNow
                && dto.AcademicYearStop == dateNow
                && dto.TeacherFirstName == "string"
                && dto.TeacherLastName == "string"
                && dto.TeacherPreferenceDescription == "string"
                && dto.LocationName == "string"))
            { return false; }

            return true;
        }
    }
}
