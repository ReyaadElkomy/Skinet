using System.Dynamic;
using System.Linq.Expressions;

namespace Core.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    protected BaseSpecification(): this(null) {}

    private readonly Expression<Func<T, bool>>? _criteria;
    public BaseSpecification(Expression<Func<T, bool>>? criteria)
    {
        _criteria = criteria;
    }
    
    public Expression<Func<T, bool>>? Criteria => _criteria;

    public Expression<Func<T, object>>? OrderBy {get; private set;}

    public Expression<Func<T, object>>? OrderByDescending {get; private set;}

    public bool IsDistinct {get; private set;}

    public int Take  {get; private set;}

    public int Skip  {get; private set;}

    public bool IsPagingEnabled  {get; private set;}

    protected void AddOrderBy(Expression<Func<T, object>> ordrByExpression)
    {
        OrderBy = ordrByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> ordrByDescExpression)
    {
        OrderByDescending = ordrByDescExpression;
    }

    protected void ApplyDistinct()
    {
        IsDistinct = true;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        if(Criteria is not null)
            query = query.Where(Criteria);

        return query;
    }
}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria) 
            : BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    protected BaseSpecification(): this(null!) {}
    public Expression<Func<T, TResult>>? Select {get; private set;}
    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}
