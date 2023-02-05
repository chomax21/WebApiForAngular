using Angular_2.Data;
using Angular_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Angular_2.Controllers
{
    [ApiController]
    public class ToDoController : ControllerBase     
    {
        private readonly ApplicationContext _context;

        public ToDoController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> CreateToDoList(int id, string message)
        {
            var toDoCase = await _context.ToDoLists.FirstOrDefaultAsync(x => x.Id == id);
            if (toDoCase == null) 
            {
                ToDoList doList = new();
                doList.Case = message;
                doList.DateTimeDateTime = DateTime.Now;
                await _context.AddAsync(doList);
                await _context.SaveChangesAsync();
                return Results.Json("Create a new ToDoCase");
            }            
            return Results.Json(toDoCase);
        }
        public async Task<IResult> GetToDoList(int id)
        {
            var toDo = await _context.ToDoLists.FirstOrDefaultAsync(x => x.Id == id);
            if (toDo == null)
            {
                return Results.NotFound();
            }
            return Results.Json(toDo);
        }

        public async Task<IResult> ChangeStatus(int id)
        {
            var toDoCase = await _context.ToDoLists.FirstOrDefaultAsync(x => x.Id == id);
            if (toDoCase == null)
            {
                return Results.NotFound();
            }
            toDoCase.IsDone = true;
            await _context.SaveChangesAsync();
            return Results.Ok("Succesfull change!");
        }
    }
}
