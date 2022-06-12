using System.Linq.Expressions;

namespace Infrastructure.Shared;

public class ParameterReplaceVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _originalParameter;
    private readonly ParameterExpression _newParameter;

    public ParameterReplaceVisitor(
        ParameterExpression originalParameter,
        ParameterExpression newParameter)
    {
        this._originalParameter = originalParameter;
        this._newParameter = newParameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (node == this._originalParameter)
        {
            return this._newParameter;
        }

        return base.VisitParameter(node);
    }
}
