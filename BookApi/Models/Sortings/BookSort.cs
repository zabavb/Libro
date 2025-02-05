using Library.Sortings;

namespace BookAPI.Models.Sortings
{
    public class BookSort
    {
        public Bool Title { get; set; }
        public Bool Price { get; set; }
        public Bool Year { get; set; }
        public Bool Language { get; set; }
        public Bool Cover { get; set; }
        public Bool IsAvaliable { get; set; }
    }
}
