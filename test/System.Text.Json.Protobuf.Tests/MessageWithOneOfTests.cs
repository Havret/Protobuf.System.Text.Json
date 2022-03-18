using System.Text.Json.Protobuf.Tests.Utils;
using Shouldly;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace System.Text.Json.Protobuf.Tests;

public class MessageWithOneOfTests
{
    [Fact]
    public void Should_serialize_message_with_one_of_when_value_is_set()
    {
        // Arrange
        var msg = new MessageWithOneOf
        {
            Int32PropertyValue = 5
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_serialize_message_with_one_of_when_value_is_not_set()
    {
        // Arrange
        var msg = new MessageWithOneOf();

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_serialize_and_deserialize_message_with_one_of_when_value_is_set()
    {
        // Arrange
        var msg = new MessageWithOneOf
        {
            Int64PropertyValue = 5
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithOneOf>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.Int32PropertyValue.ShouldBe(msg.Int32PropertyValue);
        deserialized.Int64PropertyValue.ShouldBe(msg.Int64PropertyValue);
        deserialized.PropertyCase.ShouldBe(msg.PropertyCase);
    }
    
    [Fact]
    public void Should_serialize_and_deserialize_message_with_one_of_when_value_is_not_set()
    {
        // Arrange
        var msg = new MessageWithOneOf();
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithOneOf>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.Int32PropertyValue.ShouldBe(msg.Int32PropertyValue);
        deserialized.Int64PropertyValue.ShouldBe(msg.Int64PropertyValue);
        deserialized.PropertyCase.ShouldBe(msg.PropertyCase);
    }
    
    [Fact]
    public void Should_serialize_message_with_optional_property_when_value_is_set()
    {
        // Arrange
        var msg = new MessageWithOptionalProperty
        {
            Int32Property = 5
        };
        
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_serialize_message_with_optional_property_when_value_is_not_set()
    {
        // Arrange
        var msg = new MessageWithOptionalProperty();
        
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_serialize_and_deserialize_message_with_optional_property_when_value_is_set()
    {
        // Arrange
        var msg = new MessageWithOptionalProperty()
        {
            Int32Property = 5
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithOptionalProperty>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.Int32Property.ShouldBe(msg.Int32Property);
        deserialized.HasInt32Property.ShouldBe(msg.HasInt32Property);
    }
    
    [Fact]
    public void Should_serialize_and_deserialize_message_with_optional_property_when_value_is_not_set()
    {
        // Arrange
        var msg = new MessageWithOptionalProperty();
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithOptionalProperty>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.Int32Property.ShouldBe(msg.Int32Property);
        deserialized.HasInt32Property.ShouldBe(msg.HasInt32Property);
    }
}