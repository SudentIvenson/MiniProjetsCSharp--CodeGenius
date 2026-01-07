namespace DefaultNamespace;

using System;

public class Client
{
    public string Id { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public Client(string firstName, string lastName, string? email = null, string? phone = null, string? id = null)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Email = email;
        Phone = phone;
        Id = id ?? GenerateId(firstName, lastName);
    }

    private static string GenerateId(string firstName, string lastName)
    {
        var f = string.IsNullOrWhiteSpace(firstName) ? 'X' : char.ToUpper(firstName[0]);
        var l = string.IsNullOrWhiteSpace(lastName) ? 'X' : char.ToUpper(lastName[0]);
        var rnd = new Random();
        var num = rnd.Next(0, 1000);
        return $"{f}{l}{num:000}";
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName)) throw new ArgumentException("FirstName required");
        if (string.IsNullOrWhiteSpace(LastName)) throw new ArgumentException("LastName required");
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName} [{Id}] - {Email ?? ""} {Phone ?? ""}";
    }
}