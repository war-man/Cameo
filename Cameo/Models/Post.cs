using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public ApplicationUser Author { get; set; }
    }
}
