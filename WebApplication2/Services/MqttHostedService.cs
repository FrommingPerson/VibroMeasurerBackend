using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MQTTnet.Packets;
using Newtonsoft.Json;
using WebApplication2.Hubs;

namespace WebApplication2.Services;

using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class MqttHostedService : IHostedService
{
    private IMqttClient _mqttClient;
    private readonly MqttClientOptions _mqttClientOptions;
    private readonly IHubContext<MainHub> _hubContext;
    private readonly IServiceProvider _serviceProvider;

    public MqttHostedService(IHubContext<MainHub> hubContext, IServiceProvider serviceProvider)
    {
        _hubContext = hubContext;
        _serviceProvider = serviceProvider;
        var mqttFactory = new MqttFactory();
        _mqttClient = mqttFactory.CreateMqttClient();
        _mqttClientOptions = new MqttClientOptionsBuilder()
            .WithCredentials("mqttuser", "11111111")
            .WithTcpServer("80.249.149.17")
            .Build();

        _mqttClient.ApplicationMessageReceivedAsync += HandleReceivedMessage;
    }

    private async Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs e)
    {
        var topic = e.ApplicationMessage.Topic;
        var message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MineDbContext>();
            var deviceMac = int.Parse(topic.Split('/')[0]);
            var device = await context.Devices.FirstOrDefaultAsync(x => x.DeviceMac == deviceMac);
            if (device == null)
            {
                device = new Device()
                {
                    DeviceMac = deviceMac,
                };
                context.Devices.Add(device);
                await context.SaveChangesAsync();
            }

            if (topic.Contains("sensor/data/json"))
            {
                var sensorData = JsonConvert.DeserializeObject<SensorData>(message);
                if (sensorData == null) return;
                var scannerData = new VibrationData()
                {
                    DeviceId = device.DeviceId,
                    AxMax = sensorData.AxMax,
                    AxAvg = sensorData.AxAvg,
                    AxMin = sensorData.AxMin,
                    AyMax = sensorData.AyMax,
                    AyAvg = sensorData.AzAvg,
                    AyMin = sensorData.AyMin,
                    AzMax = sensorData.AzMax,
                    AzAvg = sensorData.AyAvg,
                    AzMin = sensorData.AzMin,
                    GxMax = sensorData.GxMax,
                    GxAvg = sensorData.GxAvg,
                    GxMin = sensorData.GxMin,
                    GyMax = sensorData.GyMax,
                    GyAvg = sensorData.GzAvg,
                    GyMin = sensorData.GyMin,
                    GzMax = sensorData.GzMax,
                    GzAvg = sensorData.GyAvg,
                    GzMin = sensorData.GzMin,
                    AxDiff = sensorData.AxDiff,
                    AyDiff = sensorData.AyDiff,
                    AzDiff = sensorData.AzDiff,
                    GxDiff = sensorData.GxDiff,
                    GyDiff = sensorData.GyDiff,
                    GzDiff = sensorData.GzDiff,
                    Date = DateTime.UtcNow,
                };
                device.ScannersData.Add(scannerData);
                await context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveSensorData", scannerData);
            }
            else
            {
                if (topic.Contains("CardMountFailed")) device.CardMountFailed = bool.Parse(message);
                else if (topic.Contains("OpenError")) device.OpenError = bool.Parse(message);
                else if (topic.Contains("cardType")) device.CardType = uint.Parse(message);
                else if (topic.Contains("cardSize")) device.CardSize = uint.Parse(message);
                else if (topic.Contains("freeSize")) device.FreeSize = uint.Parse(message);
                else if (topic.Contains("RSSI")) device.Rssi = int.Parse(message);
                else if (topic.Contains("SignalQuality")) device.SignalQuality = int.Parse(message);
                else if (topic.Contains("IP")) device.Ip = message;
                else if (topic.Contains("SSID")) device.Ssid = message;
                else if (topic.Contains("BSSID")) device.Bssid = message;
                else if (topic.Contains("Channel")) device.Channel = int.Parse(message);
                else if (topic.Contains("MAC")) device.Mac = message;
                else if (topic.Contains("PubSubClient_buffer_raw")) device.PubSubClientBufferStatus = message;

                context.Devices.Update(device);
                await context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveDeviceData", device);
            }
        }

        Console.WriteLine(topic);
        Console.WriteLine(message);

        var isValid = float.TryParse(message, out var number);
        if (isValid) Console.WriteLine(number * 2);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _mqttClient.ConnectAsync(_mqttClientOptions, cancellationToken);
            Console.WriteLine("MQTT client connected.");

            var mqttSubscribeOptions = new MqttTopicFilterBuilder()
                .WithTopic("#")
                .Build();

            await _mqttClient.SubscribeAsync(mqttSubscribeOptions, cancellationToken);
            Console.WriteLine("Subscribed to topic /Sensor1/Vibra.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting MQTT service: {ex.Message}");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_mqttClient.IsConnected)
        {
            await _mqttClient.DisconnectAsync();
            Console.WriteLine("MQTT client disconnected.");
        }
    }
}

