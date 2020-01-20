using AdventOfCode2019.MathHelpers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.MathHelpers
{
    public class ModHelperTest
    {
        [Fact]
        public void TryGetModInverseTest()
        {
            var testData = new List<Tuple<BigInteger, BigInteger, bool, BigInteger>>()
            {
                Tuple.Create((BigInteger)2, (BigInteger)3, true, (BigInteger)2),
                Tuple.Create((BigInteger)(-1), (BigInteger)3, true, (BigInteger)2),
                Tuple.Create(BigInteger.Zero, (BigInteger)3, false, BigInteger.Zero),
                Tuple.Create((BigInteger)3, (BigInteger)3, false, BigInteger.Zero),
                Tuple.Create((BigInteger)16, (BigInteger)24, false, BigInteger.Zero),
                Tuple.Create((BigInteger)7, (BigInteger)25, true, (BigInteger)18),
            };

            foreach (var testExample in testData)
            {
                var isInvertible = ModHelper.TryGetModInverse(testExample.Item1, testExample.Item2, out BigInteger result);
                Assert.Equal(testExample.Item3, isInvertible);
                if (isInvertible)
                {
                    Assert.Equal(testExample.Item4, result);
                }
            }
        }
    }
}
