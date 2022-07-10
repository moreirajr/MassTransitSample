using MassTransit;
using MassTransitSample.MessageBus.Consumers;
using MassTransitSample.MessageBus.Producers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MassTransitSample.MessageBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddProducers();
            services.AddMassTransit(config => ConfigureMassTransit(services, config, configuration));

            return services;
        }

        private static void AddProducers(this IServiceCollection services)
           => services.AddScoped<IMessageBusProducer, MassTransitProducer>();

        private static void ConfigureMassTransit(IServiceCollection services, IBusRegistrationConfigurator configurator, IConfiguration configuration)
        {
            configurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("sample", false));
           
            //DEPRECATED!
            //configurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(x => ConfigureRabbitMq(x, services)));

            configurator.AddConsumersTypes(services);

            configurator.UsingRabbitMq((context, cfg) =>
            {
                ConfigureRabbitMq(cfg, services, configuration);
                cfg.ConfigureEndpoints(context);
            });

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
        }

        private static void ConfigureRabbitMq(IRabbitMqBusFactoryConfigurator cfg, IServiceCollection services, IConfiguration configuration)
        {
            cfg.Host(new Uri(configuration.GetSection("MESSAGEBUS_CONNECTION").Value));
            cfg.PublishTopology.BrokerTopologyOptions = PublishBrokerTopologyOptions.MaintainHierarchy;
            cfg.SendTopology.ConfigureErrorSettings = settings => settings.AutoDelete = true;
            cfg.SendTopology.ConfigureDeadLetterSettings = settings => settings.AutoDelete = true;
        }

        private static void AddConsumersTypes(this IRegistrationConfigurator configurator, IServiceCollection services)
        {
            services.Scan(scan =>
            {
                scan.GetApplicationDependencies()
                    .AddClasses(filter => filter.Where(x => !x.IsAbstract).AssignableToAny(typeof(IMessageBusConsumer<>)))
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();

                scan.GetApplicationDependencies()
                    .AddClasses(filter => filter.Where(x => !x.IsAbstract).AssignableToAny(typeof(IMessageBusConsumer<>)))
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsSelf()
                    .WithScopedLifetime();
            });

            var consumers = GetConsumerServices(services).ToList();
            foreach (var consumer in consumers)
                configurator.AddConsumer(consumer.ImplementationType);
        }

        private static IEnumerable<ServiceDescriptor> GetConsumerServices(IServiceCollection services)
            => services.Where(x => x.ServiceType.IsGenericType && x.ServiceType.GetGenericTypeDefinition() == typeof(IMessageBusConsumer<>));

        private static IImplementationTypeSelector GetApplicationDependencies(this ITypeSourceSelector scan) =>
            scan.FromApplicationDependencies(r => r.FullName.StartsWith("MassTransitSample"));
    }
}