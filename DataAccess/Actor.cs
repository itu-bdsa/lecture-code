using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Actor
    {
        public Actor()
        {
            Characters = new HashSet<Character>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Character> Characters { get; set; }
    }
}
