using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
{
    class ForwardChain : Chain
    {
        public ForwardChain(KnowledgeBase kB) : base(kB)
        {

        }

        public override void InitialiseFrontier()
        {
            _frontier.Clear();

            foreach (ChainNode n in _nodes)
            {
                if (n.Asserted)
                {
                    _frontier.Add(n);
                }
            }
        }

        public override List<ChainNode> Solve()
        {
            List<ChainNode> result = new List<ChainNode>();

            while (true)
            {
                bool changed = false;

                List<ChainNode> toAdd = new List<ChainNode>();

                int i = 0;
                while (i < _frontier.Count)
                {
                    _frontier[i].EstablishForward();

                    if (_frontier[i].Asserted)//asserted nodes get removed
                    {
                        int j = 0;
                        while (j < _frontier[i].Effects.Length)
                        {
                            toAdd.Add(_frontier[i].Effects[j]);

                            j += 1;
                        }
                        result.Add(_frontier[i]);
                        _frontier.RemoveAt(i);

                        changed = true;
                    }

                    else
                    {
                        i += 1;
                    }
                }

                foreach (ChainNode n in toAdd)
                {
                    if ((!_frontier.Contains(n)) && (!result.Contains(n))) //if it hasn't already been considered
                    {
                        _frontier.Add(n);
                    }
                }

                if (!changed)//failure state frontier stagnant
                {
                    return null;
                }

                bool complete = true;

                foreach (ChainNode q in _queries)
                {
                    if (!q.Asserted)
                    {
                        complete = false;
                        break;
                    }
                }

                if (complete)
                {
                    foreach (ChainNode q in _queries)
                    {
                        bool found = false;

                        foreach (ChainNode r in result)
                        {
                            if (q.Identifier == r.Identifier)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            result.Add(q);
                        }
                    }

                    return result;
                }
            }
        } 
    }
}
