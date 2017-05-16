using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
{
    class And : Statement
    {
        private Statement[] _statements;

        public static string Symbol
        {
            get
            {
                return "&";
            }
        }

        public override bool IsTrue
        {
            get
            {
                bool result = true;

                foreach (Statement b in _statements)
                {
                    result = result && b.IsTrue;

                    if (!result)
                    {
                        break;
                    }
                }

                return result;
            }
        }

        public Statement[] Stats
        {
            get
            {
                return _statements;
            }
        }

        public And(Statement[] internalStatments) : base("")
        {
            _statements = internalStatments;

            int i = 0;
            
            while(i < _statements.Length)
            {
                if (i > 0)
                {
                    this.Identifier += And.Symbol;
                }

                this.Identifier += _statements[i].Identifier;

                i += 1;
            }
        }

        public override bool Contains(string ID)
        {
            foreach (Statement s in _statements)
            {
                if (s.Contains(ID))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool ComprisedOf(string ID)
        {
            foreach (Statement s in _statements)
            {
                if (s.Identifier == ID)
                {
                    return true;
                }
            }

            return false;
        }

        public override bool CausedBy(string ID)
        {
            return this.Contains(ID);
        }
    }
}
