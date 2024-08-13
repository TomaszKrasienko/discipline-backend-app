using Microsoft.Extensions.Hosting;

namespace discipline.application.Infrastructure.DAL;

internal sealed class DbInitializer(
    IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // var subscriptions = new List<Subscription>()
        // {
        //     Subscription.Create(Guid.NewGuid(), "Free", 0, 0, ["Daily habits", "Activity rules"]),
        //     Subscription.Create(Guid.NewGuid(), "Premium", 10, 100, ["Daily habits", "Activity rules", "Calendar", "Chat"])
        // };
        //
        // var activityRules = new List<ActivityRule>()
        // {
        //     ActivityRule.Create(Guid.NewGuid(), "My test 1 activity rule", Mode.EveryDayMode()),
        //     ActivityRule.Create(Guid.NewGuid(), "My test 2 activity rule", Mode.FirstDayOfMonth()),
        //     ActivityRule.Create(Guid.NewGuid(), "My test 3 activity rule", Mode.CustomMode(), [1, 2, 4]),
        //     ActivityRule.Create(Guid.NewGuid(), "My test 4 activity rule", Mode.EveryDayMode()),
        //     ActivityRule.Create(Guid.NewGuid(), "My test 5 activity rule", Mode.EveryDayMode()),
        //     ActivityRule.Create(Guid.NewGuid(), "My test 6 activity rule", Mode.EveryDayMode()),
        //     ActivityRule.Create(Guid.NewGuid(), "My test 7 activity rule", Mode.EveryDayMode())
        // };
        //
        // var dailyProductive = DailyProductivity.Create(DateOnly.FromDateTime(DateTime.Now));
        // dailyProductive.AddActivity(Guid.NewGuid(), "Test activity 1");
        // dailyProductive.AddActivity(Guid.NewGuid(), "Test activity 2");
        // dailyProductive.AddActivity(Guid.NewGuid(), "Test activity 3");
        // dailyProductive.AddActivity(Guid.NewGuid(), "Test activity 4");
        // dailyProductive.AddActivity(Guid.NewGuid(), "Test activity 5");
        //
        //
        // var dailyProductiveTomorrow = DailyProductivity.Create(DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        // dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 6");
        // dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 7");
        // dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 8");
        // dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 9");
        // dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 10");
        // dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 11");
        // dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 12");
        //
        // var now = DateTime.Now;
        // var yesterdayUserCalendar = UserCalendar.Create(DateOnly.FromDateTime(now.AddDays(-1)));
        // yesterdayUserCalendar.AddEvent(Guid.NewGuid(), "yesterday important day");
        // yesterdayUserCalendar.AddEvent(Guid.NewGuid(), "yesterday calendar event title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "yesterday action");
        // yesterdayUserCalendar.AddEvent(Guid.NewGuid(), "yesterday meeting title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "yesterday platform",
        //     "yesterday uri", string.Empty);
        //
        // var todayUserCalendar = UserCalendar.Create(DateOnly.FromDateTime(now));
        // todayUserCalendar.AddEvent(Guid.NewGuid(), "today important day");
        // todayUserCalendar.AddEvent(Guid.NewGuid(), "today calendar event title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "today action");
        // todayUserCalendar.AddEvent(Guid.NewGuid(), "today meeting title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "today platform",
        //     "today uri", string.Empty);
        //
        // var tomorrowUserCalendar = UserCalendar.Create(DateOnly.FromDateTime(now.AddDays(1)));
        // tomorrowUserCalendar.AddEvent(Guid.NewGuid(), "tomorrow important day");
        // tomorrowUserCalendar.AddEvent(Guid.NewGuid(), "tomorrow calendar event title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "tomorrow action");
        // tomorrowUserCalendar.AddEvent(Guid.NewGuid(), "tomorrow meeting title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "tomorrow platform",
        //     "tomorrow uri", string.Empty);
        //
        // using var scope = serviceProvider.CreateScope();
        // var disciplineMongoClient = scope.ServiceProvider.GetRequiredService<IDisciplineMongoCollection>();
        // var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
        // var options = scope.ServiceProvider.GetRequiredService<IOptions<MongoOptions>>().Value;
        // await mongoClient.DropDatabaseAsync(options.Database, cancellationToken);
        //
        // var subscriptionRepository = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();
        // foreach (var subscription in subscriptions)
        // {
        //     await subscriptionRepository.AddAsync(subscription, cancellationToken);
        // }
        //
        // var activityRulesCollection = disciplineMongoClient.GetCollection<ActivityRuleDocument>();
        // await activityRulesCollection.InsertManyAsync(activityRules.Select(x => x.AsDocument()), null, cancellationToken);
        //
        // var dailyProductivityCollection = disciplineMongoClient.GetCollection<DailyProductivityDocument>();
        // await dailyProductivityCollection.InsertOneAsync(dailyProductive.AsDocument(), null, cancellationToken);
        // await dailyProductivityCollection.InsertOneAsync(dailyProductiveTomorrow.AsDocument(), null, cancellationToken);
        //
        // var userCalendarCollection = disciplineMongoClient.GetCollection<UserCalendarDocument>();
        // await userCalendarCollection.InsertManyAsync([
        //     yesterdayUserCalendar.AsDocument(), todayUserCalendar.AsDocument(), tomorrowUserCalendar.AsDocument()
        // ], null, cancellationToken);
    }
    
    

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}