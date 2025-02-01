﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth.Api.FilterAttributes
{
    public class DevOnlyActionFilter : Attribute, IFilterFactory
    {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new DevOnlyAttributeImpl(serviceProvider.GetRequiredService<IWebHostEnvironment>());
        }
        public bool IsReusable => true;
        private class DevOnlyAttributeImpl : Attribute, IAuthorizationFilter
        {
            public DevOnlyAttributeImpl(IWebHostEnvironment hostingEnv)
            {
                HostingEnv = hostingEnv;
            }

            private IWebHostEnvironment HostingEnv { get; }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!HostingEnv.IsDevelopment())
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }
}
