using System.IO.Pipelines;

namespace McProtoNet.Client
{
	internal sealed class DuplexPipe : IDuplexPipe
	{
		public PipeReader Input { get; private set; }

		public PipeWriter Output { get; private set; }

		public DuplexPipe(PipeReader input, PipeWriter output)
		{
			Input = input;
			Output = output;
		}
	}
}
