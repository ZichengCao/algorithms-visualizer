using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunnyAlgorithm { 
    class CalculateTools
    {
        public static ExpressionEvalutaion expressionEvalutaion = new ExpressionEvalutaion();
    }

    class ExpressionEvalutaion
    {
        //PriorityJudge[i][j]表示，栈顶符号为i，当前扫描符号为j
        //true表示i的优先级高，弹出栈顶的i，进行构造RPN，false表示j的优先级高，将j入栈
        private static bool[][] PriorityJudge = new bool[128][];
        private static char[] Operator = { '+', '-', '*', '/', '(', ')' };
        public ExpressionEvalutaion()
        {
            Initialize();
        }
        private void Initialize()
        {
            for (int i = 0; i < 128; i++)
            {
                PriorityJudge[i] = new bool[128];
            }
            //初值是false，只有j入栈的情况需要写入true
            for (int i = 0; i < 5; i++)
            {
                //符号栈为空时，任何符号都入栈
                PriorityJudge['#'][Operator[i]] = true;
                //栈顶符号是'('时，任何符号都入栈
                PriorityJudge['('][Operator[i]] = true;
                PriorityJudge[Operator[i]]['('] = true;
            }
            PriorityJudge['+']['*'] = true;
            PriorityJudge['+']['/'] = true;
            PriorityJudge['-']['*'] = true;
            PriorityJudge['-']['/'] = true;
        }
        private bool IsOperator(char c)
        {
            if (c >= '0' && c <= '9') return false;
            return true;
        }
        private List<string> GenerateRPN(string str)
        {
            List<string> RPN = new List<string>();
            Stack<char> symbol = new Stack<char>();
            symbol.Push('#');
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (IsOperator(c))
                {
                    if (c == ')')
                    {
                        do
                        {
                            RPN.Add(symbol.Pop().ToString());
                        } while (symbol.Peek() != '(');
                        symbol.Pop();
                    }
                    else
                    {
                        if (PriorityJudge[symbol.Peek()][c])
                        {
                            symbol.Push(c);
                        }
                        else
                        {
                            RPN.Add(symbol.Pop().ToString());
                            i--;//继续与下一个比较
                        }
                    }
                }
                else
                {
                    string temp = "";
                    while (i < str.Length && !IsOperator(str[i]))
                    {
                        temp += str[i];
                        i++;
                    }
                    i--;
                    RPN.Add(temp);
                }
            }
            while (symbol.Peek() != '#')
            {
                RPN.Add(symbol.Pop().ToString());
            }
            return RPN;
        }
        private int calc(int a, int b, char c)
        {
            switch (c)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    return a / b;
                default:
                    return 0;
            }
        }
        private int process(List<string> RPN)
        {
            Stack<int> num = new Stack<int>();
            for (int i = 0; i < RPN.Count; i++)
            {
                string s = RPN[i];
                if (IsOperator(s[0]))
                {
                    int a = num.Pop();
                    int b = num.Pop();
                    int res = calc(b, a, s[0]);
                    num.Push(res);
                }
                else
                {
                    num.Push(MyTools.StringToInt(s));
                }
            }
            return num.Peek();
        }
        public int calc(string expression)
        {
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '-' || (expression[i] == '-' && expression[i - 1] == '-'))
                {
                    expression.Insert(i, "0");
                    i++;
                }
            }
            return process(GenerateRPN(expression));
        }
    }
}
