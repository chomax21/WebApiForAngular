using Angular_2.Data;
using Microsoft.AspNetCore.Mvc;

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

        public IResult GetToDoList(int id)
        {
            var toDo = _context.ToDoLists.FirstOrDefault(x=> x.Id == id);
            return null;
        }
    }
}
