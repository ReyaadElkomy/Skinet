using Core.Entities;
using Core.Specifications;

namespace Infrastructure.Specifications;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        if(spec.Criteria is not null)
            inputQuery = inputQuery.Where(spec.Criteria); // ex: x => x.Brand == brand or x => x.Type == type
        
        if(spec.OrderBy is not null)
            inputQuery = inputQuery.OrderBy(spec.OrderBy);

        if(spec.OrderByDescending is not null)
            inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);
        
        if(spec.IsDistinct)
            inputQuery = inputQuery.Distinct();

        if(spec.IsPagingEnabled)
            inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);

        return inputQuery;
    }

    public static IQueryable<TResult> GetQuery<TSpec,TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> spec)
    {
        if(spec.Criteria is not null)
            inputQuery = inputQuery.Where(spec.Criteria); // ex: x => x.Brand == brand or x => x.Type == type
        
        if(spec.OrderBy is not null)
            inputQuery = inputQuery.OrderBy(spec.OrderBy);

        if(spec.OrderByDescending is not null)
            inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);
        
        var selectQuery = inputQuery as IQueryable<TResult>;
        if(spec.Select is not null)
            selectQuery = inputQuery.Select(spec.Select);
            
        if(spec.IsDistinct)
            selectQuery = selectQuery?.Distinct();

        if(spec.IsPagingEnabled)
            selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);

        return selectQuery ?? inputQuery.Cast<TResult>();
    }
}
