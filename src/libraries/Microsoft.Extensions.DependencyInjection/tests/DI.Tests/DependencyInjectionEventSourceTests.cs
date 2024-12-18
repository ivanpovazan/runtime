// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Specification.Fakes;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.Extensions.DependencyInjection.Tests
{
    public class DependencyInjectionEventSourceTests(
        DependencyInjectionEventSourceTests.TestEventListenerFixture fixture)
        : IClassFixture<DependencyInjectionEventSourceTests.TestEventListenerFixture>
    {
        private TestEventListener Listener => fixture.Listener;

        [Fact]
        public void ExistsWithCorrectId()
        {
            var esType = typeof(DependencyInjectionEventSource);

            Assert.NotNull(esType);

            Assert.Equal("Microsoft-Extensions-DependencyInjection", EventSource.GetName(esType));
            Assert.Equal(Guid.Parse("27837f46-1a43-573d-d30c-276de7d02192"), EventSource.GetGuid(esType));
            Assert.NotEmpty(EventSource.GenerateManifest(esType, "assemblyPathToIncludeInManifest"));
        }

        [Fact]
        public void EmitsCallSiteBuiltEvent()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var fakeDisposeCallback = new FakeDisposeCallback();
            serviceCollection.AddSingleton(fakeDisposeCallback);
            serviceCollection.AddTransient<IFakeOuterService, FakeDisposableCallbackOuterService>();
            serviceCollection.AddSingleton<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddSingleton<IFakeMultipleService>(provider => new FakeDisposableCallbackInnerService(fakeDisposeCallback));
            serviceCollection.AddScoped<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddTransient<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddSingleton<IFakeService, FakeDisposableCallbackInnerService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<IEnumerable<IFakeOuterService>>();

            var callsiteBuiltEvent = Listener.EventDataFor(serviceProvider).Single(e => e.EventName == "CallSiteBuilt");


            Assert.Equal(
                string.Join(Environment.NewLine,
                "{",
                "  \"serviceType\": \"System.Collections.Generic.IEnumerable`1[Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeOuterService]\",",
                "  \"kind\": \"IEnumerable\",",
                "  \"cache\": \"None\",",
                "  \"itemType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeOuterService\",",
                "  \"size\": \"1\",",
                "  \"items\": [",
                "    {",
                "      \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeOuterService\",",
                "      \"kind\": \"Constructor\",",
                "      \"cache\": \"Dispose\",",
                "      \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackOuterService\",",
                "      \"arguments\": [",
                "        {",
                "          \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeService\",",
                "          \"kind\": \"Constructor\",",
                "          \"cache\": \"Root\",",
                "          \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\",",
                "          \"arguments\": [",
                "            {",
                "              \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\",",
                "              \"kind\": \"Constant\",",
                "              \"cache\": \"None\",",
                "              \"value\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\"",
                "            }",
                "          ]",
                "        },",
                "        {",
                "          \"serviceType\": \"System.Collections.Generic.IEnumerable`1[Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService]\",",
                "          \"kind\": \"IEnumerable\",",
                "          \"cache\": \"None\",",
                "          \"itemType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "          \"size\": \"4\",",
                "          \"items\": [",
                "            {",
                "              \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "              \"kind\": \"Constructor\",",
                "              \"cache\": \"Root\",",
                "              \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\",",
                "              \"arguments\": [",
                "                {",
                "                  \"ref\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\"",
                "                }",
                "              ]",
                "            },",
                "            {",
                "              \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "              \"kind\": \"Factory\",",
                "              \"cache\": \"Root\",",
                "              \"method\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService <EmitsCallSiteBuiltEvent>b__0(System.IServiceProvider)\"",
                "            },",
                "            {",
                "              \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "              \"kind\": \"Constructor\",",
                "              \"cache\": \"Scope\",",
                "              \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\",",
                "              \"arguments\": [",
                "                {",
                "                  \"ref\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\"",
                "                }",
                "              ]",
                "            },",
                "            {",
                "              \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "              \"kind\": \"Constructor\",",
                "              \"cache\": \"Dispose\",",
                "              \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\",",
                "              \"arguments\": [",
                "                {",
                "                  \"ref\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\"",
                "                }",
                "              ]",
                "            }",
                "          ]",
                "        },",
                "        {",
                "          \"ref\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\"",
                "        }",
                "      ]",
                "    }",
                "  ]",
                "}"),
                JObject.Parse(GetProperty<string>(callsiteBuiltEvent, "callSite")).ToString());

            Assert.Equal("System.Collections.Generic.IEnumerable`1[Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeOuterService]", GetProperty<string>(callsiteBuiltEvent, "serviceType"));
            Assert.Equal(0, GetProperty<int>(callsiteBuiltEvent, "chunkIndex"));
            Assert.Equal(1, GetProperty<int>(callsiteBuiltEvent, "chunkCount"));
            Assert.Equal(1, callsiteBuiltEvent.EventId);
        }

        [Fact]
        public void EmitsServiceResolvedEvent()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IFakeService, FakeService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetService<IFakeService>();
            serviceProvider.GetService<IFakeService>();
            serviceProvider.GetService<IFakeService>();

            var serviceResolvedEvents = Listener.EventDataFor(serviceProvider).Where(e => e.EventName == "ServiceResolved").ToArray();

            Assert.Equal(3, serviceResolvedEvents.Length);
            foreach (var serviceResolvedEvent in serviceResolvedEvents)
            {
                Assert.Equal("Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeService", GetProperty<string>(serviceResolvedEvent, "serviceType"));
                Assert.Equal(2, serviceResolvedEvent.EventId);
            }
        }

        [Fact]
        public void EmitsExpressionTreeBuiltEvent()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IFakeService, FakeService>();

            var serviceProvider = serviceCollection.BuildServiceProvider(ServiceProviderMode.Expressions);

            serviceProvider.GetService<IFakeService>();

            var expressionTreeGeneratedEvent = Listener.EventDataFor(serviceProvider).Single(e => e.EventName == "ExpressionTreeGenerated");

            Assert.Equal("Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeService", GetProperty<string>(expressionTreeGeneratedEvent, "serviceType"));
            Assert.Equal(9, GetProperty<int>(expressionTreeGeneratedEvent, "nodeCount"));
            Assert.Equal(3, expressionTreeGeneratedEvent.EventId);
        }

        [Fact]
        public void EmitsDynamicMethodBuiltEvent()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IFakeService, FakeService>();

            var serviceProvider = serviceCollection.BuildServiceProvider(ServiceProviderMode.ILEmit);

            serviceProvider.GetService<IFakeService>();

            var expressionTreeGeneratedEvent = Listener.EventDataFor(serviceProvider).Single(e => e.EventName == "DynamicMethodBuilt");

            Assert.Equal("Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeService", GetProperty<string>(expressionTreeGeneratedEvent, "serviceType"));
            Assert.Equal(12, GetProperty<int>(expressionTreeGeneratedEvent, "methodSize"));
            Assert.Equal(4, expressionTreeGeneratedEvent.EventId);
        }

        [Fact]
        public void EmitsScopeDisposedEvent()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IFakeService, FakeService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                scope.ServiceProvider.GetService<IFakeService>();
            }

            var scopeDisposedEvent = Listener.EventDataFor(serviceProvider).Single(e => e.EventName == "ScopeDisposed");

            Assert.Equal(1, GetProperty<int>(scopeDisposedEvent, "scopedServicesResolved"));
            Assert.Equal(1, GetProperty<int>(scopeDisposedEvent, "disposableServices"));
            Assert.Equal(5, scopeDisposedEvent.EventId);
        }

        [Fact]
        public void EmitsServiceRealizationFailedEvent()
        {
            var exception = new Exception("Test error.");
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            int hashCode = serviceProvider.GetHashCode();
            DependencyInjectionEventSource.Log.ServiceRealizationFailed(exception, hashCode);

            var eventName = nameof(DependencyInjectionEventSource.Log.ServiceRealizationFailed);
            var serviceRealizationFailedEvent = Listener.EventDataFor(serviceProvider).Single(e => e.EventName == eventName);

            Assert.Equal("System.Exception: Test error.", GetProperty<string>(serviceRealizationFailedEvent, "exceptionMessage"));
            Assert.Equal(hashCode, GetProperty<int>(serviceRealizationFailedEvent, "serviceProviderHashCode"));
            Assert.Equal(6, serviceRealizationFailedEvent.EventId);
        }

        [Fact]
        public void EmitsServiceProviderBuilt()
        {
            ServiceCollection serviceCollection = new();
            FakeDisposeCallback fakeDisposeCallback = new();
            serviceCollection.AddSingleton(fakeDisposeCallback);
            serviceCollection.AddTransient<IFakeOuterService, FakeDisposableCallbackOuterService>();
            serviceCollection.AddSingleton<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddSingleton<IFakeMultipleService>(provider => new FakeDisposableCallbackInnerService(fakeDisposeCallback));
            serviceCollection.AddScoped<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddTransient<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddSingleton<IFakeService, FakeDisposableCallbackInnerService>();
            serviceCollection.AddScoped(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            serviceCollection.AddTransient<IFakeOpenGenericService<PocoClass>, FakeOpenGenericService<PocoClass>>();

            using ServiceProvider provider = serviceCollection.BuildServiceProvider();

            EventWrittenEventArgs serviceProviderBuiltEvent = Listener.EventDataFor(provider).Single(e => e.EventName == "ServiceProviderBuilt");
            GetProperty<int>(serviceProviderBuiltEvent, "serviceProviderHashCode"); // assert hashcode exists as an int
            Assert.Equal(4, GetProperty<int>(serviceProviderBuiltEvent, "singletonServices"));
            Assert.Equal(2, GetProperty<int>(serviceProviderBuiltEvent, "scopedServices"));
            Assert.Equal(3, GetProperty<int>(serviceProviderBuiltEvent, "transientServices"));
            Assert.Equal(1, GetProperty<int>(serviceProviderBuiltEvent, "closedGenericsServices"));
            Assert.Equal(1, GetProperty<int>(serviceProviderBuiltEvent, "openGenericsServices"));
            Assert.Equal(7, serviceProviderBuiltEvent.EventId);

            EventWrittenEventArgs serviceProviderDescriptorsEvent = Listener.EventDataFor(provider).Single(e => e.EventName == "ServiceProviderDescriptors");
            Assert.Equal(
                string.Join(Environment.NewLine,
                "{",
                "  \"descriptors\": [",
                "    {",
                "      \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback\",",
                "      \"lifetime\": \"Singleton\",",
                "      \"implementationInstance\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposeCallback (instance)\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeOuterService\",",
                "      \"lifetime\": \"Transient\",",
                "      \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackOuterService\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "      \"lifetime\": \"Singleton\",",
                "      \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "      \"lifetime\": \"Singleton\",",
                "      \"implementationFactory\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService <EmitsServiceProviderBuilt>b__0(System.IServiceProvider)\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "      \"lifetime\": \"Scoped\",",
                "      \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeMultipleService\",",
                "      \"lifetime\": \"Transient\",",
                "      \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeService\",",
                "      \"lifetime\": \"Singleton\",",
                "      \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeDisposableCallbackInnerService\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeOpenGenericService`1[TValue]\",",
                "      \"lifetime\": \"Scoped\",",
                "      \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeOpenGenericService`1[TVal]\"",
                "    },",
                "    {",
                "      \"serviceType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.IFakeOpenGenericService`1[Microsoft.Extensions.DependencyInjection.Specification.Fakes.PocoClass]\",",
                "      \"lifetime\": \"Transient\",",
                "      \"implementationType\": \"Microsoft.Extensions.DependencyInjection.Specification.Fakes.FakeOpenGenericService`1[Microsoft.Extensions.DependencyInjection.Specification.Fakes.PocoClass]\"",
                "    }",
                "  ]",
                "}"),
                JObject.Parse(GetProperty<string>(serviceProviderDescriptorsEvent, "descriptors")).ToString());

            GetProperty<int>(serviceProviderDescriptorsEvent, "serviceProviderHashCode"); // assert hashcode exists as an int
            Assert.Equal(0, GetProperty<int>(serviceProviderDescriptorsEvent, "chunkIndex"));
            Assert.Equal(1, GetProperty<int>(serviceProviderDescriptorsEvent, "chunkCount"));
            Assert.Equal(8, serviceProviderDescriptorsEvent.EventId);
        }

        /// <summary>
        /// Verifies that when an EventListener is enabled after the ServiceProvider has been built,
        /// the ServiceProviderBuilt events fire. This way users can get ServiceProvider info when
        /// attaching while the app is running.
        /// </summary>
        [Fact]
        public void EmitsServiceProviderBuiltOnAttach()
        {
            ServiceCollection serviceCollection = new();
            serviceCollection.AddSingleton(new FakeDisposeCallback());

            using ServiceProvider provider = serviceCollection.BuildServiceProvider();

            using var listener = new TestEventListener();
            listener.EnableEvents(DependencyInjectionEventSource.Log, EventLevel.Verbose);
            try
            {
                EventWrittenEventArgs serviceProviderBuiltEvent = listener.EventDataFor(provider).Single(e => e.EventName == "ServiceProviderBuilt");
                Assert.Equal(1, GetProperty<int>(serviceProviderBuiltEvent, "singletonServices"));

                EventWrittenEventArgs serviceProviderDescriptorsEvent = listener.EventDataFor(provider).Single(e => e.EventName == "ServiceProviderDescriptors");
                Assert.NotNull(JObject.Parse(GetProperty<string>(serviceProviderDescriptorsEvent, "descriptors")));
            }
            finally
            {
                listener.DisableEvents(DependencyInjectionEventSource.Log);
            }
        }

        private static bool TryGetProperty<T>(EventWrittenEventArgs data, string propName, out T result)
        {
            if (data.PayloadNames is { } names &&
                data.Payload is { } payload &&
                names.IndexOf(propName) is int index and >= 0 &&
                payload[index] is T value)
            {
                result = value;
                return true;
            }

            result = default;
            return false;
        }

        private static T GetProperty<T>(EventWrittenEventArgs data, string propName) =>
            TryGetProperty(data, propName, out T result)
                ? result
                : throw new ArgumentException($"No property {propName} with type {typeof(T)} found.");

        public class TestEventListenerFixture : IDisposable
        {
            internal TestEventListener Listener { get; }

            public TestEventListenerFixture()
            {
                Listener = new();
                Listener.EnableEvents(DependencyInjectionEventSource.Log, EventLevel.Verbose);
            }

            public void Dispose()
            {
                Listener.DisableEvents(DependencyInjectionEventSource.Log);
                Listener.Dispose();
            }
        }

        internal class TestEventListener : EventListener
        {
            private volatile bool _disposed;
            private readonly ConcurrentQueue<EventWrittenEventArgs> _events = new ConcurrentQueue<EventWrittenEventArgs>();

            public IEnumerable<EventWrittenEventArgs> EventDataFor(IServiceProvider serviceProvider)
            {
                int hashCode = serviceProvider.GetHashCode();

                return _events.Where(e =>
                    e.PayloadNames?.IndexOf("serviceProviderHashCode") is { } index and >= 0 &&
                    e.Payload?[index] as int? == hashCode);
            }

            protected override void OnEventWritten(EventWrittenEventArgs eventData)
            {
                if (!_disposed)
                {
                    _events.Enqueue(eventData);
                }
            }

            public override void Dispose()
            {
                _disposed = true;
                base.Dispose();
            }
        }
    }
}
