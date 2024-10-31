namespace WinttOS.wSystem.Services
{
    internal class TestService : Service
    {
        public TestService() : base("testservice", "test.service")
        {
        }

        public override void OnServiceTick()
        {
            
        }
    }
}
