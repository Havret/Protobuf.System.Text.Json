using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using System.Threading.Tasks;
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
    
    [Fact]
    public async Task Should_be_able_to_deserialize_array_of_schema_v2_to_array_of_schema_v1_when_schema_v2_has_additional_repeated_field_2()
    {
        // Arrange
        var schemaEvolutionV2 = Enumerable.Range(0, 50_000).Select(x => new SchemaEvolutionV2
        {
            Property1 = x,
            Property2 = {x + 1, x + 2}
        }).ToList();

        var jsonSerializerOptions = TestHelper.CreateJsonSerializerOptions();
        using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, schemaEvolutionV2, jsonSerializerOptions);
        memoryStream.Position = 0;

        // Act
        var deserialized = JsonSerializer.Deserialize<SchemaEvolutionV1[]>(memoryStream, jsonSerializerOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.Length.ShouldBe(schemaEvolutionV2.Count);
    }
}