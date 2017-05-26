using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine_GUI
{
    abstract class Statement
    {
        public string Identifier
        {
            get;

            set;
        }

        public abstract bool IsTrue
        {
            get;
        }

        public Statement(string identifier)
        {
            Identifier = identifier;
        }

        public abstract bool Contains(string ID);

        public abstract bool DependsOn(string ID);

        public override string ToString()
        {
            return this.Identifier + " is: " + this.IsTrue;
        }
    }
}
