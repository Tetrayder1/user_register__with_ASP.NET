using System.ComponentModel.DataAnnotations;

namespace user_register.Areas.Admin.Models
{
    public class UpdateRoleDTO
    {
        public string Id { get; set; } = null!;
        [Required(ErrorMessage = "Role name  bos ola bilmez.")]
        [Display(Name = "Role name:")]
        [DataType(DataType.Text)]
        public string Name { get; set; }= null!;

    }
}
