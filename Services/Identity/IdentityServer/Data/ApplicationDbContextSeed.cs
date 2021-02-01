using IdentityModel;
using IdentityServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Data
{
    public class ApplicationDbContextSeed
    {
        public async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<ApplicationDbContextSeed> logger, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                var ali = userManager.FindByNameAsync("ali").Result;

                if (ali == null)
                {
                    ali = new ApplicationUser
                    {
                        UserName = "ali",
                        Name = "Ali",
                        LastName = "Karaca",
                        Email = "ali@karaca.com",
                        EmailConfirmed = true,
                    };
                    var result = userManager.CreateAsync(ali, "Pa$$w0rd!").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userManager.AddClaimsAsync(ali, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Ali Karaca"),
                            new Claim(JwtClaimTypes.PreferredUserName, ali.UserName),
                            new Claim(JwtClaimTypes.WebSite, "https://github.com/alikrc"),
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("ali created");
                }
                else
                {
                    Log.Debug("ali already exists");
                }

                var demo = userManager.FindByNameAsync("demo").Result;
                if (demo == null)
                {
                    demo = new ApplicationUser
                    {
                        UserName = "demo",
                        Name = "Demo",
                        LastName = "Show",
                        Email = "demo@show.com",
                        EmailConfirmed = true
                    };
                    var result = userManager.CreateAsync(demo, "Pa$$w0rd!").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userManager.AddClaimsAsync(demo, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "John Doe"),
                            new Claim(JwtClaimTypes.PreferredUserName, demo.UserName),
                            new Claim(JwtClaimTypes.WebSite, "http://johndoe.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("demo created");
                }
                else
                {
                    Log.Debug("demo already exists");
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));

                    await SeedAsync(context, userManager, logger, retryForAvaiability);
                }
            }

        }
    }
}