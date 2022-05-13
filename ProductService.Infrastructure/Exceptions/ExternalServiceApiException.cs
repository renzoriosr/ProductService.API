namespace ProductService.Infrastructure.Exceptions
{
    public class ExternalServiceApiException : AggregateException
    {
        public ExternalServiceApiException(string message) : base(String.Format("Exception while accessing External Service API:{0}", message))
        { }
    }
}