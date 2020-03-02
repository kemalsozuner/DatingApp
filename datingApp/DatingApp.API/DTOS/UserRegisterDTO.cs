using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Data
{
    public class  UserRegisterDTO
    {
        [Required]
        public string UserName {get;set;}
        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage="Max 8 Min4")]
        public string Password {get;set;}
        
    }
}