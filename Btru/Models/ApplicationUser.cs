using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Btru.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ReadingAssignment> Assignments { get; set; }
        public ICollection<FavoriteBook> Favorites { get; set; }
        public ICollection<SleepSchedule> SleepLogs { get; set; }
        [Column(TypeName = "date")]
        public DateTime LastOnline { get; set; }
        [DefaultValue(0)]
        public int UniqueReads { get; set; }
    }
}
