using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
{
    class ForwardChain
    {
        private KnowledgeBase _kB;
        private List<ChainNode> _nodes = new List<ChainNode>();
        private List<ChainNode> _frontier = new List<ChainNode>();

        public ForwardChain(KnowledgeBase kB)
        {
            _kB = kB;
        }

        public void Populate()
        {
            //variables
            int i = 0;
            while (i < _kB.Universe.Length)
            {
                if ((_kB.Universe[i] as Variable) != null)
                {
                    ChainNode n = new ChainNode(_kB.Universe[i]);

                    if ((_kB.Universe[i] as Variable).Defined)
                    {
                        _frontier.Add(n);
                    }

                    _nodes.Add(n);
                }

                i += 1;
            }

            //connections
            i = 0;
            while (i < _kB.Assertions.Length)
            {
                if ((_kB.Assertions[i] as Variable) == null)
                {
                    int j = 0;
                    while (j < _nodes.Count)
                    {

                        int k = 0;
                        while (k < _nodes.Count)
                        {
                            if ( j != k)
                            {
                                if (_nodes[j].Stat.DependsOn(_kB.Assertions[i].Identifier))
                                {
                                    //set dependencies
                                }
                            }

                            k += 1;
                        }

                        j += 1;
                    }
                }

                i += 1;
            }

        }
    }
}
