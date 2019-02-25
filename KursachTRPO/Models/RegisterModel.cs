using System.ComponentModel.DataAnnotations;

namespace KursachTRPO.Models
{
    public class RegisterModel
    {
        [EmailAddress(ErrorMessage = "Некоректный Email")]
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Логин не указан")]
        public string Login { get; set; }

        [Required(ErrorMessage ="Роль не указана")]
        public string Role { get; set; }
    }
}
