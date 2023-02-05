﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_Web_API.Models.CoreModels;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Credit { get; set; }
    
    
    public IEnumerable<Student>? Students { get; set; }
    public IEnumerable<Teacher>? Teacher { get; set; }
}