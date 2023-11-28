namespace HolyClient.StressTest
{
	public sealed class StressTestServiceChangeStateEventArgs : EventArgs
	{
		public StressTestServiceState NewState { get; }
		public StressTestServiceState OldState { get; }

		public StressTestServiceChangeStateEventArgs(StressTestServiceState newState, StressTestServiceState oldState)
		{
			NewState = newState;
			OldState = oldState;
		}
	}
}
