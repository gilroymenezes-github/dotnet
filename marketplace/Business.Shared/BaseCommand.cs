namespace Business.Shared
{
    public class BaseCommand<T> where T: BaseModel
    {
        public CommandEnum Command { get; set; }
        public T Item { get; set; }
    }
}
