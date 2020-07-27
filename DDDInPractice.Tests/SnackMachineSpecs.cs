using DDDInPractice.Logic;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DDDInPractice.Tests
{
    public class SnackMachineSpecs
    {
        [Fact]
        public void Return_money_empties_money_in_transaction()
        {
            var snackMachine = new SnackMachine();
            snackMachine.InsertMoney(Money.Dollar);

            snackMachine.ReturnMoney();

            snackMachine.MoneyInTransaction.Should().Be(0);
        }

        [Fact]
        public void Inserted_money_goes_to_money_in_transaction()
        {
            var snackMachine = new SnackMachine();
            snackMachine.InsertMoney(Money.Dollar);
            snackMachine.InsertMoney(Money.Cent);

            snackMachine.MoneyInTransaction.Should().Be(1.01m);
        }

        [Fact]
        public void Cannot_insert_more_than_one_coin_or_bill_at_the_time()
        {
            var snackMachine = new SnackMachine();

            var twoCents = Money.Cent + Money.Cent;

            Action action = () => snackMachine.InsertMoney(twoCents);

            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Buy_snacks_trades_inserted_money_for_snack()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 10, 1));
            snackMachine.InsertMoney(Money.Dollar);


            snackMachine.BuySnack(1);            

            snackMachine.MoneyInTransaction.Should().Be(0);
            snackMachine.MoneyInside.Amount.Should().Be(1);
            snackMachine.GetSnackPile(1).Quantity.Should().Be(9);
        }

        [Fact]
        public void Cant_make_purchase_if_no_snacks()
        {
            var machine = new SnackMachine();

            Action action = () => machine.BuySnack(1);

            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Cant_make_purchase_if_the_is_not_enough_money_inserted()
        {
            var machine = new SnackMachine();
            machine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 10, 2));
            machine.InsertMoney(Money.Dollar);

            Action action = () => machine.BuySnack(1);

            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Snack_machine_returns_money_with_highest_denomination_first()
        {
            var machine = new SnackMachine();
            machine.LoadMoney(Money.Dollar);

            machine.InsertMoney(Money.Quarter);
            machine.InsertMoney(Money.Quarter);
            machine.InsertMoney(Money.Quarter);
            machine.InsertMoney(Money.Quarter);

            machine.ReturnMoney();

            machine.MoneyInside.QuarterCount.Should().Be(4);
            machine.MoneyInside.OneDollarCount.Should().Be(0);
        }

        [Fact]
        public void After_purchase_change_is_returned()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 1, 0.5m));
            snackMachine.LoadMoney(Money.TenCent * 10);

            snackMachine.InsertMoney(Money.Dollar);
            snackMachine.BuySnack(1);

            snackMachine.MoneyInside.Amount.Should().Be(1.5m);
            snackMachine.MoneyInTransaction.Should().Be(0m);
        }

        [Fact]
        public void Cant_buy_snacks_if_not_enough_change()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 1, 0.5m));
            snackMachine.InsertMoney(Money.Dollar);

            Action action = () => snackMachine.BuySnack(1);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}
