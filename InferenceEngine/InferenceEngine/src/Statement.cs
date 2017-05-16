using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
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

        //check recursively
        public abstract bool Contains(string ID);

        //check single layer
        public abstract bool ComprisedOf(string ID);

        //check single layer
        public abstract bool CausedBy(string ID);

        public override string ToString()
        {
            return this.Identifier + " is: " + this.IsTrue;
        }
    }
}
