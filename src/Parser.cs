using System;
using System.Collections.Generic;
using System.Text;
using IronPython;

namespace Lines
{
    public class Parser
    {
        public List<KeyValuePair<Tokens, string>> tokens;
        public Parser() { }
    
        public Parser(List<KeyValuePair<Tokens, string>> tokens)
        {
            this.tokens = tokens;
        }

        public void Parse()
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Key == Tokens.PYTHON)
                {
                    PythonExecute.execute(tokens[i]);
                    continue;
                }

                else if (tokens[i].Key == Tokens.WORD && tokens[i + 1].Key == Tokens.LBRACE)
                {

                    Function name = Function.none;
                    List<KeyValuePair<Tokens, string>> value = new List<KeyValuePair<Tokens, string>>();

                    switch (tokens[i].Value)
                    {
                        case "print":
                            name = Function.print;
                            break;
                        case "eval":
                            name = Function.eval;
                            break;
                        default:
                            break;
                    }
                    i++;
                    while (tokens[i].Key != Tokens.RBRACE)
                    {
                        i++;
                        if (tokens[i].Key != Tokens.RBRACE)
                            value.Add(new KeyValuePair<Tokens, string>(tokens[i].Key, tokens[i].Value));
                    }

                    ExecuteFunction(name, value);
                    continue;
                }

                else if (tokens[i].Key == Tokens.WORD && tokens[i + 1].Key == Tokens.EQ)
                {
                    string name = tokens[i].Value;
                    i += 2;
                    KeyValuePair<Tokens, string> value = new KeyValuePair<Tokens, string>(tokens[i].Key, tokens[i].Value);
                    Variables.vars[name] = value;
                    continue;
                }
            } 
        }

        private void ExecuteFunction(Function func, List<KeyValuePair<Tokens, string>> value)
        {
            switch(func)
            {
                case Function.print:
                    Print.execute(value);
                    break;
                case Function.eval:
                    Eval.execute(value);
                    break;
                default:
                    break;
            }
        }
    }
}
