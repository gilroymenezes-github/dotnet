using Infrastructure.Constants;

namespace Infrastructure.Abstractions
{
    public class BaseCommand<T> where T: BaseModel
    {
        public CommandEnums Command { get; set; }
        public T Item { get; set; }
    }
}
