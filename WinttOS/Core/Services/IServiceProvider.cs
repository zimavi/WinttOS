namespace WinttOS.Core.Services
{
    public interface IServiceProvider
    {
        public void AddService(IService service);
        public void StartService(string serviceName);
        public void FinishService(string serviceName);
        public void DoServiceProviderTick();

        /// <returns><see cref="ServiceStatus"/> and <see cref="string"/> with error msg if <see cref="ServiceStatus"/> returns ERROR</returns>
        public (ServiceStatus?, string) GetServiceStatus(string serviceName);
        public void RestartService(string serviceName);
    }
}
