namespace DefaultNamespace; 

using System;
//  
public class Ressource
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; } = true;

    public Ressource(string name, string type, int capacity = 1)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Type = type ?? "";
        Capacity = Math.Max(1, capacity);
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Name required");
        if (Capacity < 1) throw new ArgumentException("Capacity must be >= 1");
    }

    public override string ToString()
    {
        return $"{Name} ({Type}) - cap:{Capacity} - {(IsActive ? "active" : "inactive")} - {Id}";
    }
}