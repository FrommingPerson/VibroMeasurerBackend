namespace WebApplication2;

public class VibrationData
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public float AxMin { get; set; }
    public float AxAvg { get; set; }
    public float AxMax { get; set; }
    public float AyMin { get; set; }
    public float AyAvg { get; set; }
    public float AyMax { get; set; }
    public float AzMin { get; set; }
    public float AzAvg { get; set; }
    public float AzMax { get; set; }
    public float GxMin { get; set; }
    public float GxAvg { get; set; }
    public float GxMax { get; set; }
    public float GyMin { get; set; }
    public float GyAvg { get; set; }
    public float GyMax { get; set; }
    public float GzMin { get; set; }
    public float GzAvg { get; set; }
    public float GzMax { get; set; }
    public float AxDiff { get; set; }
    public float AyDiff { get; set; }
    public float AzDiff { get; set; }
    public float GxDiff { get; set; }
    public float GyDiff { get; set; }
    public float GzDiff { get; set; }
    public DateTime Date { get; set; }
}

