namespace Orchestra.Tests;

using System;
using Orchestra.Changelog;
using Orc.Serialization.Json;
using NUnit.Framework;

[TestFixture]
public class JsonSerializationFacts
{
    [TestFixture]
    public class The_RecentlyUsedItems_Model
    {
        [Test]
        public void RoundTrips_With_Init_Only_Properties()
        {
            var model = new RecentlyUsedItems
            {
                Items =
                {
                    new RecentlyUsedItem("item 1", new DateTime(2026, 05, 01, 11, 22, 33, DateTimeKind.Utc))
                },
                PinnedItems =
                {
                    new RecentlyUsedItem("pinned 1", new DateTime(2026, 05, 02, 11, 22, 33, DateTimeKind.Utc))
                }
            };

            var deserializedModel = RoundTrip(model, new JsonSerializerSettings
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(deserializedModel.Items.Count, Is.EqualTo(1));
            Assert.That(deserializedModel.Items[0].Name, Is.EqualTo("item 1"));
            Assert.That(deserializedModel.Items[0].DateTime, Is.EqualTo(new DateTime(2026, 05, 01, 11, 22, 33, DateTimeKind.Utc)));

            Assert.That(deserializedModel.PinnedItems.Count, Is.EqualTo(1));
            Assert.That(deserializedModel.PinnedItems[0].Name, Is.EqualTo("pinned 1"));
            Assert.That(deserializedModel.PinnedItems[0].DateTime, Is.EqualTo(new DateTime(2026, 05, 02, 11, 22, 33, DateTimeKind.Utc)));
        }
    }

    [TestFixture]
    public class The_KeyboardMappings_Model
    {
        [Test]
        public void RoundTrips_With_Init_Only_Mappings_Property()
        {
            var model = new KeyboardMappings
            {
                GroupName = "Main",
                Mappings =
                {
                    new KeyboardMapping
                    {
                        CommandName = "OpenSettings",
                        Text = "Ctrl + O",
                        IsEditable = true
                    }
                }
            };

            var deserializedModel = RoundTrip(model);

            Assert.That(deserializedModel.GroupName, Is.EqualTo("Main"));
            Assert.That(deserializedModel.Mappings.Count, Is.EqualTo(1));
            Assert.That(deserializedModel.Mappings[0].CommandName, Is.EqualTo("OpenSettings"));
            Assert.That(deserializedModel.Mappings[0].Text, Is.EqualTo("Ctrl + O"));
            Assert.That(deserializedModel.Mappings[0].IsEditable, Is.True);
        }
    }

    [TestFixture]
    public class The_Changelog_Model
    {
        [Test]
        public void RoundTrips_With_Init_Only_Items_Property()
        {
            var model = new Orchestra.Changelog.Changelog
            {
                Title = "1.0.0",
                Items =
                {
                    new ChangelogItem
                    {
                        Group = "General",
                        Name = "Added new feature",
                        Description = "A feature was added",
                        Type = ChangelogType.Improvement
                    }
                }
            };

            var deserializedModel = RoundTrip(model);

            Assert.That(deserializedModel.Title, Is.EqualTo("1.0.0"));
            Assert.That(deserializedModel.Items.Count, Is.EqualTo(1));
            Assert.That(deserializedModel.Items[0].Group, Is.EqualTo("General"));
            Assert.That(deserializedModel.Items[0].Name, Is.EqualTo("Added new feature"));
            Assert.That(deserializedModel.Items[0].Description, Is.EqualTo("A feature was added"));
            Assert.That(deserializedModel.Items[0].Type, Is.EqualTo(ChangelogType.Improvement));
        }
    }

    private static T RoundTrip<T>(T model, JsonSerializerSettings? settings = null)
    {
        var serializerFactory = new JsonSerializerFactory();
        var serializer = settings is null
            ? serializerFactory.CreateSerializer()
            : serializerFactory.CreateSerializer(settings);

        var serializedValue = serializer.SerializeToString(model);
        var deserializedModel = serializer.DeserializeFromString<T>(serializedValue);

        Assert.That(deserializedModel, Is.Not.Null);

        return deserializedModel!;
    }
}
