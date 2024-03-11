


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using LibraryController; // Import the LibraryController namespace
using LibraryDataAccess; // Add this using directive
using Microsoft.Extensions.Configuration;
using LibraryAPI.Services; // Add this line if LibraryService is in the LibraryAPI.Services namespace








// // namespace Api
// // {
// //     public class Startup
// //     {
// //         public Startup(IConfiguration configuration)
// //         {
// //             Configuration = configuration;
// //         }

// //         public IConfiguration Configuration { get; } // Define Configuration property

// //         // public void ConfigureServices(IServiceCollection services)
// //         // {
// //         //     services.AddSingleton<IConfiguration>(Configuration);
// //         //     services.AddControllers();
// //         //     services.AddScoped<DataAccess>(); // Register DataAccess as a service
// //         // }

// //         public void ConfigureServices(IServiceCollection services)
// // {
// //     // Register the IConfiguration instance which Configuration relies on
// //     services.AddSingleton<IConfiguration>(Configuration);

// //     // Retrieve the connection string from Configuration
// //     string connectionString = Configuration.GetConnectionString("DefaultConnection");

// //     // Register DataAccess as a scoped service, passing the connection string
// //     services.AddScoped<DataAccess>(provider => new DataAccess(connectionString));

// //      services.AddScoped<LibraryService>();

// //     services.AddControllers();
// // }


// //         public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
// //         {
// //             if (env.IsDevelopment())
// //             {
// //                 app.UseDeveloperExceptionPage();
// //             }

// //             app.UseHttpsRedirection();
// //             app.UseRouting();
// //             app.UseAuthorization();
// //             app.UseEndpoints(endpoints =>
// //             {
// //                 endpoints.MapControllers();
// //             });
// //         }
// //     }
// // }

// namespace Api
// {
//     public class Startup
//     {
//         public Startup(IConfiguration configuration)
//         {
//             Configuration = configuration;
//         }

//         public IConfiguration Configuration { get; }

// // public void ConfigureServices(IServiceCollection services)
// // {
// //     // Register the IConfiguration instance
// //     services.AddSingleton<IConfiguration>(Configuration);

// //     // Retrieve the connection string from Configuration
// //     string connectionString = Configuration.GetConnectionString("DefaultConnection");

// //     // Register DataAccess as a scoped service, passing the connection string
// //     services.AddScoped<IDataAccess>(provider => 
// //     {
// //         // Instantiate and return DataAccess
// //         return new DataAccess(connectionString);
// //     });

// //     // Register LibraryService as a scoped service
// //     services.AddScoped<LibraryService>();

// //     // Add controllers and other services
// //     services.AddControllers();
// // }
// public void ConfigureServices(IServiceCollection services)
// {
//     // Register the IConfiguration instance
//     services.AddSingleton<IConfiguration>(Configuration);

//     // Retrieve the connection string from Configuration
//     string connectionString = Configuration.GetConnectionString("DefaultConnection");

//     // Register DataAccess as a scoped service, passing the connection string
//     services.AddScoped<IDataAccess>(provider => 
//     {
//         // Instantiate and return DataAccess
//         return (IDataAccess)new DataAccess(connectionString); // Explicitly cast to IDataAccess
//     });

//     // Register LibraryService as a scoped service
//     services.AddScoped<LibraryService>();

//     // Add controllers and other services
//     services.AddControllers();
// }



//         public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//         {
//             // Configure middleware and endpoints
//         }
//     }
// }

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LibraryDataAccess;
using LibraryAPI.Services;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       
    //    public void ConfigureServices(IServiceCollection services)
    // {
    // services.AddSingleton<IConfiguration>(Configuration);

    // string connectionString = Configuration.GetConnectionString("DefaultConnection");

    // // Register DataAccess as a scoped service
    // services.AddScoped<IDataAccess>(provider => (IDataAccess)new DataAccess(connectionString));

    // // Register LibraryService as a scoped service
    // services.AddScoped<LibraryService>();

    // services.AddControllers();
    // }
    public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IConfiguration>(Configuration);

    string connectionString = Configuration.GetConnectionString("DefaultConnection");

    // Register DataAccess as a scoped service
    services.AddScoped<IDataAccess, DataAccess>(provider => new DataAccess(connectionString));

    // Register LibraryService as a scoped service
    services.AddScoped<LibraryService>();

    services.AddControllers();
}



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
