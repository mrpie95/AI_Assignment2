﻿/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine_GUI
{
    class Program
    {
        static void Main(string[] args)
        {
            KnowledgeBase KB = new KnowledgeBase();
            KB.Interpret("p2=> p3; p3 => p1; c => e; b&e => f; f&g => h; p1=>d; p1&p3 => c; a; b; p2;", AssertionEnum.Assertion);

            string query = "d;";
            KB.Interpret(query, AssertionEnum.Query);



            TruthTable tt = new TruthTable(KB);

            tt.WriteTable();

            string[] arr = KnowledgeBase.DelimitString(query, new string[] {KnowledgeBase.Delimiter}, new string[] {" "});

            foreach (string s in arr)
            {
                if (tt.Query(s) == Result.Valid)
                {
                    Console.WriteLine("YES: " + tt.Rows);
                }

                else
                {
                    Console.WriteLine("NO");
                }
            }

            Console.ReadKey();

        }
    }
}*/
