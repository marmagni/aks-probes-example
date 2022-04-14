using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ProbesApi.Controllers;

namespace ProbesApi.Services
{
    public class WarmupService
    {
        readonly IServiceProvider _provider;
        readonly IServiceCollection _services;

        public WarmupService(
            IServiceProvider provider,
            IServiceCollection services)
        {
            _services = services;
            _provider = provider;
        }

        public void CreateInstances()
        {
            foreach (var serviceType in GetServices(_services))
            {
                _provider.GetServices(serviceType);
            }
        }

        static IEnumerable<Type> GetServices(IServiceCollection services)
        {
            return services
                .Where(d => d.ImplementationType != typeof(WarmupService))
                .Where(d => d.ServiceType.ContainsGenericParameters == false)
                .Select(d => d.ServiceType)
                .Distinct();
        }
    }
}
