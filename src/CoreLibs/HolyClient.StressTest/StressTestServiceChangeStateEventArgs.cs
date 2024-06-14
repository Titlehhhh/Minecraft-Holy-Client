namespace HolyClient.StressTest;

public sealed class StressTestServiceChangeStateEventArgs : EventArgs
{
    public StressTestServiceChangeStateEventArgs(StressTestServiceState newState, StressTestServiceState oldState)
    {
        NewState = newState;
        OldState = oldState;
    }

    public StressTestServiceState NewState { get; }
    public StressTestServiceState OldState { get; }
}