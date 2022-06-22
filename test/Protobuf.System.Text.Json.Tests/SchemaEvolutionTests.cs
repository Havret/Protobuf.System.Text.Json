using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using Protobuf.System.Text.Json.Tests.Utils;
using Shouldly;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class SchemaEvolutionTests
{
    [Fact]
    public void Should_be_able_to_deserialize_array_of_schema_v2_to_array_of_schema_v1_when_schema_v2_has_additional_repeated_field()
    {
        // Arrange
        var schemaEvolutionV2 = new[]
        {
            new SchemaEvolutionV2
            {
                Property1 = 1,
                Property2 = { 2, 3 }
            }
        };
        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        var serialized = JsonSerializer.Serialize(schemaEvolutionV2, jsonSerializerOptions);

        // Act
        var deserialized = JsonSerializer.Deserialize<SchemaEvolutionV1[]>(serialized, jsonSerializerOptions);

        // Assert
        deserialized.ShouldHaveSingleItem();
    }
}