namespace WinttOS.wSystem.wAPI.Events
{
    public interface ICancelable : IEvent
    {
        public bool IsCancelled { get; set; }
    }
}
