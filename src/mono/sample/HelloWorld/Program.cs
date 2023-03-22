// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace HelloWorld
{
        internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Hello");
            CallbackManager cm = new();
            var mgt1 = new MyGenType<byte>();
            mgt1.Signup(cm);
            cm.Invoke(mgt1);
        }
    }

    public class MyGenType<T>
    {
        public string TypeToString()
        {
            if (typeof(T) == typeof(byte))
                return "BYTE";
            else
                return "UNSUPPORTED";
        }

        public void Signup(CallbackManager cm)
        {
            cm.Register(c => ((MyGenType<T>)c).TypeToString());
        }
    }

    public class CallbackManager
    {
        private Func<object, string> _cb;
        public void Register(Func<object, string> cb)
        {
            _cb = cb;
        }
        public void Invoke(object param)
        {
            if (_cb == null)
                Console.WriteLine("No registered callback");
            else
                Console.WriteLine($"Invoking callback: {_cb(param)}");
        }
    }
}
