using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using PhotoAlbum.Models;
using PhotoAlbum.Repos;
using PhotoAlbum.Services;
using Xunit;

namespace PhotoAlbum.Tests
{
    public class ThingGetterServiceTests
    {
        private readonly IThingGetterService _thingGetterService;
        private readonly Mock<IThingGetterRepo> _thingGetterRepoMock;
        private readonly AppSettings _appSettings;
        private readonly string _url;
        private readonly Faker _faker;
        private readonly List<AlbumEntry> _testData;
        private readonly List<AlbumGroups> _albumGroups;

        public ThingGetterServiceTests()
        {
            _faker = new Faker();
            _testData = new List<AlbumEntry>();
            _albumGroups = new List<AlbumGroups>();
            _url = _faker.Internet.Url();
            _appSettings = new AppSettings()
            {
                PhotoAlbumServiceUrl = _faker.Internet.Url()
            };
            
            _url = _faker.Internet.Url();
            var albumId = _faker.Random.Int();
            var fakeAlbumEntry = new Faker<AlbumEntry>()
                .RuleFor(c => c.id, _faker.Random.Int())
                .RuleFor(c => c.albumId, albumId)
                .RuleFor(c => c.title, _faker.Random.String2(25))
                .RuleFor(c => c.url, _faker.Internet.Url())
                .RuleFor(c => c.thumbnailUrl, _faker.Internet.Url());
            _testData.AddRange(fakeAlbumEntry.Generate(_faker.Random.Int(1, 500)));
            var tester = new AlbumGroups()
            {
                AlbumId = albumId,
                AlbumEntries = _testData
            };
            _albumGroups.Add(tester);

            _thingGetterRepoMock = new Mock<IThingGetterRepo>();
            _thingGetterRepoMock.Setup(x => x.GetAlbumsAsync(_url, albumId)).ReturnsAsync(_testData);

            _thingGetterService = new ThingGetterService(_thingGetterRepoMock.Object, _appSettings);

        }

        [Fact]
        public async Task GetAllAlbumEntries_ValidData_ShouldReturnExpectedData()
        {
            var result = await _thingGetterService.GetAlbums("");

            result.Should().BeEquivalentTo(_albumGroups);
        }
    }
}