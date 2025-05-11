public class Road(string name, int sensorId)
{
    private static int _nextId = 1;
    public int Id { get; init; } = Interlocked.Increment(ref _nextId);
    public string Name {get;init;} = name;
    public int SensorId {get;init;} = sensorId;
    public SignalColor signal {get;set;} =  SignalColorFactory.Get<RedSignal>();
}
