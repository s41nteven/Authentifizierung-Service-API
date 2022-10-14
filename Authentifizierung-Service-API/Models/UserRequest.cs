namespace Authentifizierung_Service_API.Models
{
    public class UserRequest
    {
        public string Username { get; set; }
        
        public string PasswordHash { get; set; }

        public int FahrtenbuchWrite { get; set; }

        public int AdressenWrite { get; set; }
    }
}
