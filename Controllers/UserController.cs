using Microsoft.AspNetCore.Mvc;


namespace Angular_2.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {
            Users = UserList.GetUserLIst();
        }
        public List<User> Users { get; set; }


        [HttpGet]
        [Route("/User/GetUser/{id}")]
        public User GetUser(int id)
        {
            var user = Users.FirstOrDefault(x => x.Id == id);
            if (user!=null)
            {
                return user;
                //return Results.Json(user);
            }
            //return Results.NotFound(new { message = "Error in Get method" });
            return null;
        }

        [HttpGet]
        [Route("/User/GetAllusers")]
        public IResult GetAllUsers()
        {
            return Results.Json(Users);
        }

        [HttpPost]
        [Route("/User/CreateUser")]
        public IResult CreateNewUser(User user)
        {
            if (user == null)
                return Results.NotFound(new { message = "Error in Create method"});
            var newUser = UserList.AddNewUser(user.FirstName, user.LastName, user.Age, user.Hobby, user.City);
                return Results.Json(newUser,statusCode: 200);
        }
        [HttpPut]
        [Route("/User/UpdateUser")]
        public IResult UpdateUser(User userIn)
        {
            if (userIn == null)
                return Results.NotFound(new { message = "Error in Upadete method"});
            var result = UserList.UpdateUser(userIn);
            if(result == null)
                return Results.NotFound(new { message = "Error in Update method" });
            return Results.Json(userIn, statusCode: 200);
        }
        [HttpDelete]
        [Route("/User/DeleteUser")]
        public IResult DeleteUser(User userIn)
        {
            if (userIn == null)
                return Results.NotFound(new { message = "Error in delete method" });
            var result = UserList.DeleteUser(userIn);
            if (result == null)
                return Results.NotFound(new { message = "Error in delete method" });
            return Results.Json(userIn,statusCode: 200);
        }
    }

    class UserList
    {
        private static List<User> instance;
        protected UserList()
        {
            instance = new List<User>()
            {
                new User { Id = 1, FirstName = "Max", LastName = "Yakovelev", City = "Cheboksary", Hobby = "Gaming" },
                new User { Id = 2, FirstName = "Kirill", Age = 23, LastName = "Yakovelev", City = "Cheboksary", Hobby = "Gaming" },
                new User { Id = 3, FirstName = "Dmitriy", LastName = "Tsarev", City = "Cheboksary", Hobby = "Gaming" },
                new User { Id = 4, FirstName = "Alexander", LastName = "Egorov", City = "Cheboksary", Hobby = "Gaming" },
                new User { Id = 5, FirstName = "Ivan", LastName = "Susanin", City = "Cheboksary", Hobby = "Gaming" }

            };
        }

        public static List<User> GetUserLIst()
        {
            if(instance == null)
            {
                new UserList();
                return instance;
            }
            return instance;
        }

        public static User AddNewUser(string fName, string lName, int age, string hobby, string city)
        {
            var ID = instance.Max(x => x.Id);

            User user = new();
            user.Id = ID + 1;
            user.FirstName = fName;
            user.LastName = lName;
            user.City = city;
            user.Hobby = hobby;
            user.Age = age;
            instance.Add(user);

            return user;
        }
        public static User UpdateUser(User user)
        {
            var resultUser = instance.FirstOrDefault(x => x.Id == user.Id);
            if (user == null)
                return null;
            resultUser.Age = user.Age;
            resultUser.FirstName = user.FirstName;
            resultUser.LastName = user.LastName;
            resultUser.City = user.City;
            resultUser.Hobby = user.Hobby;
            return resultUser;

        }
        public static User DeleteUser(User user)
        {
            var searchUser = instance.FirstOrDefault(x => x.Id == user.Id);
            instance.Remove(searchUser);
            return searchUser;
        }
    }
}
