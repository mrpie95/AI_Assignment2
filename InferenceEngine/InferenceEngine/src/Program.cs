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
            KB.Interpret("p2 => p3; p3 => p1; c => e; b & e => f; f & g => h; p1 => d; p1 & p3 => c; a; b; p2;");
            
            foreach (Statement o in KB.World)
            {
                Console.WriteLine(o.Identifier);
            }

            Console.WriteLine("Is Consictent: " + KB.CheckConsistency());
            

            //TruthTable tt = new TruthTable(KB.World);
            //tt.WriteTable();
            Console.ReadKey();

        }
    }
}
