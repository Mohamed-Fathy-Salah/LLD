public class SignalPhaseSequence (Dictionary<SignalColor, TimeSpan> phases)
{
    public List<SignalColor, TimeSpan> Phases {get;init;} = phases;
    private int _currentIndex = 0;
}
