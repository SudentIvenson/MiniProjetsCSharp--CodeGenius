namespace DefaultNamespace;

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq;
using DefaultNamespace;

public class MenuPrincipal
{
    private readonly RessourceService _ressourceService;
    private readonly ServiceReservation _reservationService;
    private readonly ConfigurationService _configurationService;
    private readonly ClientService _clientService;

    public MenuPrincipal(RessourceService ressourceService, ServiceReservation reservationService, ConfigurationService configurationService, ClientService clientService)
    {
        _ressourceService = ressourceService;
        _reservationService = reservationService;
        _configurationService = configurationService;
        _clientService = clientService;
    }

    public void Run()
    {
        while (true)
        {
            PrintBorder("MENU PRINCIPAL");
            Console.WriteLine("1) Gestion");
            Console.WriteLine("2) Configuration");
            Console.WriteLine("0) Quitter");
            PrintSeparator();
            Console.Write("Choix: ");
            var key = Console.ReadLine();

            try
            {
                switch (key)
                {
                    case "1": GestionMenu(); break;
                    case "2": ConfigurationMenu(); break;
                    case "0": return;
                    default: Console.WriteLine("Choix invalide"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur: {ex.Message}");
            }
        }
    }

    private void ConfigurationMenu()
    {
        while (true)
        {
            var cfg = _configurationService.GetConfig();
            PrintBorder("CONFIGURATION");
            Console.WriteLine($"1) DateFormat : {cfg.DateFormat}");
            Console.WriteLine($"2) Persistence : {(cfg.EnablePersistence ? "activée" : "désactivée")}");
            Console.WriteLine("3) Sauvegarder");
            Console.WriteLine("0) Retour");
            PrintSeparator();
            Console.Write("Choix: ");
            var key = Console.ReadLine();

            switch (key)
            {
                case "1":
                    Console.Write("Nouveau format date (ex: yyyy-MM-dd HH:mm): ");
                    var fmt = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(fmt))
                    {
                        var newFmt = fmt.Trim();
                        _configurationService.Update(c => c.DateFormat = newFmt);
                        Console.WriteLine("Format mis à jour.");
                    }
                    break;
                case "2":
                    Console.Write("Activer la persistence ? (o/n): ");
                    var yn = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(yn) && (yn.Trim().ToLower() == "o" || yn.Trim().ToLower() == "y"))
                        _configurationService.Update(c => c.EnablePersistence = true);
                    else
                        _configurationService.Update(c => c.EnablePersistence = false);
                    Console.WriteLine("Paramètre mis à jour.");
                    break;
                case "3":
                    _configurationService.Save();
                    Console.WriteLine("Configuration sauvegardée.");
                    break;
                case "0": return;
                default: Console.WriteLine("Choix invalide"); break;
            }
        }
    }

    private void GestionMenu()
    {
        while (true)
        {
            PrintBorder("GESTION");
            Console.WriteLine("1) Clients");
            Console.WriteLine("2) Ressources");
            Console.WriteLine("3) Réservations");
            Console.WriteLine("0) Retour");
            PrintSeparator();
            Console.Write("Choix: ");
            var key = Console.ReadLine();

            switch (key)
            {
                case "1": ClientsMenu(); break;
                case "2": RessourcesMenu(); break;
                case "3": ReservationsMenu(); break;
                case "0": return;
                default: Console.WriteLine("Choix invalide"); break;
            }
        }
    }

    private void ClientsMenu()
    {
        while (true)
        {
            PrintBorder("GESTION - CLIENTS");
            Console.WriteLine("1) Créer client");
            Console.WriteLine("2) Lister clients");
            Console.WriteLine("3) Éditer client");
            Console.WriteLine("4) Supprimer client");
            Console.WriteLine("0) Retour");
            PrintSeparator();
            Console.Write("Choix: ");
            var key = Console.ReadLine();

            switch (key)
            {
                case "1": CreateClient(); break;
                case "2": ListClients(); break;
                case "3": EditClient(); break;
                case "4": DeleteClient(); break;
                case "0": return;
                default: Console.WriteLine("Choix invalide"); break;
            }
        }
    }

