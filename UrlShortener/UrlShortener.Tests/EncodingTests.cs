using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UrlShortener.Api.Extensions;

namespace UrlShortener.Tests
{
    [TestClass]
    public class EncodingTests
    {
        private const string UlongMaxInBase62 = "LygHa16AHYF";
        private const string UlongMaxMinusOneInBase62 = "LygHa16AHYE";
        private const string EncodingBaseInBase62 = "10";
        private const ulong EncodingBase = 62;
        private const string UlongMinInBase62 = "0";

        [TestMethod]
        public void ToBase62ShouldGenerateCorrectValueWhenGivenMaxValue()
        {
            ulong.MaxValue.ToBase62().Should().Be(UlongMaxInBase62);
        }

        [TestMethod]
        public void ToBase62ShouldGenerateCorrectValueWhenGivenMaxValueMinusOne()
        {
            (ulong.MaxValue - 1).ToBase62().Should().Be(UlongMaxMinusOneInBase62);
        }

        [TestMethod]
        public void ToBase62ShouldGenerateCorrectValueWhenGivenMinValue()
        {
            (ulong.MinValue).ToBase62().Should().Be(UlongMinInBase62);
        }

        [TestMethod]
        public void ToBase62ShouldGenerateCorrectValueWhenGivenEncodingBase()
        {
            EncodingBase.ToBase62().Should().Be(EncodingBaseInBase62);
        }

        [TestMethod]
        public void FromBase62ShouldGenerateCorrectValueWhenGivenEncodingBase()
        {
            EncodingBaseInBase62.FromBase62().Should().Be(EncodingBase);
        }

        [TestMethod]
        public void FromBase62ShouldGenerateCorrectValueWhenGivenMaxValue()
        {
            UlongMaxInBase62.FromBase62().Should().Be(ulong.MaxValue);
        }

        [TestMethod]
        public void FromBase62ShouldGenerateCorrectValueWhenGivenMaxValueMinusOne()
        {
            UlongMaxMinusOneInBase62.FromBase62().Should().Be(ulong.MaxValue - 1);
        }

        [TestMethod]
        public void FromBase62ShouldGenerateCorrectValueWhenGivenMinValue()
        {
            UlongMinInBase62.FromBase62().Should().Be(ulong.MinValue);
        }

        [TestMethod]
        public void FromBase62ShouldGenerateCorrectValueWhenGivenMinValued()
        {
            "11".FromBase62().Should().Be(63);
        }

        [TestMethod]
        public void FromBase62ShouldThrowArgumentExceptionWhenGivenWrongStringWithWrongSymbols()
        {
            Action act = () => "d7s&ds".FromBase62();

            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("value");
        }

        [TestMethod]
        public void FromBase62ShouldThrowArgumentExceptionWhenGivenStringHigherThenUlongMax()
        {
            Action act = () => "LygHa16AHYG".FromBase62();

            act.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("value");
        }
    }
}