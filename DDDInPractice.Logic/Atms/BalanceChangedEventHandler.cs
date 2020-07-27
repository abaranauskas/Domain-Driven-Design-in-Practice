using DDDInPractice.Logic.Common;

namespace DDDInPractice.Logic.Atms
{
    public class BalanceChangedEventHandler : IHandler<BalanceChangedEvent>
    {
        public void Handle(BalanceChangedEvent domainEvent)
        {
            // case handler would be in different process(microservice)
            // in that case some messaging service should be used
            //EsbGateway.Instance.SendBalanceChangedMessage(domainEvent.Delta);
        }
    }
}
