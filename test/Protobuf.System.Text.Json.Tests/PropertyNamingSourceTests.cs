using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class PropertyNamingSourceTests
{
    [Fact]
    public void Should_use_custom_json_name_when_PropertyNamingSource_is_ProtobufJsonName()
    {
        // Arrange
        var msg = new MessageWithCustomJsonName
        {
            DoubleProperty = 2.5d,
            StringProperty = "test"
        };
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.AddProtobufSupport(options => options.PropertyNamingSource = PropertyNamingSource.ProtobufJsonName);
        
        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_use_proto_field_name_when_PropertyNamingSource_is_ProtobufFieldName()
    {
        // Arrange
        var msg = new MessageWithCustomJsonName
        {
            DoubleProperty = 2.5d,
            StringProperty = "test"
        };
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.AddProtobufSupport(options => options.PropertyNamingSource = PropertyNamingSource.ProtobufFieldName);
        
        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_deserialize_using_custom_json_name_when_PropertyNamingSource_is_ProtobufJsonName()
    {
        // Arrange
        var json = "{\"customDoubleProperty\": 2.5, \"stringProperty\": \"test\"}";
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.AddProtobufSupport(options => options.PropertyNamingSource = PropertyNamingSource.ProtobufJsonName);
        
        // Act
        var deserialized = JsonSerializer.Deserialize<MessageWithCustomJsonName>(json, jsonSerializerOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(2.5d, deserialized.DoubleProperty);
        Assert.Equal("test", deserialized.StringProperty);
    }

    [Fact]
    public void Should_deserialize_using_proto_field_name_when_PropertyNamingSource_is_ProtobufFieldName()
    {
        // Arrange
        var json = "{\"double_property\": 2.5, \"string_property\": \"test\"}";
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.AddProtobufSupport(options => options.PropertyNamingSource = PropertyNamingSource.ProtobufFieldName);
        
        // Act
        var deserialized = JsonSerializer.Deserialize<MessageWithCustomJsonName>(json, jsonSerializerOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(2.5d, deserialized.DoubleProperty);
        Assert.Equal("test", deserialized.StringProperty);
    }

    [Fact]
    public void Should_round_trip_with_ProtobufFieldName()
    {
        // Arrange
        var original = new MessageWithCustomJsonName
        {
            DoubleProperty = 2.5d,
            StringProperty = "test"
        };
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.AddProtobufSupport(options => options.PropertyNamingSource = PropertyNamingSource.ProtobufFieldName);
        
        // Act
        var serialized = JsonSerializer.Serialize(original, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithCustomJsonName>(serialized, jsonSerializerOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(original.DoubleProperty, deserialized.DoubleProperty);
        Assert.Equal(original.StringProperty, deserialized.StringProperty);
    }
}
