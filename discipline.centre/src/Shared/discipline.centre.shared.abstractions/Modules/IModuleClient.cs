namespace discipline.centre.shared.abstractions.Modules;

/// <summary>
/// Interface to perform internal communication between modules
/// Provides methods for both communication asynchronous and synchronous
/// </summary>
public interface IModuleClient
{
    /// <summary>
    /// Sends a synchronous request and returns response between modules
    /// </summary>
    /// <param name="path">Route for the request</param>
    /// <param name="request">Message payload</param>
    /// <typeparam name="TResult">The type of the response type</typeparam>
    /// <returns>Instance of <see cref="TResult"/> <c>null</c></returns>
    Task<TResult?> SendAsync<TResult>(string path, object request) where TResult : class;
    Task PublishAsync(object @event);
}