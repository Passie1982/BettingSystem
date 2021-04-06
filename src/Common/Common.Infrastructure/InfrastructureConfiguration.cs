﻿namespace BettingSystem.Infrastructure.Common
{
    using System;
    using System.Reflection;
    using System.Text;
    using Application.Common;
    using Application.Common.Contracts;
    using Domain.Common;
    using Events;
    using Extensions;
    using MassTransit;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Services;

    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddCommonInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            Assembly assembly)
            => services
                .AddRepositories(assembly)
                .AddTokenAuthentication(configuration)
                .AddTransient<IImageService, ImageService>()
                .AddTransient<IEventDispatcher, EventDispatcher>();

        public static IServiceCollection AddEvents(
            this IServiceCollection services,
            params Type[] handlers)
            => services
                .AddMassTransit(massTransit =>
                {
                    handlers
                        .ForEach(handler => massTransit
                            .AddConsumer(handler));

                    massTransit.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(rabbitMq =>
                    {
                        rabbitMq.Host("localhost");

                        handlers
                            .ForEach(handler => rabbitMq
                                .ReceiveEndpoint(
                                    handler.FullName,
                                    endpoint => endpoint.ConfigureConsumer(bus, handler)));
                    }));
                })
                .AddMassTransitHostedService();

        internal static IServiceCollection AddRepositories(
            this IServiceCollection services,
            Assembly assembly)
            => services
                .Scan(scan => scan
                    .FromAssemblies(assembly)
                    .AddClasses(classes => classes
                        .AssignableTo(typeof(IDomainRepository<>))
                        .AssignableTo(typeof(IQueryRepository<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

        private static IServiceCollection AddTokenAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var secret = configuration
                .GetSection(nameof(ApplicationSettings))
                .GetValue<string>(nameof(ApplicationSettings.Secret));

            var key = Encoding.ASCII.GetBytes(secret);

            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
