using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDInPractice.Logic
{
    public class SlotMap : ClassMap<Slot>
    {
        public SlotMap()
        {
            Id(x => x.Id);
            Id(x => x.Position);

            Component(x => x.SnackPile, y =>
              {
                  y.Map(x => x.Quantity);
                  y.Map(x => x.Price);
                  y.References(x => x.Snack).Not.LazyLoad();
              });

            References(x => x.SnackMachine);
        }
    }
}
