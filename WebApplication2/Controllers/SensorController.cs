using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using WebApplication2.Hubs;

namespace WebApplication2.Controllers;

[ApiController]
[Route("[controller]")]
public class SensorController
{
    private readonly MineDbContext _context;
    private readonly IHubContext<MainHub> _hubContext;

    public SensorController(MineDbContext context, IHubContext<MainHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    [AllowAnonymous]
    [HttpGet("dataFromMqtt")]
    public VibrationData GetVibrationData()
    {
        var vibro = new VibrationData()
        {
            AxMin = 23,
            AxAvg = 23,
            AxMax = 23,
            AyMin = 23,
            AyAvg = 23,
            AyMax = 23,
            AzMin = 23,
            AzAvg = 23,
            AzMax = 23,
            GxMin = 23,
            GxAvg = 23,
            GxMax = 23,
            GyMin = 23,
            GyAvg = 23,
            GyMax = 23,
            GzMin = 23,
            GzAvg = 23,
            GzMax = 23,
            Date = DateTime.Now,
        };
        // _context.VibrationDatas.Add(vibro);
        // _context.SaveChanges();
        return vibro;
    }

    [AllowAnonymous]
    [HttpGet("allData/{deviceId}")]
    public async Task<List<VibrationData>> GetVibrationDatas(int skip, int take, int deviceId)
    {
        var dataList = await _context.VibrationDatas
            .Where(x => x.DeviceId == deviceId)
            .OrderByDescending(x => x.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
         dataList.Reverse();
        return dataList;
    }

    static async Task Main(string[] args)
    {
        string broker = "80.249.149.17";
        int port = 8883;
        string clientId = Guid.NewGuid().ToString();
        string topic = "Csharp/mqtt";
        string username = "mqttuser";
        string password = "11111111";
        // Create a MQTT client factory
        var factory = new MqttFactory();

        // Create a MQTT client instance
        var mqttClient = factory.CreateMqttClient();

        // Create MQTT client options
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(broker, port) // MQTT broker address and port
            .WithCredentials(username, password) // Set username and password
            .WithClientId(clientId)
            .WithCleanSession()
            .WithTls(
                o =>
                {
                    // The used public broker sometimes has invalid certificates. This sample accepts all
                    // certificates. This should not be used in live environments.
                    // o.CertificateValidationHandler = _ => true;

                    // The default value is determined by the OS. Set manually to force version.
                    o.SslProtocol = SslProtocols.Tls12;

                    // Please provide the file path of your certificate file. The current directory is /bin.
                    var certificate = new X509Certificate("/opt/emqxsl-ca.crt", "");
                    o.Certificates = new List<X509Certificate> { certificate };
                }
            )
            .Build();

        // Connect to MQTT broker
        var connectResult = await mqttClient.ConnectAsync(options);

        if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
        {
            Console.WriteLine("Connected to MQTT broker successfully.");

            // Subscribe to a topic
            await mqttClient.SubscribeAsync(topic);

            // Callback function when a message is received
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
                return Task.CompletedTask;
            };

            // Publish a message 10 times
            for (int i = 0; i < 10; i++)
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload($"Hello, MQTT! Message number {i}")
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .WithRetainFlag()
                    .Build();

                await mqttClient.PublishAsync(message);
                await Task.Delay(1000); // Wait for 1 second
            }

            // Unsubscribe and disconnect
            await mqttClient.UnsubscribeAsync(topic);
            await mqttClient.DisconnectAsync();
        }
        else
        {
            Console.WriteLine($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
        }
    }
}