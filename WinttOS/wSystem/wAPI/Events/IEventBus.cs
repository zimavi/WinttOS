using System;

namespace WinttOS.wSystem.wAPI.Events
{
    public interface IEventBus
    {
        public void Subscribe(int @event, Action<IEvent> handler);

        public void Post<T>(int id, T @event) where T : IEvent;
    }
}
