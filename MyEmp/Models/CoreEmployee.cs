using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEmp.Models
{
    [Table("employee")]
    public class CoreEmployee
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime ? DOJ { get; set; }
        public int StateId { get; set; }
        public string? Gender { get; set; }

        public Boolean IsDeleted { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
    }


}
