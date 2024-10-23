using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.BusinessRules.Features;
using discipline.domain.Users.ValueObjects;
using discipline.domain.Users.ValueObjects.Subscriptions;

namespace discipline.domain.Users.Entities;

public sealed class Subscription : Entity<SubscriptionId>
{
    private HashSet<Feature>? _features;
    public Title Title { get; private set; }
    public Price Price { get; private set; }
    public IReadOnlyCollection<Feature> Features => _features!;

    private Subscription(SubscriptionId id, Title title, Price price) : base(id)
    {
        Title = title;
        Price = price;
    }
    
    /// <summary>
    /// Constructor for mapping to mongo documents
    /// </summary>
    public Subscription(SubscriptionId id, Title title, Price price, 
        List<Feature> features) : base(id)
    {
        Title = title;
        Price = price;
        _features = features.ToHashSet();
    }

    public static Subscription Create(SubscriptionId id, string title, decimal pricePerMonth, decimal pricePerYear,
        List<string> features)
    {
        var price = new Price(pricePerMonth, pricePerYear);
        var subscription = new Subscription(id, title, price);
        subscription.ChangeFeatures(features);
        return subscription;
    }

    private void ChangeFeatures(List<string> features)
    {
        var convertedFeatures = features.Select(x => (Feature)x).ToList();
        CheckRule(new FeaturesCanNotBeEmptyRule(convertedFeatures));
        _features = convertedFeatures.ToHashSet();
    }

    public bool IsFreeSubscription()
        => Price?.PerMonth == 0 && Price?.PerYear == 0;

    
}