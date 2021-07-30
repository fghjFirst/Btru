using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Btru.Models
{
    public class SleepSchedule
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Column(TypeName = "time")]
        public TimeSpan? WokeUp { get; set; }
        [Column(TypeName = "time")]
        public TimeSpan? WentToSleep { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
