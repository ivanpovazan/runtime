// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Sample
{
    class NativeAOTExceptionHandlingTask : BenchTask
    {
        public override string Name => "NativeAOTExceptionHandlingTask";
        Measurement[] measurements;

        public NativeAOTExceptionHandlingTask()
        {
            measurements = new Measurement[] {
                new Measure_TestTryCatchNoException(),
                new Measure_TestTryCatchThrowException(),
                new Measure_TestTryCatchExceptionFromCall(),
                new Measure_TestCatchExceptionType(),
                new Measure_TestTryFinally(),
                new Measure_TestTryFinallyThrowException(),
                new Measure_TestTryFinallyCatchException(),
                new Measure_TestInnerTryFinallyOrder(),
                new Measure_TestTryCatchWithCallInIf(),
                new Measure_TestThrowInCatch(),
                new Measure_TestUnconditionalThrowInCatch(),
                new Measure_TestThrowInMutuallyProtectingHandlers(),
                new Measure_TestExceptionInGvmCall(),
                new Measure_TestCatchHandlerNeedsGenericContext(),
                new Measure_TestFilterHandlerNeedsGenericContext(),
                new Measure_TestFilter(), // does not produce correct result with Mono AOT
                new Measure_TestFilterNested(),
                new Measure_TestIntraFrameFilterOrderBasic(), // does not produce correct result with Mono AOT
                // new Measure_TestIntraFrameFilterOrderDeep(), // stack overflows on Mono AOT
                new Measure_TestCatchAndThrow(),
                new Measure_TestRethrow(),
                // new Measure_TestCatchUnreachableViaFilter(), // stack overflows on Mono AOT
                new Measure_TestVirtualUnwindIndexSetForkedFlow(),
                new Measure_TestVirtualUnwindStackPopOnThrow(),
                new Measure_TestVirtualUnwindStackNoPopOnThrow(),
                new Measure_TestVirtualUnwindStackPopSelfOnUnwindingCatch(),
                new Measure_TestVirtualUnwindStackPopOnUnwindingCatch(),
                new Measure_TestVirtualUnwindStackNoPopOnUnwindingCatch(),
                new Measure_TestVirtualUnwindStackNoPopOnNestedUnwindingCatch(),
                new Measure_TestVirtualUnwindStackNoPopOnMutuallyProtectingUnwindingCatch(),
                new Measure_TestVirtualUnwindStackPopSelfOnUnwindingFault(),
                new Measure_TestVirtualUnwindStackPopOnUnwindingFault(),
                new Measure_TestVirtualUnwindStackNoPopOnUnwindingFault(),
                new Measure_TestVirtualUnwindStackNoPopOnNestedUnwindingFault(),
                new Measure_TestContainedNestedDispatchSingleFrame(),
                new Measure_TestContainedNestedDispatchIntraFrame(),
                new Measure_TestDeepContainedNestedDispatchSingleFrame(),
                new Measure_TestDeepContainedNestedDispatchIntraFrame(),
                new Measure_TestExactUncontainedNestedDispatchSingleFrame(),
                new Measure_TestClippingUncontainedNestedDispatchSingleFrame(),
                new Measure_TestExpandingUncontainedNestedDispatchSingleFrame(),
                new Measure_TestExactUncontainedNestedDispatchIntraFrame(),
                new Measure_TestClippingUncontainedNestedDispatchIntraFrame(),
                new Measure_TestExpandingUncontainedNestedDispatchIntraFrame(),
                new Measure_TestDeepUncontainedNestedDispatchSingleFrame(),
                new Measure_TestDeepUncontainedNestedDispatchIntraFrame(),
            };
        }

        public override Measurement[] Measurements
        {
            get
            {
                return measurements;
            }
        }

        public override void Initialize()
        {
        }

        public abstract class ExcMeasurement : BenchTask.Measurement
        {
            // limit this to only 1000 runs as with Mono some complex filtering patterns take forever
            public override int InitialSamples => 1;
        }

        class Measure_TestTryCatchNoException : ExcMeasurement
        {
            public override string Name => "TestTryCatchNoException";
        
            public override void RunStep() => TestTryCatchNoException(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestTryCatchNoException")]
            extern static void TestTryCatchNoException(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestTryCatchThrowException : ExcMeasurement
        {
            public override string Name => "TestTryCatchThrowException";
        
            public override void RunStep() => TestTryCatchThrowException(null, new Exception());
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestTryCatchThrowException")]
            extern static void TestTryCatchThrowException(NativeAOTExceptionHandlingTests @this, Exception e);
        }
        
        class Measure_TestTryCatchExceptionFromCall : ExcMeasurement
        {
            public override string Name => "TestTryCatchExceptionFromCall";
        
            public override void RunStep() => TestTryCatchExceptionFromCall(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestTryCatchExceptionFromCall")]
            extern static void TestTryCatchExceptionFromCall(NativeAOTExceptionHandlingTests @this);
        }

        class Measure_TestCatchExceptionType : ExcMeasurement
        {
            public override string Name => "TestCatchExceptionType";
        
            public override void RunStep() => TestCatchExceptionType(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestCatchExceptionType")]
            extern static void TestCatchExceptionType(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestTryFinally : ExcMeasurement
        {
            public override string Name => "TestTryFinally";
        
            public override void RunStep() => TestTryFinally(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestTryFinally")]
            extern static void TestTryFinally(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestTryFinallyThrowException : ExcMeasurement
        {
            public override string Name => "TestTryFinallyThrowException";
        
            public override void RunStep() => TestTryFinallyThrowException(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestTryFinallyThrowException")]
            extern static void TestTryFinallyThrowException(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestTryFinallyCatchException : ExcMeasurement
        {
            public override string Name => "TestTryFinallyCatchException";
        
            public override void RunStep() => TestTryFinallyCatchException(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestTryFinallyCatchException")]
            extern static void TestTryFinallyCatchException(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestInnerTryFinallyOrder : ExcMeasurement
        {
            public override string Name => "TestInnerTryFinallyOrder";
        
            public override void RunStep() => TestInnerTryFinallyOrder(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestInnerTryFinallyOrder")]
            extern static void TestInnerTryFinallyOrder(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestTryCatchWithCallInIf : ExcMeasurement
        {
            public override string Name => "TestTryCatchWithCallInIf";
        
            public override void RunStep() => TestTryCatchWithCallInIf(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestTryCatchWithCallInIf")]
            extern static void TestTryCatchWithCallInIf(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestThrowInCatch : ExcMeasurement
        {
            public override string Name => "TestThrowInCatch";
        
            public override void RunStep() => TestThrowInCatch(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestThrowInCatch")]
            extern static void TestThrowInCatch(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestUnconditionalThrowInCatch : ExcMeasurement
        {
            public override string Name => "TestUnconditionalThrowInCatch";
        
            public override void RunStep() => TestUnconditionalThrowInCatch(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestUnconditionalThrowInCatch")]
            extern static void TestUnconditionalThrowInCatch(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestThrowInMutuallyProtectingHandlers : ExcMeasurement
        {
            public override string Name => "TestThrowInMutuallyProtectingHandlers";
        
            public override void RunStep() => TestThrowInMutuallyProtectingHandlers(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestThrowInMutuallyProtectingHandlers")]
            extern static void TestThrowInMutuallyProtectingHandlers(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestExceptionInGvmCall : ExcMeasurement
        {
            public override string Name => "TestExceptionInGvmCall";
        
            public override void RunStep() => TestExceptionInGvmCall(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestExceptionInGvmCall")]
            extern static void TestExceptionInGvmCall(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestCatchHandlerNeedsGenericContext : ExcMeasurement
        {
            public override string Name => "TestCatchHandlerNeedsGenericContext";
        
            public override void RunStep() => TestCatchHandlerNeedsGenericContext(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestCatchHandlerNeedsGenericContext")]
            extern static void TestCatchHandlerNeedsGenericContext(NativeAOTExceptionHandlingTests @this);
        }

        class Measure_TestFilterHandlerNeedsGenericContext : ExcMeasurement
        {
            public override string Name => "TestFilterHandlerNeedsGenericContext";
        
            public override void RunStep() => TestFilterHandlerNeedsGenericContext(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestFilterHandlerNeedsGenericContext")]
            extern static void TestFilterHandlerNeedsGenericContext(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestFilter : ExcMeasurement
        {
            public override string Name => "TestFilter";
        
            public override void RunStep() => TestFilter(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestFilter")]
            extern static void TestFilter(NativeAOTExceptionHandlingTests @this);
        }

        class Measure_TestFilterNested : ExcMeasurement
        {
            public override string Name => "TestFilterNested";
        
            public override void RunStep() => TestFilterNested(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestFilterNested")]
            extern static void TestFilterNested(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestIntraFrameFilterOrderBasic : ExcMeasurement
        {
            public override string Name => "TestIntraFrameFilterOrderBasic";
        
            public override void RunStep() => TestIntraFrameFilterOrderBasic(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestIntraFrameFilterOrderBasic")]
            extern static void TestIntraFrameFilterOrderBasic(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestIntraFrameFilterOrderDeep : ExcMeasurement
        {
            public override string Name => "TestIntraFrameFilterOrderDeep";
        
            public override void RunStep() => TestIntraFrameFilterOrderDeep(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestIntraFrameFilterOrderDeep")]
            extern static void TestIntraFrameFilterOrderDeep(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestCatchAndThrow : ExcMeasurement
        {
            public override string Name => "TestCatchAndThrow";
        
            public override void RunStep() => TestCatchAndThrow(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestCatchAndThrow")]
            extern static void TestCatchAndThrow(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestRethrow : ExcMeasurement
        {
            public override string Name => "TestRethrow";
        
            public override void RunStep() => TestRethrow(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestRethrow")]
            extern static void TestRethrow(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestCatchUnreachableViaFilter : ExcMeasurement
        {
            public override string Name => "TestCatchUnreachableViaFilter";
        
            public override void RunStep() => TestCatchUnreachableViaFilter(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestCatchUnreachableViaFilter")]
            extern static void TestCatchUnreachableViaFilter(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindIndexSetForkedFlow : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindIndexSetForkedFlow";
        
            public override void RunStep() => TestVirtualUnwindIndexSetForkedFlow(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindIndexSetForkedFlow")]
            extern static void TestVirtualUnwindIndexSetForkedFlow(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackPopOnThrow : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackPopOnThrow";
        
            public override void RunStep() => TestVirtualUnwindStackPopOnThrow(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackPopOnThrow")]
            extern static void TestVirtualUnwindStackPopOnThrow(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackNoPopOnThrow : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackNoPopOnThrow";
        
            public override void RunStep() => TestVirtualUnwindStackNoPopOnThrow(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackNoPopOnThrow")]
            extern static void TestVirtualUnwindStackNoPopOnThrow(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackPopSelfOnUnwindingCatch : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackPopSelfOnUnwindingCatch";
        
            public override void RunStep() => TestVirtualUnwindStackPopSelfOnUnwindingCatch(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackPopSelfOnUnwindingCatch")]
            extern static void TestVirtualUnwindStackPopSelfOnUnwindingCatch(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackPopOnUnwindingCatch : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackPopOnUnwindingCatch";
        
            public override void RunStep() => TestVirtualUnwindStackPopOnUnwindingCatch(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackPopOnUnwindingCatch")]
            extern static void TestVirtualUnwindStackPopOnUnwindingCatch(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackNoPopOnUnwindingCatch : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackNoPopOnUnwindingCatch";
        
            public override void RunStep() => TestVirtualUnwindStackNoPopOnUnwindingCatch(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackNoPopOnUnwindingCatch")]
            extern static void TestVirtualUnwindStackNoPopOnUnwindingCatch(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackNoPopOnNestedUnwindingCatch : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackNoPopOnNestedUnwindingCatch";
        
            public override void RunStep() => TestVirtualUnwindStackNoPopOnNestedUnwindingCatch(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackNoPopOnNestedUnwindingCatch")]
            extern static void TestVirtualUnwindStackNoPopOnNestedUnwindingCatch(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackNoPopOnMutuallyProtectingUnwindingCatch : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackNoPopOnMutuallyProtectingUnwindingCatch";
        
            public override void RunStep() => TestVirtualUnwindStackNoPopOnMutuallyProtectingUnwindingCatch(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackNoPopOnMutuallyProtectingUnwindingCatch")]
            extern static void TestVirtualUnwindStackNoPopOnMutuallyProtectingUnwindingCatch(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackPopSelfOnUnwindingFault : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackPopSelfOnUnwindingFault";
        
            public override void RunStep() => TestVirtualUnwindStackPopSelfOnUnwindingFault(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackPopSelfOnUnwindingFault")]
            extern static void TestVirtualUnwindStackPopSelfOnUnwindingFault(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackPopOnUnwindingFault : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackPopOnUnwindingFault";
        
            public override void RunStep() => TestVirtualUnwindStackPopOnUnwindingFault(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackPopOnUnwindingFault")]
            extern static void TestVirtualUnwindStackPopOnUnwindingFault(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackNoPopOnUnwindingFault : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackNoPopOnUnwindingFault";
        
            public override void RunStep() => TestVirtualUnwindStackNoPopOnUnwindingFault(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackNoPopOnUnwindingFault")]
            extern static void TestVirtualUnwindStackNoPopOnUnwindingFault(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestVirtualUnwindStackNoPopOnNestedUnwindingFault : ExcMeasurement
        {
            public override string Name => "TestVirtualUnwindStackNoPopOnNestedUnwindingFault";
        
            public override void RunStep() => TestVirtualUnwindStackNoPopOnNestedUnwindingFault(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestVirtualUnwindStackNoPopOnNestedUnwindingFault")]
            extern static void TestVirtualUnwindStackNoPopOnNestedUnwindingFault(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestContainedNestedDispatchSingleFrame : ExcMeasurement
        {
            public override string Name => "TestContainedNestedDispatchSingleFrame";
        
            public override void RunStep() => TestContainedNestedDispatchSingleFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestContainedNestedDispatchSingleFrame")]
            extern static void TestContainedNestedDispatchSingleFrame(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestContainedNestedDispatchIntraFrame : ExcMeasurement
        {
            public override string Name => "TestContainedNestedDispatchIntraFrame";
        
            public override void RunStep() => TestContainedNestedDispatchIntraFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestContainedNestedDispatchIntraFrame")]
            extern static void TestContainedNestedDispatchIntraFrame(NativeAOTExceptionHandlingTests @this);
        }

        class Measure_TestDeepContainedNestedDispatchSingleFrame : ExcMeasurement
        {
            public override string Name => "TestDeepContainedNestedDispatchSingleFrame";
        
            public override void RunStep() => TestDeepContainedNestedDispatchSingleFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestDeepContainedNestedDispatchSingleFrame")]
            extern static void TestDeepContainedNestedDispatchSingleFrame(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestDeepContainedNestedDispatchIntraFrame : ExcMeasurement
        {
            public override string Name => "TestDeepContainedNestedDispatchIntraFrame";
        
            public override void RunStep() => TestDeepContainedNestedDispatchIntraFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestDeepContainedNestedDispatchIntraFrame")]
            extern static void TestDeepContainedNestedDispatchIntraFrame(NativeAOTExceptionHandlingTests @this);
        }

        class Measure_TestExactUncontainedNestedDispatchSingleFrame : ExcMeasurement
        {
            public override string Name => "TestExactUncontainedNestedDispatchSingleFrame";
        
            public override void RunStep() => TestExactUncontainedNestedDispatchSingleFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestExactUncontainedNestedDispatchSingleFrame")]
            extern static void TestExactUncontainedNestedDispatchSingleFrame(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestClippingUncontainedNestedDispatchSingleFrame : ExcMeasurement
        {
            public override string Name => "TestClippingUncontainedNestedDispatchSingleFrame";
        
            public override void RunStep() => TestClippingUncontainedNestedDispatchSingleFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestClippingUncontainedNestedDispatchSingleFrame")]
            extern static void TestClippingUncontainedNestedDispatchSingleFrame(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestExpandingUncontainedNestedDispatchSingleFrame : ExcMeasurement
        {
            public override string Name => "TestExpandingUncontainedNestedDispatchSingleFrame";
        
            public override void RunStep() => TestExpandingUncontainedNestedDispatchSingleFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestExpandingUncontainedNestedDispatchSingleFrame")]
            extern static void TestExpandingUncontainedNestedDispatchSingleFrame(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestExactUncontainedNestedDispatchIntraFrame : ExcMeasurement
        {
            public override string Name => "TestExactUncontainedNestedDispatchIntraFrame";
        
            public override void RunStep() => TestExactUncontainedNestedDispatchIntraFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestExactUncontainedNestedDispatchIntraFrame")]
            extern static void TestExactUncontainedNestedDispatchIntraFrame(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestClippingUncontainedNestedDispatchIntraFrame : ExcMeasurement
        {
            public override string Name => "TestClippingUncontainedNestedDispatchIntraFrame";
        
            public override void RunStep() => TestClippingUncontainedNestedDispatchIntraFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestClippingUncontainedNestedDispatchIntraFrame")]
            extern static void TestClippingUncontainedNestedDispatchIntraFrame(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestExpandingUncontainedNestedDispatchIntraFrame : ExcMeasurement
        {
            public override string Name => "TestExpandingUncontainedNestedDispatchIntraFrame";
        
            public override void RunStep() => TestExpandingUncontainedNestedDispatchIntraFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestExpandingUncontainedNestedDispatchIntraFrame")]
            extern static void TestExpandingUncontainedNestedDispatchIntraFrame(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestDeepUncontainedNestedDispatchSingleFrame : ExcMeasurement
        {
            public override string Name => "TestDeepUncontainedNestedDispatchSingleFrame";
        
            public override void RunStep() => TestDeepUncontainedNestedDispatchSingleFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestDeepUncontainedNestedDispatchSingleFrame")]
            extern static void TestDeepUncontainedNestedDispatchSingleFrame(NativeAOTExceptionHandlingTests @this);
        }
        
        class Measure_TestDeepUncontainedNestedDispatchIntraFrame : ExcMeasurement
        {
            public override string Name => "TestDeepUncontainedNestedDispatchIntraFrame";
        
            public override void RunStep() => TestDeepUncontainedNestedDispatchIntraFrame(null);
        
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "TestDeepUncontainedNestedDispatchIntraFrame")]
            extern static void TestDeepUncontainedNestedDispatchIntraFrame(NativeAOTExceptionHandlingTests @this);
        }
    }
}
