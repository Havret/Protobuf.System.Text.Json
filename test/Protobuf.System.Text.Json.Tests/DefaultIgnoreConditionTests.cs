using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using System.Text.Json.Serialization;
using Protobuf.System.Text.Json.Tests.Utils;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class DefaultIgnoreConditionTests
{
    [Fact]
    public void Should_not_ignore_null_properties_when_DefaultIgnoreCondition_set_to_Never()
    {
        // Arrange
        var msg = new MessageWithComplexProperty
        {
            NestedMessage = null
        };

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        
        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_ignore_null_properties_when_DefaultIgnoreCondition_set_to_WhenWritingNull()
    {
        // Arrange
        var msg = new MessageWithComplexProperty
        {
            NestedMessage = null
        };

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        
        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_ignore_null_properties_when_DefaultIgnoreCondition_set_to_WhenWritingDefault()
    {
        // Arrange
        var msg = new MessageWithComplexProperty
        {
            NestedMessage = null
        };

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        
        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_not_ignore_default_properties_when_DefaultIgnoreCondition_set_to_Never()
    {
        // Arrange
        var msg = new SimpleMessage();

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        
        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_ignore_default_properties_when_DefaultIgnoreCondition_set_to_WhenWritingDefault()
    {
        // Arrange
        var msg = new SimpleMessage();

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        
        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_not_ignore_default_non_nullable_properties_when_DefaultIgnoreCondition_set_to_WhenWritingNull()
    {
        // Arrange
        var msg = new SimpleMessage();

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        
        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
}