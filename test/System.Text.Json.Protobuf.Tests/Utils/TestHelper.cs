namespace System.Text.Json.Protobuf.Tests.Utils;

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