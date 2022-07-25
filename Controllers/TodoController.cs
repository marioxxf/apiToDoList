using meuToDo.Data;
using meuToDo.Models;
using meuToDo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace meuToDo.Controllers
{
    [ApiController]
    [Route(template:"v1")]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        [Route(template:"todos")]
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context)
        {
            List<Todo> todos = new List<Todo> { };
                todos = await context // Nosso AppDbContext
                .Todos // DbSet<Todo>
                .AsNoTracking() // IQueryable<Todo>
                .ToListAsync(); // Task<List<...>>
            return Ok(todos);
        }

        [HttpGet]
        [Route(template: "todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            var todo = new Todo();
            var x = new Todo();
            todo = await context // Nosso AppDbContext
            .Todos // DbSet<Todo>
            .AsNoTracking() // IQueryable<Todo>
            .FirstOrDefaultAsync(x => x.Id == id);

            return todo == null 
                ? NotFound()
                : Ok(todo);
        }

        [HttpPost(template:"todos")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model)
        {
            /*if (ModelState.IsValid)
                return BadRequest();*/
            
            var todo = new Todo
                {
                    Date = DateTime.Now,
                    Done = false,
                    Title = model.Title
                };

                try
                {
                    await context.Todos.AddAsync(todo);
                    await context.SaveChangesAsync();
                    return Created(uri:$"v1/todos/{todo.Id}", todo);
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
        }

        [HttpPut(template: "todos/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model,
            [FromRoute] int id)
        {
            /*if (ModelState.IsValid)
                return BadRequest();*/

            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
                return NotFound();

            try
            {
                todo.Title = model.Title;

                context.Todos.Update(todo);
                await context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception e )
            {
                return BadRequest();
            }
        }

        [HttpPut(template: "todos/{id}/finish")]
        public async Task<IActionResult> DoneAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            /*if (ModelState.IsValid)
                return BadRequest();*/

            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
                return NotFound();

            try
            {
                todo.Done = true;
                context.Todos.Update(todo);
                await context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
