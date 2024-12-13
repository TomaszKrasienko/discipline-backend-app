namespace discipline.centre.shared.abstractions.Modules;

/// <summary>
/// Interface for mapping and handling module requests.
/// </summary>
public interface IModuleSubscriber
{
    /// <summary>
    /// Maps module request to a specific action,
    /// allowing other modules to connect and handle the request. 
    /// </summary>
    /// <param name="path">Route to handle the request.</param>
    /// <param name="action">The function to execute when the request is called</param>
    /// <typeparam name="TRequest">Type of incoming request</typeparam>
    /// <typeparam name="TResponse">Type of returning response</typeparam>
    /// <returns>An instance of <see cref="TResponse"/></returns>
    IModuleSubscriber MapModuleRequest<TRequest, TResponse>(string path,
        Func<TRequest, IServiceProvider, Task<TResponse?>> action) where TRequest : class where TResponse : class;
}