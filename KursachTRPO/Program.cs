﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace KursachTRPO
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .UseKestrel(options => { options.Listen(System.Net.IPAddress.Any, 5000); });
    }
}
