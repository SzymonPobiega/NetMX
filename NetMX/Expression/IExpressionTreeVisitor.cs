namespace NetMX
{
    public interface IExpressionTreeVisitor
    {
        void Visit(AddExp expression);
        void Visit(AndExp expression);
        void Visit(AttributeExp expression);        
        void Visit<T>(ConstantExp<T> expression);
        void Visit(EqualExp expression);
        void Visit(GreaterExp expression);
        void Visit(GreaterOrEqualExp expression);
        void Visit(HasAttributeExp expression);
        void Visit(LessExp expression);
        void Visit(LessOrEqualExp expression);
        void Visit(NotExp expression);
        void Visit(OrExp expression);
        void Visit(MulExp expression);
        void Visit(SubExp expression);
        void Visit(DivExp expression);
        void Visit(ConvertToNumberExp expression);
        void Visit(ConvertToBooleanExp expression);
    }
}