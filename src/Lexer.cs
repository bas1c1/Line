using System;
using System.Collections.Generic;
using System.Text;

namespace Lines
{
    public class Lexer
    {
        private int index = 0;
        private List<string> lines;
        public List<KeyValuePair<Tokens, string>> tokens;

        public Lexer() { }

        public Lexer(List<string> lines)
        {
            this.lines = lines;
        }

        public List<KeyValuePair<Tokens, string>> Lex()
        {
            tokens = new List<KeyValuePair<Tokens, string>>();

            for (int i = 0; i < lines.Count; i++)
            {
               
                string line = lines[i];
                for (; index < line.Length; index++)
                {
                    if (Char.IsLetter(line[index]))
                    {
                        LexWord(line);
                    }
                    else if (Char.IsDigit(line[index]))
                    {
                        LexNum(line);
                    }
                    else
                    {
                        switch (line[index])
                        {
                            case '(':
                                Append(new KeyValuePair<Tokens, string>(Tokens.LBRACE, "("));
                                break;
                            case ')':
                                Append(new KeyValuePair<Tokens, string>(Tokens.RBRACE, ")"));
                                break;
                            case '+':
                                Append(new KeyValuePair<Tokens, string>(Tokens.PLUS, "+"));
                                break;
                            case '-':
                                Append(new KeyValuePair<Tokens, string>(Tokens.MINUS, "-"));
                                break;
                            case '*':
                                Append(new KeyValuePair<Tokens, string>(Tokens.STAR, "*"));
                                break;
                            case '/':
                                Append(new KeyValuePair<Tokens, string>(Tokens.SLASH, "/"));
                                break;
                            case '=':
                                Append(new KeyValuePair<Tokens, string>(Tokens.EQ, "="));
                                break;
                            case ':':
                                Append(new KeyValuePair<Tokens, string>(Tokens.DOTDOT, ":"));
                                break;
                            case '?':
                                Append(new KeyValuePair<Tokens, string>(Tokens.QUESTION, "?"));
                                break;
                            case '$':
                                Append(new KeyValuePair<Tokens, string>(Tokens.OTHER, "$"));
                                break;
                            case '^':
                                index++;
                                index = line.Length;
                                line = line.Substring(1);
                                Append(new KeyValuePair<Tokens, string>(Tokens.PYTHON, line));
                                break;
                            case '\"':
                                index++;
                                LexString(line);
                                break;
                            default:
                                continue;
                        }
                    }
                }
                index = 0;
            }

            return tokens;
        }

        private void LexWord(string str)
        {
            string buff = string.Empty;
            for (; index < str.Length && str[index] != ' ' && Char.IsLetter(str[index]); index++)
            {
                buff += str[index];
            }
            index--;
            Append(new KeyValuePair<Tokens, string>(Tokens.WORD, buff));
        }

        private void LexNum(string str)
        {
            string buff = string.Empty;
            for (; index < str.Length && str[index] != ' ' && Char.IsDigit(str[index]); index++)
            {
                buff += str[index];
            }
            index--;
            Append(new KeyValuePair<Tokens, string>(Tokens.NUM, buff));
        }

        private void LexString(string str)
        {
            string buff = string.Empty;
            for (int i = index; i < str.Length && str[i] != '\"'; i++)
            {
                buff += str[i];
                index++;
            }
            Append(new KeyValuePair<Tokens, string>(Tokens.STRING, buff));
        }

        private void Append(KeyValuePair<Tokens, string> value)
        {
            tokens.Add(value);
        }

        private void AppendList(List<KeyValuePair<Tokens, string>> value)
        {
            foreach (KeyValuePair<Tokens, string> pair in value)
            {
                tokens.Add(pair);
            }
        }

        public void print()
        {
            foreach (KeyValuePair<Tokens, string> pair in tokens)
            {
                Console.WriteLine($"{pair.Key} - {pair.Value};");
            }
        }
    }
}