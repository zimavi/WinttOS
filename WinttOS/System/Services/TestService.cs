using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.System.Services
{
    public class TestService : IService
    {

        #region Fields
        public bool IsRunning => _isRunning;

        public ServiceStatus Status => _status;

        public string ErrorMessage => _errMsg;

        public string Name => _name;

        #endregion

        #region Variables

        private bool _isRunning;
        private ServiceStatus _status;
        private string _errMsg;
        private string _name;

        #endregion

        public TestService()
        {
            _isRunning = false;
            _status = ServiceStatus.OFF;
            _errMsg = string.Empty;
            _name = "test.service";
        }

        #region Method
        public void onServiceFinish()
        {
            _isRunning = false;
            WinttDebugger.Info("Test Service is not now working!", this);
        }

        public void onServiceStart()
        {
            _isRunning = true;
            WinttDebugger.Info("Test Service is now working!", this);
        }

        public void onServiceTick()
        {
            WinttDebugger.Info("Test Service's tick!", this);
        }
        #endregion
    }
}
