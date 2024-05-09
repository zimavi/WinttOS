using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOSTests
{
    [TestClass]
    public class UsersTests
    {
        [TestInitialize]
        public void Setup()
        {
            WinttOS.wSystem.WinttOS.UsersManager = new(null);
        }

        [TestCleanup]
        public void Cleanup()
        {
            WinttOS.wSystem.WinttOS.UsersManager = default;
        }


        [TestMethod]
        public void AddNewUserCommand()
        {
            var cmd = new WinttOS.wSystem.Shell.Commands.Users.UsersCommand(new string [] { "users" });

            var res = cmd.Execute(new List<string> { "--add", "test" });

            Assert.AreEqual(res.Info, WinttOS.wSystem.Shell.ReturnCode.OK);
        }
    }
}
