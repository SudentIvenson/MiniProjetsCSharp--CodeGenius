namespace DefaultNamespace;

using System;
using System.Collections.Generic;
using System.Linq;

public class RessourceService
{
    private readonly List<Ressource> _list = new();
    private readonly object _lock = new();

    public Ressource Create(string name, string type, int capacity = 1)
    {
        var r = new Ressource(name, type, capacity);
        r.Validate();
        lock (_lock)
        {
            _list.Add(r);
        }
        return r;
    }

    public IEnumerable<Ressource> GetAll()
    {
        lock (_lock) { return _list.ToList(); }
    }

    public Ressource? GetById(Guid id)
    {
        lock (_lock) { return _list.FirstOrDefault(r => r.Id == id); }
    }

    public bool Update(Ressource updated)
    {
        updated.Validate();
        lock (_lock)
        {
            var idx = _list.FindIndex(r => r.Id == updated.Id);
            if (idx < 0) return false;
            _list[idx] = updated;
            return true;
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var r = _list.FirstOrDefault(x => x.Id == id);
            if (r == null) return false;
            return _list.Remove(r);
        }
    }

    public IEnumerable<Ressource> FindByType(string type)
    {
        lock (_lock) { return _list.Where(r => string.Equals(r.Type, type, StringComparison.OrdinalIgnoreCase)).ToList(); }
    }
}