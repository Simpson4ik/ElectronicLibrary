using System;
using System.Collections.Generic;
using System.Text;

public class Reader
{
    public int Id { get; set; }

    // Refactoring Technique: Rename Variable.
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}
