using discipline.application.Domain.ActivityRules;
using discipline.application.Domain.ActivityRules.Entities;
using discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.application.Domain.DailyProductivities.Entities;
using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Infrastructure.DAL.Configuration.Options;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.UsersCalendar;
using discipline.application.Infrastructure.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL;

internal sealed class DbInitializer(
    IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var activityRules = new List<ActivityRule>()
        {
            ActivityRule.Create(Guid.NewGuid(), "My test 1 activity rule", Mode.EveryDayMode()),
            ActivityRule.Create(Guid.NewGuid(), "My test 2 activity rule", Mode.FirstDayOfMonth()),
            ActivityRule.Create(Guid.NewGuid(), "My test 3 activity rule", Mode.CustomMode(), [1, 2, 4]),
            ActivityRule.Create(Guid.NewGuid(), "My test 4 activity rule", Mode.EveryDayMode()),
            ActivityRule.Create(Guid.NewGuid(), "My test 5 activity rule", Mode.EveryDayMode()),
            ActivityRule.Create(Guid.NewGuid(), "My test 6 activity rule", Mode.EveryDayMode()),
            ActivityRule.Create(Guid.NewGuid(), "My test 7 activity rule", Mode.EveryDayMode())
        };
        
        var dailyProductive = DailyProductivity.Create(DateOnly.FromDateTime(DateTime.Now));
        dailyProductive.AddActivity(Guid.NewGuid(), "Test activity 1");
        dailyProductive.AddActivity(Guid.NewGuid(), "Test activity 2");
        dailyProductive.AddActivity(Guid.NewGuid(), "Test activity 3");
        dailyProductive.AddActivity(Guid.NewGuid(), "Test activity 4");
        dailyProductive.AddActivity(Guid.NewGuid(), "Test activity 5");
        
        
        var dailyProductiveTomorrow = DailyProductivity.Create(DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 6");
        dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 7");
        dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 8");
        dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 9");
        dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 10");
        dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 11");
        dailyProductiveTomorrow.AddActivity(Guid.NewGuid(), "Test activity tomorrow 12");

        var now = DateTime.Now;
        var yesterdayUserCalendar = UserCalendar.Create(DateOnly.FromDateTime(now.AddDays(-1)));
        yesterdayUserCalendar.AddEvent(Guid.NewGuid(), "yesterday_important_day");
        yesterdayUserCalendar.AddEvent(Guid.NewGuid(), "yesterday_calendar_event_title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "yesterday_action");
        yesterdayUserCalendar.AddEvent(Guid.NewGuid(), "yesterday_meeting_title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "yesterday_platform",
            "yesterday_uri", string.Empty);
        
        var todayUserCalendar = UserCalendar.Create(DateOnly.FromDateTime(now));
        todayUserCalendar.AddEvent(Guid.NewGuid(), "today_important_day");
        todayUserCalendar.AddEvent(Guid.NewGuid(), "today_calendar_event_title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "today_action");
        todayUserCalendar.AddEvent(Guid.NewGuid(), "today_meeting_title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "today_platform",
            "today_uri", string.Empty);
        
        var tomorrowUserCalendar = UserCalendar.Create(DateOnly.FromDateTime(now.AddDays(1)));
        tomorrowUserCalendar.AddEvent(Guid.NewGuid(), "tomorrow_important_day");
        tomorrowUserCalendar.AddEvent(Guid.NewGuid(), "tomorrow_calendar_event_title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "tomorrow_action");
        tomorrowUserCalendar.AddEvent(Guid.NewGuid(), "tomorrow_meeting_title", new TimeOnly(10,0, 0), new TimeOnly(12, 0, 0), "tomorrow_platform",
            "tomorrow_uri", string.Empty);
        
        using var scope = serviceProvider.CreateScope();
        var disciplineMongoClient = scope.ServiceProvider.GetRequiredService<IDisciplineMongoCollection>();
        var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<MongoOptions>>().Value;
        await mongoClient.DropDatabaseAsync(options.Database, cancellationToken);
        
        var activityRulesCollection = disciplineMongoClient.GetCollection<ActivityRuleDocument>();
        await activityRulesCollection.InsertManyAsync(activityRules.Select(x => x.AsDocument()), null, cancellationToken);

        var dailyProductivityCollection = disciplineMongoClient.GetCollection<DailyProductivityDocument>();
        await dailyProductivityCollection.InsertOneAsync(dailyProductive.AsDocument(), null, cancellationToken);
        await dailyProductivityCollection.InsertOneAsync(dailyProductiveTomorrow.AsDocument(), null, cancellationToken);

        var userCalendarCollection = disciplineMongoClient.GetCollection<UserCalendarDocument>();
        await userCalendarCollection.InsertManyAsync([
            yesterdayUserCalendar.AsDocument(), todayUserCalendar.AsDocument(), tomorrowUserCalendar.AsDocument()
        ], null, cancellationToken);
    }
    
    

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}