﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Services
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
