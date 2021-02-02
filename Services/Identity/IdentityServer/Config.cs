using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        /*
         *  The value(s) of the audience claim(JWT aud claim) will be the name of the API resource(s)
         *  
         *  http://docs.identityserver.io/en/latest/topics/resources.html
         *  
         *  set Audience(JWT aud) = ApiResource Name
         *  
         *  Also register scopes first, resources later
         *  
         */

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource(name:"messagingApi", displayName:"Messaging API Service")
                {
                    Scopes = {
                        "messagingApi.read",
                    },
                    UserClaims = {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.PreferredUserName
                    }
                },
                new ApiResource(name:"webBffApi", displayName:"Web Bff API Service")
                {
                    Scopes = {
                        "webBffApi.read",
                    },
                    UserClaims = {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.PreferredUserName
                    }
                },
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope(name: "webBffApi.read", displayName: "webBffApi Access"),
                new ApiScope(name: "messagingApi.read", displayName: "messagingApi Access"),
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<Client> GetClients(IConfiguration Configuration) =>
            new Client[]
            {
                new Client
                {
                    ClientId = "messagingswaggerui",
                    ClientName = "Messaging Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{Configuration.GetValue<string>("MessagingApiClient")}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{Configuration.GetValue<string>("MessagingApiClient")}/swagger/" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "webBffApi.read",
                        "messagingApi.read"
                    }
                },
                new Client
                {
                    ClientId = "webbffswaggerui",
                    ClientName = "Web Bff Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{Configuration.GetValue<string>("WebBffAggregatorApiClient")}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{Configuration.GetValue<string>("WebBffAggregatorApiClient")}/swagger/" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "webBffApi.read",
                        "messagingApi.read"
                    }
                }
            };
    }
}