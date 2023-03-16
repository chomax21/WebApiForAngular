using Angular_2.Data;
using Angular_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
        [Route("create-user")]
        public async Task<IResult> CreateUser(User user)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Login == user.Login);
            if (result == null)
            {
                user.UserId = Guid.NewGuid().ToString();
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Results.Ok($"Пользователь {user.Login} успешно создан!");
            }
            return Results.Conflict("Пользователь с таким логином уже существует!");
        }


        [HttpGet]
        [Route("get-user-id")]
        public async Task<IResult> GetUserId(string login, string password) 
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            if (result != null)
                return Results.Ok(result.UserId);
            return Results.NotFound("Нет ифнормации!!!");        }

        [HttpPost]
        [Route("create")]
        public async Task<IResult> CreateToDoList(ToDoList doList)
        {                       
            if (!string.IsNullOrEmpty(doList.Case))
            {
                var user = _context.Users.FirstOrDefault(x=> x.UserId == doList.UserId);
                if (user != null)
                {
                    doList.UserId= user.UserId;
                    doList.Case = doList.Case;
                    doList.Priority = doList.Priority;
                    await _context.AddAsync(doList);
                    await _context.SaveChangesAsync();
                    return Results.Json("Succes", statusCode: 200);
                }
                else
                {
                    return Results.Conflict("Пользователя не существует!");
                }              
            }
            return Results.Json("Нет информации!!!");
        }

        [HttpGet]
        [Route("getready")]
        public async Task<IResult> GetToDoLists(string UserId)
        {
            if (UserId == null)
            {
                return Results.NotFound("Не пришел ID");
            }
            var toDoCase = _context.ToDoLists.Where(x => x.UserId == UserId);
            if (toDoCase == null)
            {
                return Results.NotFound();
            }
            var list = JsonSerializer.Serialize(toDoCase);
            return Results.Json(toDoCase, statusCode: 200);
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
            var toDoCase = await _context.ToDoLists.FirstOrDefaultAsync(x => x.Id == id);
            if (toDoCase != null)
            {
                _context.ToDoLists.Remove(toDoCase);
                return Results.Json(toDoCase, statusCode: 200);
            }
            return Results.NotFound();
        }
    }
}
