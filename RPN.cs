﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace programming2
{

    public class RPN
    {
        string equation;
        List<string> infixTokens = new List<string>();
        List<string> postfixTokens = new List<string>();
        static Dictionary<string, int> properDict = new Dictionary<string, int> {
                { "sin", 4 }, { "cos", 4 }, { "abs", 4 }, { "exp", 4 }, { "log", 4 }, {"sqrt", 4 },{"tan", 4 }, {"cosh", 4 },{"sinh", 4 },{"tanh", 4 } ,{"acos", 4 },{"asin", 4 },{"atan", 4 },
                {"^", 3 },{ "-u",3 },
                { "*", 2 },{"/", 2 },
                {"+", 1 },{"-", 1 },
                { "(", 0 },{")", 0}

            };
        public RPN(string input)
        {
            this.equation = input;
        }
        public bool properEquation()
        {
            string eq = this.equation;
            eq.Trim();
            int count = 0;
            foreach(char c in eq)
            {
                if (c == '(') count++;
                else if (c == ')') count--;
            }
            if (count != 0)
            {
                Console.Write("\nNieprawidłowa ilość nawiasów");
                return false;
            }




            return true;
        }
        public string[] generateInfixTokens()
        {
            List<string> possibleTokens = new List<string> { "abs", "cos", "exp", "log", "sin", "sqrt", "tan", "cosh", "sinh", "tanh", "acos", "asin", "atan", "^", "*", "/", "+", "-", "(", ")", "x", };
            List<string> tokens = new List<string>();
            string tmp = "";
            string tmpNum = "";
            bool isNumber = false;
            bool isDouble = false;
            foreach (char znak in equation)
            {
                //build token
                if (znak == '-' && (tokens.Count == 0 || isOperator(tokens[tokens.Count - 1])))
                {
                    tokens.Add("-u");
                    continue;
                }
                if (int.TryParse(znak.ToString(), out _) && isDouble == false)
                {
                    isNumber = true;
                    tmpNum += znak;
                }
                if (znak == '.')
                {
                    tmpNum += znak;
                    isDouble = true;
                }
                if (int.TryParse(znak.ToString(), out _) && isDouble == true)
                {
                    tmpNum += znak;
                }
                if (!int.TryParse(znak.ToString(), out _) && isDouble && znak != '.')
                {
                    tokens.Add(tmpNum);
                    tmpNum = "";
                    tmp = "";
                    isNumber = false;
                    isDouble = false;
                }
                if (isNumber && !(int.TryParse(znak.ToString(), out _)) && znak != '.')
                {
                    tokens.Add(tmpNum);
                    tmpNum = "";
                    tmp = "";
                    isNumber = false;
                    isDouble = false;
                }

                tmp += znak;
                if (possibleTokens.Contains(tmp))
                {
                    tokens.Add(tmp);
                    tmp = "";
                }
                //check if WiP token exists in the possibleTokens list 
                //foreach( string t in possibleTokens)
                //{
                //}
            }
            
            if (tmpNum != "") { tokens.Add(tmpNum); }
            foreach(string t in tokens)
            {
                this.infixTokens.Add(t);
            }
            return tokens.ToArray();
        }
        public string[] generatePostfixTokens()
        {
            List<string> tokens = this.infixTokens;
            Stack S = new Stack();
            Queue Q = new Queue();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "(") { S.Push(tokens[i]); continue; }

                if (tokens[i] == ")")
                {
                    while (S.Peek().ToString() != "(")
                    {
                        Q.Enqueue(S.Pop());
                    }
                    S.Pop();
                    continue;
                }
                if (getPriority(tokens[i]) < 10)
                {
                    while (S.Count > 0 && getPriority(tokens[i]) <= getPriority(S.Peek().ToString()))
                    {
                        Q.Enqueue(S.Pop());
                    }
                    S.Push(tokens[i]);
                    continue;
                }
                tokens[i] = tokens[i].Replace('.', ',');
                if (int.TryParse(tokens[i].ToString(), out _) || double.TryParse(tokens[i].ToString(), out _) || float.TryParse(tokens[i].ToString(), out _) || tokens[i] == "x")
                { Q.Enqueue(tokens[i]); continue; }




            }
            while (S.Count > 0) Q.Enqueue(S.Pop());

            int tabsize = Q.Count;
            string[] postfix = new string[tabsize];
            int count = 0;
            while (count != tabsize)
            {
                postfix[count] = Q.Dequeue().ToString();
                count++;
            }
            foreach(string t in postfix)
            {
                this.postfixTokens.Add(t);
            }
            return postfix;
        }
        public List<string> returnInfixTokens()
        {
            Console.Write("\n");
            foreach (string t in this.infixTokens) Console.Write(t + " ");
            return this.infixTokens;
        }
        public List<string> returnPostfixTokens()
        {
            Console.Write("\n");
            foreach (string t in this.postfixTokens) Console.Write(t + " ");
            return this.postfixTokens;
        }
        public double evaluatePostfix(double x)
        {
            string[] tokens = this.postfixTokens.ToArray();
            Stack S = new Stack();
            for (int i = 0; i < tokens.Length; i++)
            {
                if (isNumber(tokens[i]))
                {
                    S.Push(tokens[i]);
                }
                if (getPriority(tokens[i]) == 4 || tokens[i] == "-u")
                {
                    double temp = double.Parse(S.Pop().ToString());
                    S.Push(evalFun(temp, tokens[i]));
                    if(double.Parse(S.Peek().ToString()) == -3.7942080885) { Console.WriteLine("\nKończę działanie..."); return -3.7942080885; }
                }
                if (getPriority(tokens[i]) >= 1 && getPriority(tokens[i]) <= 3 && tokens[i] != "-u")
                {
                    double a = double.Parse(S.Pop().ToString());
                    double b = double.Parse(S.Pop().ToString());
                    a = evalOp(a, b, tokens[i]);
                    S.Push(a);
                }
                else if (tokens[i] == "x")
                {
                    S.Push(x);
                }

            }
            Console.Write("\n" + x + " => " + S.Pop().ToString());
            return 0.0;
        }
        public double evaluatePostfix(double x, double x_min, double x_max, int n)
        {
            
            double dx = (x_max - x_min) / n;
            for (int j = 0; j < n; j++)
            {
                evaluatePostfix(x_min);
                x_min += dx;
            }
            return 0.0;
        }
        public static bool isOperator(string op)
        {
            switch (op)
            {
                case "+": return true;
                case "-": return true;
                case "*": return true;
                case "/": return true;
                case "^": return true;
                case "(": return true;
                default: return false;

            }
        }
        public static int getPriority(string op)
        {
            if (properDict.TryGetValue(op, out _)) return properDict[op];
            else return 10;
        }
        public static bool isNumber(string token)
        {
            bool flag = double.TryParse(token, out _);
            return flag;
        }
        public static double evalFun(double a, string fun)
        {
            switch (fun)
            {
                case "-u": return (-1) * a;
                case "sin": return Math.Sin(a);
                case "cos": return Math.Cos(a);
                case "sinh": return Math.Sinh(a);
                case "cosh": return Math.Cosh(a);
                case "abs": return Math.Abs(a);
                case "exp": return Math.Exp(a);
                case "log":
                    if (a > 0)
                            return Math.Log(a);
                    else { Console.Write("Nieprawidłowa wartość. Błąd dziedziny funkcji log"); break; }
                    
                case "sqrt": return Math.Sqrt(a);
                case "tan": return Math.Tan(a);
                case "tanh": return Math.Tanh(a);
                case "acos": return Math.Acos(a);
                case "asin": return Math.Asin(a);
                case "atan": return Math.Atan(a);
            }
            return -3.7942080885;
        }
        public static double evalOp(double a, double b, string op)
        {
            switch (op)
            {
                case "+": return a + b;
                case "-": return b - a;
                case "*": return a * b;
                case "/": return b / a;
                case "^": return Math.Pow(b, a);
            }
            return -3.42;
        }
    }
}
