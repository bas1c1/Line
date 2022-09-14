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
            Dictionary<string, int> metki = new Dictionary<string, int>();

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Key == Tokens.DOTDOT && tokens[i + 1].Key == Tokens.WORD)
                {
                    i++;
                    try
                    {
                        metki.Add(tokens[i].Value, i);
                    }
                    catch { continue; }
                    
                    while (tokens[i].Value != "$" && tokens[i].Key != Tokens.OTHER)
                    {
                        i++;
                    }
                    continue;
                }
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Key == Tokens.PYTHON)
                {
                    PythonExecute.execute(tokens[i]);
                    continue;
                }

                else if (tokens[i].Key == Tokens.WORD && tokens[i + 1].Key == Tokens.QUESTION)
                {
                    object val1;
                    object val2;
                    switch (tokens[i].Value)
                    {
                        case "ifeq":
                            i += 2;

                            if (Variables.vars.ContainsKey(tokens[i].Value)) { val1 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i].Value]).Value; }
                            else { val1 = tokens[i].Value; }

                            if (Variables.vars.ContainsKey(tokens[i + 1].Value)) { val2 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i + 1].Value]).Value; }
                            else { val2 = tokens[i + 1].Value; }

                            if (val1.ToString() == val2.ToString())
                            {
                                try
                                {
                                    i = metki[tokens[i + 2].Value];
                                }
                                catch { continue; }
                                continue;
                            }
                            else { i += 3; }
                            break;
                        case "ifneq":
                            i += 2;

                            if (Variables.vars.ContainsKey(tokens[i].Value)) val1 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i].Value]).Value;
                            else val1 = tokens[i].Value;

                            if (Variables.vars.ContainsKey(tokens[i + 1].Value)) val2 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i + 1].Value]).Value;
                            else val2 = tokens[i + 1].Value;

                            if (val1.ToString() != val2.ToString())
                            {
                                try
                                {
                                    i = metki[tokens[i + 2].Value];
                                }
                                catch { continue; }
                                continue;
                            }
                            else i += 3;
                            break;
                        case "ifbigeq":
                            i += 2;

                            if (Variables.vars.ContainsKey(tokens[i].Value)) val1 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i].Value]).Value;
                            else val1 = tokens[i].Value;

                            if (Variables.vars.ContainsKey(tokens[i + 1].Value)) val2 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i + 1].Value]).Value;
                            else val2 = tokens[i + 1].Value;

                            if (int.Parse(val1.ToString()) >= int.Parse(val2.ToString()))
                            {
                                try
                                {
                                    i = metki[tokens[i + 2].Value];
                                }
                                catch { continue; }
                                continue;
                            }
                            else i += 3;
                            break;
                        case "ifleseq":
                            i += 2;

                            if (Variables.vars.ContainsKey(tokens[i].Value)) val1 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i].Value]).Value;
                            else val1 = tokens[i].Value;

                            if (Variables.vars.ContainsKey(tokens[i + 1].Value)) val2 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i + 1].Value]).Value;
                            else val2 = tokens[i + 1].Value;

                            if (int.Parse(val1.ToString()) <= int.Parse(val2.ToString()))
                            {
                                try
                                {
                                    i = metki[tokens[i + 2].Value];
                                }
                                catch { continue; }
                                continue;
                            }
                            else i += 3;
                            break;
                        case "ifbig":
                            i += 2;

                            if (Variables.vars.ContainsKey(tokens[i].Value)) val1 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i].Value]).Value;
                            else val1 = tokens[i].Value;

                            if (Variables.vars.ContainsKey(tokens[i + 1].Value)) val2 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i + 1].Value]).Value;
                            else val2 = tokens[i + 1].Value;

                            if (int.Parse(val1.ToString()) > int.Parse(val2.ToString()))
                            {
                                try
                                {
                                    i = metki[tokens[i + 2].Value];
                                }
                                catch { continue; }
                                continue;
                            }
                            else i += 3;
                            break;
                        case "ifles":
                            i += 2;

                            if (Variables.vars.ContainsKey(tokens[i].Value)) val1 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i].Value]).Value;
                            else val1 = tokens[i].Value;

                            if (Variables.vars.ContainsKey(tokens[i + 1].Value)) val2 = ((KeyValuePair<Tokens, string>)Variables.vars[tokens[i + 1].Value]).Value;
                            else val2 = tokens[i + 1].Value;

                            if (int.Parse(val1.ToString()) < int.Parse(val2.ToString()))
                            {
                                try
                                {
                                    i = metki[tokens[i + 2].Value];
                                }
                                catch { continue; }
                                continue;
                            }
                            else i += 3;
                            break;
                        default:
                            continue;
                    }
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
                        case "exit":
                            name = Function.exit;
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

                else if (tokens[i].Key == Tokens.DOTDOT && tokens[i + 1].Key == Tokens.WORD)
                {
                    while (tokens[i].Value != "$" && tokens[i].Key != Tokens.OTHER)
                    {
                        i++;
                    }
                    continue;
                }

                else if (tokens[i].Key == Tokens.WORD && tokens[i + 1].Key == Tokens.DOTDOT)
                {
                    try
                    {
                        i = metki[tokens[i].Value];
                    }
                    catch { continue; }
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
                case Function.exit:
                    Exit.execute(value);
                    break;
                default:
                    break;
            }
        }
    }
}
