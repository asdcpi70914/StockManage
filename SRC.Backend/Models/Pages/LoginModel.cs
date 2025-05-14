using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SRC.Backend.Models.Pages
{
    public class LoginModel
    {
        [StringLength(40)]
        public string Account { get; set; }

        [DataType(DataType.Password)]
        [StringLength(40)]
        public string Password { get; set; }

        [JsonIgnore]
        public string ResultMessage { get; set; }

        [JsonIgnore]
        public string ErrorMessage { get; set; }
    }
}
