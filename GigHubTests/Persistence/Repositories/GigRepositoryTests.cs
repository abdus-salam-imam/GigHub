using FluentAssertions;
using GigHub.Models;
using GigHub.Repositories;
using GigHubTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace GigHubTests.Persistence.Repositories
{
    [TestClass]
    public class GigRepositoryTests
    {
        private GigRepository _repository;
        private Mock<DbSet<Gig>> _mockGigs;

        [TestInitialize]
        public void TestInitialize()
        {

            _mockGigs = new Mock<DbSet<Gig>>();
            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.Setup(c => c.Gigs).Returns(_mockGigs.Object);

            _repository = new GigRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsInThePast_ShouldNotBeReturned ()
        {
            var gig = new Gig(){DateTime = DateTime.Now.AddDays(-1),ArtistId = "1"};
            
            
            //Arrange 
            _mockGigs.SetSource(new List<Gig>{gig});


            //Act
            var gigs = _repository.GetUpComingGigsByArtist("1");

            //Assert
            gigs.Should().BeEmpty();


        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsCancelled_ShouldNotBeReturned()
        {

            var gig = new Gig() { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };

            gig.Cancel();
            //Arrange 
            _mockGigs.SetSource(new List<Gig> { gig });


            //Act
            var gigs = _repository.GetUpComingGigsByArtist("1");

            //Assert
            gigs.Should().BeEmpty();

        }


        [TestMethod]

        public void GetUpcomingGigsByArtist_GigIsForDifferentArtist_ShouldNotBeReturend()
        {

            var gig = new Gig() { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };

            //Arrange 
            _mockGigs.SetSource(new List<Gig> { gig });


            //Act
            var gigs = _repository.GetUpComingGigsByArtist(gig.ArtistId + "-");

            //Assert
            gigs.Should().BeEmpty();

        }

        [TestMethod]

        public void GetUpcomingGigsByArtist_GigIsForGivenArtistAndInTheFuture_ShouldBeReturned()
        {

            var gig = new Gig() { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };

            //Arrange 
            _mockGigs.SetSource(new List<Gig> { gig });


            //Act
            var gigs = _repository.GetUpComingGigsByArtist(gig.ArtistId);

            //Assert
            gigs.Should().Contain(gig);



        }

    }
}
