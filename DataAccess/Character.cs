using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Character
    {
        public int Id { get; set; }
        public int? ActorId { get; set; }
        public string Name { get; set; } = null!;
        public string Species { get; set; } = null!;
        public string? Planet { get; set; }

        public virtual Actor? Actor { get; set; }
    }
}
