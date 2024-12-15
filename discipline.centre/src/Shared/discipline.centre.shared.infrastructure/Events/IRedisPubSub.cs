namespace discipline.centre.shared.infrastructure.Events;

/// <summary>
/// Interface for communication by Redis
/// </summary>
internal interface IRedisPubSub
{
    /// <summary>
    /// Sends message in JSON by Redis
    /// </summary>
    /// <param name="json">JSON message to be sent</param>
    /// <param name="route">Message route</param>
    /// <param name="cancellationToken">Token for monitoring cancellation requests.</param>
    Task SendAsync(string json, string route, CancellationToken cancellationToken = default);
}