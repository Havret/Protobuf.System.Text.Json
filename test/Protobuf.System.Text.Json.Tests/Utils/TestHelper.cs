using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Protobuf.System.Text.Json.Tests.Utils;

public class TestHelper
{
    public static JsonSerializerOptions CreateJsonSerializerOptions(Action<JsonProtobufSerializerOptions>? configure = null)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            
        };
        jsonSerializerOptions.AddProtobufSupport(configure ?? (_ => { }));
        return jsonSerializerOptions;
    }
}