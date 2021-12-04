using System.Collections.Generic;
using AdoWrapper.Markers;

namespace Web.Models
{
    public class SingleAuthor
    {
        public int ID { get; set; }
        public string AuthorName { get; set; }

        [ForeignNavigation]
        public List<Book> Books { get; set; }
    }
}
