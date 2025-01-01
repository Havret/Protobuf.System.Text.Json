using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using System.Text.Json.Serialization;
using Protobuf.System.Text.Json.Tests.Utils;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class MessageWithWellKnownTypesTests
{
    [Fact]
    public void Should_serialize_message_with_well_known_types_when_values_are_set()
    {
        // Arrange
        var msg = new MessageWithWellKnownTypes
        {
            DoubleValue = double.MaxValue,
            FloatValue = float.MaxValue,
            Int64Value = long.MaxValue,
            Uint64Value = ulong.MaxValue,
            Int32Value = int.MaxValue,
            Uint32Value = uint.MaxValue,
            BoolValue = true,
            StringValue = "some_string_value",
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_serialize_message_with_well_known_types_when_values_are_not_set()
    {
        // Arrange
        var msg = new MessageWithWellKnownTypes();
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_serialize_and_deserialize_message_with_well_known_types_when_values_are_set()
    {
        // Arrange
        var msg = new MessageWithWellKnownTypes
        {
            DoubleValue = double.MaxValue,
            FloatValue = float.MaxValue,
            Int64Value = long.MaxValue,
            Uint64Value = ulong.MaxValue,
            Int32Value = int.MaxValue,
            Uint32Value = uint.MaxValue,
            BoolValue = true,
            StringValue = "some_string_value"
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Act
        var deserialized = JsonSerializer.Deserialize<MessageWithWellKnownTypes>(serialized, jsonSerializerOptions);


        // Assert
        Assert.Equal(msg, deserialized);

    }

    [Fact]
    public void Should_serialize_and_deserialize_message_with_well_known_types_when_values_are_not_set()
    {
        // Arrange
        var msg = new MessageWithWellKnownTypes();
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Act
        var deserialized = JsonSerializer.Deserialize<MessageWithWellKnownTypes>(serialized, jsonSerializerOptions);
        
        // Assert
        Assert.Equal(msg, deserialized);
    }
}