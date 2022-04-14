using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace H2M2chat.Areas.Identity.Data
{
    public class H2M2chatUser : IdentityUser
    {
        [PersonalData]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProfilePic { get; set; }
    }
}
