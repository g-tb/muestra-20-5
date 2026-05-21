using System.Linq.Expressions;
using Domain;

namespace Repository;

public interface ICategoryRepository
{
    Category Add(Category category);
    Category? Find(Expression<Func<Category, bool>> filter);
    IList<Category> FindAll();
    Category? Update(Category updatedEntity);
    void Delete(int id);
}
