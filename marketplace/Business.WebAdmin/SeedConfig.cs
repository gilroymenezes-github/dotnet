// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Business.Shared;
using Business.Shared.Statics;
using Business.WebAdmin.Models;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Business.WebAdmin
{
    public static class SeedConfig
    {
        
       
        public static IEnumerable<ApplicationUser> Users =>
            new ApplicationUser[]
            {
                new ApplicationUser 
                { 
                    Email = $"{ApplicationConstant.SysAdminRoleName.ToLowerInvariant()}{ApplicationConstant.EmailSuffix}",  
                    UserName = $"{ApplicationConstant.SysAdminRoleName.ToLowerInvariant()}{ApplicationConstant.EmailSuffix}" 
                },
            };

        public static IEnumerable<IdentityRole> Roles =>
            new IdentityRole[]
            {
                new IdentityRole { Name = ApplicationConstant.SysAdminRoleName },
                new IdentityRole { Name = ApplicationConstant.SelfRegUserRoleName }
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "User role(s)", new List<string>{ "role"}),
                new IdentityResource("role", "User dept", new List<string>{ "role" }),
                new IdentityResource("company", "User company", new List<string> { "company"})
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
                new ApiScope("businessapi", "business api")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("businessapi")
                {
                    Scopes = { "businessapi" },
                    ApiSecrets = { new Secret("businessapisecret".Sha256()) }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "scope1" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "scope2" }
                },
                new IdentityServer4.Models.Client
                {
                    ClientId= "business.api",
                    ClientSecrets  = new[] { new Secret("businessapisecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "businessapi"
                    }
                },
                new IdentityServer4.Models.Client
                {
                    ClientId= "business.webapp",
                    ClientSecrets  = new[] { new Secret("businessappsecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                        "businessapi",
                        "role",
                        "company"
                    },
                    RedirectUris = { "https://powerunit-app.azurewebsites.net/signin-oidc" },
                    FrontChannelLogoutUri = "https://powerunit-app.azurewebsites.net/signout-oidc",
                    PostLogoutRedirectUris = { "https://powerunit-app.azurewebsites.net/signout-callback-oidc" },
                },
                new IdentityServer4.Models.Client
                {
                    ClientId= "business.webapp.local",
                    ClientSecrets  = new[] { new Secret("businessappsecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                        "businessapi",
                        "role",
                        "company"
                    },
                    RedirectUris = { "https://localhost:5051/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5051/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5051/signout-callback-oidc" },
                },
                new IdentityServer4.Models.Client
                {
                    ClientId = "business.webasm.local",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "businessapi",
                        "role",
                        "company"
                    },
                    AllowedCorsOrigins = { "https://localhost:5021" },
                    RedirectUris = { "https://localhost:5021/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:5021/authentication/logout-callback" }
                },
                new IdentityServer4.Models.Client
                {
                    ClientId = "business.webasm",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "businessapi",
                        "role",
                        "company"
                    },
                    AllowedCorsOrigins = { "https://ipomoeastorage.z10.web.core.windows.net" },
                    RedirectUris = { "https://ipomoeastorage.z10.web.core.windows.net/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://ipomoeastorage.z10.web.core.windows.net/authentication/logout-callback" }
                },
                new IdentityServer4.Models.Client
                {
                    ClientId = "business.flutter.local",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "businessapi",
                        "role",
                        "company"
                    },
                    AllowedCorsOrigins = { "http://localhost:5020" },
                    RedirectUris = { "http://localhost:5020/" },
                    PostLogoutRedirectUris = { "https://localhost:5020/authentication/logout-callback" }
                },
                new IdentityServer4.Models.Client
                {
                    ClientId = "business.flutter",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "businessapi",
                        "role",
                        "company"
                    },
                    AllowedCorsOrigins = { "https://trialstoragekilowott.z29.web.core.windows.net" },
                    RedirectUris = { "https://trialstoragekilowott.z29.web.core.windows.net/" },
                    PostLogoutRedirectUris = { "https://trialstoragekilowott.z29.web.core.windows.net/authentication/logout-callback" }
                },
                new IdentityServer4.Models.Client
                {
                    ClientId = "Registration.Foo.Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "businessapi"
                        //"Registration.Foo.ServerAPI"
                    },
                    AllowedCorsOrigins = { "https://localhost:44321" },
                    RedirectUris = { "https://localhost:44321/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:44321/authentication/logout-callback" }
                }
            };
    }
}
