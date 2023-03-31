// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace System.Reflection
{
    public static partial class IP_Diagnostics
    {
        public static IDictionary<MethodBase, IList<(bool, string)>> GetInvokedMethods() => InvokedMethods;
        internal static Dictionary<MethodBase, IList<(bool, string)>> InvokedMethods = new Dictionary<MethodBase, IList<(bool, string)>>();
    }

    internal sealed partial class MethodInvoker
    {
        private readonly MethodBase _method;
        private readonly object balanceLock = new object();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe object? InlinedInvoke(object? obj, IntPtr* args, BindingFlags invokeAttr)
        {
            if (_invokeFunc != null && (invokeAttr & BindingFlags.DoNotWrapExceptions) != 0)
            {
                return _invokeFunc(obj, args);
            }
            return Invoke(obj, args, invokeAttr);
        }

        private bool _invoked;
        private bool _strategyDetermined;
        private InvokerEmitUtil.InvokeFunc? _invokeFunc;

        [DebuggerStepThrough]
        [DebuggerHidden]
        private unsafe object? Invoke(object? obj, IntPtr* args, BindingFlags invokeAttr)
        {
            if (!_strategyDetermined)
            {
                if (!_invoked)
                {
                    // The first time, ignoring race conditions, use the slow path.
                    _invoked = true;
                }
                else
                {
                    if (RuntimeFeature.IsDynamicCodeCompiled)
                    {
                        _invokeFunc = InvokerEmitUtil.CreateInvokeDelegate(_method);
                    }
                    _strategyDetermined = true;
                }
            }

            lock (balanceLock)
            {
                var st = new System.Diagnostics.StackTrace(true);
                if (IP_Diagnostics.InvokedMethods.TryGetValue(_method, out IList<(bool, string)>? entries))
                {
                    entries?.Add((_invokeFunc != null, st.ToString()));
                }
                else
                {
                    IP_Diagnostics.InvokedMethods[_method] = new List<(bool, string)>();
                    IP_Diagnostics.InvokedMethods[_method].Add((_invokeFunc != null, st.ToString()));
                }
            }

            object? ret;
            if ((invokeAttr & BindingFlags.DoNotWrapExceptions) == 0)
            {
                try
                {
                    if (_invokeFunc != null)
                    {
                        ret = _invokeFunc(obj, args);
                    }
                    else
                    {
                        ret = InterpretedInvoke(obj, args);
                    }
                }
                catch (Exception e)
                {
                    throw new TargetInvocationException(e);
                }
            }
            else if (_invokeFunc != null)
            {
                ret = _invokeFunc(obj, args);
            }
            else
            {
                ret = InterpretedInvoke(obj, args);
            }

            return ret;
        }
    }
}
