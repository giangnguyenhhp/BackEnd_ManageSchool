using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_Web_API.Models.CoreModels;

public class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string DateOfBirth { get; set; }
    public string Gender { get; set; }
    


    public Specialization? Specialization { get; set; }
    public IEnumerable<Course>? Course { get; set; }
}