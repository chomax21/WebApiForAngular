namespace Angular_2.Models
{
    public class User
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Hobby { get; set; }
        public string City { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"[FirstName: {FirstName}, LastName: {LastName}, Age: {Age}, Hobby: {Hobby}, City: {City}]"; 
        }
    }
}
