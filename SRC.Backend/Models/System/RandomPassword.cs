namespace SRC.Backend.Models.System
{
    public class RandomPassword
    {
        public Serilog.ILogger _logger { get; set; }
        public RandomPassword(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public string NewPassowrd()
        {
            try
            {
                string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
                int passwordLength = 9;//密碼長度
                char[] chars = new char[passwordLength];
                Random rd = new Random();

                for (int i = 0; i < passwordLength; i++)
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }

                string pwd = new string(chars);

                return pwd;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, $"產生隨機密碼發生異常：{ex.Message}");
                return "";
            }
        }
    }
}
