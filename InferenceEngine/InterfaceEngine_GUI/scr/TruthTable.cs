using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine_GUI
{
    class TruthTable
    {
        private List<Statement> _assertions = new List<Statement>();
        private List<List<bool>> _values = new List<List<bool>>();
        private int _numberRows;

        public TruthTable(Statement[] labels)
        {
            foreach (Statement s in labels)
            {
                _assertions.Add(s);
                _values.Add(new List<bool> ());
            }

            this.OrderVariables();
            this.OrderDependancies();

            this.Populate();
        }
        
        public void Populate()
        {
            _values.Clear();

            _numberRows = 1;
            foreach (Statement s in _assertions)
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
            while (i < _assertions.Count)
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

            //booleans created before here

            //Assertions
            int flipDepth = 1;

            i = _assertions.Count - 1;

            while(i >= 0)
            {
                bool setTo = false;
                bool increaseFlipSize = false;

                if (((_assertions[i] as Variable) != null))
                {
                    Variable a = (_assertions[i] as Variable);

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
                while(j < _assertions.Count)
                {
                    Variable a = (_assertions[j] as Variable);

                    if ( a != null)
                    {
                        if (!a.Defined)
                        {
                            a.SetValue(_values[j][i]);
                        }
                    }

                    j += 1;
                }

                //Assertions set
                j = 0;
                while (j < _assertions.Count)
                {
                    if ((_assertions[j] as Variable) == null)
                    {
                        _values[j][i] = _assertions[j].IsTrue;
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
            while (i < _assertions.Count)            
            {
                if ((_assertions[i] as Variable) == null)
                {
                    int j = 0;
                    while (j < _numberRows)
                    {
                        if (!_values[i][j])
                        {
                            this.RemoveRow(j);
                        }

                        else
                        {
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
            while(i < _assertions.Count)
            {
                Console.Write("[");
                Console.Write(_assertions[i].Identifier);
                Console.Write("]");

                i += 1;
            }

            Console.WriteLine();

            i = 0;
            while (i < _numberRows)
            {
                int j = 0;
                while(j < _assertions.Count)
                {
                    int leftBuffer = 0, rightBuffer = 0;

                    int tempSize = _assertions[j].Identifier.Length - 1;

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

                i += 1;
            }
        }

        public void OrderVariables()
        {
            int i = 0;
            while (i < _assertions.Count)
            {
                bool repeat = false;
                int j = i + 1;

                while (j < _assertions.Count)
                {
                    if ((_assertions[i] as Variable) != null)
                    {
                        if ((_assertions[i] as Variable).Defined)
                        {
                            repeat = false;
                            break;
                        }

                        else
                        {
                            if ((_assertions[j] as Variable) != null)
                            {
                                if ((_assertions[j] as Variable).Defined)
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
                                if ((_assertions[j] as Variable) != null)
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
                        if ((_assertions[j] as Variable) !=  null)
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
            while (i < _assertions.Count)
            {
                bool repeat = false;
                int j = i + 1;

                while (j < _assertions.Count)
                {
                    if (_assertions[i].Contains(_assertions[j].Identifier))
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
            Statement tempS = _assertions[i];
            _assertions[i] = _assertions[j];
            _assertions[j] = tempS;

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

        public Result Query(string theQuery)
        {
            int i = 0;
            while(i < _assertions.Count)
            {
                if (_assertions[i].Identifier == theQuery)
                {
                    bool falseFound = false, trueFound = false;
                    Result result;

                    foreach (bool b in _values[i])
                    {
                        if (b)
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

                    if (trueFound && falseFound)
                    {
                        result = (Result)(1);
                    }

                    else if (trueFound)
                    {
                        result = (Result)(0);
                    }

                    else
                    {
                        result = (Result)(2);
                    }

                    return result;
                }

                i += 1;
            }
            return (Result)(1);//unknown
        }
    }
}
