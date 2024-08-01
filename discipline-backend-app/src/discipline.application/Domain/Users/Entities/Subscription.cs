using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.Users.Exceptions;
using discipline.application.Domain.Users.ValueObjects;

namespace discipline.application.Domain.Users.Entities;

internal sealed class Subscription
{
    private readonly HashSet<Feature> _features = new HashSet<Feature>();
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public Price Price { get; private set; }
    public IReadOnlyCollection<Feature> Features => _features;

    private Subscription(EntityId id)
        => Id = id;
    
    //For mongo
    internal Subscription(EntityId id, Title title, Price price, List<Feature> features) : this(id)
    {
        Title = title;
        Price = price;
        _features = features.ToHashSet();
    }

    internal static Subscription Create(Guid id, string title, decimal pricePerMonth, decimal pricePerYear,
        List<string> features)
    {
        var subscription = new Subscription(id);
        subscription.ChangeTitle(title);
        subscription.ChangePrice(pricePerMonth, pricePerYear);
        subscription.ChangeFeatures(features);
        return subscription;
    }
    
    private void ChangeFeatures(List<string> features)
    {
        if (!features.Any())
        {
            throw new EmptyFeaturesListException();
        }

        foreach (var feature in features)
        {
            AddFeature(feature);
        }
    }

    private void ChangeTitle(string title)
        => Title = title;

    private void ChangePrice(decimal pricePerMonth, decimal pricePerYear)
        => Price = new Price(pricePerMonth, pricePerYear);

    internal bool IsFreeSubscription()
        => Price?.PerMonth == 0 && Price?.PerYear == 0;

    internal void AddFeature(string feature)
        => _features.Add(feature);
    
}