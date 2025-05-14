namespace SRC.Backend.Models.Brain
{
    public class SystemHelper : ISystemHelper
    {
        public string CreateDateFolder(string rootPath)
        {
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(rootPath + dateFolder))
            {
                Directory.CreateDirectory(rootPath + dateFolder);
            }

            return dateFolder;
        }
    }
}
