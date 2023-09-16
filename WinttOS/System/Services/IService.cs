namespace WinttOS.System.Services
{
    public interface IService
    {
        public bool IsRunning { get; }
        public ServiceStatus Status { get; }
        public string ErrorMessage { get; }
        public string Name { get; }
        public void onServiceStart();
        public void onServiceFinish();
        public void onServiceTick();
    }
}
