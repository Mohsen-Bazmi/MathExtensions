using System;
using System.Numerics;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace BigIntExt.Tests
{
    public class BigIntegerExtensionsTests
    {
        [Property]
        public void Negative_binary_string_to_big_integer(uint positiveNum)
        {
            $"-{Convert.ToString(positiveNum, 2)}"
            .ParseBigIntFromBase(2)
            .Should().Be(new BigInteger(positiveNum * -1));
        }
        [Property]
        public void Converts_positive_big_integer_to_binary_string(PositiveInt positiveNum)
        {
            new BigInteger(positiveNum.Get).ToStringBase(2)
            .Should().Be(Convert.ToString(positiveNum.Get, 2));
        }

        [Property]
        public void Converts_negative_big_integer_to_binary_string(NegativeInt negativeNum)
        {
            new BigInteger(negativeNum.Get)
            .ToStringBase(2)
            .Should().Be("-" + Convert.ToString(-negativeNum.Get, 2));
        }

        [Fact]
        public void Binary_string_to_big_integer_is_mututually_convertable()
        {
            const string strNum = "30414093201713378043612608166064768844377641568960512000000000000";
            BigInteger.Parse(strNum)
            .ToString().Should().Be(strNum);
        }

        [Fact]
        public void Converts_binary_string_to_big_integer_example()
        {
            const string binary = "11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111110000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111100000000000000000000000000000000000000000000111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000000000000000000000000000111111111111111111111111111111111111111111111111111111111111";
            var @decimal = BigInteger.Parse("1224720827664335609236962600350409901130557945655466490308353887654080091535814597443052133607584817472045898830723416116847734939819330962911308156981182426499905234519676499984730528101986471765868543");

            binary.ParseBigIntFromBase(2)

            .Should().Be(@decimal);
        }
        [Property]
        public void Binary_string_to_big_integer(PositiveInt positiveNum)
        {
            Convert.ToString(positiveNum.Get, 2)
            .ParseBigIntFromBase(2)
            .Should().Be(new BigInteger(positiveNum.Get));
        }


        [Fact]
        public void Zero_binary_string_to_big_integer()
        {
            "0".ParseBigIntFromBase(2)
            .Should().Be(new BigInteger(0));
        }

        [Fact]
        public void Converts_big_integer_to_binary_string_example()
        {
            BigInteger decimalNum = BigInteger.Parse("1224720827664335609236962600350409901130557945655466490308353887654080091535814597443052133607584817472045898830723416116847734939819330962911308156981182426499905234519676499984730528101986471765868543");
            const string expected = "11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111110000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111100000000000000000000000000000000000000000000111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000000000000000000000000000111111111111111111111111111111111111111111111111111111111111";

            decimalNum.ToStringBase(2)

            .Should().Be(expected);

        }

        [Theory]
        [InlineData(10, "a")]
        [InlineData(11, "b")]
        [InlineData(12, "c")]
        [InlineData(13, "d")]
        [InlineData(14, "e")]
        [InlineData(15, "f")]
        public void Converts_big_integer_to_hex_string(int num, string expected)
        {
            new BigInteger(num)
            .ToStringBase(16)
            .Should().Be(expected);
        }

        [Theory]
        [InlineData(10, "a")]
        [InlineData(11, "b")]
        [InlineData(12, "c")]
        [InlineData(13, "d")]
        [InlineData(14, "e")]
        [InlineData(15, "f")]
        public void Converts_hex_string_to_big_integer(int expected, string num)
        {
            num.ParseBigIntFromBase(16)
            .Should().Be(new BigInteger(expected));
        }

        [Property]
        public void Returns_decimal_unchanged(ulong positiveNum)
        {
            var bigInteger = new BigInteger(positiveNum);
            bigInteger.ToStringBase(10)
            .Should().Be(bigInteger.ToString());
        }

        [Property]
        public void Formats_hex_string_to_big_integer(ulong positiveNum)
        {
            var bigInteger = new BigInteger(positiveNum);
            bigInteger.ToStringWithPrefixBase(16)
            .Should().Be("0x" + bigInteger.ToStringBase(16));
        }
        [Property]
        public void Formats_octal_string_to_big_integer(ulong positiveNum)
        {
            var bigInteger = new BigInteger(positiveNum);
            bigInteger.ToStringWithPrefixBase(8)
            .Should().Be("0o" + bigInteger.ToStringBase(8));
        }

        [Property]
        public void Formats_binary_string_to_big_integer(ulong positiveNum)
        {
            var bigInteger = new BigInteger(positiveNum);
            bigInteger.ToStringWithPrefixBase(2)
            .Should().Be("0b" + bigInteger.ToStringBase(2));
        }

        [Property]
        public void Formats_decimal_string_to_big_integer(ulong positiveNum)
        {
            var bigInteger = new BigInteger(positiveNum);
            bigInteger.ToStringWithPrefixBase(10)
            .Should().Be(bigInteger.ToString());
        }

        [Property]
        public void Supports_uncommon_bases(ulong positiveNum)
        {
            var bigInteger = new BigInteger(positiveNum);
            bigInteger.ToStringWithPrefixBase(5)
            .Should().Be("5_" + bigInteger.ToStringBase(5));
        }

        [Property]
        public void Returns_negative_decimal_unchanged(NegativeInt num)
        {
            var bigInteger = new BigInteger(num.Get);
            bigInteger.ToStringBase(10)
            .Should().Be(bigInteger.ToString());
        }

        [Property]
        public void Formats_negative_hex_string_to_big_integer(NegativeInt num)
        {
            var bigInteger = new BigInteger(num.Get);
            bigInteger.ToStringWithPrefixBase(16)
            .Should().Be("-0x" + bigInteger.ToStringBase(16).Substring(1));
        }

        [Property]
        public void Formats_negative_octal_string_to_big_integer(NegativeInt num)
        {
            var bigInteger = new BigInteger(num.Get);
            bigInteger.ToStringWithPrefixBase(8)
            .Should().Be("-0o" + bigInteger.ToStringBase(8).Substring(1));
        }

        [Property]
        public void Formats_negative_binary_string_to_big_integer(NegativeInt num)
        {
            var bigInteger = new BigInteger(num.Get);
            bigInteger.ToStringWithPrefixBase(2)
            .Should().Be("-0b" + bigInteger.ToStringBase(2).Substring(1));
        }

        [Property]
        public void Formats_negative_decimal_string_to_big_integer(NegativeInt num)
        {
            var bigInteger = new BigInteger(num.Get);
            bigInteger.ToStringWithPrefixBase(10)
            .Should().Be("-" + bigInteger.ToString().Substring(1));
        }

        [Property]
        public void Supports_negative_uncommon_bases(NegativeInt num)
        {
            var bigInteger = new BigInteger(num.Get);
            bigInteger.ToStringWithPrefixBase(5)
            .Should().Be("-5_" + bigInteger.ToStringBase(5).Substring(1));
        }

        [Fact]
        public void Converts_python_formatted_decimal_0_to_big_integer()
        {
            Convert.ToString(0, 2)
            .ParseBigInt()
            .Should().Be(new BigInteger(0));
        }

        [Property]
        public void Converts_python_formatted_binary_to_big_integer(PositiveInt positiveNum)
        {
            ("0b" + Convert.ToString(positiveNum.Get, 2))
            .ParseBigInt()
            .Should().Be(new BigInteger(positiveNum.Get));
        }

        [Property]
        public void Converts_python_formatted_octal_to_big_integer(PositiveInt positiveNum)
        {
            ("0o" + Convert.ToString(positiveNum.Get, 8))
            .ParseBigInt()
            .Should().Be(new BigInteger(positiveNum.Get));
        }

        [Property]
        public void Converts_python_formatted_hex_to_big_integer(PositiveInt positiveNum)
        {
            ("0x" + Convert.ToString(positiveNum.Get, 16))
            .ParseBigInt()
            .Should().Be(new BigInteger(positiveNum.Get));
        }

        [Property]
        public void Converts_python_formatted_decimal_to_big_integer(ulong positiveNum)
        {
            Convert.ToString(Convert.ToInt64(positiveNum))
            .ParseBigInt()
            .Should().Be(new BigInteger(positiveNum));
        }

        [Property]
        public void Converts_python_formatted_uncommon_systems_to_big_integer(ulong positiveNm)
        {
            new BigInteger(positiveNm).ToStringWithPrefixBase(13)
            .ParseBigInt()
            .Should().Be(new BigInteger(positiveNm));
        }

        [Fact]
        public void Formats_10_to_python_string_13_base()
        {
            new BigInteger(10).ToStringWithPrefixBase(13)
            .Should().Be("13_a");
        }


        [Fact]
        public void Formats_a_from_13_based_string()
        {
            "a"
            .ParseBigIntFromBase(13)
            .Should().Be(new BigInteger(10));
        }

        [Fact]
        public void Formats_a_from_python_string_13_base()
        {
            "13_a"
            .ParseBigInt()
            .Should().Be(new BigInteger(10));
        }

        [Fact]
        public void Rejects_to_convert_strings_with_digits_larger_than_base_to_big_int()
        {
            Action act = () => "l".ParseBigIntFromBase(13);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Rejects_to_convert_non_numbers()
        {
            "magic string"
            .TryParseBigInt(out var result)
            .Should().BeFalse(); result.Should().Be(default);
        }
        [Property]
        public void Rejects_to_parse_invalid_string(int hex)
        {
            (Convert.ToString(hex, 16) + "f")
            .TryParseBigIntFromBase(2, out var result)
            .Should().BeFalse(); result.Should().Be(default);
        }
        [Fact]
        public void Parses_valid_strings()
        {
            "1101"
            .TryParseBigIntFromBase(2, out var result)
            .Should().BeTrue(); result.Should().Be(13);
        }

        [Fact]
        public void Rejects_empty_strings()
        {
            string.Empty
            .TryParseBigInt(out var _)
            .Should().BeFalse();
        }

        [Theory]
        [InlineData("=")]
        [InlineData("[")]
        [InlineData("]")]
        [InlineData("+")]
        [InlineData("-")]
        public void Rejects_non_alphabetical_characters(string nonAlphabet)
        {
            nonAlphabet
            .TryParseBigInt(out var _)
            .Should().BeFalse();
        }

        [Theory]
        [InlineData("a")]
        public void Rejects_alphabets_without_prefixes(string alphabetsWithoutPrefixes)
        {
            alphabetsWithoutPrefixes
            .TryParseBigInt(out var _)
            .Should().BeFalse();
        }

        [Theory]
        [InlineData("a")]
        public void Rejects_alphabets_for_less_than_10_base(string alphabetsWithoutPrefixes)
        {
            Action act = () => alphabetsWithoutPrefixes.ParseBigIntFromBase(9);
            act.Should().Throw<ArgumentException>();
        }

        [Fact(Skip = "Decide finally")]
        public void Calculates_Sin1()
        {
            var x = 6283185307179586;
            Math.Sin(x)
            .Should().Be(.5);
        }
    }
}