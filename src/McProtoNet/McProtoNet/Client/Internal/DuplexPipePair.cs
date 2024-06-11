using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McProtoNet.Client
{
	internal sealed class DuplexPipePair
	{
		public IDuplexPipe Transport { get; }
		public IDuplexPipe Application { get; }

		private readonly Pipe pipe1;
		private readonly Pipe pipe2;

		public DuplexPipePair()
		{
			pipe1 = new Pipe();

			pipe2 = new Pipe();

			Transport = new DuplexPipe(pipe1.Reader, pipe2.Writer);


			Application = new DuplexPipe(pipe2.Reader, pipe1.Writer);

		}

		public void Reset()
		{
			pipe1.Reset();
			pipe2.Reset();
		}
	}
}
