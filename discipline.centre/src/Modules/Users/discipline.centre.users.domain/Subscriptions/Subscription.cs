using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions.Rules;
using discipline.centre.users.domain.Subscriptions.ValueObjects;

namespace discipline.centre.users.domain.Subscriptions;

public sealed class Subscription : AggregateRoot<SubscriptionId, Ulid>
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
    /// <param name="id"></param>
    /// <param name="title"></param>
    /// <param name="price"></param>
    /// <param name="features"></param>
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
        var price = Price.Create(pricePerMonth, pricePerYear);
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

    public bool IsFree()
        => Price is { PerMonth: 0, PerYear: 0 };

    
}