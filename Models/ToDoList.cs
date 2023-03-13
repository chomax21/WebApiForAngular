namespace Angular_2.Models
{
    public class ToDoList       
    {
        public int Id { get; set; }
        public string Case { get; set; }
        public DateTimeOffset DateTime { get; set; } = DateTimeOffset.Now;
        public bool IsDone { get; set; } = false;    
        public int Priority { get; set; }
        public string UserId { get; set; }

    }
}
