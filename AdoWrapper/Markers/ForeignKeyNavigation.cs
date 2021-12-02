using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoWrapper.Markers
{
    /// <summary>
    /// Declares a property to be the foreign key of other property in other class with the specified name
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyNavigation:Attribute
    {
        public string ForeignKeyName { get; init; }

        public ForeignKeyNavigation(string foreignKeyName)
        {
            ForeignKeyName = foreignKeyName;
        }
    }
}
