using System;
using System.Globalization;
using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace NetMX.Expression
{
    public class ExpressionParser
    {
        public static IExpression<T> Parse<T>(string expressionText)
        {
            var stream = new ANTLRStringStream(expressionText);
            var lexer = new netmxprLexer(stream);
            var tokens = new CommonTokenStream(lexer);

            var parser = new netmxprParser(tokens) { TreeAdaptor = new CommonTreeAdaptor() };
            var result = parser.parse();
            var tree = (BaseTree)result.Tree;

            return (IExpression<T>) VisitNode(tree);
        }

        public static object VisitNode(ITree node)
        {
            switch (node.Type)
            {
                case netmxprParser.AND:
                    return VisitAnd(node);
                case netmxprParser.OR:
                    return VisitOr(node);
                case netmxprParser.Number:
                    return VisitNumber(node);
                case netmxprParser.PLUS:
                    return VisitAdd(node);
                case netmxprParser.MINUS:
                    return VisitSub(node);
                case netmxprParser.MUL:
                    return VisitMul(node);
                case netmxprParser.DIV:
                    return VisitDiv(node);
                case netmxprParser.EQUALS:
                case netmxprParser.EQUAL_OBJ:
                    return VisitEquals(node);               
                case netmxprParser.MORE:
                    return VisitGreater(node);
                case netmxprParser.LESS:
                    return VisitLess(node);
                case netmxprParser.GE:
                    return VisitGreaterOrEqual(node);
                case netmxprParser.LE:
                    return VisitLessOrEqual(node);
                case netmxprParser.Literal:
                    return VisitLiteral(node);
                case netmxprParser.NOT:
                    return VisitNot(node);
                case netmxprParser.FunctionName:
                    return VisitFunction(node);
                default:
                    throw new InvalidOperationException("Invalid node: "+node);
            }
        }

        private static object VisitFunction(ITree node)
        {
            switch (node.Text)
            {
                case "attr":
                    return VisitFunctionAttr(node);
                case "has":
                    return VisitFunctionHas(node);                
                case "bool":
                    return VisitFunctionBool(node);
                case "num":
                    return VisitFunctionNumber(node);
                default:
                    throw new InvalidOperationException("Invalid function: "+node);
            }
        }

        private static object VisitFunctionAttr(ITree node)
        {
            var argumentExp = VisitChild<string>(node, 0);
            return new AttributeExp(argumentExp);
        }

        private static object VisitFunctionHas(ITree node)
        {
            var argumentExp = VisitChild<string>(node, 0);
            return new HasAttributeExp(argumentExp);
        }

        private static object VisitFunctionBool(ITree node)
        {
            var argumentExp = VisitChild<object>(node, 0);
            return new ConvertToBooleanExp(argumentExp);
        }

        private static object VisitFunctionNumber(ITree node)
        {
            var argumentExp = VisitChild<object>(node, 0);
            return new ConvertToNumberExp(argumentExp);
        }

        private static object VisitNot(ITree node)
        {
            return new NotExp(VisitChild<bool>(node, 0));
        }

        private static object VisitLiteral(ITree node)
        {
            return new ConstantExp<string>(node.Text.Trim('\'','"'));
        }

        private static object VisitGreater(ITree node)
        {
            return new GreaterExp(
                VisitChildAsNumber(node, 0),
                VisitChildAsNumber(node, 1));
        }

        private static object VisitLess(ITree node)
        {
            return new LessExp(
                VisitChildAsNumber(node, 0),
                VisitChildAsNumber(node, 1));
        }

        private static object VisitGreaterOrEqual(ITree node)
        {
            return new GreaterOrEqualExp(
                VisitChildAsNumber(node, 0),
                VisitChildAsNumber(node, 1));
        }

        private static object VisitLessOrEqual(ITree node)
        {
            return new LessOrEqualExp(
                VisitChildAsNumber(node, 0),
                VisitChildAsNumber(node, 1));
        }

        private static object VisitEquals(ITree node)
        {
            return new EqualExp(
                VisitChildAsNumber(node, 0),
                VisitChildAsNumber(node, 1));
        }        

        private static object VisitAdd(ITree node)
        {
            return new AddExp(
                VisitChildAsNumber(node, 0),
                VisitChildAsNumber(node, 1));
        }

        private static object VisitSub(ITree node)
        {
            return new SubExp(
                VisitChildAsNumber(node, 0),
                VisitChildAsNumber(node, 1));
        }

        private static object VisitMul(ITree node)
        {
            return new MulExp(
                VisitChildAsNumber(node, 0),
                VisitChildAsNumber(node, 1));
        }

        private static object VisitDiv(ITree node)
        {
            return new DivExp(
                VisitChildAsNumber(node, 0),
                VisitChildAsNumber(node, 1));
        }

        private static object VisitNumber(ITree node)
        {
            var value = decimal.Parse(node.Text, CultureInfo.InvariantCulture);
            return new ConstantExp<Number>(value);
        }

        private static object VisitAnd(ITree node)
        {
            return new AndExp(
                VisitChild<bool>(node, 0), 
                VisitChild<bool>(node, 1));
        }

        private static object VisitOr(ITree node)
        {
            return new OrExp(
                VisitChild<bool>(node, 0),
                VisitChild<bool>(node, 1));
        }

        private static IExpression<T> VisitChild<T>(ITree node, int index)
        {
            var child = node.GetChild(index);
            var result = VisitNode(child) as IExpression<T>;    
            if (result == null)
            {
                throw new InvalidOperationException(string.Format("Expecting expression of type {0}, got {1}", typeof(T).FullName, child));
            }
            return  result;
        }

        private static IExpression<Number> VisitChildAsNumber(ITree node, int index)
        {
            var childExpression = VisitChild<object>(node, index);
            var numberExpression = childExpression as IExpression<Number>;
            if (numberExpression != null)
            {
                return numberExpression;
            }
            return new ConvertToNumberExp(childExpression);
        }
    }
}