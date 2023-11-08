using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SIS.API.DTO;
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
    public class CoordinationRoleController : ControllerBase
    {
        private readonly ILogger<CoordinationRoleController> _logger;
        private readonly ISISCoordinationRoleRepository _repository;
        private readonly IMapper _mapper;

        public CoordinationRoleController(ILogger<CoordinationRoleController> logger, ISISCoordinationRoleRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CoordinationRoleDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public ActionResult<IEnumerable<CoordinationRoleDTO>> Get()
        {
            return Ok(_mapper.Map<List<CoordinationRoleDTO>>(_repository.CoordinationRoles.Values.ToList()));
        }


        [HttpDelete(Name = "DeleteCoordinationRole")]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public ActionResult Delete([Required] int id)
        {
            var coordinationRoleToDelete = _repository.CoordinationRoles.Values.FirstOrDefault(cr => cr.CoordinationRoleId == id);
            if (coordinationRoleToDelete == null)
            {
                return NotFound();
            }

            _repository.Delete(coordinationRoleToDelete);
            return NoContent();
        }


        [HttpPut]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public IActionResult Put([Required] int id, [FromBody][Required] CoordinationRoleDTO dto)
        {
            //If DTO comes back with default values or is null
            if (!IsValid(dto))
            {
                return BadRequest("Invalid request data.");
            }

            //Get instance by id
            var coordinationRoleToUpdate = _repository.CoordinationRoles.Values.FirstOrDefault(cr => cr.CoordinationRoleId == id);

            //If instance does not exist
            if (coordinationRoleToUpdate == null)
            {
                return NotFound();
            }

            //else:  UPDATE
            _repository.Update(coordinationRoleToUpdate, _mapper.Map<CoordinationRole>(dto));
            return Ok($"Coordination Role with id:{id} has succesfully been updated.");

        }


        [HttpPost]
#if ProducesConsumes
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#endif
        public IActionResult Post([FromBody][Required] CoordinationRoleDTO dto)
        {
            //If DTO comes back with default values or is null
            if (!IsValid(dto))
            {
                return BadRequest("Invalid request data.");
            }

            var coordinationRoleToCreate = _mapper.Map<CoordinationRole>(dto);

            //else INSERT
            var efCoordinationRoleId = _repository.Insert(_mapper.Map<CoordinationRole>(dto));
            dto.CoordinationRoleId = efCoordinationRoleId;
            return CreatedAtAction(nameof(Get), new { id = efCoordinationRoleId }, dto);
        }


        private bool IsValid(CoordinationRoleDTO dto)
        {
            if (dto == null ||
                (dto.Name == "string"
                && dto.AssignmentPercentage == 0
                && dto.CoordinationRoleId == 0)) 
            { return false; }

            return true;
        }
    }
}
