using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using Google.Protobuf;
using Protobuf.System.Text.Json.Tests.Utils;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class SimpleMessageTests
{
    [Fact]
    public void Should_serialize_message_with_primitive_types()
    {
        // Arrange
        var msg = new SimpleMessage
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
            StringProperty = "hello",
            BytesProperty = ByteString.CopyFromUtf8("abc")
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        
        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_deserialize_message_with_primitive_types()
    {
        // Arrange
        var msg = new SimpleMessage
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
            StringProperty = "hello",
            BytesProperty = ByteString.CopyFromUtf8("abc")
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<SimpleMessage>(serialized, jsonSerializerOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(msg.Int32Property, deserialized.Int32Property);
        Assert.Equal(msg.Int64Property, deserialized.Int64Property);
    }
}