using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H2M2chat.Models
{
    public class Room
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoomId { get; set; }
        [Required]
        [StringLength(50)]
        public string? About { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoomIcon { get; set; }

        [Required]
        public bool IsGC { get; set; } = true;
        [Required]
        public bool IsVisable { get; set; }  = true;

        public List<Message> messages = new();
        public List<string> Chatters = new();
    }
}
