namespace Backend.Domain.Events.Categories;

public class CategoryCreatedEvent(Category item) : BaseEvent
{
    public Category Item { get; } = item;
}
