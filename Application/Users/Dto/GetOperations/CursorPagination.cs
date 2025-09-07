using Domain.Sort;

namespace Application.Users.Dto.GetOperations;

/// <summary>
/// Generic cursor pagination for items
/// </summary>
/// <typeparam name="TId">Id of the entity (used as a cursor)</typeparam>
public readonly record struct CursorPagination<T, TId> where T : ICursorSortable<TId>
{
    public IEnumerable<T> Items { get; }
    public TId? NextCursor { get; }
    public DateTime? NextCursorDate { get; }

    public bool HasNextCursor => NextCursor is not null && NextCursorDate is not null;

    public CursorPagination(IEnumerable<T>? items, int maxPageSize)
    {
        var list = items?.ToList() ?? [];

        var hasNextCursor = list.Count > maxPageSize;

        NextCursor = hasNextCursor ? list[^1].Id : default;
        NextCursorDate = hasNextCursor ? list[^1].CreatedAt : null;

        if (hasNextCursor)
        {
            list.RemoveAt(list.Count - 1);
        }

        Items = list;
    }

    private CursorPagination(
        IEnumerable<T> items, 
        TId? nextCursor,
        DateTime? nextCursorDate)
    {
        Items = items;
        NextCursor = nextCursor;
        NextCursorDate = nextCursorDate;
    }

    public CursorPagination<U, TId> MapUsing<U>(Func<T, U> converter) where U : ICursorSortable<TId>
    {
        var items = Items.Select(converter);

        return new CursorPagination<U, TId>(items, NextCursor, NextCursorDate);
    }
}
