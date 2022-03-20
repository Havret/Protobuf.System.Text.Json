using Newtonsoft.Json;

namespace System.Text.Json.Protobuf.Benchmark.Shared;

public static class BenchmarkHelper
{
    public static JsonSerializerOptions JsonSerializerOptions = CreateJsonSerializerOptions();

    private static JsonSerializerOptions CreateJsonSerializerOptions()
    {
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonSerializerOptions.AddProtobufSupport();
        return jsonSerializerOptions;
    }

    public static JsonSerializerSettings JsonSerializerSettings = CreateJsonSerializerSettings();

    private static JsonSerializerSettings CreateJsonSerializerSettings()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new NewtonsoftProtobufJsonConverter(3, true));
        return settings;
    }
}