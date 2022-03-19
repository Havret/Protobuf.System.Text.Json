using System.Text.Json.Protobuf.Benchmark.Shared;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;

namespace System.Text.Json.Protobuf.Benchmark;

public class Benchmarks
{
    private TestMessage _testMessage = new()
    {
        DoubleProperty = 2.5d,
        FloatProperty = 3.5f,
        Int32Property = 1,
        Int64Property = 2,
        Uint32Property = 3,
        Uint64Property = 4,
        Sint32Property = 5,
        Sint64Property = 6,
        Fixed32Property = 7,
        Fixed64Property = 8,
        Sfixed32Property = 9,
        Sfixed64Property = 10,
        BoolProperty = true,
        StringProperty = "hello"
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