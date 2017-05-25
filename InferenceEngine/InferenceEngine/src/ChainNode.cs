using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
{
    class ChainNode
    {
        private List<ChainNode> _effect = new List<ChainNode>();
        private List<ChainNode> _cause = new List<ChainNode>();
        private Statement _stat;

        public bool Asserted
        {
            get;

            set;
        }

        public string Identifier
        {
            get
            {
                return _stat.Identifier;
            }
        }


        public Statement Stat
        {
            get
            {
                return _stat;
            }
        }

        //Checks if the current chainnode is an OR statement or an AND statement
        public bool IsOr
        {
            get;

            set;
        }

        public ChainNode[] Effects
        {
            get
            {
                return _effect.ToArray();
            }
        }

        public ChainNode[] Causes
        {
            get
            {
                return _cause.ToArray();
            }
        }

        public ChainNode(Statement stat)
        {
            _stat = stat;

            this.Asserted = false;

            this.IsOr = false;

            _effect = new List<ChainNode>();

            _cause = new List<ChainNode>();
        }

        public void AddCause(ChainNode toAdd)
        {
            _cause.Add(toAdd);
        }

        public void AddEffect(ChainNode toAdd)
        {
            _effect.Add(toAdd);
        }

        public void RemoveCause(ChainNode toRemove)
        {
            int i = 0;
            while (i < _cause.Count)
            {
                if (toRemove.Identifier == _cause[i].Identifier)
                {
                    _cause.RemoveAt(i);
                }

                else
                {
                    i += 1;
                }
            }
        }

        public void RemoveEffect(ChainNode toRemove)
        {
            int i = 0;
            while (i < _effect.Count)
            {
                if (toRemove.Identifier == _effect[i].Identifier)
                {
                    _effect.RemoveAt(i);
                }

                else
                {
                    i += 1;
                }
            }
        }

        public void EstablishForward()
        {
            if (!this.IsOr)
            {
                foreach (ChainNode c in _cause)
                {
                    if (!c.Asserted)
                    {
                        return;
                    }
                    //if all causes are asserted then this is asserted
                }

                this.Asserted = true;
            }

            else
            {
                foreach (ChainNode c in _cause)
                {
                    if (c.Asserted)
                    {
                        this.Asserted = true;
                    }
                    //if any causes are asserted then this is asserted
                }
            }
        }

        public List<ChainNode> EstablishBackward(List<ChainNode> visited)
        {
            //checks if this node has been visited before
            foreach (ChainNode v in visited)
            {
                if (this.Identifier == v.Identifier)
                {
                    //it has visited this before, therefore there is recursion
                    return null;
                }
            }
            visited.Add(this);
            if (!this.IsOr) //AND
            {
                List<ChainNode> result = new List<ChainNode>();
                foreach (ChainNode c in _cause)
                {
                    List<ChainNode> temp = new List<ChainNode>();
                    temp = c.EstablishBackward(visited);

                    //If the node does not have any causes (at the end)
                    if (temp == null)
                    {
                        return null;
                        
                    }
                    //else check if the node is already apart of the result
                    foreach (ChainNode t in temp)
                    {
                        bool found = false;
                        foreach (ChainNode r in result)
                        {
                            if (r.Identifier == t.Identifier)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            result.Add(t);
                        }
                    }
                }
                result.Add(this);
                return result;
            }
            else //OR statements
            {
                List<ChainNode> result = new List<ChainNode>();
                foreach (ChainNode c in _cause)
                {
                    result = c.EstablishBackward(visited);
                    if (result == null) //If there is no causes, then add the current chainnode
                    {
                        result = new List<ChainNode>();
                        result.Add(this);
                        return result;
                    }
                    else
                    {
                        result.Add(this);
                        return result;
                    }
                }
                // This is where it adds the nodes if it doesn't have any children
                if (this.Asserted)
                {
                    result.Add(this);
                    return result;
                }
            }
            return null;
        }
            

        public string Description()
        {
            string result = this.Identifier;

            result += "\n";

            result += "Cause: ";

            int i = 0;
            while (i < _cause.Count)
            {
                if (i != 0)
                {
                    result += ", ";
                }

                result += _cause[i].Identifier;

                i += 1;
            }

            result += "\n";

            result += "Effect: ";

            i = 0;
            while(i < _effect.Count)
            {
                if (i != 0)
                {
                    result += ", ";
                }

                result += _effect[i].Identifier;

                i += 1;
            }

            result += "\n";

            return result;
        }

        public bool CheckCausedBy(ChainNode b)
        {
            if (this.IsOr)
            {
                foreach (ChainNode c in this.Causes)
                {
                    if (c == b)//if a cause node is b then this is caused by b
                    {
                        return true;
                    }

                    else if (c.CheckCausedBy(b))
                    {
                        return true;
                    }
                }

                return false;
            }

            else
            {
                bool result = true;

                foreach (ChainNode c in this.Causes)
                {
                    if (c != b)//if a cause node is b then this is caused by b
                    {
                        return true;
                    }

                    else if (c.CheckCausedBy(b))
                    {
                        return true;
                    }
                }

                return false;
            }

        }

        public static void EstablishRelationship(ChainNode parent, ChainNode child)
        {
            parent.AddEffect(child);

            child.AddCause(parent);
        }
    }
}
