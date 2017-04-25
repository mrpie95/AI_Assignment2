using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
{
    class Implication : Statement
    {
        private Statement _statementA, _statementB;

        public static string Symbol
        {
            get
            {
                return "=>";
            }
        }

        public override bool IsTrue
        {
            get
            {
                if (_statementA.IsTrue && !_statementB.IsTrue)
                {
                    return false;
                }

                return true;
            }
        }

        public Implication(Statement A, Statement B) : base(A.Identifier + " " + Implication.Symbol + " " + B.Identifier)
        {
            _statementA = A;
            _statementB = B;
        }
    }
}
