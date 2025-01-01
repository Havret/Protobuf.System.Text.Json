using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using Google.Protobuf.WellKnownTypes;
using Protobuf.System.Text.Json.Tests.Utils;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class MessageWithDurationTests
{
    [Fact]
    public void Should_serialize_message_with_duration()
    {
        // Arrange
        var msg = new MessageWithDuration
        {
            DurationProperty = new Duration {Seconds = 256, Nanos = 512}
        };

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_serialize_message_with_duration_when_value_not_set()
    {
        // Arrange
        var msg = new MessageWithDuration();

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_serialize_and_deserialize_message_with_duration()
    {
        // Arrange
        var msg = new MessageWithDuration
        {
            DurationProperty = new Duration {Seconds = 256, Nanos = 500}
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithDuration>(serialized, jsonSerializerOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(deserialized, msg);
    }

    [Fact]
    public void Should_serialize_and_deserialize_message_with_duration_when_value_is_not_set()
    {
        // Arrange
        var msg = new MessageWithDuration();
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithDuration>(serialized, jsonSerializerOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(deserialized, msg);
    }

    [Fact]
    public void Should_serialize_duration_property_as_complex_object_when_TreatDurationAsTimeSpan_option_set_to_false()
    {
        // Arrange
        var msg = new MessageWithDuration
        {
            DurationProperty = new Duration {Seconds = 256, Nanos = 512}
        };

        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonSerializerOptions.AddProtobufSupport(options =>
        {
            options.TreatDurationAsTimeSpan = false;
        });

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_deserialize_duration_property_from_complex_object_when_TreatDurationAsTimeSpan_option_set_to_false()
    {
        // Arrange
        var msg = new MessageWithDuration
        {
            DurationProperty = new Duration {Seconds = 256, Nanos = 512}
        };

        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonSerializerOptions.AddProtobufSupport(options =>
        {
            options.TreatDurationAsTimeSpan = false;
        });

        // Act
        var payload = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithDuration>(payload, jsonSerializerOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(deserialized, msg);
    }
}