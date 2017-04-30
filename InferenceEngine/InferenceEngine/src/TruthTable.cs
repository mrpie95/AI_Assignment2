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
        private List<bool[]> _values = new List<bool[]>();
        private int _numberRows;

        public TruthTable(Statement[] labels)
        {
            foreach (Statement s in labels)
            {
                _labels.Add(s);
                _values.Add(new bool[] { });
            }

            this.OrderAssertions();
            this.OrderDependancies();

            this.Populate();
        }
        
        public void Populate()
        {
            _values.Clear();

            _numberRows = 1;
            foreach (Statement s in _labels)
            {
                //solve for number of rows
                if ((s as Assertion) != null)
                {
                    if (!(s as Assertion).Defined)
                    {
                        _numberRows *= 2;
                    }
                }
            }

            int i = 0;
            while (i < _labels.Count)
            {
                _values.Add(new bool[_numberRows]);
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

                if (((_labels[i] as Assertion) != null))
                {
                    Assertion a = (_labels[i] as Assertion);

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
                    Assertion a = (_labels[j] as Assertion);

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
                while (j < _labels.Count)
                {
                    if ((_labels[j] as Assertion) == null)
                    {
                        _values[j][i] = _labels[j].IsTrue;
                    }

                    j += 1;
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
                int j = 0;
                while(j < _labels.Count)
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

                i += 1;
            }
        }

        public void OrderAssertions()
        {
            int i = 0;
            while (i < _labels.Count)
            {
                bool repeat = false;
                int j = i + 1;

                while (j < _labels.Count)
                {
                    if ((_labels[i] as Assertion) != null)
                    {
                        if ((_labels[i] as Assertion).Defined)
                        {
                            repeat = false;
                            break;
                        }

                        else
                        {
                            if ((_labels[j] as Assertion) != null)
                            {
                                if ((_labels[j] as Assertion).Defined)
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
                                if ((_labels[j] as Assertion) != null)
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
                        if ((_labels[j] as Assertion) !=  null)
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

            bool[] tempB = _values[i];
            _values[i] = _values[j];
            _values[j] = tempB;
        }       
    }
}
