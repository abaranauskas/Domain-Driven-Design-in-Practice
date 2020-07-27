using DDDInPractice.Logic;

namespace DddInPractice.Logic.UI
{
    public partial class App
    {
        public App()
        {
            Initer.Init(@"Server=(localdb)\MSSQLLocalDB;Database=DddInPractice;Trusted_Connection=True");            
        }
    }
}
