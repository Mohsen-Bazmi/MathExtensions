using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BigIntExt
{
    public static class BigIntegerExtensions
    {
        static class Prefixes
        {
            public const string BINARY = "0b";
            public const string OCTAL = "0o";
            public const string HEXADECIMAL = "0x";
        }
        public static bool TryParseBigInt(this string prefixFormattedNumber, out BigInteger result)
        {
            try
            {
                result = prefixFormattedNumber.ParseBigInt();
                return true;
            }
            catch (ArgumentException)
            {
                result = default;
                return false;
            }
        }
        public static BigInteger ParseBigInt(this string prefixFormattedNumber)
        {
            var splitted = prefixFormattedNumber.Split('_');
            var isANumberInUncommonSystem = splitted.Length == 2 && splitted[0].All(char.IsDigit);
            if (isANumberInUncommonSystem)
            {
                var number = splitted[1];
                var fromBase = int.Parse(splitted[0]);
                return ParseBigIntFromBase(number, fromBase);
            }
            if (prefixFormattedNumber.Length > 1)
            {
                return prefixFormattedNumber.Substring(0, 2) switch
                {
                    Prefixes.BINARY => ParseBigIntFromBase(prefixFormattedNumber.Substring(2), 2),
                    Prefixes.OCTAL => ParseBigIntFromBase(prefixFormattedNumber.Substring(2), 8),
                    Prefixes.HEXADECIMAL => ParseBigIntFromBase(prefixFormattedNumber.Substring(2), 16),
                    _ => ParseBigIntFromBase(prefixFormattedNumber, 10),
                };
            }
            return ParseBigIntFromBase(prefixFormattedNumber, 10);
        }

        public static bool TryParseBigIntFromBase(this string nonDecimalNumber, int toBase, out BigInteger result)
        {
            try
            {
                result = nonDecimalNumber.ParseBigIntFromBase(toBase);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static BigInteger ParseBigIntFromBase(this string nonDecimalNumber, int toBase)
        {
            if (nonDecimalNumber.Length == 0)
                throw new ArgumentException("Can not parse empty string to BigInteger.");
            if (nonDecimalNumber[0] == '-' && nonDecimalNumber.Length > 1)
            {
                nonDecimalNumber = nonDecimalNumber.Substring(1);
                return -1 * ParsePositiveFromBase(nonDecimalNumber, toBase);
            }
            return ParsePositiveFromBase(nonDecimalNumber, toBase);

        }

        static BigInteger ParsePositiveFromBase(this string nonDecimalNumber, int originalBase)
        {
            var decimalResult = new BigInteger(0);

            for (var i = 0; i < nonDecimalNumber.Length; i++)
            {
                var currentChar = nonDecimalNumber[nonDecimalNumber.Length - i - 1];
                decimalResult += GetNumberBehindChar(currentChar, originalBase)
                               * BigInteger.Pow(originalBase, i);
            }

            return decimalResult;
        }

        static byte GetNumberBehindChar(char currentChar, int originalBase)
        {
            if (char.IsDigit(currentChar))
                return byte.Parse(currentChar.ToString());
            if (originalBase < 11 || currentChar < 97 || currentChar > 122)
                throw new ArgumentException($"The character '{currentChar}' is not a number in base '{originalBase}'.");
            var numberBehindCurrentChar = byte.Parse((currentChar - 87).ToString());
            if (numberBehindCurrentChar > originalBase)
                throw new ArgumentException($"The character '{currentChar}' represents number '{numberBehindCurrentChar}' which is greater than base '{originalBase}'");
            return numberBehindCurrentChar;

        }


        public static string ToStringBase(this BigInteger decimalNum, int toBase)
        {
            if (decimalNum == 0)
                return "0";
            if (decimalNum < 0)
                return "-" + PositiveToString(decimalNum * -1, toBase);
            return PositiveToString(decimalNum, toBase);

        }
        public static string ToStringWithPrefixBase(this BigInteger decimalNum, int toBase)
        => (decimalNum < 0) switch
        {
            true => "-" + PositiveToStringFromBase(-1 * decimalNum, toBase),
            _ => PositiveToStringFromBase(decimalNum, toBase),
        };
        static string PositiveToStringFromBase(BigInteger decimalNum, int toBase)
        => toBase switch
        {
            2 => Prefixes.BINARY,
            8 => Prefixes.OCTAL,
            16 => Prefixes.HEXADECIMAL,
            10 => string.Empty,
            var others => others + "_",
        } + ZeroOrPositiveToStringFromBase(decimalNum, toBase);

        static string ZeroOrPositiveToStringFromBase(BigInteger decimalNum, int toBase)
        {
            if (decimalNum == 0)
                return "0";
            return PositiveToString(decimalNum, toBase);
        }
        static string PositiveToString(BigInteger decimalNum, int toBase)
        {
            StringBuilder strNum = new StringBuilder();
            while (decimalNum > 0)
            {
                var remainder = decimalNum % toBase;
                var strRemainder = remainder.ToString()[0];
                if (remainder > 9)
                    strRemainder = (char)(remainder + 87);
                strNum.Insert(0, strRemainder);
                decimalNum /= toBase;
            }

            return strNum.ToString();
        }

        public static double Sin(this BigInteger bigint)
        {
            throw new NotImplementedException();
        }

        public static object MultiplyByDouble(this BigInteger left, double right)
        {//3435.9738368 22965466323057171944206184518407098948955535888671875
            if (right.ToString().Length > 15)
                return (double)left * right;
            while (right % 1 != 0 && left % 10 == 0)
            {
                right *= 10;
                left /= 10;
            }
            if (right % 1 == 0)
                return left * (BigInteger)right;
            while (left % 2 == 0)
            {
                right *= 2;
                left /= 2;
                if (4503599627370496 < right || right < -4503599627370496)
                    return (double)left * right;
                if (right % 1 == 0)
                    return left * (BigInteger)right;
            }
            while (left % 5 == 0)
            {
                right *= 5;
                left /= 5;
                if (4503599627370496 < right || right < -4503599627370496)
                    return (double)left * right;
                if (right % 1 == 0)
                    return left * (BigInteger)right;
            }
            return (double)left * right;
        }
    }
}