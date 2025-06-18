using Microsoft.AspNetCore.Routing.Template;

namespace SRC.Backend.Models.Config
{
    public class SysAppsetting
    {

        public int PaginationTake { get; set; }
        public string AdminAccount { get; set; }
        public string ADServer { get; set; }
        public int ADPort { get; set; }
        public bool IsSSL { get; set; }
        public void InitFolder()
        {

        }

        public void CheckFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"初始化資料夾{path}發生異常,{ex.Message}");
            }
        }

    }
}
