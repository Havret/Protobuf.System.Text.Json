using System.Text.Json.Protobuf.Tests.Utils;
using Shouldly;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace System.Text.Json.Protobuf.Tests;

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
}