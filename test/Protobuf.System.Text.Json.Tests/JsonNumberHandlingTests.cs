using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using System.Text.Json.Serialization;
using Protobuf.System.Text.Json.Tests.Utils;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class JsonNumberHandlingTests
{
    [Fact]
    public void Should_deserialize_numbers_as_strings_when_NumberHandling_set_to_AllowReadingFromString()
    {
        // Arrange
        var json =
        """
        {
          "doubleProperty" : "0.12",
          "floatProperty" : "7.89",
          "int32Property" : "123",
          "int64Property" : "456",
          "uint32Property" : "789",
          "uint64Property" : "101112",
          "sint32Property" : "1314",
          "sint64Property" : "151617",
          "fixed32Property" : "1819",
          "fixed64Property" : "202122",
          "sfixed32Property" : "2324",
          "sfixed64Property" : "252627"
        }
        """;
        
        var options = TestHelper.CreateJsonSerializerOptions();
        options.NumberHandling = JsonNumberHandling.AllowReadingFromString;

        // Act
        var deserialized = JsonSerializer.Deserialize<SimpleMessage>(json, options);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(0.12d, deserialized.DoubleProperty);
        Assert.Equal(7.89f, deserialized.FloatProperty);
        Assert.Equal(123, deserialized.Int32Property);
        Assert.Equal(456L, deserialized.Int64Property);
        Assert.Equal(789u, deserialized.Uint32Property);
        Assert.Equal(101112ul, deserialized.Uint64Property);
        Assert.Equal(1314, deserialized.Sint32Property);
        Assert.Equal(151617L, deserialized.Sint64Property);
        Assert.Equal(1819u, deserialized.Fixed32Property);
        Assert.Equal(202122ul, deserialized.Fixed64Property);
        Assert.Equal(2324, deserialized.Sfixed32Property);
        Assert.Equal(252627L, deserialized.Sfixed64Property);
    }
    
    [Fact]
    public void Should_fail_deserializing_numbers_as_strings_when_NumberHandling_not_set_to_AllowReadingFromString()
    {
        // Arrange
        var json =
        """
        {
          "doubleProperty" : "0.12",
          "floatProperty" : "7.89",
          "int32Property" : "123",
          "int64Property" : "456",
          "uint32Property" : "789",
          "uint64Property" : "101112",
          "sint32Property" : "1314",
          "sint64Property" : "151617",
          "fixed32Property" : "1819",
          "fixed64Property" : "202122",
          "sfixed32Property" : "2324",
          "sfixed64Property" : "252627"
        }
        """;

        var options = TestHelper.CreateJsonSerializerOptions();
        // Explicitly disable reading numbers from strings
        options.NumberHandling = JsonNumberHandling.Strict;

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<SimpleMessage>(json, options));
    }
}