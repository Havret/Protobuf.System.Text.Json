using System.Text.Json.Protobuf.Tests.Utils;
using Shouldly;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace System.Text.Json.Protobuf.Tests;

public class MessageWithMapsTests
{
    [Fact]
    public void Should_serialize_message_with_map_field()
    {
        // Arrange
        var msg = new MessageWithMaps
        {
            MapIntToString = {[1] = "1-value"},
            MapStringToComplexType =
            {
                ["string_key"] = new NestedMessageAsKey
                {
                    MapStringToInt = {["a"] = 1}
                }
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
    public void Should_deserialize_message_with_map_field()
    {
        // Arrange
        var msg = new MessageWithMaps
        {
            MapIntToString = {[1] = "1-value"},
            MapStringToComplexType =
            {
                ["string_key"] = new NestedMessageAsKey
                {
                    MapStringToInt = {["a"] = 1}
                }
            }
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithMaps>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.ShouldBeEquivalentTo(msg);
    }
}