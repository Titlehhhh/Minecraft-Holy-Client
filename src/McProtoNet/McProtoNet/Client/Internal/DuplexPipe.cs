using System.IO.Pipelines;

namespace McProtoNet.Client;

internal sealed class DuplexPipe : IDuplexPipe
{
    public DuplexPipe(PipeReader input, PipeWriter output)
    {
        Input = input;
        Output = output;
    }

    public PipeReader Input { get; }

    public PipeWriter Output { get; }
}