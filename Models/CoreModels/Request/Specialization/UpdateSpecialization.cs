﻿namespace ASP_Web_API.Models.CoreModels.Request.Specialization;

public class UpdateSpecialization
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    
    public IEnumerable<int>? StudentIds { get; set; }
}