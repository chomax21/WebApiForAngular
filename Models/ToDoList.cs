namespace Angular_2.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string Case { get; set; }
        public DateTimeOffset DateTime { get; set; } = DateTimeOffset.Now;
        public bool IsDone { get; set; } = false;
        public int Priority
        {
            get { return Priority; }
            set 
            {
                if (value < 0 && value > 2)
                    throw new ArgumentException("Приоритет должен иметь значения от 0 до 2");
                Priority = value;
            }
        }
    }
}
