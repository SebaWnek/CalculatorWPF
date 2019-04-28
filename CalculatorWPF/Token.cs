using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF
{
    public class Token
    {
        //properties of token
        double value = 0;
        int numberOfParams = 2;
        int precedence = 10;
        string symbol = " ";
        public enum Associativity { left, right };
        Associativity assoc = Associativity.left;
        public enum Type { number, oper, leftBra, rightBra, notAToken, error }
        Type typeOf { get; set; } = Type.notAToken;

        //access to properties
        public string GetSign
        {
            get
            {
                //check if number or operator when returning
                if (typeOf == Type.number)
                {
                    return value.ToString();
                }
                else
                {
                    return symbol.ToString();
                }
            }
        }

        public int NumberOfParams
        {
            get
            {
                return numberOfParams;
            }
        }

        public Associativity GetAssoc
        {
            get
            {
                return assoc;
            }
        }

        public int GetPrecedence
        {
            get
            {
                return precedence;
            }
        }
        public Type GetTokenType
        {
            get
            {
                return typeOf;
            }
        }

        //change string into token
        public static Token ToToken(string num)
        {
            Token t = new Token();
            //list of possible tokens
            if (double.TryParse(num, out t.value))
            {
                t.typeOf = Type.number;
                return t;
            }
            else if (num == "+")
            {
                t.symbol = "+";
                t.precedence = 10;
                t.typeOf = Type.oper;
                return t;
            }
            else if (num == "-")
            {
                t.symbol = "-";
                t.precedence = 10;
                t.typeOf = Type.oper;
                return t;
            }
            else if (num == "*")
            {
                t.symbol = "*";
                t.precedence = 20;
                t.typeOf = Type.oper;
                return t;
            }
            else if (num == "/")
            {
                t.symbol = "/";
                t.precedence = 20;
                t.typeOf = Type.oper;
                return t;
            }
            else if (num == "(")
            {
                t.symbol = "(";
                t.precedence = 30;
                t.typeOf = Type.leftBra;
                return t;
            }
            else if (num == ")")
            {
                t.symbol = ")";
                t.precedence = 30;
                t.typeOf = Type.rightBra;
                return t;
            }
            else if (num == "_")
            {
                t.symbol = "_";
                t.precedence = 40;
                t.typeOf = Type.oper;
                t.numberOfParams = 1;
                t.assoc = Associativity.right;
                return t;
            }
            else if (num == "√")
            {
                t.symbol = "√";
                t.precedence = 25;
                t.typeOf = Type.oper;
                t.numberOfParams = 1;
                t.assoc = Associativity.right;
                return t;
            }
            else if (num == "!")
            {
                t.symbol = "!";
                t.precedence = 25;
                t.typeOf = Type.oper;
                t.numberOfParams = 1;
                t.assoc = Associativity.left;
                return t;
            }
            else if (num == "^")
            {
                t.symbol = "^";
                t.precedence = 25;
                t.typeOf = Type.oper;
                return t;
            }
            //if unknown operator
            else
            {
                System.Windows.MessageBox.Show("Detected wrong sign!");
                t.typeOf = Type.error;
                return t;
            }
        }



    }
}
