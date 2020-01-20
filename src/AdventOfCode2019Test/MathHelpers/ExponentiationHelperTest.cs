using AdventOfCode2019.Challenges.Day22;
using AdventOfCode2019.MathHelpers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.MathHelpers
{
    public class ExponentiationHelperTest
    {
        [Fact]
        public void GetExponentiationByPowersIntegersTest()
        {
            BigInteger MultiplyIntegers(BigInteger x, BigInteger y)
            {
                return x * y;
            }
            var testData = new List<Tuple<BigInteger, BigInteger, Func<BigInteger, BigInteger, BigInteger>, BigInteger>>()
            {
                Tuple.Create(
                    (BigInteger)2,
                    (BigInteger)0,
                    (Func<BigInteger, BigInteger, BigInteger>)MultiplyIntegers,
                    (BigInteger)1),
                Tuple.Create(
                    (BigInteger)2,
                    (BigInteger)1,
                    (Func<BigInteger, BigInteger, BigInteger>)MultiplyIntegers,
                    (BigInteger)2),
                Tuple.Create(
                    (BigInteger)2,
                    (BigInteger)29,
                    (Func<BigInteger, BigInteger, BigInteger>)MultiplyIntegers,
                    (BigInteger)536870912),
                Tuple.Create(
                    (BigInteger)2,
                    (BigInteger)59,
                    (Func<BigInteger, BigInteger, BigInteger>)MultiplyIntegers,
                    BigInteger.Parse("576460752303423488")),
                Tuple.Create(
                    (BigInteger)2,
                    (BigInteger)64,
                    (Func<BigInteger, BigInteger, BigInteger>)MultiplyIntegers,
                    BigInteger.Parse("18446744073709551616")),
            };

            foreach (var testExample in testData)
            {
                var result = ExponentiationHelper.GetExponentiationByPowers<BigInteger>(
                    x: testExample.Item1,
                    exponent: testExample.Item2,
                    MultiplyFunction: testExample.Item3,
                    identity: BigInteger.One);
                Assert.Equal(testExample.Item4, result);
            }
        }

        [Fact]
        public void GetExponentiationByPowersLCFTest()
        {
            var testData = new List<Tuple<LCG, BigInteger, Func<LCG, LCG, LCG>, LCG>>()
            {
                // f(f(f(x))) where f(x) = 2*x + 3 (mod 5)
                // f(f(f(x)))   = f(f(2x + 3))
                //              = f(2*(2x + 3) + 3)
                //              = f(4x + 6 + 3)
                //              = f(4x + 9)
                //              = 2*(4x + 9) + 3
                //              = 8x + 18 + 3
                //              = 8x + 21 (mod 5)
                Tuple.Create(
                    new LCG(2, 3, 5),
                    (BigInteger)0,
                    (Func<LCG, LCG, LCG>)LCG.Compose,
                    LCG.GetIdentity(5)),
                Tuple.Create(
                    new LCG(2, 3, 5),
                    (BigInteger)1,
                    (Func<LCG, LCG, LCG>)LCG.Compose,
                    new LCG(2, 3, 5)),
                Tuple.Create(
                    new LCG(2, 3, 5),
                    (BigInteger)3,
                    (Func<LCG, LCG, LCG>)LCG.Compose,
                    new LCG(8, 21, 5)),
            };

            foreach (var testExample in testData)
            {
                var result = ExponentiationHelper.GetExponentiationByPowers<LCG>(
                    x: testExample.Item1,
                    exponent: testExample.Item2,
                    MultiplyFunction: testExample.Item3,
                    identity: LCG.GetIdentity(testExample.Item1.M));
                Assert.Equal(testExample.Item4, result);
            }
        }
    }
}
