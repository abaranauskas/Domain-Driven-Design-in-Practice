using DDDInPractice.Logic.Common;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDInPractice.Logic.Utils
{
    public class EventListener :
        IPostInsertEventListener,
        IPostDeleteEventListener,
        IPostUpdateEventListener,
        IPostCollectionUpdateEventListener
    {
        public void OnPostDelete(PostDeleteEvent @event)
        {
            DispatchEvent(@event.Entity as AggregateRoot);
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            DispatchEvent(@event.Entity as AggregateRoot);
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            DispatchEvent(@event.Entity as AggregateRoot);
        }        

        public void OnPostUpdateCollection(PostCollectionUpdateEvent @event)
        {
            DispatchEvent(@event.AffectedOwnerOrNull as AggregateRoot);
        }

        private void DispatchEvent(AggregateRoot aggregateRoot)
        {
            foreach (IDomainEvent domainEvent in aggregateRoot.DomainEvents)
            {
                DomainEvents.Dispatch(domainEvent);
            }

            aggregateRoot.ClearEvents();
        }
    }
}
