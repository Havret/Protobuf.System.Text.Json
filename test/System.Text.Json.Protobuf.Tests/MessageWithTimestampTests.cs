using System.Text.Json.Protobuf.Tests.Utils;
using Google.Protobuf.WellKnownTypes;
using Shouldly;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace System.Text.Json.Protobuf.Tests;

public class MessageWithTimestampTests
{
    [Fact]
    public void Should_serialize_message_with_timestamp()
    {
        // Arrange
        var msg = new MessageWithTimestamp
        {
            TimestampProperty = Timestamp.FromDateTime(new DateTime(1991, 01, 25, 0, 0, 0, DateTimeKind.Utc))
        };
        
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_serialize_message_with_timestamp_when_value_not_set()
    {
        // Arrange
        var msg = new MessageWithTimestamp();
        
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }
    
    [Fact]
    public void Should_serialize_and_deserialize_message_with_timestamp()
    {
        // Arrange
        var msg = new MessageWithTimestamp
        {
            TimestampProperty = Timestamp.FromDateTime(new DateTime(1991, 01, 25, 0, 0, 0, DateTimeKind.Utc))
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithTimestamp>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.ShouldBeEquivalentTo(msg);
    }
    
    [Fact]
    public void Should_serialize_and_deserialize_message_with_timestamp_when_value_is_not_set()
    {
        // Arrange
        var msg = new MessageWithTimestamp();
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithTimestamp>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.ShouldBeEquivalentTo(msg);
    }
}