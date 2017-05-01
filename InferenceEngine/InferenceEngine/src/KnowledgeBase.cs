using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
{ 
    class KnowledgeBase
    {
        private List<Statement> _world = new List<Statement>();

        public static string Delimiter
        {
            get
            {
                return ";";
            }
        }

        public Statement[] World
        {
            get
            {
                return _world.ToArray();
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

        public void Interpret(string input)
        {
            List<string> stats = new List<string>(KnowledgeBase.DelimitString(input, new string[] { KnowledgeBase.Delimiter }, new string[] { " " }));

            foreach (string s in stats)
            {
                this.GenerateStatement(s, 0);
            }
        }

        private Statement GenerateStatement(string input, int depth)
        {
            //The order these sections are placed implies the order of logical operations

            string[] deconstruction;

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
                    localWorld.Add(this.GenerateStatement(s, depth + 1));
                }

                Statement created = new Implication(localWorld[0], localWorld[1]);

                if (depth == 0)
                {
                    _world.Add(created);
                }

                return created;
            }

            //And section
            deconstruction = KnowledgeBase.DelimitString(input, new string[] { And.Symbol }, new string[] { });

            if (deconstruction.Length > 1)
            {
                List<Statement> localWorld = new List<Statement>();
                foreach (string s in deconstruction)
                {
                    localWorld.Add(this.GenerateStatement(s, depth + 1));
                }

                Statement created = new And(localWorld.ToArray());

                if (depth == 0)
                {
                    _world.Add(created);
                }

                return created;
            }

            //Or section
            deconstruction = KnowledgeBase.DelimitString(input, new string[] { Or.Symbol }, new string[] { });

            if (deconstruction.Length > 1)
            {
                List<Statement> localWorld = new List<Statement>();
                foreach (string s in deconstruction)
                {
                    localWorld.Add(this.GenerateStatement(s, depth + 1));
                }

                Statement created = new Or(localWorld.ToArray());

                if (depth == 0)
                {
                    _world.Add(created);
                }

                return created;
            }

            //Variables section
            //Default
            //all variables live in the _world

            //search for variables of the same name

            foreach (Statement s in _world)
            {
                if ((s as Variable) != null)
                {
                    Variable a = (s as Variable);

                    if (a.Identifier == input)
                    {
                        if (depth == 0)
                        {
                            a.SetValue(true);
                            a.Defined = true;
                        }

                        return a;
                    }
                }
            }

            // if the variable dosn't yet exist

            Variable asserted = new Variable(input, false);

            _world.Add(asserted);

            if (depth == 0)
             {
                 asserted.SetValue(true);
                 asserted.Defined = true;
             }

             return asserted;
        }

        public bool CheckConsistency()
        {
            bool result = true;

            int i = 0;
            while(i < _world.Count)
            {
                int j = i + 1;
                while(j < _world.Count)
                {
                    if (_world[i].Identifier == _world[j].Identifier)
                    {
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
