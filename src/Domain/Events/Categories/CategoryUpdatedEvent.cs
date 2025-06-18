namespace Backend.Domain.Events.Categories;

public class CategoryUpdatedEvent(Category item) : BaseEvent
{
    public Category Item { get; } = item;
}
