// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;

namespace IP_Values
{
    public class IP_Value<T>
    {
        public T Value = default;
        public T Increment(int inc)
        {
            if (typeof(T) == typeof(int))
                Value = (T)(object)((int)(object)Value + inc);
            else if (typeof(T) == typeof(string))
                Value = (T)(object)((string)(object)Value + inc.ToString());
            return Value;
        }

        public void Print() => Console.WriteLine(Value);
    }
}