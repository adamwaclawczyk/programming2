using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;


namespace programming2
{
    class Program
    {
        
        static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.Write("\nCommand line args count doesn't match template");
                Console.Write("\ndotnet run equation x x_min x_max n");
                goto end;
            }
            RPN myRPN = new RPN(args[0].ToString());
            if (!myRPN.properEquation()) goto end;
            try
            {
                myRPN.generateInfixTokens();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto end;
            }
            
            if (myRPN.invalidTokens) goto end;
            myRPN.returnInfixTokens();
            myRPN.generatePostfixTokens();
            myRPN.returnPostfixTokens();
            Console.Write("\n");
            try
            {
                myRPN.evaluatePostfix(double.Parse(args[1]));
            }
            catch(Exception ex)
            {
                Console.Write("\n"+ex.Message);
            }
            myRPN.evaluatePostfix(double.Parse(args[2]), double.Parse(args[3]), int.Parse(args[4]));
            
        end:
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
        
    }


}
