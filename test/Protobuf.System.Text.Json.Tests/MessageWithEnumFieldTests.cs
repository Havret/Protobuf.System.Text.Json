using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using Protobuf.System.Text.Json.Tests.Utils;
using Shouldly;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

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

    [Fact]
    public void Should_serialize_enum_value_using_proto_enum_value_name()
    {
        // Arrange
        var msg = new MessageWithEnum
        {
            EnumField = TestEnum.FirstOption
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions(options =>
        {
            options.UseStringProtoEnumValueNames = true;
        });
 
        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_serialize_enum_value_as_number_when_using_proto_value_name_is_not_possible()
    {
        // Arrange
        var msg = new MessageWithEnum
        {
            EnumField = (TestEnum) 99
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions(options =>
        {
            options.UseStringProtoEnumValueNames = true;
        });
 
        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_deserialize_message_with_enum_field_when_value_serialized_using_proto_value_name()
    {
        // Arrange
        var msg = new MessageWithEnum
        {
            EnumField = TestEnum.FirstOption
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions(options =>
        {
            options.UseStringProtoEnumValueNames = true;
        });

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithEnum>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.ShouldBeEquivalentTo(msg);
    }
    
    [Fact]
    public void Should_throw_exception_when_string_enum_value_cannot_be_deserialized()
    {
        // Arrange
        var msg = new MessageWithEnum
        {
            EnumField = TestEnum.FirstOption
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions(options =>
        {
            options.UseStringProtoEnumValueNames = true;
        });
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var invalidPayload = serialized.Replace("FIRST_OPTION", "INVALID_OPTION");

        // Act & Assert
        var exception = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<MessageWithEnum>(invalidPayload, jsonSerializerOptions));
        exception.Message.ShouldContain("'INVALID_OPTION' is not a valid value for type System.Text.Json.Protobuf.Tests.TestEnum.");
    }
}