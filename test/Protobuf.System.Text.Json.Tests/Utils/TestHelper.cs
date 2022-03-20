using System.Text.Json;

namespace Protobuf.System.Text.Json.Tests.Utils;

public class TestHelper
{
    public static JsonSerializerOptions CreateJsonSerializerOptions()
    {
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonSerializerOptions.AddProtobufSupport();
        return jsonSerializerOptions;
    }
}