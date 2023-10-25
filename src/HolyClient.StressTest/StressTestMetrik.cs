namespace HolyClient.StressTest
{
	public struct StressTestMetrik
	{
		public int CPS { get; }
		public int BotsOnline { get; }

		public StressTestMetrik(int cPS, int botsOnline)
		{
			CPS = cPS;
			BotsOnline = botsOnline;
		}
	}
}
