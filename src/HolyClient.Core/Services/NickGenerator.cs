using McProtoNet.MultiVersion.Contracts;

namespace HolyClient.Core.Services
{
	public class NickGenerator : INickGenerator
	{
		private string? _base;

		public NickGenerator(string? @base)
		{
			_base = @base;
			if (_base is not null)
				if (_base.Length >= 16)
				{
					throw new ArgumentException("Ник слишком длинный!");
				}
		}

		public string Generate()
		{
			string id = Guid.NewGuid().ToString().Replace("-", "").Remove(16);
			if (_base is null)
			{
				return id;
			}

			return _base + id.Remove(16 - _base.Length);
		}
	}
}
