using DDDInPractice.Logic;
using DDDInPractice.Logic.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DDDInPractice.Tests
{
    public class TempTests
    {
        [Fact]
        public void Test()
        {
            SessionFactory.Init(@"Server=(localdb)\MSSQLLocalDB;Database=DddInPractice;Trusted_Connection=True");

            HeadOfficeInstance.Init();

            var office = HeadOfficeInstance.Instance;
        }
    }
}
