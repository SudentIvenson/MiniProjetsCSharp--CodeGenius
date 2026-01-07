namespace DefaultNamespace;

using System;
using System.Collections.Generic;
using System.Linq;

public class ClientService
{
    private readonly List<Client> _list = new();
    private readonly object _lock = new();

    public Client Create(string firstName, string lastName, string? email = null, string? phone = null)
    {
        var c = new Client(firstName, lastName, email, phone);
        c.Validate();
        lock (_lock)
        {
            _list.Add(c);
        }
        return c;
    }

    public IEnumerable<Client> GetAll()
    {
        lock (_lock) { return _list.ToList(); }
    }

    public Client? GetById(string id)
    {
        lock (_lock) { return _list.FirstOrDefault(x => x.Id == id); }
    }

    public bool Update(Client updated)
    {
        updated.Validate();
        lock (_lock)
        {
            var idx = _list.FindIndex(x => x.Id == updated.Id);
            if (idx < 0) return false;
            _list[idx] = updated;
            return true;
        }
    }

    public bool Delete(string id)
    {
        lock (_lock)
        {
            var c = _list.FirstOrDefault(x => x.Id == id);
            if (c == null) return false;
            return _list.Remove(c);
        }
    }
}
