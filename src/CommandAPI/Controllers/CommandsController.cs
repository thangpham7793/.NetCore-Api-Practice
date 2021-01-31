using System.Collections.Generic;
using CommandAPI.Data;
using CommandAPI.Dto;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandAPIRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands() => Ok(_mapper.Map<IEnumerable<CommandReadDto>>(_repo.GetAllCommands()));

        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var cmd = _repo.GetCommandById(id);
            if (cmd == null)
            {
                return NotFound();
            }
            // map cmd to CmdReadDto
            return Ok(_mapper.Map<CommandReadDto>(cmd));
        }

        [HttpPost]
        public ActionResult CreateCommand(CommandCreateDto dto)
        {
            var cmd = _mapper.Map<Command>(dto);
            _repo.CreateCommand(cmd);
            // this add Id to cmd (by reference)
            _repo.SaveChanges();

            var cmdReadDto = _mapper.Map<CommandReadDto>(cmd);
            return CreatedAtRoute(nameof(GetCommandById), new { Id = cmdReadDto.Id }, cmdReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto dto)
        {
            var existingCmd = _repo.GetCommandById(id);
            if (existingCmd == null)
            {
                return NotFound();
            }
            // modify the existing cmd in place, so doesn't return anything
            _mapper.Map(dto, existingCmd);
            _repo.UpdateCommand(existingCmd);
            _repo.SaveChanges();

            return NoContent();
        }

        private Command _getCommandByIdFromRepo(int id) => _repo.GetCommandById(id);

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var cmdToPatch = _getCommandByIdFromRepo(id);
            if (cmdToPatch == null)
            {
                return NotFound();
            }
            var dtoToPatch = _mapper.Map<CommandUpdateDto>(cmdToPatch);
            patchDoc.ApplyTo(dtoToPatch);
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(dtoToPatch, cmdToPatch);
            _repo.UpdateCommand(cmdToPatch);
            _repo.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteById(int id)
        {
            var cmdToDelete = _getCommandByIdFromRepo(id);
            if (cmdToDelete == null)
            {
                return NotFound();
            }
            _repo.DeleteCommand(cmdToDelete);
            _repo.SaveChanges();
            return NoContent();
        }
    }
}