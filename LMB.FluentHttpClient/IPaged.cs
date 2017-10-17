namespace LMB.FluentHttpClient
{
    /// <summary>
    /// Represents an query that can be paged.
    /// </summary>
    public interface IPaged
    {
        /// <summary>
        /// Total amount of posts for the query.
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Amount of pages that exist for the given Page and PageSize combination.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Current page.
        /// </summary>
        int Page { get; }

        /// <summary>
        /// Amount of posts for the current page.
        /// </summary>
        int PageSize { get; }
    }
}
