using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
{
    class BackwardChain : Chain
    {
        public BackwardChain(KnowledgeBase kB) : base(kB)
        {

        }

        public override void InitialiseFrontier()
        {
            _frontier.Clear();
        }

        public override List<ChainNode> Solve()
        {
            //Foreach statement in knowledge base
            foreach (Statement q in _kB.Queries)
            {
                foreach(ChainNode n in _nodes)
                {
                    if (q.Identifier == n.Identifier)
                    {
                        List<ChainNode> output = new List<ChainNode>();
                        output = n.EstablishBackward(output);
                        return output;              
                    }
                }

            }
            return null;
        }
    }
}
