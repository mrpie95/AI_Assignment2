using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine_GUI
{
    class Interpreter
    {
        private static String IF = "if";
        private static String AND = "and";
        private static String THEN = "then";
        private static String OR = "or";
        private static String FOR = "for";
        private static String LOGICAL_AND = "&";
        private static String LOGICAL_OR = "|";

        private String _input;

        public Interpreter(String _input)
        {
            this._input = _input;
        }

        public String Input 
        {
        
            get {return _input;}

            set { _input = value; }

        }


        private String Validate()
        {
            
            int j;

            _input = _input.ToLower();

            char[] _inputArray = _input.Trim().ToCharArray();

            _input = "";

            //removes multiple spaces so the split function can turn split everything into a array
            for (int i = 0; i < (_inputArray.Length); i++)
            {
                if (_inputArray[i] == ' ')
                {
                    //counts the number of spaces
                    j = 1;

                    //leaves on space so that the String.split function can be used
                    if ((i + 1) < _inputArray.Length)
                    {
                        _input += _inputArray[i];

                        //works out if there is more spaces to skip
                        while (true)
                        {
                            if (((i + j) < _input.Length) && (_inputArray[i + j] == ' '))
                                j++;
                            else
                            {
                                i += j-1;
                                break;
                            }
                        }
                    }
                }
                else
                    _input += _inputArray[i];
            }

            return _input;
        }

        //returns a list of proccessed strings that can be used as statements to calculate the truthtable
        public List<String> ProcessInput()
        {
            List<String> _results = new List<String>();
            String _result = "";

            String[] _split_input = Validate().Split(' ');

            _results.Add(Validate()+";");

            int j;
           
            //used to calculate spaces between arguments 
            j = 2;

            if (Validate().Contains(IF) && (_split_input[0] != FOR))
            {
                _results.Clear();
                if ((_split_input[0] == IF) && (_split_input.Length >= 4))
                {
                    //checks for interpreter reserved words
                    if ((_split_input[j - 1] != OR) && (_split_input[j - 1] != AND) && (_split_input[j - 1] != THEN) && (_split_input[j - 1] != IF))
                    {
                        _result = _split_input[j - 1];
                    }
                    else
                    {
                        _results.Clear();
                        _results.Add("#ERROR: [" + _split_input[j + 1].ToUpper() + "] is a interperter reserved word please use a different name");
                        return _results;
                    }

                    while (true)
                    {
                        if (j > _split_input.Length)
                        {
                            break;
                        }
                        else if (_split_input[j] == AND)
                        {
                            //checks for interpreter reserved words
                            if ((_split_input[j + 1] != OR) && (_split_input[j + 1] != AND))
                            {
                                if (_result != "")
                                    _result += LOGICAL_AND;

                                _result += _split_input[j + 1];
                                j += 2;
                            }
                            else
                            {
                                _results.Clear();
                                _results.Add("#ERROR: [" + _split_input[j + 1].ToUpper() + "] is a interperter reserved word please use a different name");
                                break;
                            }
                        }
                        else if (_split_input[j] == OR)
                        {
                            //checks for interpreter reserved words
                            if ((_split_input[j + 1] != OR) && (_split_input[j + 1] != AND))
                            {
                                _result += "=>" + _split_input[_split_input.Length - 1] + ";";
                                //_result = _result.Remove(0, 1);
                                _results.Add(_result);

                                _result = "";

                                _result += _split_input[j + 1];
                                j += 2;
                            }
                            else
                            {
                                _results.Clear();
                                _results.Add("#ERROR: [" + _split_input[j + 1].ToUpper() + "] is a interperter reserved word please use a different name");
                                break;
                            }
                        }
                        else if (_split_input[j] == THEN)
                        {
                            _result += "=>" + _split_input[j + 1] + ";";
                            _results.Add(_result);
                            break;
                        }
                        else
                        {
                            _results.Clear();
                            _results.Add("#ERROR: A critical error happened... you weren't suppose to be able to trigger this....");
                            break;
                        }
                    }
                }
            }


            /*else if ((Validate().Contains(FOR)))
            {
                int _for_count = (_input.Length - _input.Replace(FOR, "").Length) / (FOR.Length);
                List<FOR> _forStatements = new List<FOR>();

                for (int i = 1; i <= _for_count+1; i+=2)
                {

                    char[] _for_statement = _split_input[i].ToArray();
                    int _name_length = 0;
                    String temp = "";
                    FOR tempFOR = new FOR();

                    for (j = 0; j < (_for_statement.Length); j++)
                    {
                        if (_for_statement[j] == '(')
                        {
                            tempFOR.Name = temp;
                            temp = "";
                        }

                        else if (_for_statement[j] == ',')
                        {
                            tempFOR.Size1 = Int32.Parse(temp);
                            temp = "";
                        }
                    
                        else if (_for_statement[j] == ')')
                        {
                            tempFOR.Size2 = Int32.Parse(temp);
                            temp = "";
                        }
                        else
                            temp += _for_statement[j];
                    }

                    _forStatements.Add(tempFOR);
                }

                foreach (FOR f in _forStatements)
                    Console.WriteLine(f.Name);
            }*/
           

            return _results;
        }

    }
}
