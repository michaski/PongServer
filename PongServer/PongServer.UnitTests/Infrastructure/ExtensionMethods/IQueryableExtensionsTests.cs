
using FluentAssertions;
using PongServer.Domain.Entities;
using PongServer.Infrastructure.Extensions;

namespace PongServer.UnitTests.Infrastructure.ExtensionMethods
{
    public class IQueryableExtensionsTests
    {
        private List<Host> _hosts;

        public IQueryableExtensionsTests()
        {
            _hosts = new List<Host>(
                new Host[]
                {
                    new Host
                    {
                        Id = Guid.NewGuid(),
                        Ip = "192.168.0.1",
                        IsAvailable = true,
                        Name = "Host1",
                        Port = 20,
                        Owner = null
                    },
                    new Host
                    {
                        Id = Guid.NewGuid(),
                        Ip = "192.168.0.2",
                        IsAvailable = true,
                        Name = "Host2",
                        Port = 20,
                        Owner = null
                    },
                    new Host
                    {
                        Id = Guid.NewGuid(),
                        Ip = "192.168.0.3",
                        IsAvailable = true,
                        Name = "Official Server",
                        Port = 20,
                        Owner = null
                    }
                });
        }

        [Fact]
        public void SearchInField_ForEmptySearchQuery_ReturnsAllItems()
        {
            var searchResult = _hosts.AsQueryable<Host>()
                .SearchInField(host => host.Name, "")
                .ToList();

            searchResult.Should().BeEquivalentTo(_hosts);
        }

        [Fact]
        public void SearchInField_ForNullSearchQuery_ReturnsAllItems()
        {
            var searchResult = _hosts.AsQueryable<Host>()
                .SearchInField(host => host.Name, null)
                .ToList();

            searchResult.Should().BeEquivalentTo(_hosts);
        }

        [Fact]
        public void SearchInField_ForSingleMatchSearch_ReturnsCorrectItem()
        {
            var searchResult = _hosts.AsQueryable<Host>()
                .SearchInField(host => host.Name, "official")
                .ToList();

            searchResult.Should().HaveCount(1);
            searchResult[0].Should().BeEquivalentTo(_hosts[2]);
        }

        [Fact]
        public void SearchInField_ForMultipleMatchSearch_ReturnsCorrectNumberOfItems()
        {
            var searchResult = _hosts.AsQueryable<Host>()
                .SearchInField(host => host.Name, "host")
                .ToList();

            searchResult.Should().HaveCount(2);
        }
    }
}
