using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UrlShortener.Api.Extensions;

namespace UrlShortener.Tests
{
    [TestClass]
    public class EncodingTests
    {
        [TestMethod]
        public void ShouldGenerateCorrectValueWhenGivenMaxValue()
        {
            ulong.MaxValue.ToBase62().Should().Be("LygHa16AHYF");
        }

        [TestMethod]
        public void ShouldGenerateCorrectValueWhenGivenMaxValueMinusOne()
        {
            (ulong.MaxValue - 1).ToBase62().Should().Be("LygHa16AHYE");
        }

        [TestMethod]
        public void ShouldGenerateCorrectValueWhenGivenMinValue()
        {
            (ulong.MinValue).ToBase62().Should().Be("0");
        }

        [TestMethod]
        public void ShouldGenerateCorrectValueWhenGivenEncodingBase()
        {
            ((ulong)62).ToBase62().Should().Be("10");
        }
    }
}