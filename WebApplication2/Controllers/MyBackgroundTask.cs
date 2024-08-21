namespace WebApplication2.Controllers;

public class MyBackgroundTask : IHostedService {

    public Task StartAsync(CancellationToken cancellationToken) {
        // Implementation logic here
        Console.WriteLine("Success");
        return Task.CompletedTask; 
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        // Clean up any resources

        return Task.CompletedTask;
    }

}