using Data.Model;
using Data.Repositories;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Data.Services.Test
{
    public class RequestServiceUntTests
    {
        private const string EXPECTED_XML_NULL_FIELD_CASE = "<Request><dateRequested>01.01.2018 00:00:00</dateRequested><content><ix>11</ix><name>aaa</name></content></Request>";
        private const string EXPECTED_XML_NO_NULL_FIELD = "<Request><dateRequested>02.01.2018 00:00:00</dateRequested><content><ix>12</ix><name>bbb</name><visits>7</visits></content></Request>";
        private readonly IRequestRepository _substitute;
        private readonly IRequestService _sut;

        public RequestServiceUntTests()
        {
            _substitute = Substitute.For<IRequestRepository>();
            _substitute.SaveRequests(Arg.Any<IEnumerable<Request>>()).Returns(string.Empty);
            _substitute.GetRequests().Returns(new List<Request> { new Request { Index = 11, Name = "aaa", Visits = null, Date = new DateTime(2018, 1, 1, 0, 0, 0) } });

            _sut = new RequestService(_substitute);
        }

        [Fact]
        public void GetRequestsTest_ResultXmlEqualsExample()
        {
            var result = _sut.GetRequests();

            Assert.Equal(result, EXPECTED_XML_NULL_FIELD_CASE);
        }

        [Fact]
        public void GetRequestsTest_ResultXmlNotEqualsExample()
        {
            var result = _sut.GetRequests();

            Assert.NotEqual(result, EXPECTED_XML_NO_NULL_FIELD);
        }
    }
}
