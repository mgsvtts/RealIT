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

    public CursorPagination(IEnumerable<T>? items, int page)
    {
        var list = items?.ToList() ?? [];

        var hasNextCursor = list.Count > page;

        NextCursor = hasNextCursor ? list[^1].Id : default;
        NextCursorDate = hasNextCursor ? list[^1].CreatedAt : null;

        if (hasNextCursor)
        {
            list.RemoveAt(list.Count - 1);
        }

        Items = list;
    }
}
