using System;
using System.Collections.Generic;
using System.Globalization;

namespace NetMX.Expression
{
    public class ExpressionGenerator
    {
        public static string Generate(IExpression expression)
        {
            var visitor = new ExpressionVisitor();
            expression.Accept(visitor);
            return visitor.GetFinalValue();
        }

        private class StackValue
        {
            private readonly string _value;
            private readonly int _precedence;

            public StackValue(string value, int precedence)
            {
                _value = value;
                _precedence = precedence;
            }

            public int Precedence
            {
                get { return _precedence; }
            }

            public string Value
            {
                get { return _value; }
            }
        }

        private class ExpressionVisitor : IExpressionTreeVisitor
        {
            private readonly Stack<StackValue> _stack = new Stack<StackValue>();

            public string GetFinalValue()
            {
                return _stack.Pop().Value;
            }

            public void Visit(AddExp expression)
            {
                HandleBinaryOperator("+", 4);
            }

            public void Visit(AndExp expression)
            {
                HandleBinaryOperator("and", 1);
            }

            public void Visit(AttributeExp expression)
            {
                HandleFunction("attr");
            }            

            public void Visit<T>(ConstantExp<T> expression)
            {
                if (typeof(T) == typeof(string))
                {
                    Push(string.Format(CultureInfo.InvariantCulture, "'{0}'", expression.ConstantValue), int.MaxValue);
                }
                else if (typeof(T) == typeof(Number))
                {
                    Push(string.Format(CultureInfo.InvariantCulture, "{0}", expression.ConstantValue), int.MaxValue);
                }
                else
                {
                    throw new InvalidOperationException("Not supported constant type: " + expression.ConstantValue);
                }
            }

            public void Visit(EqualExp expression)
            {
                HandleBinaryOperator("=", 2);
            }

            public void Visit(GreaterExp expression)
            {
                HandleBinaryOperator(">", 3);
            }

            public void Visit(GreaterOrEqualExp expression)
            {
                HandleBinaryOperator(">=", 3);
            }

            public void Visit(HasAttributeExp expression)
            {
                HandleFunction("has");
            }

            public void Visit(LessExp expression)
            {
                HandleBinaryOperator("<", 3);
            }

            public void Visit(LessOrEqualExp expression)
            {
                HandleBinaryOperator("<=", 3);
            }

            public void Visit(NotExp expression)
            {
                var arg = _stack.Pop();
                var expr = string.Format("~{0}", arg);
                Push(expr, 6);
            }

            public void Visit(OrExp expression)
            {
                HandleBinaryOperator("or", 0);
            }

            public void Visit(MulExp expression)
            {
                HandleBinaryOperator("*", 5);
            }

            public void Visit(SubExp expression)
            {
                HandleBinaryOperator("-", 4);
            }

            public void Visit(DivExp expression)
            {
                HandleBinaryOperator("/", 5);
            }

            public void Visit(ConvertToNumberExp expression)
            {
                HandleFunction("num");
            }

            public void Visit(ConvertToBooleanExp expression)
            {
                HandleFunction("bool");
            }

            private void HandleFunction(string name)
            {
                var arg = Pop(int.MinValue);
                var expr = string.Format("{0}({1})", name, arg);
                Push(expr, int.MaxValue);
            }

            private void HandleBinaryOperator(string op, int precedence)
            {
                var right = Pop(precedence);
                var left = Pop(precedence);
                var expr = string.Format("{0} {1} {2}", left, op, right);
                Push(expr, precedence);
            }

            private void Push(string value, int precedence)
            {
                _stack.Push(new StackValue(value, precedence));
            }

            private string Pop(int precedence)
            {
                var value = _stack.Pop();
                if (value.Precedence < precedence)
                {
                    return "(" + value.Value + ")";
                }
                return value.Value;
            }
        }
    }
}