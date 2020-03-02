using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Data
{
    public class  UserLoginDTO
    {
        [Required]
        public string UserName {get;set;}
        [Required]        
        public string Password {get;set;}
        
    }
}