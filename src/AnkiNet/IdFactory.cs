namespace AnkiNet;

internal static class IdFactory
{
    public static long Create() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}