using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDInPractice.Logic
{
    public class Snack : AggregateRoot
    {
        public static Snack None = new Snack(0, "None");
        public static Snack Chocolate = new Snack(1, "Chocolate");
        public static Snack Soda = new Snack(2, "Soda");
        public static Snack Gum = new Snack(3, "Gum");
        protected Snack() //just cuz of NHibernate
        {

        }
        
        private Snack(long id, string name)
        {
            Name = name;
        }

        public virtual string Name { get; protected set; } //protected and virtual, just cuz of NHibernate
    }
}
