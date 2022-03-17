using System.Text.Json.Protobuf.Tests.Utils;
using Shouldly;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace System.Text.Json.Protobuf.Tests;

public class MessageWithRepeatedListTests
{
    [Fact]
    public void Should_serialize_message_with_repeated_list()
    {
        // Arrange
        var msg = new MessageWithRepeatedList
        {
            Int32List = {1, 2, 3},
            Int64List = {int.MaxValue + 1L, int.MaxValue + 2L, int.MaxValue + 3L}
        };

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);

        // Assert
        var approver = new ExplicitApprover();
        approver.VerifyJson(serialized);
    }

    [Fact]
    public void Should_deserialize_message_with_repeated_list()
    {
        // Arrange
        var msg = new MessageWithRepeatedList
        {
            Int32List = {1, 2, 3},
            Int64List = {int.MaxValue + 1L, int.MaxValue + 2L, int.MaxValue + 3L}
        };

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithRepeatedList>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();

        deserialized.Int32List.ShouldBe(msg.Int32List);
        deserialized.Int64List.ShouldBe(msg.Int64List);
    }
}