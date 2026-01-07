﻿

namespace DefaultNamespace;

using System;
using System.Collections.Generic;
using System.Linq;

public class ServiceReservation
{
    private readonly List<Reservation> _list = new();
    private readonly object _lock = new();

    public Reservation CreateReservation(Guid ressourceId, string clientId, DateTime start, DateTime end, string? notes = null)
    {
        var r = new Reservation(ressourceId, clientId, start, end, notes);
        r.Validate();

        lock (_lock)
        {
            // check conflicts
            if (_list.Any(existing => existing.ConflictsWith(r)))
            {
                throw new InvalidOperationException("Reservation conflict for the chosen resource and time.");
            }

            _list.Add(r);
        }

        return r;
    }

    public IEnumerable<Reservation> GetAll()
    {
        lock (_lock) { return _list.ToList(); }
    }

    public IEnumerable<Reservation> GetByRessource(Guid ressourceId)
    {
        lock (_lock) { return _list.Where(x => x.RessourceId == ressourceId).ToList(); }
    }

    public Reservation? GetById(Guid id)
    {
        lock (_lock) { return _list.FirstOrDefault(r => r.Id == id); }
    }

    public bool CancelReservation(Guid id)
    {
        lock (_lock)
        {
            var r = _list.FirstOrDefault(x => x.Id == id);
            if (r == null) return false;
            r.Status = ReservationStatus.Cancelled;
            return true;
        }
    }
}
