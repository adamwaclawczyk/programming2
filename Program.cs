using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;


namespace onp22
{
    class Program
    {
        public static Dictionary<string, int> properDict = new Dictionary<string, int> {
                { "-u",5 },{ "sin", 4 }, { "cos", 4 }, { "abs", 4 }, { "exp", 4 }, { "log", 4 }, {"sqrt", 4 },{"tan", 4 }, {"cosh", 4 },{"sinh", 4 },{"tanh", 4 } ,{"acos", 4 },{"asin", 4 },{"atan", 4 },
                {"^", 3 },
                { "*", 2 },{"/", 2 },
                {"+", 1 },{"-", 1 },
                { "(", 0 },{")", 0}

            };
        static void Main(string[] args)
        {

            string equation = Console.ReadLine();
            StringBuilder equationbuilder = new StringBuilder(equation);
            //TODO//equation integrity
            //
            //

            //get tokens



            Queue kolejka = new Queue();
            Stack stos = new Stack();



            //string[] testr = Regex.Split(input, pattern);


            Dictionary<int, List<string>> priorytety = new Dictionary<int, List<string>>();
            priorytety.Add(4, new List<string> { "abs", "cos", "exp", "log", "sin", "sqrt", "tan", "cosh", "sinh", "tanh", "acos", "asin", "atan" });
            priorytety.Add(3, new List<string> { "^" });
            priorytety.Add(2, new List<string> { "*", "/" });
            priorytety.Add(1, new List<string> { "+", "-" });
            priorytety.Add(0, new List<string> { "(", ")" });
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

            //ready tokens

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "(") { stos.Push(tokens[i]); continue; }

                if (tokens[i] == ")")
                {
                    while (stos.Peek().ToString() != "(")
                    {
                        kolejka.Enqueue(stos.Pop());
                    }
                    stos.Pop();
                    continue;
                }
                //if(priorytety[4].Contains(tokens[i]) || priorytety[3].Contains(tokens[i]) || priorytety[2].Contains(tokens[i]) || priorytety[1].Contains(tokens[i]) || priorytety[0].Contains(tokens[i]))
                //{
                //    int pkey=0;
                //    int stoskey=0;
                //    for(int j = 0; j < priorytety.Count; j++)
                //    {
                //        if (priorytety[j].Contains(tokens[i])) { pkey = j; }
                //    }
                //    for(int j = 0;j < priorytety.Count && stos.Count>0; j++)
                //    {
                //        if (priorytety[j].Contains(stos.Peek())) stoskey = j;
                //    }

                //    while(stos.Count > 0 && pkey <= stoskey)
                //    {
                //        kolejka.Enqueue(stos.Pop());
                //        for (int j = 0; j < priorytety.Count && stos.Count > 0; j++)
                //        {
                //            if (priorytety[j].Contains(stos.Peek())) stoskey = j;
                //        }
                //    }
                //    stos.Push(tokens[i]);
                //    continue;
                //}
                if (getPriority(tokens[i]) < 10)
                {


                    while (stos.Count > 0 && getPriority(tokens[i]) <= getPriority(stos.Peek().ToString()))
                    {
                        kolejka.Enqueue(stos.Pop());
                    }
                    stos.Push(tokens[i]);
                    continue;
                }





                tokens[i] = tokens[i].Replace('.', ',');
                if (int.TryParse(tokens[i].ToString(), out _) || double.TryParse(tokens[i].ToString(), out _) || float.TryParse(tokens[i].ToString(), out _) || tokens[i] == "x")
                { kolejka.Enqueue(tokens[i]); continue; }




            }
            while (stos.Count > 0) kolejka.Enqueue(stos.Pop());

            int tabsize = kolejka.Count;
            string[] postfix = new string[tabsize];
            int count = 0;
            while (count != tabsize)
            {
                postfix[count] = kolejka.Dequeue().ToString();
                count++;
            }

            Console.WriteLine("Tokeny po rozbiciu w 1. fazie");
            foreach (string st in tokens)
            {
                Console.WriteLine(st);
            }
            Console.WriteLine("Postfix:");
            for (int i = 0; i < postfix.Length; i++)
            {
                Console.Write(postfix[i] + " ");
            }
            calcRNP(postfix, 1, 1, 10, 5);


            Console.Read();



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
        public static void findToken(string s)
        {



        }
        public static void calcRNP(string[] tokens, double x, double x_min, double x_max, int n)
        {
            double dx = (x_max - x_min) / n;

            for (int j = -1; j < n; j++)
            {

                Stack S = new Stack();
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (isNumber(tokens[i]))
                    {
                        S.Push(tokens[i]);
                    }
                    if (getPriority(tokens[i]) == 4)
                    {
                        double temp = double.Parse(S.Pop().ToString());
                        S.Push(evalFun(temp, tokens[i]));
                    }
                    if (getPriority(tokens[i]) >= 1 && getPriority(tokens[i]) <= 3)
                    {
                        double a = double.Parse(S.Pop().ToString());
                        double b = double.Parse(S.Pop().ToString());
                        a = evalOp(a, b, tokens[i]);
                        S.Push(a);
                    }
                    if (tokens[i] == "x" && j == -1)
                    {
                        S.Push(x);
                    }
                    else if (tokens[i] == "x")
                    {
                        S.Push(x_min);

                    }

                }

                if (j == -1) Console.Write("\n" + x + " => " + S.Pop().ToString());
                else Console.Write("\n" + x_min + " => " + S.Pop().ToString());
                x_min += dx;
            }


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
                case "-u": return -1 * a;
                case "sin": return Math.Sin(a);
                case "cos": return Math.Cos(a);
                case "sinh": return Math.Sinh(a);
                case "cosh": return Math.Cosh(a);
                case "abs": return Math.Abs(a);
                case "exp": return Math.Exp(a);
                case "log": return Math.Log(a);
                case "sqrt": return Math.Sqrt(a);
                case "tan": return Math.Tan(a);
                case "tanh": return Math.Tanh(a);
                case "acos": return Math.Acos(a);
                case "asin": return Math.Asin(a);
                case "atan": return Math.Atan(a);
            }


            return -3.69420;
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
                default: return -3.42;
            }
        }
    }


}