// await mqttClient.PublishAsync(new MqttApplicationMessage()
//  {
//      Topic = "Danken",
//      PayloadSegment = Encoding.UTF8.GetBytes("how are you doing?"),
//  });

public class SensorData
{
    public int SensorId { get; set; }

    public int DeviceId { get; set; }
    public float AxMin { get; set; }
    public float AxMax { get; set; }
    public float AxAvg { get; set; }
    public float AyMin { get; set; }
    public float AyMax { get; set; }
    public float AyAvg { get; set; }
    public float AzMin { get; set; }
    public float AzMax { get; set; }
    public float AzAvg { get; set; }
    public float GxMin { get; set; }
    public float GxMax { get; set; }
    public float GxAvg { get; set; }
    public float GyMin { get; set; }
    public float GyMax { get; set; }
    public float GyAvg { get; set; }
    public float GzMin { get; set; }
    public float GzMax { get; set; }
    public float GzAvg { get; set; }
    public float AxDiff { get; set; }
    public float AyDiff { get; set; }
    public float AzDiff { get; set; }
    public float GxDiff { get; set; }
    public float GyDiff { get; set; }
    public float GzDiff { get; set; }
}


/*
 1/sensor

 {
   "axMin": 1.0,
   "axMax": 6.0,
   "axAvg": 8.0,
   "ayMin": 10.0,
   "ayMax": 3.0,
   "ayAvg": 8.0,
   "azMin": 4.0,
   "azMax": 6.4,
   "azAvg": 1.2,
   "gxMin": 8.2,
   "gxMax": 9.0,
   "gxAvg": 1.0,
   "gyMin": 1.0,
   "gyMax": 1.0,
   "gyAvg": 3.0,
   "gzMin": 4.0,
   "gzMax": 5.0,
   "gzAvg": 6.0,
   "axDiff": 7.0,
   "ayDiff": 7.0,
   "azDiff": 5.0,
   "gxDiff": 2.0,
   "gyDiff": 1.0,
   "gzDiff": 7.0
   }


   TEST/SD_CARD/error/CardMountFailed
   TEST/SD_CARD/info/cardType
   TEST/SD_CARD/info/cardSize
   TEST/SD_CARD/info/freeSize
   TEST/SD_CARD/error/OpenError
   TEST/SD_CARD/error/WriteError
   TEST/wifi/RSSI
   TEST/wifi/SignalQuality
   TEST/wifi/IP
   TEST/wifi/SSID
   TEST/wifi/BSSID
   TEST/wifi/Channel
   TEST/wifi/MAC
   TEST/warning/PubSubClient_buffer_status
   TEST/sensor/Ax/min
   TEST/sensor/Ax/max
   TEST/sensor/Ax/avg
   TEST/sensor/Ay/min
   TEST/sensor/Ay/max
   TEST/sensor/Ay/avg
   TEST/sensor/Az/min
   TEST/sensor/Az/max
   TEST/sensor/Az/avg
   TEST/sensor/Gx/min
   TEST/sensor/Gx/max
   TEST/sensor/Gx/avg
   TEST/sensor/Gy/min
   TEST/sensor/Gy/max
   TEST/sensor/Gy/avg
   TEST/sensor/Gz/min
   TEST/sensor/Gz/max
   TEST/sensor/Gz/avg
   TEST/sensor/Ax/diff
   TEST/sensor/Ay/diff
   TEST/sensor/Az/diff
   TEST/sensor/Gx/diff
   TEST/sensor/Gy/diff
   TEST/sensor/Gz/diff
 */