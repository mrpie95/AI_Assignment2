using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine_GUI
{
    class Or : Statement
    {
        private Statement[] _statements;

        public static string Symbol
        {
            get
            {
                return "|";
            }
        }

        public override bool IsTrue
        {
            get
            {
                bool result = false;

                foreach (Statement b in _statements)
                {
                    result = b.IsTrue;

                    if (result)
                    {
                        break;
                    }
                }

                return result;
            }
        }

        public Or(Statement[] internalStatments) : base("")
        {
            _statements = internalStatments;

            int i = 0;

            while (i < _statements.Length)
            {
                if (i > 0)
                {
                    this.Identifier += Or.Symbol;
                }

                this.Identifier += _statements[i].Identifier;

                i += 1;
            }
        }

        public override bool Contains(string ID)
        {
            foreach (Statement s in _statements)
            {
                if(s.Contains(ID))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
