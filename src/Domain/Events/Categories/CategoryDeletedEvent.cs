namespace Backend.Domain.Events.Categories;

public class CategoryDeletedEvent(Category item) : BaseEvent
{
    public Category Item { get; } = item;
}
