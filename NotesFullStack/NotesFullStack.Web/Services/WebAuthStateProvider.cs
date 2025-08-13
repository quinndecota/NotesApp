﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace NotesFullStack.Web.Services
{
    public class WebAuthStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        public WebAuthStateProvider(ILoggerFactory loggerFactory): base(loggerFactory)
        {
            
        }
        protected override TimeSpan RevalidationInterval => TimeSpan.FromDays(30);

        protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            //authenticationState.User.Claims.
            return Task.FromResult(true);
        }
    }
}
