﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
           new List<IdentityResource>
           {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
           };
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource("WeatherApi"),
                new ApiResource("CalendarApi"),
                //new ApiResource("ClientApi")
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client> {
               /* new Client {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = { "WeatherApi", "CalendarApi" }
                },*/
                 new Client {
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:2000/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:2000/Home/Index" },

                    AllowedScopes = {
                        "WeatherApi",
                        "CalendarApi",
                        //"ClientApi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },

                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    
                    RequireConsent = false
                }
            };
    }
}
