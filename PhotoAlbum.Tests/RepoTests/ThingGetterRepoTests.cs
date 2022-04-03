using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using PhotoAlbum.Models;
using PhotoAlbum.Repos;
using Xunit;

namespace PhotoAlbum.Tests.RepoTests
{
    public class ThingGetterRepoTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly ThingGetterRepo _thingGetterRepo;
        private readonly string _url;
        private readonly Faker _faker;
        private readonly List<AlbumEntry> _testData;
        
        public ThingGetterRepoTests()
        {
            _faker = new Faker();
            _testData = new List<AlbumEntry>();
            _url = _faker.Internet.Url();
            var fakeAlbumEntry = new Faker<AlbumEntry>()
                .RuleFor(c => c.id, _faker.Random.Int())
                .RuleFor(c => c.albumId, _faker.Random.Int())
                .RuleFor(c => c.title, _faker.Random.String2(25))
                .RuleFor(c => c.url, _faker.Internet.Url())
                .RuleFor(c => c.thumbnailUrl, _faker.Internet.Url());
            _testData.AddRange(fakeAlbumEntry.Generate(_faker.Random.Int()));

            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(s => s.RequestUri == new Uri(_url) && s.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(_testData))
                });

            _thingGetterRepo = new ThingGetterRepo(new HttpClient(_httpMessageHandlerMock.Object));
        }

        [Fact]
        public async Task GetAlbums_OkResult_ShouldReturnExpectedData()
        {
            var result = await _thingGetterRepo.GetAlbumsAsync(_url);

            result.Should().BeEquivalentTo(_testData);
        }

        [Fact]
        public async Task GetAlbums_InvalidUrl_ShouldThrowException()
        {
            Func<Task> func = async () => await _thingGetterRepo.GetAlbumsAsync(_faker.Random.String2(5));

            await func.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}