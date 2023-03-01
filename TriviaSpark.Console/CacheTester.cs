using System.Diagnostics;
using System.Runtime.Caching;
using TriviaSpark.Core.Cache;

public static class CacheTester
{
    private static Stopwatch _timer = new Stopwatch();

    public static void TestCache()
    {
        MemoryCache cache = MemoryCache.Default;
        int size = (int)1e6;

        Start();
        for (int idx = 0; idx < size; idx++)
        {
            cache.Add(idx.ToString(), "Value" + idx.ToString(), GetPolicy(idx, cache));
        }
        long prevCnt = cache.GetCount();
        Stop($"Added    {prevCnt} items");

        Start();
        SignaledChangeMonitor.Signal("NamedData");
        Stop($"Removed  {prevCnt - cache.GetCount()} entries");
        prevCnt = cache.GetCount();

        Start();
        SignaledChangeMonitor.Signal();
        Stop($"Removed  {prevCnt - cache.GetCount()} entries");
    }

    private static CacheItemPolicy GetPolicy(int idx, MemoryCache cache)
    {
        string name = idx % 10 == 0 ? "NamedData" : null;

        CacheItemPolicy cip = new CacheItemPolicy();
        cip.AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(1);
        var monitor = new SignaledChangeMonitor(idx.ToString(), name);
        cip.ChangeMonitors.Add(monitor);
        return cip;
    }

    private static void Start()
    {
        _timer.Start();
    }

    private static void Stop(string msg = null)
    {
        _timer.Stop();
        Console.WriteLine($"{msg} | {_timer.Elapsed.TotalSeconds} sec");
        _timer.Reset();
    }
}