using System.Net.NetworkInformation;
using System.Security.AccessControl;

namespace LongmanDictionary.Services;

public static class ReachabilityChecker
{
    public static async Task<bool> CheckAsync(TimeSpan timeout)
    {
        using var pingSender = new Ping();
        
        try
        {
            var replay = await pingSender.SendPingAsync(LdUrls.MainPage, (int)timeout.TotalMilliseconds);
            return replay.Status == IPStatus.Success;
        }
        catch
        {
            return false;
        }
    }
}