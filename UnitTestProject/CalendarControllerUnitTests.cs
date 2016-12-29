using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApiRestServiceExample.Controllers;
using WebApiRestServiceExample.Models;
using WebApiRestServiceExample.Providers;

namespace UnitTestProject
{
    [TestClass]
    public class CalendarControllerUnitTests
    {
        [TestMethod]
        public async Task TestCorrectParametersToGoogleMaps()
        {
            const string city = "Copenhagen";
            var gmMock = new Mock<IGoogleMapsProvider>();
            var dtMock = new Mock<IDateTimeProvider>();

            // Need to call setup once
            gmMock.Setup(m => m.GetTimeZoneName(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.Run(() => ""));

            var controller = new CalendarController(gmMock.Object, dtMock.Object);

            await controller.GetTime(city);

            // Verify that GoogleMaps are called with the value that the client is passing along (we are only passing city, so country is null)
            gmMock.Verify(m => m.GetTimeZoneName(city, null));
        }

        [TestMethod]
        public async Task TestValidResponseReturned()
        {
            const string city = "Copenhagen";
            var gmMock = new Mock<IGoogleMapsProvider>();
            var dtMock = new Mock<IDateTimeProvider>();

            // Need to call setup once
            gmMock.Setup(m => m.GetTimeZoneName(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.Run(() => "Central European Standard Time"));

            var controller = new CalendarController(gmMock.Object, dtMock.Object);

            var response = await controller.GetTime(city) as OkNegotiatedContentResult<LocalTimeModel>;
            // Assert that we get an OK response and that the LocalTimeModel is not null
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Content);
        }
    }
}

