using System;
using System.Collections.Generic;

namespace WinttOS.wSystem.Scheduling
{
    using PS = wAPI.PrivilegesSystem;

    public class TaskScheduler
    {

        //private List<sTask> _startupTasks = new();
        //private List<sTask> _rapidTasks = new();
        private List<sTask> _shutdownTasks;


        public TaskScheduler()
        {
            _shutdownTasks = new List<sTask>();
        }

        public void ScheduleTask(sTask task)
        {
            switch (task.Point)
            {
                case SchedulePoint.SYS_SHUTDOWN:
                    _shutdownTasks.Add(task);
                    break;

                default:
                    throw new NotImplementedException(task.Point.ToString());
            }
        }

        internal void CallShutdownSchedule()
        {
            if (_shutdownTasks.Count == 0)
                return;

            var currSet = WinttOS.UsersManager.CurrentUser.UserAccess.PrivilegeSet;
            WinttOS.CurrentExecutionSet = currSet;

            foreach(sTask task in _shutdownTasks)
            {
                if (task.NeedHighPrivilege)
                {
                    WinttOS.CurrentExecutionSet = PS.PrivilegesSet.HIGHEST;

                    task.Callback();

                    WinttOS.CurrentExecutionSet = currSet;
                }
                else
                {
                    task.Callback();
                }
            }

            WinttOS.CurrentExecutionSet = PS.PrivilegesSet.HIGHEST;
        }
    }
}
