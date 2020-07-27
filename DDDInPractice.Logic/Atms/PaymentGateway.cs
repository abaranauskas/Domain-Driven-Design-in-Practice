using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDInPractice.Logic.Atms
{
    public class PaymentGateway //in proper isolated model. monain class should nor work with such gateways it is responsibility of application layer(i.e. contrioller or MVVM)
    {
        public void ChargePayment(decimal amount)
        {

        }
    }
}
