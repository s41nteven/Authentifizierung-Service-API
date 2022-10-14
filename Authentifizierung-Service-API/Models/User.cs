using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentifizierung_Service_API.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(50)]
        [Column("Username")]
        public string Username { get; set; } = string.Empty;

        [Column("PasswordHash")]
        public byte[] PasswordHash { get; set; }

        [Column("PasswordSalt")]
        public byte[] PasswordSalt { get; set; }

        [Column("Fahrtenbuch")]
        private int _FahrtenbuchWrite = 0;

        public int FahrtenbuchWrite
        {
            get { return _FahrtenbuchWrite; }
            set 
            { 
                if(value != _FahrtenbuchWrite) { _FahrtenbuchWrite = value; } 
            }
        }

        [Column("Adressen")]
        private int _AdressenWrite = 0;

        public int AdressenWrite
        {
            get { return _AdressenWrite; }
            set
            {
                if (value != _AdressenWrite) { _AdressenWrite = value; }
            }
        }
    }
}
