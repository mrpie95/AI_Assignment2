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

        public static void EstablishRelationship(ChainNode parent, ChainNode child)
        {
            parent.AddEffect(child);

            child.AddCause(parent);
        }
    }
}
