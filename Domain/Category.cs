namespace Domain;

public class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }
    
    public void Update(Category updatedCategory)
    {
        Id = updatedCategory.Id;
        Name = updatedCategory.Name;
    }
}