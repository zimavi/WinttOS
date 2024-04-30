using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.wAPI.PrivilegesSystem;

namespace WinttOS.wSystem.wAPI.Events
{
    public sealed class EventBus : IEventBus
    {
        private Dictionary<int, List<Action<IEvent>>> _eventRegistry;

        public EventBus() 
        {
            _eventRegistry = new();
        }

        public void Subscribe(int @event, Action<IEvent> handler)
        {
            if(!_eventRegistry.ContainsKey(@event))
            {
                _eventRegistry.Add(@event, new());
            }

            _eventRegistry[@event].Add(handler);
        }

        public void Post<T>(int id, T @event)
            where T : IEvent
        {
            if (!_eventRegistry.ContainsKey(id))
                return;

            if (!SystemEvents.FromValue(id).IsNull())
            {
                if(WinttOS.CurrentExecutionSet.Value < PrivilegesSet.HIGHEST.Value)
                {
                    return;
                }
            }

            foreach(var handler in _eventRegistry[id])
            {
                handler(@event);
            }
        } 
    }
}
