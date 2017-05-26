using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine_GUI
{ 
    class KnowledgeBase
    {
        private List<Statement> _assertions = new List<Statement>();
        private List<Statement> _queries = new List<Statement>();
        private List<Statement> _universe = new List<Statement>();

        public static string Delimiter
        {
            get
            {
                return ";";
            }
        }

        public Statement[] Assertions
        {
            get
            {
                return _assertions.ToArray();
            }
        }

        public Statement[] Queries
        {
            get
            {
                return _queries.ToArray();
            }
        }

        public Statement[] Universe
        {
            get
            {
                return _universe.ToArray();
            }
        }

        public KnowledgeBase()
        {
        }

        public static string[] DelimitString(string input, string[] delimiters, string[] remove)
        {
            List<string> result = new List<string>();
            
            //current string being manipulated, for addition to result.
            List<char> snippet = new List<char>();

            int i = 0;
            while (i < input.Length)
            {
                bool delimitorFound = false;
                bool removeFound = false;

                //add the next character to the current snippet
                snippet.Add(input[i]);

                //loop: Start at the end of snippet and check if the string ends in a delimitor or remove.
                int j = 0;
                while (j < snippet.Count)
                {
                    if ((delimitorFound) || (removeFound))
                    {
                        break;
                    }

                    //put the characters in the right order
                    string tempStr = "";
                    //index from j before end of snippet
                    int k = snippet.Count - 1 - j;
                    while (k < snippet.Count)
                    {
                        tempStr += snippet[k];

                        k += 1;
                    }
                    
                    foreach (string s in delimiters)
                    {
                        if ((delimitorFound) || (removeFound))
                        {
                            break;
                        }

                        if (tempStr == s)
                        {
                            //remove delimitor found
                            snippet.RemoveRange( snippet.Count - tempStr.Length, tempStr.Length);
                            result.Add(new String(snippet.ToArray()));
                            snippet.Clear();
                            delimitorFound = true;
                        }
                    }
                    
                    foreach (string s in remove)
                    {
                        if ((delimitorFound) || (removeFound))
                        {
                            break;
                        }

                        if (tempStr == s)
                        {
                            //remove remove found
                            snippet.RemoveRange(snippet.Count - tempStr.Length, tempStr.Length);
                            removeFound = true;
                        }
                    }
                    j += 1;
                }

                //save last snippet if delimitor not found
                if ((!delimitorFound) && (i == input.Length - 1))
                {
                    result.Add(new String(snippet.ToArray()));
                    snippet.Clear();
                    delimitorFound = true;
                }

                i += 1;
            }

            return result.ToArray();
        }

        public void Interpret(string input, AssertionEnum isAssertion)
        {
            List<string> stats = new List<string>(KnowledgeBase.DelimitString(input, new string[] { KnowledgeBase.Delimiter }, new string[] { " " }));

            if (isAssertion == AssertionEnum.Assertion)
            {
                foreach (string s in stats)
                {
                    Statement stat = this.GenerateStatement(s);

                    if ((stat as Variable) != null)
                    {
                        (stat as Variable).SetValue(true);
                        (stat as Variable).Defined = true;
                    }

                    bool found = false;

                    foreach (Statement statments in _assertions)
                    {
                        if (statments.Identifier == stat.Identifier)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        _assertions.Add(stat);
                    }
                }
            }

            else if(isAssertion == AssertionEnum.Query)
            {
                foreach (string s in stats)
                {
                    Statement stat = this.GenerateStatement(s);

                    //can't set query variables

                    bool found = false;

                    foreach (Statement statments in _queries)
                    {
                        if (statments.Identifier == stat.Identifier)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        _queries.Add(stat);
                    }
                }
            }
        }

        private Statement GenerateStatement(string input)
        {
            //The order these sections are placed implies the order of logical operations

            string[] deconstruction;
            Statement created = null;

            if (created == null)
            {
                //Implication section
                deconstruction = KnowledgeBase.DelimitString(input, new string[] { Implication.Symbol }, new string[] { });

                //if 1 string is returned the string did not contain the delimitor
                if (deconstruction.Length > 1)
                {
                    if (deconstruction.Length > 2)
                    {
                        throw new Exception("Implication format failure");
                    }

                    List<Statement> localWorld = new List<Statement>();
                    foreach (string s in deconstruction)
                    {
                        localWorld.Add(this.GenerateStatement(s));
                    }

                    created = new Implication(localWorld[0], localWorld[1]);
                }
            }

            if (created == null)
            {
                //And section
                deconstruction = KnowledgeBase.DelimitString(input, new string[] { And.Symbol }, new string[] { });

                if (deconstruction.Length > 1)
                {
                    List<Statement> localWorld = new List<Statement>();
                    foreach (string s in deconstruction)
                    {
                        localWorld.Add(this.GenerateStatement(s));
                    }

                    created = new And(localWorld.ToArray());
                }
            }

            if (created == null)
            {
                //Or section
                deconstruction = KnowledgeBase.DelimitString(input, new string[] { Or.Symbol }, new string[] { });

                if (deconstruction.Length > 1)
                {
                    List<Statement> localWorld = new List<Statement>();
                    foreach (string s in deconstruction)
                    {
                        localWorld.Add(this.GenerateStatement(s));
                    }

                    created = new Or(localWorld.ToArray());
                }
            }

            //Variables section

            if (created == null)
            {
                created = new Variable(input, false);
            }

            //search for statements with the same identifier

            if (created == null)
            {
                throw new Exception(input + " not understood");
            }

            bool found = false;

            foreach (Statement s in _universe)
            {
                if (s.Identifier == created.Identifier)
                {
                    created = s;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                _universe.Add(created);
            }

            return created;
        }

        public bool CheckConsistency()
        {
            bool result = true;

            int i = 0;
            while(i < _universe.Count)
            {
                int j = i + 1;
                while(j < _universe.Count)
                {
                    if (_universe[i].Identifier == _universe[j].Identifier)
                    {
                        Console.WriteLine(_universe[i].Identifier + " found at [" + i + ", " + j + "]");
                        result = false;
                        return result;
                    }
                    j += 1;
                }

                i += 1;
            }

            return result;
        }
    }
}
