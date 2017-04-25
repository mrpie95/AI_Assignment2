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
        private int _columnWidth;

        public TruthTable(Statement[] labels)
        {
            foreach (Statement s in labels)
            {
                _labels.Add(s);
            }
        }

        public void populate()
        {
            int depth = 0;
            foreach (Statement s in _labels)
            {
                if ((s as Assertion) != null)
                {
                    Assertion a = (s as Assertion);
                    if (a.Defined)
                    {
                        //only one permiatation exists 2^0
                        depth += 1;
                    }

                    else
                    {
                        //2^1
                        depth += 2;
                    }
                }

                int i = 0;
                while(i < _values.Count)
                {
                    _values[i] = new bool[depth];                    

                    i += 1;
                }

                i = _values.Count - 1;
                while (i >= 0)
                {
                    //gap between toggling
                    int gap = 1;
                    int j = 0;

                    if (((_labels[i] as Assertion) != null) && (_labels[i] as Assertion).Defined)
                    {
                        //pre defined assertions
                        j = 0;
                        while (j < depth)
                        {
                            _values[i][j] = _labels[i].IsTrue
                            j += 1;
                        }
                    }

                    else
                    {
                        //standard initial state

                        j = 0;
                        while (j < depth)
                        {
                            _values[i][j] = !_values[i][j - 1];

                            j += 1;
                        }
                    }

                    i -= 1;
                }

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

            i = 0;
            while (i < _labels.Count)
            {
                int j = 0;
                while(j < _values[i].Length)
                {
                    Console.Write("[");
                    if (_values[i][j])
                    {
                        Console.Write("1");
                    }

                    else
                    {
                        Console.Write("0");
                    }
                    Console.Write("]");
                    j += 1;
                }

                i += 1;
            }
        }
    }
}
