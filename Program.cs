using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace onp22
{
    class Program
    {
         static void Main(string[] args)
        {
            
            string equation = Console.ReadLine();
            StringBuilder equationbuilder = new StringBuilder(equation);
            //equation integrity
            //zalozenie, ze wprowadzamy poprawne rownania
            //usun spacje

            //rozbij na tokeny



            Queue kolejka = new Queue();
            Stack stos = new Stack();

            Dictionary<int, List<string>> priorytety = new Dictionary<int, List<string>>();
            priorytety.Add(4, new List<string> { "abs", "cos", "exp", "log", "sin", "sqrt", "tan", "cosh", "sinh", "tanh", "acos", "asin", "atan"});
            priorytety.Add(3, new List<string> { "^" });
            priorytety.Add(2, new List<string> { "*","/" });
            priorytety.Add(1, new List<string> { "+","-" });
            priorytety.Add(0, new List<string> { "(",")" });
            List<string> possibleTokens = new List<string> { "abs", "cos", "exp", "log", "sin", "sqrt", "tan", "cosh", "sinh", "tanh", "acos", "asin", "atan", "^", "*", "/", "+", "-", "(", ")", "x", };

            List<string> tokens = new List<string>();
            string tmp = "";
            string tmpNum = "";
            bool isNumber = false;
            bool isDouble = false;
            foreach(char znak in equation)
            {
                //build token

                if (int.TryParse(znak.ToString(), out _) && isDouble == false)
                {
                    isNumber = true;
                    tmpNum += znak;
                }
                if(znak == '.')
                {
                    tmpNum += znak;
                    isDouble = true;
                }
                if(int.TryParse(znak.ToString(),out _)   && isDouble==true)
                {

                    tmpNum += znak;
                }
                if(!int.TryParse(znak.ToString(), out _) && isDouble && znak != '.')
                {
                    tokens.Add(tmpNum);
                    tmpNum = "";
                    tmp = "";
                    isNumber = false;
                    isDouble = false;
                }
                if(isNumber && !(int.TryParse(znak.ToString(),out _)) && znak!='.')
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

            //gotowe tokeny

            for(int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "(") { stos.Push(tokens[i]); continue; }

                if(tokens[i] == ")")
                {
                    while(stos.Peek().ToString() != "(" )
                    {
                        kolejka.Enqueue(stos.Pop());
                    }
                    stos.Pop();
                    continue;
                }
                List<string> test = priorytety[4];
                if(priorytety[4].Contains(tokens[i]) || priorytety[3].Contains(tokens[i]) || priorytety[2].Contains(tokens[i]) || priorytety[1].Contains(tokens[i]) || priorytety[0].Contains(tokens[i]))
                {
                    int pkey=0;
                    int stoskey=0;
                    for(int j = 0; j < priorytety.Count; j++)
                    {
                        if (priorytety[j].Contains(tokens[i])) { pkey = j; }
                    }
                    for(int j = 0;j < priorytety.Count && stos.Count>0; j++)
                    {
                        if (priorytety[j].Contains(stos.Peek())) stoskey = j;
                    }

                    while(stos.Count > 0 && pkey <= stoskey)
                    {
                        kolejka.Enqueue(stos.Pop());
                        for (int j = 0; j < priorytety.Count && stos.Count > 0; j++)
                        {
                            if (priorytety[j].Contains(stos.Peek())) stoskey = j;
                        }
                    }
                    stos.Push(tokens[i]);
                    continue;
                }

                tokens[i] = tokens[i].Replace('.', ',');
                if (int.TryParse(tokens[i].ToString(), out _) || double.TryParse(tokens[i].ToString(), out _) || float.TryParse(tokens[i].ToString(),out _) || tokens[i] == "x")
                { kolejka.Enqueue(tokens[i]); continue;}




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
            Console.Read();



        }
    }
}
