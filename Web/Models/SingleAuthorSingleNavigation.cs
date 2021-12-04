using AdoWrapper.Markers;

namespace Web.Models
{
    public class SingleAuthorSingleNavigation
    {
        public int ID { get; set; }
        public string AuthorName { get; set; }

        [ForeignNavigation]
        public Book SingleBook { get; set; }
    }
}
