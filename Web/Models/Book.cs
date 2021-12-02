using AdoWrapper.Markers;

namespace Web.Models
{
    public class Book
    {
        public int Id { get; set; }

        [ForeignKeyNavigation(nameof(Author.ID))]
        public int AuthorId { get; set; }
        public string Name { get; set; }
    }
}