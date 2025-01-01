using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using Protobuf.System.Text.Json.Tests.Utils;
using SmartAnalyzers.ApprovalTestsExtensions;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class MessageWithRepeatedListTests
{
    [Fact]
    public void Should_serialize_message_with_repeated_list()
    {
        // Arrange
        var msg = new MessageWithRepeatedList
        {
            Int32List = {1, 2, 3},
            Int64List = {int.MaxValue + 1L, int.MaxValue + 2L, int.MaxValue + 3L},
            EnumList = {Corpus.Images, Corpus.News},
            NestedMessageList =
            {
                new NestedField
                {
                    Field = 4
                }
            }
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
            Int64List = {int.MaxValue + 1L, int.MaxValue + 2L, int.MaxValue + 3L},
            EnumList = {Corpus.Images, Corpus.News},
            NestedMessageList =
            {
                new NestedField
                {
                    Field = 4
                }
            }
        };

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();

        // Act
        var serialized = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithRepeatedList>(serialized, jsonSerializerOptions);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(msg.Int32List, deserialized.Int32List);
        Assert.Equal(msg.Int64List, deserialized.Int64List);
        Assert.Equal(msg.EnumList, deserialized.EnumList);
        Assert.Equal(msg.NestedMessageList, deserialized.NestedMessageList);
    }
}