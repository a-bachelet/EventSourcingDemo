using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain.Read.ToDoList.Query;
using Domain.Write.ToDoList.Command;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controller
{
    [Route("api/todolists")]
    public class ToDoListController : ControllerBase
    {
        #region Queries

        [HttpGet]
        public async Task<IActionResult> GetToDoLists([FromQuery] GetToDoLists query)
        {
            var response = await _mediator.Send(query);

            return Ok(response.Data.ToList());
        }

        [HttpGet("{toDoListId}")]
        public async Task<IActionResult> GetToDoList([FromRoute] GetToDoList query)
        {
            var response = await _mediator.Send(query);

            return response.Data != null ? Ok(response.Data) : NotFound();
        }

        [HttpGet("{toDoListId}/todos")]
        public async Task<IActionResult> GetToDoListToDos([FromRoute] GetToDoListToDos query)
        {
            var response = await _mediator.Send(query);

            return response.Data != null ? Ok(response.Data) : NotFound();
        }
        
        #endregion
        
        #region Commands
        
        private readonly IMediator _mediator;

        public ToDoListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddToDoList([FromBody] AddToDoList command)
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
        
        [HttpPut("{toDoListId}")]
        public async Task<IActionResult> UpdateToDoList([FromRoute, FromBody] UpdateToDoList command)
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

        [HttpDelete("{toDoListId}")]
        public async Task<IActionResult> DeleteToDoList([FromRoute] DeleteToDoList command)
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
        
        #endregion
    }
}