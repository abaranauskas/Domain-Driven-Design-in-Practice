using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDInPractice.Logic
{
    public sealed class Money : ValueObject<Money>
    {
        public static readonly Money None = new Money(0, 0, 0, 0, 0, 0);
        public static readonly Money Cent = new Money(1, 0, 0, 0, 0, 0);
        public static readonly Money TenCent = new Money(0, 1, 0, 0, 0, 0);
        public static readonly Money Quarter = new Money(0, 0, 1, 0, 0, 0);
        public static readonly Money Dollar = new Money(0, 0, 0, 1, 0, 0);
        public static readonly Money FiveDollar = new Money(0, 0, 0, 0, 1, 0);
        public static readonly Money TwentyDollar = new Money(0, 0, 0, 0, 0, 1);

        public int OneCentCount { get; }
        public int TenCentCount { get; }
        public int QuarterCount { get; }
        public int OneDollarCount { get; }
        public int FiveDollarCount { get; }
        public int TwentyDollarCount { get; }
        public decimal Amount =>
            OneCentCount * 0.01m +
            TenCentCount * 0.1m +
            QuarterCount * 0.25m +
            OneDollarCount +
            FiveDollarCount * 5 +
            TwentyDollarCount * 20;

        private Money() //for NHibernate
        {
        }

        public Money Allocate(decimal amount)
        {
            if (!CanAllocate(amount))
                throw new InvalidOperationException();

            return AllocateCore(amount);
        }

        private Money AllocateCore(decimal amount)
        {
            int twentyDollarCount = Math.Min((int)(amount / 20), TwentyDollarCount);
            amount = amount - twentyDollarCount * 20;

            int fiveDollarCount = Math.Min((int)(amount / 5), FiveDollarCount);
            amount = amount - fiveDollarCount * 5;

            int oneDollarCount = Math.Min((int)amount, OneDollarCount);
            amount = amount - oneDollarCount;

            int quarterCount = Math.Min((int)(amount / 0.25m), QuarterCount);
            amount = amount - quarterCount * 0.25m;

            int tenCentCount = Math.Min((int)(amount / 0.1m), TenCentCount);
            amount = amount - tenCentCount * 0.1m;

            int oneCentCount = Math.Min((int)(amount / 0.01m), OneCentCount);
            amount = amount - tenCentCount * 0.01m;

            return new Money(oneCentCount, tenCentCount, quarterCount, oneDollarCount, fiveDollarCount, twentyDollarCount);
        }

        public bool CanAllocate(decimal amount)
        {
            Money money = AllocateCore(amount);
            return money.Amount == amount;
        }

        public Money(
            int oneCentCount,
            int tenCentCount,
            int quarterCount,
            int oneDollarCount,
            int fiveDollarCount,
            int twentyDollarCount)
        {
            if (oneCentCount < 0)
                throw new InvalidOperationException(nameof(oneCentCount));
            if (tenCentCount < 0)
                throw new InvalidOperationException(nameof(tenCentCount));
            if (quarterCount < 0)
                throw new InvalidOperationException(nameof(quarterCount));
            if (oneDollarCount < 0)
                throw new InvalidOperationException(nameof(oneDollarCount));
            if (fiveDollarCount < 0)
                throw new InvalidOperationException(nameof(fiveDollarCount));
            if (twentyDollarCount < 0)
                throw new InvalidOperationException(nameof(twentyDollarCount));

            OneCentCount += oneCentCount;
            TenCentCount += tenCentCount;
            QuarterCount += quarterCount;
            OneDollarCount += oneDollarCount;
            FiveDollarCount += fiveDollarCount;
            TwentyDollarCount += twentyDollarCount;
        }

        public static Money operator +(Money money1, Money money2)
        {
            var sum = new Money(money1.OneCentCount + money2.OneCentCount,
                money1.TenCentCount + money2.TenCentCount,
                money1.QuarterCount + money2.QuarterCount,
                money1.OneDollarCount + money2.OneDollarCount,
                money1.FiveDollarCount + money2.FiveDollarCount,
                money1.TwentyDollarCount + money2.TwentyDollarCount);

            return sum;
        }

        public static Money operator -(Money money1, Money money2)
        {
            var sub = new Money(money1.OneCentCount - money2.OneCentCount,
                money1.TenCentCount - money2.TenCentCount,
                money1.QuarterCount - money2.QuarterCount,
                money1.OneDollarCount - money2.OneDollarCount,
                money1.FiveDollarCount - money2.FiveDollarCount,
                money1.TwentyDollarCount - money2.TwentyDollarCount);

            return sub;
        }

        public static Money operator *(Money money1, int multiplier)
        {
            var money = new Money(money1.OneCentCount * multiplier,
                money1.TenCentCount * multiplier,
                money1.QuarterCount * multiplier,
                money1.OneDollarCount * multiplier,
                money1.FiveDollarCount * multiplier,
                money1.TwentyDollarCount * multiplier);

            return money;
        }

        protected override bool EqualsCore(Money other)
        {
            return OneCentCount == other.OneCentCount &&
                   TenCentCount == other.TenCentCount &&
                   QuarterCount == other.QuarterCount &&
                   OneDollarCount == other.OneDollarCount &&
                   FiveDollarCount == other.FiveDollarCount &&
                   TwentyDollarCount == other.TwentyDollarCount;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hashCode = OneCentCount;
                hashCode = (hashCode * 397) ^ TenCentCount;
                hashCode = (hashCode * 397) ^ QuarterCount;
                hashCode = (hashCode * 397) ^ OneDollarCount;
                hashCode = (hashCode * 397) ^ FiveDollarCount;
                hashCode = (hashCode * 397) ^ TwentyDollarCount;
                return hashCode;
            }
        }

        public override string ToString()
        {
            if (Amount < 1)
            {
                return "C" + (Amount * 100).ToString("0");
            }

            return "$" + Amount.ToString("0.00");
        }
    }
}
