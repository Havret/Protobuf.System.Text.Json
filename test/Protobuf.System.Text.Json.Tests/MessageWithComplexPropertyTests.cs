using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using System.Text.Json.Serialization;
using Protobuf.System.Text.Json.Tests.Utils;
using Shouldly;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class MessageWithComplexPropertyTests
{
    [Fact]
    public void Should_serialize_message_with_complex_property()
    {
        // Arrange
        var msg = new MessageWithComplexProperty
        {
            NestedMessage = new NestedMessage
            {
                Property = "hello"
            }
        };

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_deserialize_message_with_complex_property()
    {
        // Arrange
        var msg = new MessageWithComplexProperty
        {
            NestedMessage = new NestedMessage
            {
                Property = "hello"
            }
        };

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithComplexProperty>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldBeEquivalentTo(msg);
    }
    
    [Fact]
    public void Should_deserialize_message_with_complex_property_when_no_value_is_set()
    {
        // Arrange
        var msg = new MessageWithComplexProperty
        {
            NestedMessage = null
        };

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        
        // make sure the value won't be ignored
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;


        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithComplexProperty>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldBeEquivalentTo(msg);
    }
}