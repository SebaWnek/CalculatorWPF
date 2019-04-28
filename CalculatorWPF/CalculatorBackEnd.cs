using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF
{
    internal static class CalculatorBackEnd
    {
        public static double Calculate(string equation)
        {
            //check if doesn't start with a plus
            if (equation.Length >= 3 && equation.Substring(0, 3) == " + ")
            {
                equation = equation.Substring(3);
            }
            //correct if starts with a minus
            else if (equation.Length >= 3 && equation.Substring(0, 3) == " - ")
            {
                equation = equation.Substring(1);
            }
            //correct if start with sqrt or parenthesis
            else if (equation.Length >= 3 && (equation.Substring(0, 3) == " √ " || equation.Substring(0,3) == " ( "))
            {
                equation = equation.Substring(1);
            }
            //if first is space, and not as above, then it's operator
            else if (equation[0] == ' ')
            {
                System.Windows.MessageBox.Show("Something is wrong!");
                return 0;
            }
            //if operator left at the end and not factorial
            else if (equation.Length >= 3 && equation[equation.Length - 1] == ' ' && equation[equation.Length - 2] != ')' && equation[equation.Length - 2] != '!')
            {
                System.Windows.MessageBox.Show("Something is wrong!");
                return 0;
            }

            //rearrange equation into array of tokens
            equation = equation.Replace("  ", " ");
            equation = equation.Trim();
            string[] eq = equation.Split(' ');

            //change array of strings into tokens with more properties
            Token[] tokens = Tokenize(eq);

            //create quee and rearrange tokens into reversed polish notation
            Queue<Token> tokensAfter = new Queue<Token>(ShuntingYard(tokens));

            //final calculation
            double result = CalculateExpression(tokensAfter);

            return result;
        }

        //change array of strings to array of tokens
        static Token[] Tokenize(string[] eq)
        {
            Token[] tokens = new Token[eq.Length];
            for (int i = 0; i < eq.Length; i++)
            {
                //check if unitary minus
                if ((i == 0 && eq[i] == "-"))
                {
                    eq[i] = "_";
                }
                //create token from char
                tokens[i] = Token.ToToken(eq[i]);
            }
            return tokens;
        }

        static Queue<Token> ShuntingYard(Token[] tokens)
        {
            //create queue and stack
            Queue<Token> output = new Queue<Token>();
            Stack<Token> operators = new Stack<Token>();

            //go over all input tokens
            for (int i = 0; i < tokens.Length; i++)
            {
                //read token
                Token t = tokens[i];

                //if token is a number
                if (t.GetTokenType == Token.Type.number)
                {
                    output.Enqueue(t);
                }

                //if token is an operator
                else if (t.GetTokenType == Token.Type.oper)
                {
                    //check for multiple operators
                    if (i>0 && t.NumberOfParams == 2 && (tokens[i-1].GetTokenType == Token.Type.oper || tokens[i-1].GetTokenType==Token.Type.leftBra))
                    {
                        System.Windows.MessageBox.Show("Something is wrong!");
                        output.Clear();
                        return output;
                    }
                    //if token is sqrt and previous is number add multiply before
                    if (i > 0 && t.GetSign == "√" && tokens[i - 1].GetTokenType == Token.Type.number)
                    {
                        Token multiple = Token.ToToken("*");
                        operators.Push(multiple);
                    }
                    //check precedense
                    while (operators.Count > 0 && (operators.Peek().GetPrecedence > t.GetPrecedence ||
                        (operators.Peek().GetPrecedence == t.GetPrecedence && operators.Peek().GetAssoc == Token.Associativity.left))
                        && operators.Peek().GetTokenType != Token.Type.leftBra)
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Push(t);
                }
                //left bracket
                else if (t.GetTokenType == Token.Type.leftBra)
                {
                    if (i > 0 && tokens[i-1].GetTokenType == Token.Type.number)
                    {
                        Token multiple = Token.ToToken("*");
                        operators.Push(multiple);
                    }
                    operators.Push(t);
                }
                //right bracket
                else if (t.GetTokenType == Token.Type.rightBra)
                {
                    bool end = true;
                    do
                    {
                        //if no more operators then must be mismatched brackets
                        if (operators.Count == 0)
                        {
                            System.Windows.MessageBox.Show("Unmatched parenthesis!");
                        }

                        else if (operators.Peek().GetTokenType != Token.Type.leftBra)
                        {
                            output.Enqueue(operators.Pop());
                        }
                        //left bracket found
                        else if (operators.Peek().GetTokenType == Token.Type.leftBra)
                        {
                            operators.Pop();
                            end = false;
                        }

                    }
                    while (end);
                }
            }
            //if operators left on stack add them to queue
            if (operators.Count > 0)
            {
                int tmp = operators.Count;
                for (int i = 0; i < tmp; i++)
                {
                    output.Enqueue(operators.Pop());
                }
            }
            return output;
        }

        //calculatin result from RPN
        static double CalculateExpression(Queue<Token> queue)
        {
            if (queue.Count == 0)
            {
                return 0;
            }
            //stack for result
            Stack<double> awaiting = new Stack<double>();
            while (queue.Count > 0)
            {
                //check if operand or operator, operand push to stack, operator perform
                if (queue.Peek().GetTokenType == Token.Type.number)
                {
                    awaiting.Push(double.Parse(queue.Dequeue().GetSign));
                }
                //check if operator using 2 operands
                else if (queue.Peek().GetTokenType == Token.Type.oper && queue.Peek().NumberOfParams == 2)
                {
                    double op2 = awaiting.Pop();
                    double op1 = awaiting.Pop();
                    double resultTmp = 0;

                    //different cases
                    switch (queue.Dequeue().GetSign)
                    {
                        case "-":
                            resultTmp = op1 - op2;
                            break;
                        case "+":
                            resultTmp = op1 + op2;
                            break;
                        case "*":
                            resultTmp = op1 * op2;
                            break;
                        case "/":
                            resultTmp = op1 / op2;
                            break;
                        case "^":
                            resultTmp = Math.Pow(op1, op2);
                            break;
                    }
                    awaiting.Push(resultTmp);
                }
                //if operator with one parameter
                else if (queue.Peek().GetTokenType == Token.Type.oper && queue.Peek().NumberOfParams == 1)
                {
                    double op1 = awaiting.Pop();
                    double resultTmp = 0;
                    switch (queue.Dequeue().GetSign)
                    {
                        case "_":
                            resultTmp = -op1;
                            break;
                        case "√":
                            resultTmp = Math.Sqrt(op1);
                            break;
                        case "!":
                            resultTmp = 1;
                            for (int i = 1; i <= op1; i++)
                            {
                                resultTmp *= i;
                            }
                            break;
                    }
                    awaiting.Push(resultTmp);
                }
            }
            return awaiting.Pop();
        }
    }
}

