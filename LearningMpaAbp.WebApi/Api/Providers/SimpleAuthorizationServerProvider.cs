using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Dependency;
using LearningMpaAbp.App;
using LearningMpaAbp.Authorization;
using LearningMpaAbp.MultiTenancy;
using LearningMpaAbp.Users;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LearningMpaAbp.Api.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider, ITransientDependency
    {
        private readonly LogInManager _logInManager;

        public SimpleAuthorizationServerProvider(LogInManager logInManager)
        {
            _logInManager = logInManager;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }
            var isValidClient = string.CompareOrdinal(clientId, "app") == 0 &&
                                string.CompareOrdinal(clientSecret, "app") == 0;
            if (isValidClient)
            {
                context.OwinContext.Set("as:client_id", clientId);
                context.Validated(clientId);
            }
            else
            {
                context.SetError("invalid client");
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var tenantId = context.Request.Query["tenantId"];
            var result = await GetLoginResultAsync(context, context.UserName, context.Password, tenantId);
            if (result.Result == AbpLoginResultType.Success)
            {
                //var claimsIdentity = result.Identity;                
                var claimsIdentity = new ClaimsIdentity(result.Identity);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                var ticket = new AuthenticationTicket(claimsIdentity, new AuthenticationProperties());
                context.Validated(ticket);
            }
        }

        public override  Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.OwinContext.Get<string>("as:client_id");
            var currentClient = context.ClientId;

            // enforce client binding of refresh token
            if (originalClient != currentClient)
            {
                context.Rejected();
                return Task.FromResult<object>(null);
            }

            // chance to change authentication ticket for refresh token requests
            var newId = new ClaimsIdentity(context.Ticket.Identity);
            newId.AddClaim(new Claim("newClaim", "refreshToken"));

            var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(OAuthGrantResourceOwnerCredentialsContext context, 
            string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    CreateExceptionForFailedLoginAttempt(context, loginResult.Result, usernameOrEmailAddress, tenancyName);
                    //throw CreateExceptionForFailedLoginAttempt(context,loginResult.Result, usernameOrEmailAddress, tenancyName);
                    return loginResult;
            }
        }

        private void CreateExceptionForFailedLoginAttempt(OAuthGrantResourceOwnerCredentialsContext context, 
            AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName)
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    throw new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                case AbpLoginResultType.InvalidPassword:
                    context.SetError(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                    break;
                //    return new UserFriendlyException(("LoginFailed"), ("InvalidUserNameOrPassword"));
                case AbpLoginResultType.InvalidTenancyName:
                    context.SetError(L("LoginFailed"), L("ThereIsNoTenantDefinedWithName", tenancyName));
                    break;
                //    return new UserFriendlyException(("LoginFailed"), string.Format("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                case AbpLoginResultType.TenantIsNotActive:
                    context.SetError(L("LoginFailed"), L("TenantIsNotActive", tenancyName));
                    break;
                //    return new UserFriendlyException(("LoginFailed"), string.Format("TenantIsNotActive {0}", tenancyName));
                case AbpLoginResultType.UserIsNotActive:
                    context.SetError(L("LoginFailed"), L("UserIsNotActiveAndCanNotLogin", usernameOrEmailAddress));
                    break;
                //    return new UserFriendlyException(("LoginFailed"), string.Format("UserIsNotActiveAndCanNotLogin {0}", usernameOrEmailAddress));
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    context.SetError(L("LoginFailed"), L("UserEmailIsNotConfirmedAndCanNotLogin"));
                    break;
                    //    return new UserFriendlyException(("LoginFailed"), ("UserEmailIsNotConfirmedAndCanNotLogin"));
                    //default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    //    //Logger.Warn("Unhandled login fail reason: " + result);
                    //    return new UserFriendlyException(("LoginFailed"));
            }
        }

        private static string L(string name, params object[] args)
        {
            //return new LocalizedString(name);
            return IocManager.Instance.Resolve<ILocalizationService>().L(name, args);
        }

    }
}
