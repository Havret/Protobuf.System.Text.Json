using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using Protobuf.System.Text.Json.Tests.Utils;
using Shouldly;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class ContractEvolutionTests
{
    [Fact]
    public void Should_deserialize_a_new_version_of_a_message_using_the_old_version_of_the_contract()
    {
        // Arrange
        var msg = new MessageWithVersionMismatchV2
        {
            DoubleProperty = 1d,
            FloatProperty = 2f,
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        
        // Act
        var payload = JsonSerializer.Serialize(msg, jsonSerializerOptions);
        var deserialized = JsonSerializer.Deserialize<MessageWithVersionMismatch>(payload, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.DoubleProperty.ShouldBe(msg.DoubleProperty);
    }
}