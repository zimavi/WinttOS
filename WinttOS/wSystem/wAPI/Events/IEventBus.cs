using System;

namespace WinttOS.wSystem.wAPI.Events
{
    public interface IEventBus
    {
        public void Subscribe(int @event, Action<IEvent> handler);
    }
}
