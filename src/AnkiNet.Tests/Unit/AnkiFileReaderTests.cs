using FluentAssertions;

namespace AnkiNet.Tests.Unit;

public class AnkiFileReaderTests
{
    [Fact]
    public async Task WhenRead_ThenNoExceptionIsThrown()
    {
        var action = async () => _ = await AnkiFileReader.ReadFromFileAsync("unknown");
        await action.Should().ThrowExactlyAsync<FileNotFoundException>();
    }
}