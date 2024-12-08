using Microsoft.Extensions.Configuration;
using RoadStatucConsoleApp.Interface;
using RoadStatucConsoleApp.Models;
using RoadStatucConsoleApp.Service;

namespace RoadStatucConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                Console.WriteLine("Required parameter 'road ID' was not provided, exiting!");
                Environment.Exit(1);
                return;
            }
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Set the base path for configuration files
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            string roadId = args[0];            
            string appId = configuration["APISettings:AppId"];
            string appKey = configuration["APISettings:AppKey"];
            
            using (var httpClient = new HttpClient())
            {
                try
                {
                    IRoadStatusService serviceClient = new RoadStatusService(appId, appKey, httpClient);
                    var roadStatus = await serviceClient.GetRoadStatusAsync(roadId);
                    Console.WriteLine($"The status of the {roadStatus.DisplayName} is as follows");
                    Console.WriteLine($"Road Status is {roadStatus.StatusSeverity}");
                    Console.WriteLine($"Road Status Description is {roadStatus.StatusSeverityDescription}");
                    Environment.Exit(0);
                }
                catch (NotFoundException ex)
                {
                    //return a user friendly error message.
                    Console.WriteLine($"{roadId} is not a valid road");
                    //Not returning the internal raw Api error message to the user. This can be used for logging.
                    //Console.WriteLine($"Api error message : {ex.Message}");
                    Environment.Exit(1);
                }
                catch (Exception ex)
                {
                    //For all other errors show unhandled exception in the service.
                    Console.WriteLine(ex.Message);
                    Environment.Exit(1);
                }
            }
        }
    }
}
