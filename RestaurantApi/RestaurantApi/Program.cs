using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RestaurantApi
{
    public static class Program
    {
        
        public static void Main(string[] args)
        {
            try
            {
              

                CreateWebHostBuilder(args).Build().Run();
            }
            catch(Exception e)
            {
                
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }
      
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            
            WebHost.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((hostingContext, config) =>
             {
                 config.SetBasePath(Directory.GetCurrentDirectory());
                 config.AddJsonFile(
                     path:"config.json", optional: false, reloadOnChange: true);
             })
                .UseStartup<Startup>();
    }
}
