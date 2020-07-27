using NHibernate.Proxy;
using Remotion.Linq.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DDDInPractice.Logic
{
    public class SnackMachine : AggregateRoot  //cuz of NHibernate cant be sealed
    {
        public SnackMachine()
        {
            MoneyInside = Money.None;
            MoneyInTransaction = 0;
            Slots = new List<Slot>
            {
                new Slot(this, 1),
                new Slot(this, 2),
                new Slot(this, 3)
            };
        }

        public virtual Money MoneyInside { get; protected set; }   //cuz of NHibernate
        public virtual decimal MoneyInTransaction { get; protected set; }
        protected virtual IList<Slot> Slots { get; set; }

        public virtual void InsertMoney(Money money)
        {
            Money[] coinsAndNotes = { Money.Cent, Money.TenCent, Money.Quarter, Money.Dollar, Money.FiveDollar, Money.TwentyDollar };
            if (!coinsAndNotes.Contains(money))
                throw new InvalidOperationException();

            MoneyInTransaction += money.Amount;
            MoneyInside += money;
        }

        public virtual void ReturnMoney()
        {
            Money moneyToReturn = MoneyInside.Allocate(MoneyInTransaction);
            MoneyInside -= moneyToReturn;
            MoneyInTransaction = 0;
        }

        public virtual string CanBySnack(int position)
        {
            var snackPile = GetSnackPile(position);
            if (snackPile.Quantity == 0)
                return "The pile is empty";

            if (MoneyInTransaction < snackPile.Price)
                return "Not eanough money";

            if (!MoneyInside.CanAllocate(MoneyInTransaction - snackPile.Price))
                return "Not eanough change";

            return string.Empty;
        }

        public virtual void BuySnack(int position)
        {
            if (CanBySnack(position) != string.Empty)
                throw new InvalidOperationException();

            var slot = GetSlot(position);          
            slot.SnackPile = slot.SnackPile.SubtractOne();
            var change = MoneyInside.Allocate(MoneyInTransaction - slot.SnackPile.Price);

            MoneyInside -= change;
            MoneyInTransaction = 0;
        }

        public virtual void LoadSnacks(int position, SnackPile snackPile)
        {
            GetSlot(position).SnackPile = snackPile;
        }

        public virtual SnackPile GetSnackPile(int position)
        {
            return GetSlot(position).SnackPile;
        }

        private Slot GetSlot(int position)
        {
            return Slots.Single(x => x.Position == position);
        }

        public virtual void LoadMoney(Money monay)
        {
            MoneyInside += monay;
        }
    }
}
