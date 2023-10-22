using HolyClient.Core.Services;

namespace HolyClient.Core.StressTest
{
	class NickProvider : INickProvider
	{
		private readonly string _baseNick;

		public NickProvider(string baseNick)
		{
			if (baseNick.Length > 16)
				throw new ArgumentException("Nick long");
			_baseNick = baseNick;

		}
		private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		public string GetNextNick()
		{
			var stringChars = new char[15 - _baseNick.Length];
			var random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			var finalString = new String(stringChars);
			return _baseNick + finalString;
		}
	}

}
