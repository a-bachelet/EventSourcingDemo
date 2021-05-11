using System.Net;
using System.Threading.Tasks;
using Domain.Read.ToDo.Query;
using Domain.Write.ToDo.Command;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controller
{
    [Route("api/todos")]
    public class ToDoController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public ToDoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Queries

        [HttpGet("{toDoId}")]
        public async Task<IActionResult> GetToDo([FromRoute] GetToDo query)
        {
            var response = await _mediator.Send(query);

            return response.Data != null ? Ok(response.Data) : NotFound();
        }

        #endregion
        
        #region Commands
        
        [HttpPost]
        public async Task<IActionResult> AddToDo([FromBody] AddToDo command)
        {
            try
            {
                await _mediator.Send(command);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }

            return StatusCode((int) HttpStatusCode.Created);
        }

        [HttpPut("{toDoId}")]
        public async Task<IActionResult> UpdateToDo([FromRoute, FromBody] UpdateToDo command)
        {
            try
            {
                await _mediator.Send(command);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }

            return StatusCode((int) HttpStatusCode.Created);
        }
        
        [HttpPost("{toDoId}/start")]
        public async Task<IActionResult> StartToDo([FromRoute] StartToDo command)
        {
            try
            {
                await _mediator.Send(command);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }

            return StatusCode((int) HttpStatusCode.OK);
        }
        
        [HttpPost("{toDoId}/finish")]
        public async Task<IActionResult> StartToDo([FromRoute] FinishToDo command)
        {
            try
            {
                await _mediator.Send(command);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }

            return StatusCode((int) HttpStatusCode.OK);
        }
        
        [HttpPost("{toDoId}/reset")]
        public async Task<IActionResult> ResetToDo([FromRoute] ResetToDo command)
        {
            try
            {
                await _mediator.Send(command);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }

            return StatusCode((int) HttpStatusCode.OK);
        }

        [HttpDelete("{toDoId}")]
        public async Task<IActionResult> DeleteToDo([FromRoute] DeleteToDo command)
        {
            try
            {
                await _mediator.Send(command);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }

            return StatusCode((int) HttpStatusCode.NoContent);
        }
        
        #endregion
    }
}