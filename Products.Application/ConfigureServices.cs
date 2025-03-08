using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.Behaviors;
using Products.Application.Products.EventHandlers;
using Products.Domain.Events;

namespace Products.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        
        services.AddTransient<INotificationHandler<ProductCreatedEvent>, ProductCreatedEventHandler>();
        services.AddTransient<INotificationHandler<ProductUpdatedEvent>, ProductUpdatedEventHandler>();
        
        return services;
    }
}