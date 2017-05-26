using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine_GUI
{
    class Variable : Statement
    {
        private bool _value;

        public override bool IsTrue
        {
            get
            {
                return _value;
            }
        }

        public bool Defined
        {
            get;

            set;
        }

        public Variable(string identifier, bool initialAssertion) : base(identifier)
        {
            _value = initialAssertion;
            this.Defined = false;
        }

        public bool SetValue(bool input)
        {
            //if defined don't overwrite
            if (!Defined)
            { 
                _value = input;
                return true;
            }

            return false;
        }

        public override bool Contains(string ID)
        {
            if (ID == this.Identifier)
            {

            }

            return false;
        }

        public override bool DependsOn(string ID)
        {
            return false;
        }
    }
}
