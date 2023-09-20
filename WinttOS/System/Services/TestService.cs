using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.System.Services
{
    public class TestService : Service
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

        public TestService() : base("TestService", "test.service")
        {
            _isRunning = false;
            _status = ServiceStatus.OFF;
            _errMsg = string.Empty;
            _name = "test.service";
        }

        #region Method
        
        public override void ServiceTick()
        {
            
        }
        #endregion
    }
}
