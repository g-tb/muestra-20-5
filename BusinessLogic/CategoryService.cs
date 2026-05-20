using BusinessLogic.Dtos;
using Domain;
using Repository;

namespace BusinessLogic;

public class CategoryService
{
    private readonly CategoryMemoryRepository _repository;

    public CategoryService(CategoryMemoryRepository categoryRepository)
    {
        _repository = categoryRepository;
    }
    public CategoryDTO AddMovie(CategoryDTO category)
    {
        return CategoryDTO.fromEntity(_repository.Add(category.toEntity()));
    }

    public IList<CategoryDTO> GetAll()
    {
        return _repository.FindAll().Select(x => CategoryDTO.fromEntity(x)).ToList();
    }
    
    public CategoryDTO FindById(int id)
    {
        return CategoryDTO.fromEntity(_repository.Find(x => x.Id == id));
    }

    public CategoryDTO Update(CategoryDTO updatedCategory)
    {
        return CategoryDTO.fromEntity(_repository.Update(updatedCategory.toEntity()));
    }

    public void Delete(int id)
    {
        _repository.Delete(id);
    }
}