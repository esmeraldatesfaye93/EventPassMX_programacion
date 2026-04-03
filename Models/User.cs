namespace EventPassMX_programacion
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public User() { }
        public User(string u, string p) { Username = u; Password = p; }
    }
}
