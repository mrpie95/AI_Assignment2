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

        public Implication(Statement A, Statement B) : base(A.Identifier + Implication.Symbol + B.Identifier)
        {
            _statementA = A;
            _statementB = B;
        }

        public override bool Contains(string ID)
        {
            if (_statementA.Contains(ID))
            {
                return true;
            }

            if (_statementB.Contains(ID))
            {
                return true;
            }

            return false;
        }

        public override bool ComprisedOf(string ID)
        {

            if (_statementA.Identifier == ID)
            {
                return true;
            }

            if (_statementB.Identifier == ID)
            {
                return true;
            }

            return false;
        }

        public override bool CausedBy(string ID)
        {
            if (_statementA.Identifier == ID)
            {
                return true;
            }

            return false;
        }
    }
}
