namespace WebApplication2;

public class Device
{
    public int DeviceMac { get; set; }
    public int DeviceId { get; set; }
    public bool CardMountFailed { get; set; }
    public uint CardType { get; set; }
    public long CardSize { get; set; }
    public long FreeSize { get; set; }
    public bool OpenError { get; set; }
    public bool WriteError { get; set; }
    public int Rssi { get; set; }
    public int SignalQuality { get; set; }
    public string? Ip { get; set; }
    public string? Ssid { get; set; }
    public string? Bssid { get; set; }
    public List<VibrationData> ScannersData { get; set; } = new List<VibrationData>();
    public int Channel { get; set; }
    public string? Mac { get; set; }
    public string? PubSubClientBufferStatus { get; set; }
}



