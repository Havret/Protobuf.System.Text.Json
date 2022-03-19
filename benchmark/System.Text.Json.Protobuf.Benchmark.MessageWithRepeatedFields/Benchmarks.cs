using System.Text.Json.Protobuf.Benchmark.Shared;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;

namespace System.Text.Json.Protobuf.Benchmark.MessageWithRepeatedFields;

[MarkdownExporter]
public class Benchmarks
{
    private TestMessage _testMessage = new()
    {
        DoubleProperty = {Enumerable.Range(0, 100).Select(x => x + 2d)},
        FloatProperty = {Enumerable.Range(0, 100).Select(x => x + 3.5f)},
        Int32Property = {Enumerable.Range(0, 100)},
        Int64Property = {Enumerable.Range(0, 100).Select(x => x + 1L)},
        Uint32Property = {Enumerable.Range(0, 100).Select(x => (uint) x)},
        Uint64Property = {Enumerable.Range(0, 100).Select(x => (ulong) x)},
        Sint32Property = {Enumerable.Range(0, 100)},
        Sint64Property = {Enumerable.Range(0, 100).Select(x => x + 1L)},
        Fixed32Property = {Enumerable.Range(0, 100).Select(x => (uint) x)},
        Fixed64Property = {Enumerable.Range(0, 100).Select(x => (ulong) x)},
        Sfixed32Property = {Enumerable.Range(0, 100).Select(x => x)},
        Sfixed64Property = {Enumerable.Range(0, 100).Select(x => (long) x)},
        BoolProperty = {Enumerable.Range(0, 100).Select(x => x % 2 == 0)},
        StringProperty = {Enumerable.Range(0, 100).Select(x => "hello" + x)}
    };

    [Benchmark]
    public void SystemTextJson()
    {
        var serialized = JsonSerializer.Serialize(_testMessage, BenchmarkHelper.JsonSerializerOptions);
        var _ = JsonSerializer.Deserialize<TestMessage>(serialized, BenchmarkHelper.JsonSerializerOptions);
    }

    [Benchmark]
    public void NewtonsoftJsonWithProtobufParserAndFormatter()
    {
        var serialized = JsonConvert.SerializeObject(_testMessage, BenchmarkHelper.JsonSerializerSettings);
        var _ = JsonConvert.DeserializeObject<TestMessage>(serialized, BenchmarkHelper.JsonSerializerSettings);
    }
}