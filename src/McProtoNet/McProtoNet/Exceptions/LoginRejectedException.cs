namespace McProtoNet
{
	public sealed class LoginRejectedException : Exception
	{
		public LoginRejectedException(string reason) : base(reason)
		{

		}
	}
}
