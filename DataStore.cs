using System;
using System.Collections.Generic;

namespace EventPassMX_programacion
{
    public class Evento
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        
    }

    public class Ticket
    {
        public Guid Id { get; set; }
        public string Usuario { get; set; }
        public Evento Evento { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal Precio { get; set; }
    }

    public static class DataStore
    {
        
        private static readonly List<User> _userList = new List<User>();
        public static IReadOnlyList<User> Users => _userList.AsReadOnly();

        
        public static List<Evento> Eventos { get; } = new List<Evento>
        {
            new Evento { Nombre = "Concierto A", Precio = 550.00m },
            new Evento { Nombre = "Teatro B", Precio = 320.50m },
            new Evento { Nombre = "Festival C", Precio = 790.00m }
        };

       
        private static readonly List<Ticket> _tickets = new List<Ticket>();

        public static bool AddUser(string username, string password)
        {
            return AddUser(new User(username, password));
        }

        public static bool AddUser(User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                return false;

            if (_userList.Exists(u => string.Equals(u.Username, user.Username, StringComparison.OrdinalIgnoreCase)))
                return false;

            _userList.Add(user);
            return true;
        }

        public static bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            var u = _userList.Find(x => string.Equals(x.Username, username, StringComparison.OrdinalIgnoreCase));
            return u != null && u.Password == password;
        }

        public static User GetUser(string username)
        {
            return _userList.Find(x => string.Equals(x.Username, username, StringComparison.OrdinalIgnoreCase));
        }

        public static Ticket CreateTicket(string username, Evento evento)
        {
            if (string.IsNullOrWhiteSpace(username) || evento == null)
                return null;

            var t = new Ticket
            {
                Id = Guid.NewGuid(),
                Usuario = username,
                Evento = evento,
                FechaCompra = DateTime.Now,
                Precio = evento.Precio
            };
            _tickets.Add(t);
            return t;
        }

        public static List<Ticket> GetTicketsForUser(string username)
        {
            return _tickets.FindAll(x => x.Usuario == username);
        }
    }
}
