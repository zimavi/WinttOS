namespace WinttOS.wSystem.Services
{
    public interface IServiceManager
    {
        public void AddService(Service service);
        public void StartService(string serviceName);
        public void FinishService(string serviceName);

        /// <returns><see cref="ServiceStatus"/> and <see cref="string"/> with error msg if <see cref="ServiceStatus"/> returns ERROR</returns>
        public (ServiceStatus, string) GetServiceStatus(string serviceName);
        public void RestartService(string serviceName);
    }
}
