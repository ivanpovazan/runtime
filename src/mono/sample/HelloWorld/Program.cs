// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using IP_Values;
namespace HelloWorld
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IP_Value<int> v1 = new IP_Value<int>();
            IP_Value<string> v2 = new IP_Value<string>();
            
            v1.Print();
            var incremented1 = v1.Increment(10);
            Console.WriteLine($"Incremented v1: {incremented1}");
            v1.Print();

            v2.Print();
            var incremented2 = v2.Increment(10);
            Console.WriteLine($"Incremented v2: {incremented2}");
            v2.Print();

            Console.WriteLine("Concat: " + v1.Value + v2.Value);
        }
    }
}
