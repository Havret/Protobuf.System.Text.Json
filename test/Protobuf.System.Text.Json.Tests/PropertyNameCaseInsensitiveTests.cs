using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using Shouldly;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class PropertyNameCaseInsensitiveTests
{
    [Fact]
    public void Should_support_PropertyNameCaseInsensitive_setting()
    {
        var originalMessage = new SimpleMessage
        {
            DoubleProperty = 2.5d
        };
        
        // Serialize message using PascalCase naming policy (default)
        var serializerOptions = new JsonSerializerOptions();
        serializerOptions.AddProtobufSupport();
        
        var payload = JsonSerializer.Serialize(originalMessage, serializerOptions);
        
        // Deserialize message using CamelCase naming policy (custom)
        // with PropertyNameCaseInsensitive option set to true
        var deserializerOptions = new JsonSerializerOptions();
        deserializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        deserializerOptions.PropertyNameCaseInsensitive = true;
        deserializerOptions.AddProtobufSupport();
        
        var deserialized = JsonSerializer.Deserialize<SimpleMessage>(payload, deserializerOptions);
        
        deserialized.ShouldBeEquivalentTo(originalMessage);
    }
}