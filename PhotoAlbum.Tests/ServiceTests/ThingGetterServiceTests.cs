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
        private readonly List<AlbumEntry> _testData2;
        private readonly List<AlbumGroups> _albumGroups;
        private readonly int _albumId;
        private readonly int _albumId2;
        private AlbumGroups? _albumGroup2;
        private readonly Faker<AlbumEntry> _fakeAlbumEntry;

        public ThingGetterServiceTests()
        {
            _faker = new Faker();
            _testData = new List<AlbumEntry>();
            _testData2 = new List<AlbumEntry>();
            _albumGroups = new List<AlbumGroups>();
            _url = _faker.Internet.Url();
            _appSettings = new AppSettings()
            {
                PhotoAlbumServiceUrl = _url
            };
            
            _albumId = _faker.Random.Int();
            _albumId2 = _faker.Random.Int();
            _fakeAlbumEntry = new Faker<AlbumEntry>()
                .RuleFor(c => c.id, _faker.Random.Int())
                .RuleFor(c => c.albumId, _albumId)
                .RuleFor(c => c.title, _faker.Random.String2(25))
                .RuleFor(c => c.url, _faker.Internet.Url())
                .RuleFor(c => c.thumbnailUrl, _faker.Internet.Url());

            _thingGetterRepoMock = new Mock<IThingGetterRepo>();

            _thingGetterService = new ThingGetterService(_thingGetterRepoMock.Object, _appSettings);

        }

        [Fact]
        public async Task GetAlbums_ValidData_ShouldReturnExpectedDataForSpecificAlbumId()
        {

            _testData.AddRange(GenerateFakeAlbums(_faker.Random.Int(1, 500), _albumId));
            var albumGroup1 = new AlbumGroups()
            {
                AlbumId = _albumId,
                AlbumEntries = _testData
            };
            
            _albumGroups.Add(albumGroup1);
            
            _thingGetterRepoMock.Setup(x => x.GetAlbumsAsync(_url, _albumId)).ReturnsAsync(_testData);

            var result = await _thingGetterService.GetAlbumsAsync(_albumId.ToString());

            result.Should().BeEquivalentTo(_albumGroups);
        }

        [Fact] public async Task GetAlbums_ValidData_ShouldReturnExpectedDataForAllAlbums()
        {
            _testData.AddRange(GenerateFakeAlbums(_faker.Random.Int(1, 500), _albumId));
            var albumGroupList = new List<AlbumGroups>();
            albumGroupList.Add(new AlbumGroups()
                {
                    AlbumEntries = _testData,
                    AlbumId = _albumId
                }
            );

                _testData2.AddRange(GenerateFakeAlbums(_faker.Random.Int(1, 500), _albumId2));
                albumGroupList.Add(new AlbumGroups()
                {
                    AlbumEntries = _testData2,
                    AlbumId = _albumId2
                });
                var combinedList = new List<AlbumEntry>();
                combinedList.AddRange(_testData);
                combinedList.AddRange(_testData2);

                _thingGetterRepoMock.Setup(x => x.GetAlbumsAsync(_url, -1)).ReturnsAsync(combinedList);

            var result = await _thingGetterService.GetAlbumsAsync("");

            result.Should().BeEquivalentTo(albumGroupList);
        }

        private List<AlbumEntry> GenerateFakeAlbums(int count, int albumId)
        {
            var response = new Faker<AlbumEntry>()
                .RuleFor(c => c.id, _faker.Random.Int())
                .RuleFor(c => c.albumId, albumId)
                .RuleFor(c => c.title, _faker.Random.String2(25))
                .RuleFor(c => c.url, _faker.Internet.Url())
                .RuleFor(c => c.thumbnailUrl, _faker.Internet.Url());
            return response.Generate(count);
        }
    }
}