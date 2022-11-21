using CryptographyLaba2;

namespace CryptographyLaba2Tests
{
    public class AlgebraFixture
    {
        [Test]
        public void FactorizeNumTests()
        {
            CollectionAssert.AreEquivalent(Algebra.FactorizeNum(8051), new List<long> { 97, 83});
            CollectionAssert.AreEquivalent(Algebra.FactorizeNum(49), new List<long> { 7, 7 });
            CollectionAssert.AreEquivalent(Algebra.FactorizeNum(19), new List<long> { 19 });
            CollectionAssert.AreEquivalent(Algebra.FactorizeNum(4928), new List<long> { 2,2,2,2,2,2,7,11 });
        }

        [Test]
        public void SolveDiscreteLogTests()
        {
            Assert.That(Algebra.SolveDiscreteLog(5, 3, 23), Is.EqualTo(16));
            Assert.That(Algebra.SolveDiscreteLog(28, 5, 19), Is.EqualTo(2));
        }

        [Test]
        public void SolveEulerFunctionTests()
        {
            Assert.That(Algebra.SolveEulerFunction(97), Is.EqualTo(96));
            Assert.That(Algebra.SolveEulerFunction(49), Is.EqualTo(36));
            Assert.That(Algebra.SolveEulerFunction(391), Is.EqualTo(352));
            Assert.That(Algebra.SolveEulerFunction(200), Is.EqualTo(80));
        }

        [Test]
        public void SolveMobiusFunctionTests()
        {
            Assert.That(Algebra.SolveMobiusFunction(1), Is.EqualTo(1));
            Assert.That(Algebra.SolveMobiusFunction(49), Is.EqualTo(0));
            Assert.That(Algebra.SolveMobiusFunction(42), Is.EqualTo(-1));
            Assert.That(Algebra.SolveMobiusFunction(6), Is.EqualTo(1));
        }

        [Test]
        public void FindLegendreSymbolTests()
        {
            Assert.That(Algebra.FindLegendreSymbol(10,3), Is.EqualTo(1));
            Assert.That(Algebra.FindLegendreSymbol(20, 61), Is.EqualTo(1));
            Assert.That(Algebra.FindLegendreSymbol(22, 103), Is.EqualTo(-1));
            Assert.That(Algebra.FindLegendreSymbol(10, 5), Is.EqualTo(0));
        }

        [Test]
        public void FindDiscreteSquareTests()
        {
            Assert.That(Algebra.FindDiscreteSquare(10, 13), Is.EqualTo(6));
            Assert.That(Algebra.FindDiscreteSquare(56, 101), Is.EqualTo(37));
            Assert.That(Algebra.FindDiscreteSquare(8218, 10007), Is.EqualTo(9872));
        }

        [Test]
        public void IsPrimeMillerRabinMethodTests()
        {
            Assert.IsFalse(Algebra.IsPrimeMillerRabinMethod(221, 10));
            Assert.IsFalse(Algebra.IsPrimeMillerRabinMethod(221, 2));
            Assert.IsFalse(Algebra.IsPrimeMillerRabinMethod(90, 1));
            Assert.IsTrue(Algebra.IsPrimeMillerRabinMethod(2213, 1));
        }
    }
}