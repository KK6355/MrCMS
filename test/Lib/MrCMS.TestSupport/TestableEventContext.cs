using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using MrCMS.Events;
using MrCMS.Services;

namespace MrCMS.TestSupport
{
    public class TestableEventContext : IEventContext
    {
        private readonly IEventContext _coreEventContext;
        private readonly IEventContext _fakeEventContext;
        public readonly HashSet<Type> DisabledTypes = new HashSet<Type>();
        public bool FakeNonCoreEvents = true;

        public TestableEventContext(IEventContext coreEventContext = null)
        {
            _fakeEventContext = A.Fake<IEventContext>();
            _coreEventContext = coreEventContext ?? _fakeEventContext;
        }

        public IEventContext FakeEventContext
        {
            get { return _fakeEventContext; }
        }

        public async Task Publish<TEvent, TArgs>(TArgs args) where TEvent : IEvent<TArgs>
        {
            if (typeof(ICoreEvent).IsAssignableFrom(typeof(TEvent)) || !FakeNonCoreEvents)
            {
                await _coreEventContext.Publish<TEvent, TArgs>(args);
            }
            else
            {
                if (!IsDisabled(typeof(TEvent)))
                    await _fakeEventContext.Publish<TEvent, TArgs>(args);
            }
        }

        public async Task Publish(Type eventType, object args)
        {
            if (typeof(ICoreEvent).IsAssignableFrom(eventType) || !FakeNonCoreEvents)
            {
                await _coreEventContext.Publish(eventType, args);
            }
            else
            {
                if (!IsDisabled(eventType))
                    await _fakeEventContext.Publish(eventType, args);
            }
        }

        public IDisposable Disable<T>()
        {
            return new TestableEventContextDisabler(_coreEventContext.Disable<T>(), this, typeof(T));
        }

        public IDisposable Disable(params Type[] types)
        {
            return new TestableEventContextDisabler(_coreEventContext.Disable(types), this, types);
        }

        public IDisposable DisableAll()
        {
            return new TestableEventContextDisabler(_coreEventContext.DisableAll(), this);
        }

        private bool IsDisabled(Type eventType)
        {
            return DisabledTypes.Any(type => type.IsAssignableFrom(eventType));
        }

        public class TestableEventContextDisabler : IDisposable
        {
            private readonly IDisposable _coreContextDisable;
            private readonly TestableEventContext _testableEventContext;
            private readonly HashSet<Type> _typesToDisable = new HashSet<Type>();

            public TestableEventContextDisabler(IDisposable coreContextDisable,
                TestableEventContext testableEventContext, params Type[] types)
            {
                _coreContextDisable = coreContextDisable;
                _testableEventContext = testableEventContext;

                foreach (Type type in types)
                {
                    if (_testableEventContext.DisabledTypes.Add(type))
                    {
                        _typesToDisable.Add(type);
                    }
                }
            }

            public void Dispose()
            {
                _coreContextDisable.Dispose();
                foreach (Type type in _typesToDisable)
                {
                    _testableEventContext.DisabledTypes.Remove(type);
                }
            }
        }
    }
}