    private void RessourcesMenu()
    {
        while (true)
        {
            PrintBorder("GESTION - RESSOURCES");
            Console.WriteLine("1) Créer ressource");
            Console.WriteLine("2) Lister ressources");
            Console.WriteLine("0) Retour");
            PrintSeparator();
            Console.Write("Choix: ");
            var key = Console.ReadLine();

            switch (key)
            {
                case "1": CreateRessource(); break;
                case "2": ListRessources(); break;
                case "0": return;
                default: Console.WriteLine("Choix invalide"); break;
            }
        }
    }

    private void ReservationsMenu()
    {
        while (true)
        {
            PrintBorder("GESTION - RÉSERVATIONS");
            Console.WriteLine("1) Créer réservation");
            Console.WriteLine("2) Lister réservations");
            Console.WriteLine("3) Annuler réservation");
            Console.WriteLine("0) Retour");
            PrintSeparator();
            Console.Write("Choix: ");
            var key = Console.ReadLine();

            switch (key)
            {
                case "1": CreateReservation(); break;
                case "2": ListReservations(); break;
                case "3": CancelReservation(); break;
                case "0": return;
                default: Console.WriteLine("Choix invalide"); break;
            }
        }
    }

    // Helper d'affichage
    private void PrintBorder(string title)
    {
        var width = Math.Max(40, title.Length + 4);
        var pad = (width - title.Length - 2) / 2;
        Console.WriteLine(new string('=', width));
        Console.WriteLine(new string(' ', pad) + " " + title + " " + new string(' ', pad));
        Console.WriteLine(new string('=', width));
    }

    private void PrintSeparator()
    {
        Console.WriteLine(new string('-', 40));
    }

    private void CreateClient()
    {
        try
        {
            // Clear screen to make the form obvious
            try { Console.Clear(); } catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Console.Clear failed: {ex.Message}"); }
            PrintBorder("CRÉER CLIENT");
            Console.Out.Flush();

            string first;
            do
            {
                Console.WriteLine("-- Saisir le prénom du client --");
                Console.Write("> ");
                Console.Out.Flush();
                first = Console.ReadLine() ?? string.Empty;
                if (first.Trim() == "0") { Console.WriteLine("Création annulée."); return; }
                if (string.IsNullOrWhiteSpace(first)) Console.WriteLine("Prénom requis, réessayer.");
            } while (string.IsNullOrWhiteSpace(first));

            string last;
            do
            {
                Console.WriteLine("-- Saisir le nom du client --");
                Console.Write("> ");
                Console.Out.Flush();
                last = Console.ReadLine() ?? string.Empty;
                if (last.Trim() == "0") { Console.WriteLine("Création annulée."); return; }
                if (string.IsNullOrWhiteSpace(last)) Console.WriteLine("Nom requis, réessayer.");
            } while (string.IsNullOrWhiteSpace(last));

            // Email: format attendu example@example.com
            string? email;
            while (true)
            {
                Console.WriteLine("Email (optionnel, Enter pour passer). Format attendu: exemple@domaine.tld :");
                Console.Write("> ");
                Console.Out.Flush();
                email = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(email)) { email = null; break; }
                if (ConstraintService.IsValidEmail(email)) break;
                Console.WriteLine("Email invalide, réessayer ou Enter pour passer.");
            }

            // Phone: format attendu xxxx-xx-xx (8 chiffres), le système affichera +509 ...
            string? phone;
            while (true)
            {
                Console.WriteLine("Phone (optionnel, Enter pour passer). Format attendu: xxxx-xx-xx (8 chiffres) :");
                Console.Write("> ");
                Console.Out.Flush();
                phone = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(phone)) { phone = null; break; }
                if (ConstraintService.IsValidPhone(phone)) break;
                Console.WriteLine("Phone invalide, réessayer ou Enter pour passer.");
            }

