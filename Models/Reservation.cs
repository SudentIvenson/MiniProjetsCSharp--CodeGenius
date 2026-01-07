namespace DefaultNamespace;

using System;

public enum ReservationStatus { Confirmed, Cancelled }

public class Reservation
{
    public Guid Id { get; init; }
    public Guid RessourceId { get; set; }
    public string ClientId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;
    public string? Notes { get; set; }

    public Reservation(Guid ressourceId, string clientId, DateTime start, DateTime end, string? notes = null)
    {
        Id = Guid.NewGuid();
        RessourceId = ressourceId;
        ClientId = clientId;
        Start = start;
        End = end;
        Notes = notes;
    }

    public void Validate()
    {
        if (Start >= End) throw new ArgumentException("Start must be before End");
    }

    public bool ConflictsWith(Reservation other)
    {
        if (other == null) return false;
        if (RessourceId != other.RessourceId) return false;
        if (Status == ReservationStatus.Cancelled || other.Status == ReservationStatus.Cancelled) return false;
        return Start < other.End && other.Start < End; // overlap
    }

    public override string ToString()
    {
        return $"Reservation {Id} - Ressource:{RessourceId} Client:{ClientId} {Start} -> {End} ({Status})";
    }
}