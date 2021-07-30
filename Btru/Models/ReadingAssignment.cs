using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Btru.Models
{
    public class ReadingAssignment
    {
        public int Id { get; set; }
        [Required]
        public virtual Book Book { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }
        [DefaultValue(false)]
        public bool Read { get; set; }
        [DefaultValue(false)]
        public bool Reading { get; set; }

    }
}
