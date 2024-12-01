using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoListAPI.Models;
using TodoListAPI.Services;

namespace TodoListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        public TodosController()
        {
        }


        [HttpGet]
        public ActionResult<List<Todos>> GetAll() => TodosService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Todos> Get(int id)
        {
            var todo = TodosService.GetTodos(id);

            if(todo == null) return NotFound();

            return todo;
            
        }

        [HttpPost]
        public ActionResult Create(Todos todos)
        {
            TodosService.CreateTodos(todos);

            return CreatedAtAction(nameof(Get), new {id = todos.Id}, todos);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Todos todo)
        {
            if(id != todo.Id) return BadRequest();

            var existingPizza = TodosService.GetTodos(id);

            if(existingPizza is null) return NotFound();

            TodosService.UpdateTodo(todo);

            return NoContent();
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, Todos todos)
        {   
            var todo = TodosService.GetTodos(id);

            if(todo is null) return NoContent();

            TodosService.DeleteTodo(id);

            return NoContent();
        }
    }
}
