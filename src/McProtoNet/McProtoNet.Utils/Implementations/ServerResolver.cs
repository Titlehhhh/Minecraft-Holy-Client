using DnsClient;

namespace McProtoNet.Utils;

public class ServerResolver : IServerResolver
{
    public async Task<SrvRecord> ResolveAsync(string host, CancellationToken cancellationToken = default)
    {
        LookupClient lookupClient = new();

        var response = await lookupClient.QueryAsync(new DnsQuestion($"_minecraft._tcp.{host}", QueryType.SRV),
            cancellationToken);
        if (response.HasError != true && response.Answers.SrvRecords().Any())
        {
            var result = response.Answers.SrvRecords()
                .OrderBy(record => record.Priority)
                .ThenByDescending(record => record.Weight)
                .ThenBy(record => Guid.NewGuid())
                .First();
            var target = result.Target.Value.Trim('.');
            return new SrvRecord(target, result.Port);
        }


        throw new SrvNotFoundException();
    }
}