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
        eval
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

    public class PythonExecute
    {
        public static void execute(KeyValuePair<Tokens, string> value)
        {
            ScriptEngine engine = Python.CreateEngine();
            engine.Execute(value.Value);
        }
    }
}
