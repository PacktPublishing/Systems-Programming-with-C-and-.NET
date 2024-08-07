﻿using System.Diagnostics;
using ExtensionLibrary;

namespace Profiling;

internal class PrimeCalculator
{
    public void Run()
    {
        var limit = 100000;

        var stopwatch = Stopwatch.StartNew();
        var sum = SumOfPrimes(limit);
        stopwatch.Stop();

        $"Sum of primes up to {limit}: {sum}".Dump();
        $"Time taken: {stopwatch.ElapsedMilliseconds} ms".Dump();
    }

    private long SumOfPrimes(int limit)
    {
        long sum = 0;
        for (var i = 2; i <= limit; i++)
            if (IsPrime(i))
                sum += i;

        return sum;
    }

    private bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (var i = 2; i <= Math.Sqrt(number); i++)
            if (number % i == 0)
                return false;

        return true;
    }

    private bool IsPrimeOptimized(int number)
    {
        if (number < 2) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        var upperBound = Math.Sqrt(number);
        for (var i = 2; i <= upperBound; i++)
            if (number % i == 0)
                return false;

        return true;
    }
}