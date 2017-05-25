using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.src
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Arguments invalid please enter <file name> and <mode: TT, FC, BC>");
                return;
            }

            KnowledgeBase KB = new KnowledgeBase();
            KB.Load(args[1]);

            if (args[0].ToLower() == "fc".ToLower())
            {
                ChainF(KB);
            }

            else if (args[0].ToLower() == "tt".ToLower())
            {
                TTable(KB);
            }

            else if (args[0].ToLower() == "bc".ToLower())
            {
                throw new NotImplementedException("BC not implemented yet!");
            }

            else
            {
                Console.WriteLine(args[1] + " was not recognised");
            }
            
        }

        static void ChainF(KnowledgeBase KB)
        {
            ForwardChain f = new ForwardChain(KB);

            Console.WriteLine(f.Solution());
        }

        static void TTable(KnowledgeBase KB)
        {
            TruthTable tt = new TruthTable(KB);


            Console.Write(tt.Solution());
        }
    }
}
