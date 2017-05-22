using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
{
    abstract class Chain
    {
        protected KnowledgeBase _kB;
        protected List<ChainNode> _queries = new List<ChainNode>();
        protected List<ChainNode> _nodes = new List<ChainNode>();
        protected List<ChainNode> _frontier = new List<ChainNode>();

        public ChainNode[] Nodes
        {
            get
            {
                return _nodes.ToArray();
            }
        }

        public Chain(KnowledgeBase kB)
        {
            _kB = kB;

            this.Populate();

            this.InitialiseFrontier();
        }

        public void Populate()
        {
            _nodes.Clear();

            foreach (Statement stat in _kB.Universe)// populate with all (includes assertions and queieries)
            {
                if ((stat as Implication) != null)//implications express relationships not nodes
                {
                    //don't add implications
                }

                else
                {
                    ChainNode toAdd = new ChainNode(stat);

                    if ((stat as Variable) != null)
                    {
                        toAdd.IsOr = true;
                    }

                    _nodes.Add(toAdd);
                }

            }

            foreach (Statement stat in _kB.Assertions)// set assertions
            {
                foreach (ChainNode chan in _nodes)
                {
                    if (chan.Identifier == stat.Identifier)
                    {
                        chan.Asserted = true;
                    }
                }
            }

            foreach (Statement stat in _kB.Universe)// set relationships
            {
                if ((stat as Variable) != null)
                {
                    //no self evident causes
                }

                else if ((stat as And) != null)
                {
                    And a = (stat as And);

                    int toFind = a.Stats.Length;

                    int index = 0;
                    ChainNode[] parents = new ChainNode[toFind];

                    ChainNode child = null;

                    foreach (ChainNode n in _nodes)
                    {
                        if (n.Stat.Identifier != a.Identifier)
                        {
                            if (a.ComprisedOf(n.Identifier))
                            {
                                parents[index] = n;

                                index += 1;
                            }
                        }

                        else
                        {
                            child = n;
                        }
                    }

                    foreach (ChainNode p in parents)
                    {
                        ChainNode.EstablishRelationship(p, child);
                    }
                }

                else if ((stat as Implication) != null)
                {
                    Implication i = (stat as Implication);

                    ChainNode parent = null;
                    ChainNode child = null;

                    foreach (ChainNode n in _nodes)
                    {
                        if (i.ComprisedOf(n.Identifier))
                        {
                            //relationship always established after here
                            if (i.CausedBy(n.Identifier))
                            {
                                parent = n;
                            }

                            else
                            {
                                child = n;
                            }
                        }
                    }

                    ChainNode.EstablishRelationship(parent, child);
                }

                else//exaustive list
                {
                    throw new Exception("Unknown statement: " + stat.Identifier);
                }
            }

            /*int j = 0;
            while (j < _nodes.Count)//remove non-variables
            {
                List<ChainNode> causes = new List<ChainNode>();
                List<ChainNode> effects = new List<ChainNode>();

                if ((_nodes[j].Stat as Variable) == null)//non variable
                {
                    foreach (ChainNode c in _nodes[j].Causes)
                    {
                        causes.Add(c);

                        c.RemoveEffect(_nodes[j]);//disassociate
                    }

                    foreach (ChainNode e in _nodes[j].Effects)
                    {
                        effects.Add(e);
                        
                        e.RemoveCause(_nodes[j]);//disassociate
                    }

                    _nodes.RemoveAt(j);

                    foreach (ChainNode c in causes)
                    {
                        foreach (ChainNode e in effects)
                        {
                            e.AddCause(c);//associate
                            c.AddEffect(e);
                        }
                    }
                }

                else
                {
                    j += 1;
                }
            }*/

            foreach (ChainNode n in _nodes) // fill queries
            {
                foreach (Statement q in _kB.Queries)
                {
                    if (q.Identifier == n.Identifier)
                    {
                        _queries.Add(n);
                    }
                }
            }
        }

        public abstract void InitialiseFrontier();

        public abstract List<ChainNode> Solve();

        private void Clean(List<ChainNode> toClean)
        {
            int i = 0;
            while (i < toClean.Count)
            {
                if ((toClean[i].Stat as Variable) == null)//remove none variables
                {
                    toClean.RemoveAt(i);
                }

                else
                {
                    i += 1;
                }
            }
        }

        public string Solution()
        {
            this.InitialiseFrontier();

            List<ChainNode> solved = this.Solve();

            this.Clean(solved);

            string result = "";

            if (solved == null)
            {
                result = "No\n";
            }

            else
            {
                result = "Yes: ";
                foreach (ChainNode s in solved)
                {
                    result += s.Identifier;

                    if (s != solved.Last())
                    {
                        result += ", ";
                    }
                }
            }

            return result;
        }

        public string Description()
        {
            string result = "";

            foreach (ChainNode n in _nodes)
            {
                result += n.Description() + "\n";
            }

            return result;
        }
    }
}