using Angular_2.Data;
using Angular_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Angular_2.Controllers
{
    [ApiController]
    [Route("api")]
    public class ToDoController : ControllerBase     
    {
        private readonly ApplicationContext _context;

        public ToDoController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IResult> CreateToDoList(int id, string message, int priority = 0)
        {
            var toDoCase = await _context.ToDoLists.FirstOrDefaultAsync(x => x.Id == id && x.IsDone == false);
            if (toDoCase == null) 
            {
                try
                {
                    ToDoList doList = new();
                    doList.Case = message;
                    doList.Priority = priority;
                    await _context.AddAsync(doList);
                    await _context.SaveChangesAsync();
                    return Results.Json("Create a new ToDoCase", null, "text", 200);
                }
                catch (ArgumentException aEx)
                {
                    return Results.Json("Аргументом приоритета являются значения от 0 до 2! " + aEx.Message, statusCode: 400);
                }                
            }            
            return Results.Json("ID Занят!!!");
        }

        [HttpGet]
        [Route("getready")]
        public async Task<IResult> GetToDoList(int id)
        {
            var toDoCase = await _context.ToDoLists.FirstOrDefaultAsync(x => x.Id == id && x.IsDone == true);
            if (toDoCase == null)
            {
                return Results.NotFound();
            }
            return Results.Json(toDoCase);
        }

        [HttpGet]
        [Route("getnotready")]
        public async Task<IResult> GetNotComplitedToDoList(int id)
        {
            var toDoCase = await _context.ToDoLists.FirstOrDefaultAsync(x => x.Id == id && x.IsDone == false);
            if (toDoCase == null)
            {
                return Results.NotFound();
            }
            return Results.Json(toDoCase, statusCode: 200);
        }

        [HttpPut]
        [Route("change")]
        public async Task<IResult> ChangeStatus(int id)
        {
            var toDoCase = await _context.ToDoLists.FirstOrDefaultAsync(x => x.Id == id);
            if (toDoCase == null)
            {
                return Results.NotFound();
            }
            toDoCase.IsDone = true;
            await _context.SaveChangesAsync();
            return Results.Json(toDoCase, statusCode: 200);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IResult> DeleteToDoList(int id)
        {
            var toDoCase = await _context.ToDoLists.FirstOrDefaultAsync(x=>x.Id == id);
            if (toDoCase != null)
            {
                _context.ToDoLists.Remove(toDoCase);
                return Results.Json(toDoCase, statusCode: 200);
            }
            return Results.NotFound();
        }
    }
}
