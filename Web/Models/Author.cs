using System;
using System.Collections.Generic;
using AdoWrapper.Markers;

namespace Web.Models
{
    public class Author:IEquatable<Author>
    {
        public int ID { get; set; }
        public string AuthorName { get; set; }

        [ForeignNavigation]
        public List<Book> Books { get; set; }

        public bool Equals(Author? other)
        {
            if (other is null)
                return false;

            return this.ID == other.ID;
        }
    }
}