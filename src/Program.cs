using System;
using System.Collections.Generic;
using System.IO;

namespace Lines
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>();
            string[] temp = File.ReadAllText(args[0]).Split('\n');
            foreach (string str in temp)
            {
                lines.Add(str);
            }
            Lexer lexer = new Lexer(lines);
            lexer.Lex();
            //lexer.print();
            Parser parser = new Parser(lexer.tokens);
            parser.Parse();
            Console.ReadKey();
        }
    }
}
