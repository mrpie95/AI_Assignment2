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
            KnowledgeBase KB = new KnowledgeBase();
            KB.Interpret("a&b=>c; a; b");
            
            foreach (Statement o in KB.World)
            {
                Console.WriteLine(o.Identifier);
            }

            Console.WriteLine("Is Consictent: " + KB.CheckConsistency());            

            TruthTable tt = new TruthTable(KB.World);
            tt.WriteTable();


            Console.ReadKey();

        }
    }
}
