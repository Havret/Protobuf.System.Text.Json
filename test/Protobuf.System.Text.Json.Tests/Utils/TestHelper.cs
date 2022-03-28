using System.Text.Json;
using System.Text.Json.Serialization;

namespace Protobuf.System.Text.Json.Tests.Utils;

public class TestHelper
{
    public static JsonSerializerOptions CreateJsonSerializerOptions()
    {
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonSerializerOptions.AddProtobufSupport();
        return jsonSerializerOptions;
    }
}