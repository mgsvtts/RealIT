namespace Domain.Sort;

public interface ICursorSortable<T>
{
    public T Id { get; }
    public DateTime CreatedAt { get; }
}