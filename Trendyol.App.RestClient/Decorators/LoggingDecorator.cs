namespace Trendyol.App.RestClient.Decorators
{
    public class LoggingDecorator : RestClientDecorator
    {
        public LoggingDecorator(RestSharp.RestClient decoratedClient) 
            : base(decoratedClient)
        {
        }
    }
}