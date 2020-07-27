using System;
using DDDInPractice.Logic;
using FluentAssertions;
using Xunit;

namespace DDDInPractice.Tests
{
    public class MoneySpecs
    {
        [Fact]
        public void Sum_of_two_money_produces_correct_result()
        {
            //Arrange
            var money1 = new Money(1, 2, 3, 4, 5, 6);
            var money2 = new Money(1, 2, 3, 4, 5, 6);

            //Act
            var sum = money1 + money2;

            //Assert
            sum.OneCentCount.Should().Be(2);
            sum.TenCentCount.Should().Be(4);
            sum.QuarterCount.Should().Be(6);
            sum.OneDollarCount.Should().Be(8);
            sum.FiveDollarCount.Should().Be(10);
            sum.TwentyDollarCount.Should().Be(12);
        }

        [Fact]
        public void Two_money_instances_are_equal_if_they_contain_same_values()
        {
            //Arrange
            var money1 = new Money(1, 2, 3, 4, 5, 6);
            var money2 = new Money(1, 2, 3, 4, 5, 6);

            money1.Should().Be(money2);
            money1.GetHashCode().Should().Be(money2.GetHashCode());
        }

        [Fact]
        public void Two_money_instances_are__not_equal_if_they_contain_different_values()
        {
            //Arrange
            var dollar = new Money(0, 0, 0, 1, 0, 0);
            var hundredCents = new Money(100, 0, 0, 0, 0, 0);

            dollar.Should().NotBe(hundredCents);
            dollar.GetHashCode().Should().NotBe(hundredCents.GetHashCode());
        }

        [Theory]
        [InlineData(-1, 0, 0, 0, 0, 0)]
        [InlineData(0, -2, 0, 0, 0, 0)]
        [InlineData(0, 0, -3, 0, 0, 0)]
        [InlineData(0, 0, 0, -4, 0, 0)]
        [InlineData(0, 0, 0, 0, -5, 0)]
        [InlineData(0, 0, 0, 0, 0, -6)]
        public void Cannot_create_money_with_negative_values(int oneCentCount, int tenCentCount,
            int quarterCount, int oneDollarCount, int fiveDollarCount, int twentyDollarCount)
        {

            Action action = () => new Money(oneCentCount, tenCentCount, quarterCount,
                oneDollarCount, fiveDollarCount, twentyDollarCount);


            action.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 0, 0, 0, 0.01)]
        [InlineData(1, 2, 0, 0, 0, 0, 0.21)]
        [InlineData(1, 2, 3, 0, 0, 0, 0.96)]
        [InlineData(1, 2, 3, 4, 0, 0, 4.96)]
        [InlineData(1, 2, 3, 4, 5, 0, 29.96)]
        [InlineData(1, 2, 3, 4, 5, 6, 149.96)]
        [InlineData(11, 0, 0, 0, 0, 0, 0.11)]
        [InlineData(110, 0, 0, 0, 100, 0, 501.1)]
        public void Amount_calculated_correctly(int oneCentCount, int tenCentCount,
            int quarterCount, int oneDollarCount, int fiveDollarCount, int twentyDollarCount,
            decimal expectedAmount)
        {

            var money = new Money(oneCentCount, tenCentCount, quarterCount,
                oneDollarCount, fiveDollarCount, twentyDollarCount);


            money.Amount.Should().Be(expectedAmount);
        }

        [Fact]
        public void Subtraction_of_two_money_produces_correct_result()
        {
            //Arrange
            var money1 = new Money(2, 2, 3, 4, 5, 6);
            var money2 = new Money(1, 2, 3, 4, 5, 5);

            //Act
            var sum = money1 - money2;

            //Assert
            sum.OneCentCount.Should().Be(1);
            sum.TenCentCount.Should().Be(0);
            sum.QuarterCount.Should().Be(0);
            sum.OneDollarCount.Should().Be(0);
            sum.FiveDollarCount.Should().Be(0);
            sum.TwentyDollarCount.Should().Be(1);
        }

        [Fact]
        public void Cannot_subtract_more_than_exist()
        {
            //Arrange
            var money1 = new Money(1, 2, 3, 4, 5, 6);
            var money2 = new Money(1, 2, 3, 4, 5, 10);

            //Act
            Func<Money> func = () => money1 - money2;

            //Assert
            func.Should().Throw<InvalidOperationException>();
        }


        [Theory]
        [InlineData(1, 0, 0, 0, 0, 0, "C1")]
        [InlineData(0, 0, 0, 1, 0, 0, "$1.00")]
        [InlineData(1, 0, 0, 1, 0, 0, "$1.01")]
        [InlineData(0, 0, 2, 1, 0, 0, "$1.50")]
        public void To_string_should_return_amount_of_money(int oneCentCount, int tenCentCount,
            int quarterCount, int oneDollarCount, int fiveDollarCount, int twentyDollarCount,
            string expectedstring)
        {

            var money = new Money(oneCentCount, tenCentCount, quarterCount,
                oneDollarCount, fiveDollarCount, twentyDollarCount);


            money.ToString().Should().Be(expectedstring);
        }
    }
}
