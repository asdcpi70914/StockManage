using SRC.DB.Models.Funcs;

namespace SRC.DB.Interfaces.Authority
{
    public interface IDF_Menu
    {
        List<SRCMenu> GetMenus(string account);
    }
}
