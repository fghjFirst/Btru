using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Btru.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }

        
        public ICollection<ReadingAssignment> AssignmentTo { get; set; }
        public ICollection<FavoriteBook> FavoriteOf { get; set; }
    }
}