using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lines
{
    enum Function
    {
        none,
        print,
        exit,
        eval,
        input
    }

    public class Print
    {
        public static void execute(List<KeyValuePair<Tokens, string>> value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                if (Variables.vars.ContainsKey(value[i].Value))
                    Console.WriteLine(((KeyValuePair<Tokens, string>)Variables.vars[value[i].Value]).Value);
                else
                    Console.WriteLine(value[i].Value);
            }
        }
    }

    public class Input
    {
        public static void execute(List<KeyValuePair<Tokens, string>> value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                string result = Console.ReadLine();
                KeyValuePair<Tokens, string> val = new KeyValuePair<Tokens, string>(Tokens.STRING, result);
                Variables.vars["input"] = val;
            }
        }
    }
 
    public class Exit
    {
        public static void execute(List<KeyValuePair<Tokens, string>> value)
        {
            Environment.Exit(0);
        }
    }

    public class Eval
    {
        public static void execute(List<KeyValuePair<Tokens, string>> value)
        {
            int? result = null;
            int num1;
            int num2;

            if (Variables.vars.ContainsKey(value[0].Value))
                num1 = int.Parse(((KeyValuePair<Tokens, string>)Variables.vars[value[0].Value]).Value);
            else
                num1 = int.Parse(value[0].Value);

            if (Variables.vars.ContainsKey(value[1].Value))
                num2 = int.Parse(((KeyValuePair<Tokens, string>)Variables.vars[value[1].Value]).Value);
            else
                num2 = int.Parse(value[1].Value);

            switch (value[2].Value)
            {
                case "+":
                    result = num1 + num2;
                    break;
                case "-":
                    result = num1 - num2;
                    break;
                case "*":
                    result = num1 * num2;
                    break;
                case "/":
                    result = num1 / num2;
                    break;
            }

            KeyValuePair<Tokens, string> val = new KeyValuePair<Tokens, string>(Tokens.NUM, result.ToString());
            Variables.vars["eval"] = val;
        }
    }

    public class PythonExecute
    {
        public static void execute(KeyValuePair<Tokens, string> value)
        {
            ScriptEngine engine = Python.CreateEngine();
            engine.Execute(value.Value);
        }
    }
}
