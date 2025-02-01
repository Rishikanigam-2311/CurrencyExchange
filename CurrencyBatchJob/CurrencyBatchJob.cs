using CurrecnyExchangeStorage.DB;

public class TaskSchedulerService
{
    private readonly TimeSpan[] SchedulerTime = new[]
    {
        new TimeSpan(9, 0, 0),
    };

    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);  

    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentTime = DateTime.Now.TimeOfDay;

        foreach (var scheduledTime in SchedulerTime)
        {
            if (currentTime >= scheduledTime && currentTime < scheduledTime.Add(_checkInterval))
            {
                await ExecuteScheduledTask(scheduledTime);
            }
        }

    }

    private Task ExecuteScheduledTask(TimeSpan scheduledTime)
    {
        double result = DBBatchJob;
        Console.WriteLine($"Task executed at {scheduledTime}");
        return Task.CompletedTask;
    }
    private bool _taskExecutedToday = false;
}
