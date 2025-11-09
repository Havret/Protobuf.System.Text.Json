using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using Protobuf.System.Text.Json.Tests.Utils;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class JsonNamingPolicyTests
{
    [Fact]
    public void Should_serialize_message_with_primitive_types()
    {
        // Arrange
        var msg = new SimpleMessage
        {
            DoubleProperty = 2.5d
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        jsonSerializerOptions.PropertyNamingPolicy = new JsonLowerCaseNamingPolicy();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_ignore_PropertyNamingPolicy_when_UseProtobufJsonNames_set_to_true()
    {
        // Arrange
        var msg = new SimpleMessage
        {
            DoubleProperty = 2.5d
        };
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.PropertyNamingPolicy = new JsonLowerCaseNamingPolicy();
#pragma warning disable CS0618 // Type or member is obsolete
        jsonSerializerOptions.AddProtobufSupport(options => options.UseProtobufJsonNames = true);
#pragma warning restore CS0618 // Type or member is obsolete
        
        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_use_protobuf_json_name_when_PropertyNamingSource_set_to_ProtobufJsonName()
    {
        // Arrange
        var msg = new SimpleMessage
        {
            DoubleProperty = 2.5d
        };
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.PropertyNamingPolicy = new JsonLowerCaseNamingPolicy();
        jsonSerializerOptions.AddProtobufSupport(options => options.PropertyNamingSource = PropertyNamingSource.ProtobufJsonName);
        
        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_use_protobuf_field_name_when_PropertyNamingSource_set_to_ProtobufFieldName()
    {
        // Arrange
        var msg = new SimpleMessage
        {
            DoubleProperty = 2.5d
        };
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.PropertyNamingPolicy = new JsonLowerCaseNamingPolicy();
        jsonSerializerOptions.AddProtobufSupport(options => options.PropertyNamingSource = PropertyNamingSource.ProtobufFieldName);
        
        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_deserialize_using_protobuf_field_name_when_PropertyNamingSource_set_to_ProtobufFieldName()
    {
        // Arrange
        var json = "{\"double_property\": 2.5}";
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.AddProtobufSupport(options => options.PropertyNamingSource = PropertyNamingSource.ProtobufFieldName);
        
        // Act
        var deserialized = JsonSerializer.Deserialize<SimpleMessage>(json, jsonSerializerOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(2.5d, deserialized.DoubleProperty);
    }

    private class JsonLowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToLower();
        }
    }
}