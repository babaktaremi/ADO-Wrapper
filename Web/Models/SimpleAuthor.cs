using System;

namespace Web.Models
{
    public class SimpleAuthor:IEquatable<SimpleAuthor>
    {
        public int ID { get; set; }
        public string AuthorName { get; set; }
        public bool Equals(SimpleAuthor? other)
        {
            if (other is null)
                return false;

            return this.ID == other.ID;
        }
    }
}
