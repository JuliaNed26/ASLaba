using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace CryptographyLaba2
{
    public static class Algebra
    {
        public static long GDC(long num1, long num2) => num1 == 0 ? num2 : GDC(Math.Max(num1, num2) % Math.Min(num1, num2), Math.Min(num1, num2));

        public static bool IsPrime(long number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        public static bool IsPrimeMillerRabinMethod(long number, long raundNumber)
        {
            if (number == 2 || number == 3)
            {
                return true;
            }
            if(number < 2 || number % 2 == 0)
            {
                return false;
            }

            long oddNum = number - 1;
            long twoPow = 0;
            while(oddNum % 2 == 0)
            {
                oddNum /= 2;
                twoPow++;
            }
            Random rand = new Random();
            for(int i = 0; i < raundNumber; i++)
            {
                long a = rand.Next(2, (int)number - 2);
                long x = (long)BigInteger.ModPow(a, oddNum, number);
                if(x == 1 || x == number - 1)
                {
                    continue;
                }
                for (int l = 0; l < twoPow - 1; l++)
                {
                    x = (long)BigInteger.ModPow(x, 2, number);
                    if(x == 1 || x != number - 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static IEnumerable<long> FactorizeNum(long number)
        {
            var random = new Random();
            long x = 2, y = 2;
            long c = random.Next(0, 100);
            long divisor = 1;

            if(number == 1 || IsPrime(number))
            {
                yield return number;
                yield break;
            }

            else if (number % 2 == 0)
            {
                divisor = 2;
                yield return divisor;
            }

            else
            {
                while (divisor == 1)
                {
                    x = GetFuncResult(x);
                    y = GetFuncResult(GetFuncResult(y));
                    divisor = GDC(Math.Abs(y - x), number);
                    if (divisor == number)
                    {
                        foreach (var d in FactorizeNum(number))
                        {
                            yield return d;
                        }
                        yield break;
                    }
                    else if (divisor != 1)
                    {
                        yield return divisor;
                    }

                }
            }

            foreach (var d in FactorizeNum(number / divisor))
            {
                yield return d;
            }

            long GetFuncResult(long num) => (num * num + c) % number;
        }

        public static long SolveDiscreteLog(long numUnderPow, long powResult, long modulo)
        {
            var moduloPow = (long num, long pow) => (long)Math.Pow(num, pow) % modulo;

            long k = (long)Math.Sqrt(modulo) + 1;
            long numUnderPowInPowK = moduloPow(numUnderPow, k);
            List<long> babyStepResults = new List<long>();
            for (int i = 1; i <= k; i++)
            {
                babyStepResults.Add(moduloPow(numUnderPowInPowK, i));
            }
            for (int i = 1; i < k; i++)
            {
                long curGiantStepResult = (long)(powResult * Math.Pow(numUnderPow, i)) % modulo;
                if (babyStepResults.Contains(curGiantStepResult))
                {
                    return (babyStepResults.IndexOf(curGiantStepResult) + 1) * k - i;
                }
            }
            return 0;
        }

        public static long SolveEulerFunction(long number)
        {
            if(IsPrime(number))
            {
                return number - 1;
            }

            var factorizationResult = FactorizeNum(number).ToList();
            if(factorizationResult.Count() == 2)
            {
                return (factorizationResult[0] - 1) * (factorizationResult[1] - 1);
            }

            factorizationResult = factorizationResult.Distinct().ToList();
            double result = number;
            foreach(long factor in factorizationResult)
            {
                result *= (1 - (1 / (double)factor));
            }
            return (long)result;
        }

        public static long SolveMobiusFunction(long number)
        {
            if (number == 1)
            {
                return 1;
            }

            var factorizationResult = FactorizeNum(number).ToList();
            if(factorizationResult.Count() == 2 && factorizationResult[0] == factorizationResult[1])
            {
                return 0;
            }

            return (long)Math.Pow(-1, factorizationResult.Distinct().Count());
        }

        public static int FindLegendreSymbol(long number, ulong primeNum)
        {
            if (!IsPrime((long)primeNum) || primeNum == 2)
            {
                throw new ArgumentException("The second number should be prime and not 2");
            }
            return FindJacobiSymbol(number, primeNum);
        }

        public static long FindDiscreteSquare(long number, ulong modulo)
        {
            if(FindLegendreSymbol(number, (ulong)modulo) != 1)
            {
                throw new Exception($"The number {number} is not a squre in F{modulo}");
            }

            long a = 0;
            for(long i = 2; i < (long)modulo; i++)
            {
                if(FindLegendreSymbol((i * i + (long)modulo - number) % (long)modulo, modulo) == -1)
                {
                    a = i;
                    break;
                }
            }

            Tuple<long, long> result = new Tuple<long, long>(1, 0);
            Tuple<long, long> evenPowExpr = new Tuple<long, long>(a, 1);
            long power = (long)(modulo + 1) / 2;
            while (power > 0)
            {
                if (power % 2 != 0)
                {
                    result = Multiply(result, evenPowExpr);
                }
                evenPowExpr = Multiply(evenPowExpr, evenPowExpr);
                power /= 2;
            }

            //Check x in Fp
            if (result.Item2 != 0)
            {
                return 0;
            }

            //Check x * x = n
            if (result.Item1 * result.Item1 % (long)modulo != number)
            {
                return 0;
            }

            return (long)result.Item1;


            Tuple<long, long> Multiply(Tuple<long, long> firstExpr, Tuple<long, long> secondExpr)
            {
                long sqrtASubstrN = (a * a + (long)modulo - number) % (long)modulo;

                return new Tuple<long, long>(
                    (firstExpr.Item1 * secondExpr.Item1 + firstExpr.Item2 * secondExpr.Item2 * sqrtASubstrN) % (long)modulo,
                    (firstExpr.Item1 * secondExpr.Item2 + secondExpr.Item1 * firstExpr.Item2) % (long)modulo);
            }
        }

        private static int FindJacobiSymbol(long num1, ulong oddNum)
        {
            if(oddNum % 2 == 0 || oddNum == 0)
            {
                throw new ArgumentException("The second number should be odd and >= 1");
            }
            if (num1 % (long)oddNum == 0)
            {
                return 0;
            }
            if (num1 == 1)
            {
                return 1;
            }
            if(num1 == -1)
            {
                return (int)Math.Pow(-1, (oddNum - 1) / 2);
            }
            if (num1 % 2 == 0)
            {
                return FindJacobiSymbol(num1 / 2, oddNum) * (int)Math.Pow(-1, (oddNum * oddNum - 1) / 8);
            }
            return GDC(num1, (long)oddNum) == 1 ? FindJacobiSymbol((long)oddNum % num1, (ulong)num1) * (int)Math.Pow(-1, (num1 - 1) * ((long)oddNum - 1) / 4)
                                                : FindJacobiSymbol(num1 % (long)oddNum, oddNum);
        }
    }
}