using System.ComponentModel.DataAnnotations;

namespace MyEmp.Models
{
    public class CoreState
    {
        [Key]
        public int Id { get; set; }
        public string StateName { get; set; }
    }
}
