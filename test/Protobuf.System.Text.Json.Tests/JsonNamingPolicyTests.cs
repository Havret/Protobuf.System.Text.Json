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
        jsonSerializerOptions.AddProtobufSupport(options => options.UseProtobufJsonNames = true);
        
        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    private class JsonLowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToLower();
        }
    }
}