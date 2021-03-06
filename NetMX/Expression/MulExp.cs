﻿using System;

namespace NetMX
{
    public class MulExp : BinaryExp<Number, Number, Number>
    {
        public MulExp(IExpression<Number> left, IExpression<Number> right)
            : base(left, right)
        {
        }

        public override void Accept(IExpressionTreeVisitor visitor)
        {
            base.Accept(visitor);
            visitor.Visit(this);
        }

        public override Number Evaluate(Func<Number> leftValue, Func<Number> rightValue)
        {
            return leftValue() * rightValue();
        }
    }
}