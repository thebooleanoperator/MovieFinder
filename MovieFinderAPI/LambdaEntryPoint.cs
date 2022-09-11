using Microsoft.AspNetCore.Hosting;

namespace MovieFinder
{
    public class LambdaFunction : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            /*builder
                .UseStartup<Startup>();*/
        }
    }
}
