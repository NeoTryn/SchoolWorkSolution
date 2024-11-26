using System;
using System.Collections.Generic;

class LambdaTest
{
    public static decimal[] Converter(decimal[] values, Func<decimal, decimal> converterFunc)
    {
        if (values == null) { return new decimal[0]; }

        decimal[] result = new decimal[values.Length];
        int i = 0;
        foreach (decimal value in values)
        {
            result[i++] = converterFunc(value);
        }
        return result;
    }

    public static void ForEach(decimal[] values, Action<decimal> action)
    {
        foreach (decimal value in values)
        {
            action(value);
        }
    }

    public static decimal ArithmeticOperation(decimal x, decimal y, Func<decimal, decimal, decimal> operation)
    {
        return operation(x, y);
    }

    public static decimal ArithmeticOperation(decimal x, decimal y, Func<decimal, decimal, decimal> operation, Action<string> logFunction)
    {
        try
        {
            return operation(x, y);
        }
        catch (Exception e)
        {
            logFunction(e.Message);
        }
        return 0;
    }

    public static void RunCommand(Action command)
    {
        command();
    }

    public static decimal[] Filter(decimal[] values, Func<decimal, bool> filterFunction)
    {
        List<decimal> result = new List<decimal>();
        foreach (decimal value in values)
        {
            if (filterFunction(value))
            {
                result.Add(value);
            }
        }
        return result.ToArray();
    }
}


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Beispiel 1: Converter");
        decimal[] converted = LambdaTest.Converter(new decimal[] { -10, 0, 10, 20, 30 }, value => value + 273.15M);
        LambdaTest.ForEach(converted, value => Console.WriteLine(value));

        Console.WriteLine("Beispiel 2: Filter");
        decimal[] freezed = LambdaTest.Filter(converted, val => val < 273.15M);
        LambdaTest.ForEach(freezed, value => Console.WriteLine(value));

        Console.WriteLine("Beispiel 3: Division");
        decimal result = LambdaTest.ArithmeticOperation(2, 0, (x, y) => y == 0 ? 0 : x / y);
        Console.WriteLine(result);
        result = LambdaTest.ArithmeticOperation(2, 0, (x, y) => x / y, message => Console.Error.WriteLine(message));
        Console.WriteLine(result);

        Console.WriteLine("Beispiel 4: Callback Funktion");
        LambdaTest.RunCommand(() =>
        {
            Console.WriteLine("Hello World.");
            Console.WriteLine("Hello World again.");
        });

        Console.ReadLine();
    }
}
