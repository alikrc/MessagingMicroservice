using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Data
{
    public class ConfigurationDbContextSeed
    {
        public async Task SeedAsync(ConfigurationDbContext context, IConfiguration configuration)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients(configuration))
                {
                    context.Clients.Add(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            // register scopes before resources see: http://docs.identityserver.io/en/latest/topics/resources.html
            // To migrate to v4 you need to split up scope and resource registration, typically by first registering all your scopes 
            if (!context.ApiScopes.Any())
            {
                foreach (var api in Config.ApiScopes)
                {
                    context.ApiScopes.Add(api.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            //register resources after scopes see: http://docs.identityserver.io/en/latest/topics/resources.html
            if (!context.ApiResources.Any())
            {
                foreach (var api in Config.ApiResources)
                {
                    context.ApiResources.Add(api.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
