using Angular_2.Data;
using Angular_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        [Route("user")]
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
        [Route("user-id")]
        public async Task<IResult> GetUserId(string login, string password) 
        {            
            var identity = GetIdentity(login, password);
            if (identity == null)
                return Results.BadRequest(new { errorText = "Нет ифнормации!!!" });

            var now = DateTime.UtcNow;
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("superpasswordsuperpassword"));
            var jwt = new JwtSecurityToken(
                issuer: "angular_2_server",
                audience: "angular_2_client",
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(1)),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            string userId = "userId";
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            if (result != null)
            {
                userId= result.UserId;
            }
            var response = new
            {
                acces_token = encodedJwt,
                id = userId,
            };
                return Results.Json(response);
        }

        private ClaimsIdentity GetIdentity(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Id",user.UserId),                  
                    new Claim("Login",user.Login)                   
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
                return claimsIdentity;
            }
            return null;
        }

        [HttpPost]
        [Route("item")]
        [Authorize]
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
        [Route("ready-item")]
        [Authorize]
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
        [Route("not-ready-item")]
        [Authorize]
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
        [Route("item")]
        [Authorize]
        public async Task<IResult> ChangeStatus(ToDoList doListInFront)
        {
            var doListInBack = await _context.ToDoLists.FirstOrDefaultAsync(x => x.Id == doListInFront.Id);
            if (doListInBack == null)
            {
                return Results.NotFound();
            }
            if (doListInBack.UserId == doListInFront.UserId)
            {
                doListInBack.Case = doListInFront.Case;
                doListInBack.Priority = doListInFront.Priority;
            }
            else
            {
                return Results.BadRequest("Data lost");
            }
            await _context.SaveChangesAsync();
            return Results.Json(doListInBack, statusCode: 200);
        }

        [HttpDelete]
        [Route("item")]
        [Authorize]
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
