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
        public bool IsRunning => isRunning;

        public ServiceStatus Status => status;

        public string ErrorMessage => errorMessage;

        #endregion

        #region Variables

        private bool isRunning;
        private ServiceStatus status;
        private string errorMessage;

        #endregion

        public TestService() : base("TestService", "test.service")
        {
            isRunning = false;
            status = ServiceStatus.OFF;
            errorMessage = string.Empty;
        }

        #region Method
        
        public override void ServiceTick()
        {
            
        }
        #endregion
    }
}
