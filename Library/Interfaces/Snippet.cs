namespace Library.Interfaces
{
    public class Snippet
    {
        public bool IsFailedToFetch { get; set; }

        public Snippet(bool isFailedToFetch) =>
            IsFailedToFetch = isFailedToFetch;
    }

    public class SingleSnippet<T> : Snippet
    {
        public T Item { get; set; }

        public SingleSnippet(bool isFailedToFetch, T item) : base(isFailedToFetch) =>
            Item = item;
    }

    public class CollectionSnippet<T> : Snippet
    {
        public ICollection<T> Items { get; set; }

        public CollectionSnippet(bool isFailedToFetch, ICollection<T> items) : base(isFailedToFetch) =>
            Items = items;
    }
}   