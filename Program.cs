namespace DefaultNamespace;

using System;

class Program
{
    static void Main(string[] args)
    {
        var resService = new RessourceService();
        var reservService = new ServiceReservation();
        var configService = new ConfigurationService();
        var clientService = new ClientService();

        // exemples
        var r1 = resService.Create("Salle A", "Salle", 10);
        var r2 = resService.Create("Projecteur 1", "Materiel", 1);

        var menu = new MenuPrincipal(resService, reservService, configService, clientService);
        Console.WriteLine("Bienvenue au gestionnaire de réservations (console)");
        menu.Run();
    }
}
