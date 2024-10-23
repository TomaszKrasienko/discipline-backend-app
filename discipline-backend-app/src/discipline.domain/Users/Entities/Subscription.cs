using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Exceptions;
using discipline.domain.Users.ValueObjects;
using discipline.domain.Users.ValueObjects.Subscriptions;

namespace discipline.domain.Users.Entities;

public sealed class Subscription : Entity<SubscriptionId>
{
    private readonly HashSet<Feature> _features = new HashSet<Feature>();
    public Title Title { get; private set; }
    public Price Price { get; private set; }
    public IReadOnlyCollection<Feature> Features => _features;

    private Subscription(SubscriptionId id) : base(id)
    {
        
    }
    
    //For mongo
    public Subscription(SubscriptionId id, Title title, Price price, List<Feature> features) : this(id)
    {
        Title = title;
        Price = price;
        _features = features.ToHashSet();
    }

    public static Subscription Create(SubscriptionId id, string title, decimal pricePerMonth, decimal pricePerYear,
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

    public bool IsFreeSubscription()
        => Price?.PerMonth == 0 && Price?.PerYear == 0;

    internal void AddFeature(string feature)
        => _features.Add(feature);
    
}