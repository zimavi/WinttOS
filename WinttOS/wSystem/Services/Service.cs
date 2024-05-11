using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Processing;

namespace WinttOS.wSystem.Services
{
    public abstract class Service : Process
    {
        public Service(string ProcessName) : this(ProcessName, ProcessName) { }

        public Service(string ProcessName, string ServiceName) : base(ProcessName, ProcessType.Service)
        {
            this.ServiceName = ServiceName;
        }

        public bool IsServiceRunning { get; private set; } = false;
        public ServiceStatus ServiceStatus { get; private set; } = ServiceStatus.no_data;
        public string ServiceErrorMessage { get; private set; } = null;
        public string ServiceName { get; private set; } = null;

        public override void Stop()
        {
            base.Stop();

            Logger.DoOSLog("[Info] Stopping service " + ServiceName + " (PID" + ProcessID + ")");

            OnServiceStop();

            IsServiceRunning = false;
            ServiceStatus = ServiceStatus.OFF;

        }

        public override void Start()
        {
            base.Start();

            Logger.DoOSLog("[Info] Starting service " + ServiceName + " (PID " + ProcessID + ")");

            OnServiceStart();

            IsServiceRunning = true;
            ServiceStatus = ServiceStatus.OK;
        }

        public virtual void OnServiceStart()
        { }
        public virtual void OnServiceStop()
        { }
        public abstract void OnServiceTick();

        public override void Update()
        {
            base.Update();

            ServiceStatus = ServiceStatus.PENDING;

            OnServiceTick();

            ServiceStatus = ServiceStatus.OK;
        }
    }
}
