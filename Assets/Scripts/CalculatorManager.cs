using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class CalculatorManager : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    private string input = "0";  

    void Start()
    {
        displayText.text = input; 
    }

    public void OnButtonClick(string value)
    {
        if (value == "AC")
        {
            input = "0";   
        }
        else if (value == "=")
        {
            input = EvaluateExpression(input).ToString();
        }
        else
        {
           
            if (input == "0")
                input = value;
            else
                input += value;
        }

        displayText.text = input;
    }

  
    double EvaluateExpression(string expr)
    {
        List<string> tokens = Tokenize(expr);
        Stack<double> numbers = new Stack<double>();
        Stack<string> ops = new Stack<string>();

        foreach (string token in tokens)
        {
            double num;

            if (double.TryParse(token, out num))
            {
                numbers.Push(num);
            }
            else if (IsOperator(token))
            {
                while (ops.Count > 0 && Precedence(ops.Peek()) >= Precedence(token))
                {
                    double b = numbers.Pop();
                    double a = numbers.Pop();
                    string op = ops.Pop();
                    numbers.Push(ApplyOp(a, b, op));
                }
                ops.Push(token);
            }
        }

        
        while (ops.Count > 0)
        {
            double b = numbers.Pop();
            double a = numbers.Pop();
            string op = ops.Pop();
            numbers.Push(ApplyOp(a, b, op));
        }

        return numbers.Pop();
    }

    List<string> Tokenize(string expr)
    {
        List<string> tokens = new List<string>();
        string number = "";

        for (int i = 0; i < expr.Length; i++)
        {
            char c = expr[i];

            // Handle negative or positive at start or after another operator
            if ((c == '-' || c == '+') && (i == 0 || IsOperator(expr[i - 1].ToString())))
            {
                number += c; // attach sign to number
            }
            else if (char.IsDigit(c) || c == '.')
            {
                number += c;
            }
            else
            {
                if (number != "")
                {
                    tokens.Add(number);
                    number = "";
                }
                tokens.Add(c.ToString());
            }
        }

        if (number != "")
            tokens.Add(number);

        return tokens;
    }


    bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "x" || c == "÷";
    }

    int Precedence(string op)
    {
        if (op == "x" || op == "÷") return 2;
        if (op == "+" || op == "-") return 1;
        return 0;
    }

    double ApplyOp(double a, double b, string op)
    {
        switch (op)
        {
            case "+": return a + b;
            case "-": return a - b;
            case "x": return a * b;
            case "÷": return a / b;
        }
        return 0;
    }
}
