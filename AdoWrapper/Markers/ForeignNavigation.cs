using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoWrapper.Markers
{
    /// <summary>
    /// Marks the property as foreign navigation property. Used for joining situations
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignNavigation:Attribute
    {
    }
}
