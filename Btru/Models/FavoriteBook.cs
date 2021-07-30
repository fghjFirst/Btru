using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Btru.Models
{
    public class FavoriteBook
    {
        public int Id { get; set; }
        [Required]
        public virtual Book Book { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}