            var c = _clientService.Create(first.Trim(), last.Trim(), email == null ? null : email.Trim(), phone == null ? null : phone.Trim());
            PrintSeparator();
            Console.WriteLine($"Client créé:");
            Console.WriteLine($"Prenom: {c.FirstName}");
            Console.WriteLine($"Nom: {c.LastName}");
            Console.WriteLine($"ID client: {c.Id}");
            Console.WriteLine($"Email: {(ConstraintService.IsValidEmail(c.Email) ? c.Email : (string.IsNullOrWhiteSpace(c.Email) ? "(aucun)" : "(invalide)"))}");
            Console.WriteLine($"Phone: {ConstraintService.FormatPhone(c.Phone)}");
            PrintSeparator();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur pendant la création du client: {ex.GetType().Name} - {ex.Message}");
        }
    }

    private void ListClients()
    {
        PrintBorder("LISTE DES CLIENTS");
        var all = _clientService.GetAll().ToList();
        if (!all.Any()) { Console.WriteLine("(Aucun client)"); PrintSeparator(); return; }
        foreach (var c in all)
        {
            Console.WriteLine($"Prenom: {c.FirstName}");
            Console.WriteLine($"Nom: {c.LastName}");
            Console.WriteLine($"Id: {c.Id}");
            Console.WriteLine($"Email: {(ConstraintService.IsValidEmail(c.Email) ? c.Email : (string.IsNullOrWhiteSpace(c.Email) ? "(aucun)" : "(invalide)"))}");
            Console.WriteLine($"Phone: {ConstraintService.FormatPhone(c.Phone)}");
            Console.WriteLine(new string('-', 20));
        }
        PrintSeparator();
    }

    private void EditClient()
    {
        Console.Write("Id client à éditer: ");
        var idS = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(idS)) { Console.WriteLine("Id invalide"); return; }
        var id = idS.Trim().ToUpper();
        if (!Regex.IsMatch(id, @"^[A-Z]{2}\d{3}$")) { Console.WriteLine("Id invalide (format attendu: LLDDD)"); return; }
        var c = _clientService.GetById(id);
        if (c == null) { Console.WriteLine("Client introuvable"); return; }

        Console.Write($"Prénom ({c.FirstName}): ");
        var first = Console.ReadLine();
        Console.Write($"Nom ({c.LastName}): ");
        var last = Console.ReadLine();

        // Email: validate format or keep existing
        string? email;
        while (true)
        {
            Console.Write($"Email ({c.Email ?? "(aucun)"}): ");
            email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email)) { email = null; break; } // keep existing
            if (ConstraintService.IsValidEmail(email)) break;
            Console.WriteLine("Email invalide, réessayer ou Enter pour conserver la valeur actuelle.");
        }

        // Phone: validate or keep existing
        string? phone;
        while (true)
        {
            Console.Write($"Phone ({ConstraintService.FormatPhone(c.Phone)}): ");
            phone = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(phone)) { phone = null; break; } // keep existing
            if (ConstraintService.IsValidPhone(phone)) break;
            Console.WriteLine("Phone invalide, réessayer ou Enter pour conserver la valeur actuelle.");
        }

        if (!string.IsNullOrWhiteSpace(first)) c.FirstName = first;
        if (!string.IsNullOrWhiteSpace(last)) c.LastName = last;
        if (!string.IsNullOrWhiteSpace(email)) c.Email = email;
        if (!string.IsNullOrWhiteSpace(phone)) c.Phone = phone;

        var ok = _clientService.Update(c);
        Console.WriteLine(ok ? "Client mis à jour" : "Échec mise à jour");
    }

    private void DeleteClient()
    {
        Console.Write("Id client à supprimer: ");
        var idS = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(idS)) { Console.WriteLine("Id invalide"); return; }
        var id = idS.Trim().ToUpper();
        if (!Regex.IsMatch(id, @"^[A-Z]{2}\d{3}$")) { Console.WriteLine("Id invalide (format attendu: LLDDD)"); return; }
        var ok = _clientService.Delete(id);
        Console.WriteLine(ok ? "Client supprimé" : "Client introuvable");
    }

    private void CreateRessource()
    {
        Console.Write("Nom ressource: ");
        var name = Console.ReadLine() ?? string.Empty;
        Console.Write("Type: ");
        var type = Console.ReadLine() ?? string.Empty;
        Console.Write("Capacité (entier): ");
        var capS = Console.ReadLine();
        int cap;
        if (!int.TryParse(capS, out cap)) cap = 1;
        var r = _ressourceService.Create(name, type, cap);
        PrintSeparator();
        Console.WriteLine($"Ressource créée: {r}");
        PrintSeparator();
    }

    private void ListRessources()
    {
        PrintBorder("LISTE DES RESSOURCES");
        var all = _ressourceService.GetAll().ToList();
        if (!all.Any()) { Console.WriteLine("(Aucune ressource)"); PrintSeparator(); return; }
        foreach (var r in all)
        {
            Console.WriteLine($"Nom: {r.Name}");
            Console.WriteLine($"Type: {r.Type}");
            Console.WriteLine($"Capacité: {r.Capacity}");
            Console.WriteLine($"Active: {(r.IsActive ? "oui" : "non")}");
            Console.WriteLine($"Id: {r.Id}");
            Console.WriteLine(new string('-', 20));
        }
        PrintSeparator();
    }

    private void CreateReservation()
    {
        var cfg = _configurationService.GetConfig();
        var fmt = string.IsNullOrWhiteSpace(cfg.DateFormat) ? "yyyy-MM-dd HH:mm" : cfg.DateFormat;

        Console.Write("Id client (ou 'l' pour lister): ");
        var clientS = Console.ReadLine();
        if (string.Equals(clientS, "l", StringComparison.OrdinalIgnoreCase))
        {
            ListClients();
            Console.Write("Id client: ");
            clientS = Console.ReadLine();
        }

        Console.Write("Id ressource (ou 'l' pour lister): ");
        var resS = Console.ReadLine();
        if (string.Equals(resS, "l", StringComparison.OrdinalIgnoreCase))
        {
            ListRessources();
            Console.Write("Id ressource: ");
            resS = Console.ReadLine();
        }

        Console.Write($"Start ({fmt}): ");
        var startS = Console.ReadLine();
        Console.Write($"End ({fmt}): ");
        var endS = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(clientS)) { Console.WriteLine("ClientId invalide"); return; }
        var clientId = clientS.Trim().ToUpper();
        if (!Regex.IsMatch(clientId, @"^[A-Z]{2}\d{3}$")) { Console.WriteLine("ClientId invalide (format LLDDD attendu)"); return; }

        if (!Guid.TryParse(resS, out var resId)) { Console.WriteLine("RessourceId invalide"); return; }

        var client = _clientService.GetById(clientId);
        if (client == null) { Console.WriteLine("Client introuvable"); return; }

        var res = _ressourceService.GetById(resId);
        if (res == null) { Console.WriteLine("Ressource introuvable"); return; }

        if (!DateTime.TryParseExact(startS, fmt, CultureInfo.InvariantCulture, DateTimeStyles.None, out var start)) { Console.WriteLine("Start invalide"); return; }
        if (!DateTime.TryParseExact(endS, fmt, CultureInfo.InvariantCulture, DateTimeStyles.None, out var end)) { Console.WriteLine("End invalide"); return; }

        try
        {
            var reservation = _reservationService.CreateReservation(resId, clientId, start, end);
            PrintSeparator();
            Console.WriteLine($"Réservation créée: {reservation}");
            PrintSeparator();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur création réservation: {ex.Message}");
        }
    }

    private void ListReservations()
    {
        PrintBorder("LISTE DES RÉSERVATIONS");
        var all = _reservationService.GetAll().ToList();
        if (!all.Any()) { Console.WriteLine("(Aucune réservation)"); PrintSeparator(); return; }
        foreach (var r in all)
        {
            Console.WriteLine($"Id: {r.Id}");
            Console.WriteLine($"RessourceId: {r.RessourceId}");
            Console.WriteLine($"ClientId: {r.ClientId}");
            Console.WriteLine($"Start: {r.Start}");
            Console.WriteLine($"End: {r.End}");
            Console.WriteLine($"Status: {r.Status}");
            Console.WriteLine($"Notes: {r.Notes ?? "(aucune)"}");
            Console.WriteLine(new string('-', 20));
        }
        PrintSeparator();
    }

    private void CancelReservation()
    {
        Console.Write("Id réservation: ");
        var idS = Console.ReadLine();
        if (!Guid.TryParse(idS, out var id)) throw new ArgumentException("Id invalide");
        var ok = _reservationService.CancelReservation(id);
        PrintSeparator();
        Console.WriteLine(ok ? "Réservation annulée" : "Réservation introuvable");
        PrintSeparator();
    }
}