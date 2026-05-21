using System.Linq.Expressions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CategoryEfRepository : ICategoryRepository
{
    private readonly MovieManagerDbContext _context;

    public CategoryEfRepository(MovieManagerDbContext context)
    {
        _context = context;
    }

    public Category Add(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
        return category;
    }

    public Category? Find(Expression<Func<Category, bool>> filter)
    {
        return _context.Categories.FirstOrDefault(filter);
    }

    public IList<Category> FindAll()
    {
        return _context.Categories.ToList();
    }

    public Category? Update(Category updatedEntity)
    {
        Category? existingCategory = _context.Categories.FirstOrDefault(category => category.Id == updatedEntity.Id);
        if (existingCategory is null)
        {
            return null;
        }

        existingCategory.Update(updatedEntity);
        _context.SaveChanges();
        return existingCategory;
    }

    public void Delete(int id)
    {
        Category? existingCategory = _context.Categories.FirstOrDefault(category => category.Id == id);
        if (existingCategory is null)
        {
            return;
        }

        _context.Categories.Remove(existingCategory);
        _context.SaveChanges();
    }
}
