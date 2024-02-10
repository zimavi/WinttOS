using System;

namespace WinttOS.wSystem.Scheduling
{
    public class sTask
    {
        public readonly Action Callback;

        public readonly SchedulePoint Point;

        public readonly DateTime Period;

        public readonly bool NeedHighPrivilege = false;


        public sTask(Action callback, SchedulePoint point, DateTime period) : this(callback, point, false, period)
        { }

        internal sTask(Action callback, SchedulePoint point, bool needRaisedPrivileges, DateTime period)
        {
            Callback = callback;
            Point = point;
            Period = period;
            NeedHighPrivilege = needRaisedPrivileges;
        }
    }
}
