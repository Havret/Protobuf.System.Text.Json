using System.Text.Json.Protobuf.Tests.Utils;
using Shouldly;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace System.Text.Json.Protobuf.Tests;

public class MessageWithEnumFieldTests
{
    [Fact]
    public void Should_serialize_message_with_enum_field()
    {
        // Arrange
        var msg = new MessageWithEnum
        {
            EnumField = TestEnum.FirstOption
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
 
        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_deserialize_message_with_with_enum_field()
    {
        // Arrange
        var msg = new MessageWithEnum
        {
            EnumField = TestEnum.FirstOption
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithEnum>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.ShouldBeEquivalentTo(msg);
    }
}