using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventPassMX_programacion
{
    public class Evento
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public Artista Artista { get; set; }
        public DateTime Fecha { get; set; }
        public string Ciudad { get; set; }
        public string Categoria { get; set; }
        public bool IsSoldOut { get; set; }
        public bool IsNew { get; set; }
    }

    public class Artista
    {
        public string Nombre { get; set; }
        public string Genero { get; set; }
        public string Descripcion { get; set; }
    }

    public class Ticket
    {
        public Guid Id { get; set; }
        public string Usuario { get; set; }
        public Evento Evento { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal Precio { get; set; }
        public AccessLevel Access { get; set; } = AccessLevel.General;
        public string QRCode { get; set; }
    }

    public enum AccessLevel
    {
        General = 0,
        VIP = 1,
        Backstage = 2
    }

    public class ResaleListing
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TicketId { get; set; }
        public string Seller { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class MultimediaItem
    {
        public string EventName { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public bool IsVipOnly { get; set; }
    }

    public class VoteEntry
    {
        public string EventName { get; set; }
        public string OptionId { get; set; }
        public string OptionName { get; set; }
        public int Votes { get; set; }
    }

    public class DigitalMemory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public Guid TicketId { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public List<MultimediaItem> Items { get; set; } = new List<MultimediaItem>();
        public string Summary { get; set; }
    }

    public static class DataStore
    {
        private static readonly string _dataPath;
        private static readonly object _persistenceLock = new object();

        static DataStore()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EventPassMX");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            _dataPath = Path.Combine(dir, "datastore.json");
            Load();
        }

        private static readonly List<User> _userList = new List<User>();
        public static IReadOnlyList<User> Users => _userList.AsReadOnly();

        public static List<Artista> Artistas { get; } = new List<Artista>
        {
            new Artista { Nombre = "Artista A", Genero = "Pop", Descripcion = "Artista principal del Concierto A" },
            new Artista { Nombre = "Artista B", Genero = "Teatro/Performance", Descripcion = "Protagonista de Teatro B" },
            new Artista { Nombre = "Artista C", Genero = "Festival", Descripcion = "Line-up principal del Festival C" }
        };

        public static List<Evento> Eventos { get; } = new List<Evento>
        {
            new Evento { Nombre = "Concierto A", Precio = 550.00m, Artista = Artistas[0], Fecha = DateTime.Now.AddDays(12), Ciudad = "Ciudad de México", Categoria = "Música", IsNew = false, IsSoldOut = false },
            new Evento { Nombre = "Teatro B", Precio = 320.50m, Artista = Artistas[1], Fecha = DateTime.Now.AddDays(20), Ciudad = "Guadalajara", Categoria = "Teatro", IsNew = true, IsSoldOut = false },
            new Evento { Nombre = "Festival C", Precio = 790.00m, Artista = Artistas[2], Fecha = DateTime.Now.AddDays(45), Ciudad = "Monterrey", Categoria = "Festival", IsNew = false, IsSoldOut = true }
        };

        private static readonly List<Ticket> _tickets = new List<Ticket>();
        private static readonly List<ResaleListing> _resales = new List<ResaleListing>();
        private static readonly List<MultimediaItem> _multimedia = new List<MultimediaItem>();
        private static readonly List<VoteEntry> _votes = new List<VoteEntry>();
        private static readonly Dictionary<string, int> _points = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static readonly List<DigitalMemory> _memories = new List<DigitalMemory>();

        private class PersistentModel
        {
            public List<User> Users { get; set; } = new List<User>();
            public List<Ticket> Tickets { get; set; } = new List<Ticket>();
            public List<ResaleListing> Resales { get; set; } = new List<ResaleListing>();
            public List<MultimediaItem> Multimedia { get; set; } = new List<MultimediaItem>();
            public List<VoteEntry> Votes { get; set; } = new List<VoteEntry>();
            public Dictionary<string,int> Points { get; set; } = new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase);
            public List<DigitalMemory> Memories { get; set; } = new List<DigitalMemory>();
        }

        private static void Save()
        {
            try
            {
                lock(_persistenceLock)
                {
                    var model = new PersistentModel
                    {
                        Users = _userList,
                        Tickets = _tickets,
                        Resales = _resales,
                        Multimedia = _multimedia,
                        Votes = _votes,
                        Points = _points,
                        Memories = _memories
                    };
                    var opts = new JsonSerializerOptions { WriteIndented = true, ReferenceHandler = ReferenceHandler.IgnoreCycles };
                    var json = JsonSerializer.Serialize(model, opts);
                    File.WriteAllText(_dataPath, json);
                }
            }
            catch { }
        }

        private static void Load()
        {
            try
            {
                lock(_persistenceLock)
                {
                    if (!File.Exists(_dataPath)) return;
                    var txt = File.ReadAllText(_dataPath);
                    if (string.IsNullOrWhiteSpace(txt)) return;
                    var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var model = JsonSerializer.Deserialize<PersistentModel>(txt, opts);
                    if (model == null) return;
                    _userList.Clear();
                    _userList.AddRange(model.Users ?? new List<User>());
                    _tickets.Clear();
                    _tickets.AddRange(model.Tickets ?? new List<Ticket>());
                    _resales.Clear();
                    _resales.AddRange(model.Resales ?? new List<ResaleListing>());
                    _multimedia.Clear();
                    _multimedia.AddRange(model.Multimedia ?? new List<MultimediaItem>());
                    _votes.Clear();
                    _votes.AddRange(model.Votes ?? new List<VoteEntry>());
                    _memories.Clear();
                    _memories.AddRange(model.Memories ?? new List<DigitalMemory>());
                    _points.Clear();
                    if (model.Points != null)
                    {
                        foreach (var kv in model.Points) _points[kv.Key] = kv.Value;
                    }
                }
            }
            catch { }
        }

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
            Save();
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
                Precio = evento.Precio,
                QRCode = GenerateQrCode()
            };
            _tickets.Add(t);
            AddPointsForUser(username, evento.Precio);
            Save();
            return t;
        }

        private static string GenerateQrCode()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("/", "_").Replace("+", "-");
        }

        public static Ticket CreateTicketWithDiscount(string username, Evento evento, decimal discountAmount)
        {
            if (string.IsNullOrWhiteSpace(username) || evento == null)
                return null;

            var finalPrice = evento.Precio - discountAmount;
            if (finalPrice < 0) finalPrice = 0;

            var t = new Ticket
            {
                Id = Guid.NewGuid(),
                Usuario = username,
                Evento = evento,
                FechaCompra = DateTime.Now,
                Precio = finalPrice,
                QRCode = GenerateQrCode()
            };
            _tickets.Add(t);
            AddPointsForUser(username, finalPrice);
            Save();
            return t;
        }

        public static ResaleListing CreateResale(Guid ticketId, string seller, decimal price)
        {
            var ticket = _tickets.Find(t => t.Id == ticketId && t.Usuario == seller);
            if (ticket == null) return null;
            if (price > ticket.Precio) return null;

            var r = new ResaleListing { TicketId = ticketId, Seller = seller, Price = price };
            _resales.Add(r);
            Save();
            return r;
        }

        public static List<ResaleListing> GetAvailableResales(string eventName = null)
        {
            var list = _resales.FindAll(r => r.IsAvailable);
            if (!string.IsNullOrWhiteSpace(eventName))
            {
                list = list.FindAll(r => _tickets.Find(t => t.Id == r.TicketId)?.Evento?.Nombre == eventName);
            }
            return list;
        }

        public static bool BuyResale(Guid resaleId, string buyer)
        {
            var r = _resales.Find(x => x.Id == resaleId && x.IsAvailable);
            if (r == null) return false;
            var ticket = _tickets.Find(t => t.Id == r.TicketId);
            if (ticket == null) return false;

            ticket.Usuario = buyer;
            ticket.QRCode = GenerateQrCode();
            r.IsAvailable = false;
            AddPointsForUser(buyer, r.Price);
            Save();
            return true;
        }

        public static void AddMultimedia(MultimediaItem item)
        {
            if (item == null) return;
            _multimedia.Add(item);
            Save();
        }

        public static List<MultimediaItem> GetMultimediaForEvent(string eventName, bool onlyVip = false)
        {
            var list = _multimedia.FindAll(m => string.Equals(m.EventName, eventName, StringComparison.OrdinalIgnoreCase));
            if (onlyVip) list = list.FindAll(m => m.IsVipOnly == true);
            return list;
        }

        public static void VoteForOption(string eventName, string optionId, string optionName)
        {
            var v = _votes.Find(x => x.EventName == eventName && x.OptionId == optionId);
            if (v == null)
            {
                v = new VoteEntry { EventName = eventName, OptionId = optionId, OptionName = optionName, Votes = 0 };
                _votes.Add(v);
            }
            v.Votes++;
            Save();
        }

        public static List<VoteEntry> GetVotesForEvent(string eventName)
        {
            return _votes.FindAll(x => x.EventName == eventName);
        }

        public static void AddPointsForUser(string username, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(username)) return;
            var earned = (int)Math.Floor(amount / 200m);
            if (earned <= 0) return;
            if (!_points.ContainsKey(username)) _points[username] = 0;
            _points[username] += earned;
            Save();
        }

        public static int GetPoints(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return 0;
            return _points.TryGetValue(username, out var p) ? p : 0;
        }

        public static bool RedeemPoints(string username, int pointsToRedeem)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            if (!_points.TryGetValue(username, out var p) || p < pointsToRedeem) return false;
            _points[username] = p - pointsToRedeem;
            Save();
            return true;
        }

        public static Ticket PurchaseVIP(string username, Evento evento, VIPPackage package)
        {
            var t = CreateTicket(username, evento);
            if (t == null) return null;
            t.Access = AccessLevel.VIP;
            AddPointsForUser(username, evento.Precio);
            Save();
            return t;
        }

        public class VIPPackage
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
        }

        public static DigitalMemory GenerateDigitalMemory(string username, Guid ticketId)
        {
            var ticket = _tickets.Find(t => t.Id == ticketId && t.Usuario == username);
            if (ticket == null) return null;
            var mem = new DigitalMemory { Username = username, TicketId = ticketId };
            var allowVip = ticket.Access == AccessLevel.VIP;
            foreach (var m in _multimedia)
            {
                if (string.Equals(m.EventName, ticket.Evento.Nombre, StringComparison.OrdinalIgnoreCase))
                {
                    if (!m.IsVipOnly || allowVip) mem.Items.Add(m);
                }
            }
            mem.Summary = $"Boleto: {ticket.Evento.Nombre} - Comprado: {ticket.FechaCompra} - Items: {mem.Items.Count}";
            _memories.Add(mem);
            Save();
            return mem;
        }

        public static List<Ticket> GetTicketsForUser(string username)
        {
            return _tickets.FindAll(x => x.Usuario == username);
        }
    }
}
