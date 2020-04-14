using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCard.Domain
{
    public class Payment
    {
        public int PayId { get; set; }
        public int ContractorId { get; set; }
        public int JobId { get; set; }
        public int Amount { get; set; }
        [Required]
        public DateTime? PayDate { get; set; }
        [Required]
        public int? CheckNo { get; set; }
    }
}
