using System.Reflection;

namespace StockAnalyzer.Shared.CQRS;

public class Mediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = _serviceProvider.GetRequiredService(handlerType);

        if (handler == null)
            throw new Exception($"No handler found for {request.GetType()}");

        var result = await handler.Handle((dynamic)request);
        return result;
    }
}

public static class MediatorExtensions
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddScoped<Mediator>();

        var assembly = Assembly.GetEntryAssembly();
        if (assembly == null)
            return;

        // Scan the assembly for types that end with "Handler"
        var handlerTypes = assembly.GetTypes()
                                   .Where(t => t.Name.EndsWith("Handler") && !t.IsAbstract && t.IsClass);

        foreach (var handlerType in handlerTypes)
        {
            Type interfaceType = handlerType.GetInterfaces().Single(IsHandlerInterface);

            // Register the type as a transient service
            services.AddScoped(interfaceType, handlerType);
        }
    }

    private static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        Type typeDefinition = type.GetGenericTypeDefinition();

        return typeDefinition == typeof(IRequestHandler<,>);
    }
}
