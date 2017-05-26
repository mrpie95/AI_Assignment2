using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
{
    class TruthTable
    {
        private List<Statement> _labels = new List<Statement>();
        private KnowledgeBase _kB;
        private List<List<bool>> _values = new List<List<bool>>();
        private List<bool> _valid = new List<bool>();
        private int _numberRows;

         public List<Statement> Statements
        {
            get { return _labels; }
        }

        public List<List<bool>> Assertions
        {
            get { return _values; }
        }

        public int Rows
        {
            get
            {
                return _numberRows;
            }
        }

        public bool[] Valid
        {
            get
            {
                return _valid.ToArray();
            }
        }

        public TruthTable(KnowledgeBase kB)
        {
            _kB = kB;
            
            //add in assertions specifically
            foreach (Statement s in _kB.Assertions)
            {
                bool found = false;
                foreach (Statement stat in _labels)
                {
                    if (stat.Identifier == s.Identifier)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _labels.Add(s);
                    _values.Add(new List<bool>());
                }
            }

            //load in queries specifically
            foreach (Statement s in kB.Queries)
            {
                bool found = false;
                foreach (Statement stat in _labels)
                {
                    if (stat.Identifier == s.Identifier)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _labels.Add(s);
                    _values.Add(new List<bool>());
                }
            }
            
            foreach (Statement s in kB.Universe)
            {
                if ((s as Variable) != null)
                {
                    bool found = false;
                    foreach (Statement stat in _labels)
                    {
                        if (stat.Identifier == s.Identifier)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        _labels.Add(s);
                        _values.Add(new List<bool>());
                    }
                }
            }

            this.OrderVariables();
            this.OrderDependancies();

            this.Populate();

            this.Clean();
        }
        
        public void Populate()
        {
            _values.Clear();

            _numberRows = 1;
            foreach (Statement s in _labels)
            {
                //solve for number of rows
                if ((s as Variable) != null)
                {
                    if (!(s as Variable).Defined)
                    {
                        _numberRows *= 2;
                    }
                }
            }

            int i = 0;
            while (i < _labels.Count)
            {
                _values.Add(new List<bool> (_numberRows));

                int j = 0;
                while (j < _numberRows)
                {
                    _values[i].Add(false);
                    j += 1;
                }

                i += 1;
            }

            i = 0;
            while (i < _numberRows)
            {
                _valid.Add(true);

                i += 1;
            }

            //booleans created before here

            //Assertions
            int flipDepth = 1;

            i = _labels.Count - 1;

            while(i >= 0)
            {
                bool setTo = false;
                bool increaseFlipSize = false;

                if (((_labels[i] as Variable) != null))
                {
                    Variable a = (_labels[i] as Variable);

                    int j = 0;
                    while (j < _numberRows)
                    {
                        if (a.Defined)
                        {
                            _values[i][j] = a.IsTrue;
                        }

                        else
                        {
                            increaseFlipSize = true;
                            _values[i][j] = setTo;

                            if (j % flipDepth == flipDepth - 1)
                            {
                                setTo = !setTo;
                            }
                        }
                        j += 1;
                    }
                }

                if (increaseFlipSize)
                {
                    flipDepth *= 2;
                    increaseFlipSize = false;
                }

                i -= 1;
            }

            //Fill composites            
            i = 0;
            while (i < _numberRows)
            {
                int j = 0;
                while(j < _labels.Count)
                {
                    Variable a = (_labels[j] as Variable);

                    if ( a != null)
                    {
                        if (!a.Defined)
                        {
                            a.SetValue(_values[j][i]);
                        }
                    }

                    j += 1;
                }

                //Assertions have been set
                j = 0;
                while (j < _labels.Count)
                {
                    if ((_labels[j] as Variable) == null)
                    {
                        _values[j][i] = _labels[j].IsTrue;
                    }

                    j += 1;
                }

                i += 1;
            }
        }

        /// <summary>
        /// Remove rows which don't match the given assertions
        /// </summary>
        public void Clean()
        {
            int i = 0;
            while (i < _labels.Count)
            {
                if ((_labels[i] as Variable) == null)
                {
                    bool found = false;

                    int k = 0;
                    while (k < _kB.Assertions.Length)
                    {
                        if (_labels[i] == _kB.Assertions[k])
                        {
                            found = true;
                            break;
                        }

                        k += 1;
                    }

                    if (found)
                    {
                        int j = 0;
                        while (j < _numberRows)
                        {
                            if (!_values[i][j])
                            {

                                _valid[j] = false;
                            }

                            
                            j += 1;
                        }
                    }                    
                }

                i += 1;
            }
        }

        public void WriteTable()
        {
            int i = 0;
            while(i < _labels.Count)
            {
                Console.Write("[");
                Console.Write(_labels[i].Identifier);
                Console.Write("]");

                i += 1;
            }

            Console.WriteLine();

            i = 0;
            while (i < _numberRows)
            {
                if (_valid[i])
                {
                    int j = 0;
                    while (j < _labels.Count)
                    {
                        int leftBuffer = 0, rightBuffer = 0;

                        int tempSize = _labels[j].Identifier.Length - 1;

                        if (tempSize % 2 == 0)
                        {
                            leftBuffer = tempSize / 2;
                            rightBuffer = tempSize / 2;
                        }

                        else
                        {
                            leftBuffer = tempSize / 2 + 1; // left gets the left overs
                            rightBuffer = tempSize / 2;
                        }

                        Console.Write("[");

                        while (leftBuffer > 0)
                        {
                            Console.Write(" ");
                            leftBuffer -= 1;
                        }

                        if (_values[j][i])
                        {
                            Console.Write("1");
                        }

                        else
                        {
                            Console.Write("0");
                        }

                        while (rightBuffer > 0)
                        {
                            Console.Write(" ");
                            rightBuffer -= 1;
                        }

                        Console.Write("]");
                        j += 1;
                    }

                    Console.WriteLine();
                }

                i += 1;
            }
        }

        public void OrderVariables()
        {
            int i = 0;
            while (i < _labels.Count)
            {
                bool repeat = false;
                int j = i + 1;

                while (j < _labels.Count)
                {
                    if ((_labels[i] as Variable) != null)
                    {
                        if ((_labels[i] as Variable).Defined)
                        {
                            repeat = false;
                            break;
                        }

                        else
                        {
                            if ((_labels[j] as Variable) != null)
                            {
                                if ((_labels[j] as Variable).Defined)
                                {
                                    this.Swap(i, j);

                                    repeat = true;
                                    break;
                                }

                                else
                                {
                                    //do nothing, equal priority
                                }
                            }

                            else
                            {
                                if ((_labels[j] as Variable) != null)
                                {
                                    this.Swap(i, j);

                                    repeat = true;
                                    break;
                                }
                            }
                        }
                    }

                    else
                    {
                        if ((_labels[j] as Variable) !=  null)
                        {
                            this.Swap(i, j);

                            repeat = true;
                            break;
                        }
                    }

                    j += 1;
                }

                if (!repeat)
                {
                    i += 1;
                }
            }
        }

        public void OrderDependancies()
        {
            int i = 0;
            while (i < _labels.Count)
            {
                bool repeat = false;
                int j = i + 1;

                while (j < _labels.Count)
                {
                    if (_labels[i].Contains(_labels[j].Identifier))
                    {
                        this.Swap(i, j);

                        repeat = true;
                        break;
                    }

                    j += 1;
                }

                if (!repeat)
                {
                    i += 1;
                }
            }
        }

        public void Swap(int i, int j)
        {
            Statement tempS = _labels[i];
            _labels[i] = _labels[j];
            _labels[j] = tempS;

            List<bool> tempB = _values[i];
            _values[i] = _values[j];
            _values[j] = tempB;
        }

        public void RemoveRow(int index)
        {
            int i = 0;
            while (i < _values.Count)
            {
                _values[i].RemoveAt(index);

                i += 1;
            }

            _numberRows -= 1;
        }

        public void RemoveColumn(int index)
        {
            _labels.RemoveAt(index);

            _values.RemoveAt(index);
        }

        public Result Query(string theQuery)
        {
            int i = 0;
            while (i < _labels.Count)
            {
                if (_labels[i].Identifier == theQuery)
                {
                    bool falseFound = false, trueFound = false;
                    Result result;

                    int j = 0;
                    while(j < _values[i].Count)
                    {
                        if (_valid[j])
                        {
                            if (_values[i][j])
                            {
                                trueFound = true;
                            }

                            else
                            {
                                falseFound = true;
                            }

                            if (trueFound && falseFound)
                            {
                                break;
                            }
                        }

                        j += 1;
                    }

                    if (trueFound && falseFound)
                    {
                        result = (Result)(1);//unknown
                    }

                    else if (trueFound)
                    {
                        result = (Result)(0);//true
                    }

                    else
                    {
                        result = (Result)(2);//false
                    }

                    return result;
                }

                i += 1;
            }
            
            return (Result)(1);//unknown
        }

        public int ValidRows()
        {
            int result = 0;

            foreach (bool b in _valid)
            {
                if (b)
                {
                    result += 1;
                }
            }

            return result;
        }

        public string Solution()
        {
            string result = "";

            foreach (Statement s in _kB.Queries)
            {
                if (this.Query(s.Identifier) == Result.Valid)
                {
                    result += String.Format("YES: " + this.ValidRows() + "\n");
                }

                else
                {
                    result += String.Format("NO\n");
                }
            }

            return result;
        }
    }
}
