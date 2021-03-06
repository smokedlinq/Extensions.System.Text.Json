using A3;
using AutoFixture;
using FluentAssertions;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Extensions.System.Text.Json.Tests
{
    public class JsonReadOnlyCollectionConverterTests
    {
        [Theory]
        [A3Data]
        public void ReadOnlyCollectionPropertyIsPopulatedDuringDeserialization(JsonObject obj)
            => A3<JsonSerializerOptions>
            .Arrange(setup =>
            {
                var json = JsonSerializer.Serialize(obj);
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonReadOnlyCollectionConverter());

                setup.Sut(options);
                setup.Parameter(json);
            })
            .Act((JsonSerializerOptions sut, string json) => JsonSerializer.Deserialize<JsonObject>(json, sut))
            .Assert(result => result.Collection.Should().BeEquivalentTo(obj.Collection));

        [Theory]
        [A3Data]
        public void ReadOnlyListPropertyIsPopulatedDuringDeserialization(JsonObject obj)
            => A3<JsonSerializerOptions>
            .Arrange(setup =>
            {
                var json = JsonSerializer.Serialize(obj);
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonReadOnlyCollectionConverter());

                setup.Sut(options);
                setup.Parameter(json);
            })
            .Act((JsonSerializerOptions sut, string json) => JsonSerializer.Deserialize<JsonObject>(json, sut))
            .Assert(result => result.List.Should().BeEquivalentTo(obj.List));

        [Theory]
        [A3Data]
        public void ObjectPropertyIsPopulatedDuringDeserialization(JsonObject obj)
            => A3<JsonSerializerOptions>
            .Arrange(setup =>
            {
                obj.Object = new JsonObject();

                var json = JsonSerializer.Serialize(obj);
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonReadOnlyCollectionConverter());

                setup.Sut(options);
                setup.Parameter(json);
            })
            .Act((JsonSerializerOptions sut, string json) => JsonSerializer.Deserialize<JsonObject>(json, sut))
            .Assert(result => result.Object.Should().NotBeNull());

        public class JsonObjectFixture : ICustomizeFixture<JsonObject>
        {
            public JsonObject Customize(IFixture fixture)
            {
                var value = new JsonObject();
                value.Collection.AddMany(fixture.Create<int>, 5);
                value.List.AddMany(fixture.Create<int>, 5);
                return value;
            }
        }

        public class JsonObject
        {
            public ICollection<int> Collection { get; } = new List<int>();
            public IList<int> List { get; } = new List<int>();
            public JsonObject? Object { get; set; }
        }
    }
}
