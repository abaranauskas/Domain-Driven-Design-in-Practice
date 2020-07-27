using DDDInPractice.Logic.Common;
using System;

namespace DDDInPractice.Logic.Atms
{
    public class Atm : AggregateRoot
    {
        private const decimal CommissionRate = 0.01m;

        public virtual Money MoneyInside { get; set; } = Money.None;
        public virtual decimal MoneyCharged { get; set; }

        public virtual string CanTakeMoney(decimal amount)
        {
            if (amount <= 0) return "Invalida amount";
            if (MoneyInside.Amount < amount) return "Not enough money";
            if (!MoneyInside.CanAllocate(amount)) return "Not enough change";

            return string.Empty;
        }

        public virtual void TakeMoney(decimal amount)
        {
            var output = MoneyInside.Allocate(amount);
            MoneyInside -= output;


            MoneyCharged += CalculateCommissionWithRate(amount);
            
            AddDomainEvent(new BalanceChangedEvent(MoneyCharged));
            //DomainEvents.Raise(new BalanceChangedEvent(MoneyCharged));
        }

        public virtual decimal CalculateCommissionWithRate(decimal amount)
        {
            var commission = amount * CommissionRate;
            decimal lessThanCent = commission % 0.01m;

            if (lessThanCent > 0)
            {
                commission = commission - lessThanCent + 0.01m;
            }

            return amount + commission;
        }

        public virtual void LoadMoney(Money money)
        {
            MoneyInside += money;
        }
    }
}
