using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("/Todo")]
        public ActionResult<List<TodoModel>> Get([FromServices] AppDbContext context)
            => Ok(context.Todos!.ToList());

        [HttpGet]
        [Route("/Todo/{id:int}")]
        public ActionResult<TodoModel> GetById([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var todo = context.Todos!.FirstOrDefault(x => x.Id == id)!;
            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost]
        [Route("/Todo")]
        public ActionResult<TodoModel> Post([FromServices] AppDbContext context, [FromBody] TodoModel todo)
        {
            context.Todos?.Add(todo);
            context.SaveChanges();
            return Created($"/Todo/{todo.Id}", todo);
        }

        [HttpPut]
        [Route("/Todo/{id:int}")]
        public ActionResult<TodoModel> Put([FromRoute] int id, [FromBody] TodoModel todo, [FromServices] AppDbContext context)
        {
            var model = context.Todos!.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return NotFound();

            model.Title = todo.Title;
            model.Done = todo.Done;

            context.Todos!.Update(model);
            context.SaveChanges();

            return Ok(model);
        }

        [HttpDelete]
        [Route("/Todo/{id:int}")]
        public ActionResult Delete([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var model = context.Todos!.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return NotFound();
                
            context.Todos!.Remove(model);
            context.SaveChanges();

            return Ok();
        }
    }
}