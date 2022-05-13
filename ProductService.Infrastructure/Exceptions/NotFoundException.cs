namespace ProductService.Infrastructure.Exceptions
{
    public class NotFoundException : AggregateException
    {
        public NotFoundException(string message) : base(string.Format("404, Not Found", message))
        { }
    }
}
