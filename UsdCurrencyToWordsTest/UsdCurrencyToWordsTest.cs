using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UsdCurrencyToWords;
using System;
using System.Numerics;

namespace UsdCurrencyToWordsTest
{
    [TestClass]
    public class UsdCurrencyToWordsTest
    {
        private readonly CurrencyToWords usd = new CurrencyToWords();
        [TestMethod]
        public void Check_If_User_Input_Is_Empty_String()
        {
            try
            {
                usd.ChangeCurrencyToWords("");
            }
            catch (System.FormatException e)
            {
                StringAssert.Contains(e.Message, "Input can't be empty");
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void Check_If_User_Input_Is_Not_A_Valid_Number_Or_Uncommon_Input()
        {
            try
            {
                usd.ChangeCurrencyToWords("sh52425@#!123.4S/*-*/-+@#!#!#@#!##_*&**");
            }
            catch (System.FormatException e)
            {
                StringAssert.Contains(e.Message, "Input accepts only a number format followed by 2 digits decimal numbers (e.g. 123.45)");
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void Check_If_Input_Number_Is_Valid()
        {
            string inputNumber = "123";

            Dictionary<string, BigInteger> expected = new Dictionary<string, BigInteger> { { "number", 123 }, { "decimalNumber", 0 } };

            Dictionary<string, BigInteger> actual = usd.ConvertInputNumber(inputNumber);

            foreach (KeyValuePair<string, BigInteger> kvp in actual)
            {
                Assert.AreEqual(expected.ContainsKey(kvp.Key), actual.ContainsKey(kvp.Key));
                Assert.AreEqual(expected.ContainsValue(kvp.Value), actual.ContainsValue(kvp.Value));
            }
        }

        [TestMethod]
        public void Check_If_Input_Number_With_Decimal_Is_Valid()
        {
            string inputNumber = "123.45";

            Dictionary<string, BigInteger> expected = new Dictionary<string, BigInteger> { { "number", 123 }, { "decimalNumber", 45 } };

            Dictionary<string, BigInteger> actual = usd.ConvertInputNumber(inputNumber);

            foreach (KeyValuePair<string, BigInteger> kvp in actual)
            {
                Assert.AreEqual(expected.ContainsKey(kvp.Key), actual.ContainsKey(kvp.Key));
                Assert.AreEqual(expected.ContainsValue(kvp.Value), actual.ContainsValue(kvp.Value));
            }
        }

        [TestMethod]
        public void Check_If_Decimal_Digits_More_Than_Two_Digits()
        {
            string inputNumber = "123.4512314556422342";

            try
            {
                usd.ChangeCurrencyToWords(inputNumber);
            }
            catch (System.Exception e)
            {
                StringAssert.Contains(e.Message, "Decimal digits is out of range, only accept maximum 2 number (e.g. 123.45)");
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void Check_If_Number_Is_Decimal_And_Get_The_Words()
        {
            BigInteger inputSingleDigit = 1;
            BigInteger inputDoubleDigit = 20;

            string actualSingleDigitWords = usd.GetDecimalWords(inputSingleDigit);
            string actualDoubleDigitWords = usd.GetDecimalWords(inputDoubleDigit);

            string expectedSingleDigitWords = "one";
            string expectedDoubleDigitWords = "twenty";

            Assert.AreEqual(expectedSingleDigitWords, actualSingleDigitWords);
            Assert.AreEqual(expectedDoubleDigitWords, actualDoubleDigitWords);
        }

        [TestMethod]
        public void Get_Words_When_Input_Is_Valid()
        {
            List<string> number = new List<string>() { "123.45", "8.1", "8", "24", "100", "1455", "1234567898765432123456789" };
            List<string> actual = new List<string>();

            number.ForEach(delegate (String inputNumber)
            {
                actual.Add(usd.ChangeCurrencyToWords(inputNumber));
            });

            List<string> expected = new List<string>() {
                "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FOURTY-FIVE CENTS",
                "EIGHT DOLLARS AND ONE CENT",
                "EIGHT DOLLARS",
                "TWENTY-FOUR DOLLARS",
                "ONE HUNDRED DOLLARS",
                "ONE THOUSAND AND FOUR HUNDRED AND FIFTY-FIVE DOLLARS",
                "ONE SEPTILLION AND TWO HUNDRED AND THIRTY-FOUR SEXTILLION AND FIVE HUNDRED AND SIXTY-SEVEN QUINTILLION AND EIGHT HUNDRED AND NINETY-EIGHT QUADRILLION AND SEVEN HUNDRED AND SIXTY-FIVE TRILLION AND FOUR HUNDRED AND THIRTY-TWO BILLION AND ONE HUNDRED AND TWENTY-THREE MILLION AND FOUR HUNDRED AND FIFTY-SIX THOUSAND AND SEVEN HUNDRED AND EIGHTY-NINE DOLLARS" };

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i].ToLower().Trim(), actual[i]);
            }
        }

        [TestMethod]
        public void Get_Cent_Word_If_Decimal_Is_Valid()
        {
            string number = "8.1";

            string actual = usd.ChangeCurrencyToWords(number);
            string expected = "eight dollars and one cent";

            Assert.AreEqual(expected.Contains("cent"), actual.ToLower().Trim().Contains("cent"));
        }

        [TestMethod]
        public void Max_Value_Check()
        {
            BigInteger maximumValue = (BigInteger)Math.Pow(10, 63);

            string actual = usd.ChangeCurrencyToWords(maximumValue.ToString());
            string expected = "one vigintillion and fifty-seven quattuordecillion and eight hundred and fifty-seven tredecillion and nine hundred and fifty-nine duodecillion and nine hundred and fourty-two undecillion and seven hundred and twenty-six decillion and nine hundred and sixty-nine nonillion and eight hundred and twenty-seven octillion and three hundred and ninety-three septillion and three hundred and seventy-eight sextillion and six hundred and eighty-nine quintillion and one hundred and seventy-five quadrillion and fourty trillion and four hundred and thirty-eight billion and one hundred and seventy-two million and six hundred and fourty-seven thousand and four hundred and twenty-four dollars";

            Assert.AreEqual(expected, actual.ToLower().Trim());
        }

        [TestMethod]

        public void Input_Negative_Number()
        {
            try
            {
                usd.ChangeCurrencyToWords("-1");
            }
            catch (System.FormatException e)
            {
                StringAssert.Contains(e.Message, "Negative number is not allowed");
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }
    }
}