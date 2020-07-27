using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDInPractice.Logic
{
    public class Slot : Entity
    {
        protected Slot()
        {

        }

        public Slot(SnackMachine snackMachine, int position)
        {
            SnackMachine = snackMachine;
            Position = position;
            SnackPile = SnackPile.Empty;            
        }

        public virtual SnackPile SnackPile{ get; set; }       
        public virtual SnackMachine SnackMachine { get; protected set; }
        public virtual int Position { get; protected set; }
    }
}
