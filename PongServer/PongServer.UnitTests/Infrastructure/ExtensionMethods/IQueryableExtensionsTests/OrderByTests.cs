using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using PongServer.Domain.Entities;
using PongServer.Domain.Enums;
using PongServer.Infrastructure.Extensions;

namespace PongServer.UnitTests.Infrastructure.ExtensionMethods.IQueryableExtensionsTests
{
    public class OrderByTests
    {
        private List<Host> _hosts;

        public OrderByTests()
        {
            _hosts = new List<Host>(
                new Host[]
                {
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
                        Ip = "192.168.0.1",
                        IsAvailable = true,
                        Name = "Host1",
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
        public void OrderBy_ForOrderingDirectionNotSpecified_ReturnsCollectionInOriginalOrder()
        {
            var orderingResult = _hosts.AsQueryable<Host>()
                .OrderBy(host => host.Name, sortingOrder: null)
                .ToList();

            orderingResult.Should().BeEquivalentTo(_hosts);
        }

        [Fact]
        public void OrderBy_ForAscendingOrdering_ReturnsCollectionInAscendingOrderRegardingSpecifiedField()
        {
            var orderingResult = _hosts.AsQueryable<Host>()
                .OrderBy(host => host.Name, SortingOrder.Ascending)
                .ToList();

            orderingResult.Should().BeInAscendingOrder(host => host.Name);
        }

        [Fact]
        public void OrderBy_ForDescendingOrdering_ReturnsCollectionInDescendingOrderRegardingSpecifiedField()
        {
            var orderingResult = _hosts.AsQueryable<Host>()
                .OrderBy(host => host.Name, SortingOrder.Descending)
                .ToList();

            orderingResult.Should().BeInDescendingOrder(host => host.Name);
        }
    }
}
