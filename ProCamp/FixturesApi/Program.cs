using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FixturesApi
{
    /// <summary>
    /// Main program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates app
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls($"http://0.0.0.0:{CommonLibrary.Constants.Ports.FixturesPort}")
                .UseStartup<Startup>();
    }
}