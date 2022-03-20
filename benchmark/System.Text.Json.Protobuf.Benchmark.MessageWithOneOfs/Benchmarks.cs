using System.Text.Json.Protobuf.Benchmark.Shared;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;

namespace System.Text.Json.Protobuf.Benchmark.MessageWithOneOfs;

public class Benchmarks
{
    private TestMessage _testMessage = new()
    {
        DoublePropertyValue = 2.5d,
        FloatPropertyValue = 3.5f,
        Int32PropertyValue = 1,
        Int64PropertyValue = 2,
        Uint32PropertyValue = 3,
        Uint64PropertyValue = 4,
        Sint32PropertyValue = 5,
        Sint64PropertyValue = 6,
        Fixed32PropertyValue = 7,
        Fixed64PropertyValue = 8,
        Sfixed32PropertyValue = 9,
        Sfixed64PropertyValue = 10,
        BoolPropertyValue = true,
    };

    [Benchmark]
    public void SystemTextJsonProtobufSupport()
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