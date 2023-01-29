using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson09.DAL.Entities
{
    [Table("Employees")]
    public class Employee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Patronymic { get; set; } = null!;
        public DateTime Birthday { get; set; }
    }
}
