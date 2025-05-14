using SRC.DB.Models.Funcs;

namespace SRC.Backend.Models.System
{
    public class SRCUIAuthority
    {
        private SRCUIAuthority() { }

        public bool Display { get; protected set; }

        public static SRCUIAuthority CreateUIAuthority(IList<SRCMenu> authorityList, string funcUrl)
        {
            if (authorityList == null)
            {
                return new SRCUIAuthority()
                {
                    Display = false
                };

            }


            string upperUrl = funcUrl?.ToUpper();
            return new SRCUIAuthority()
            {
                Display = authorityList.Where(m => m.url.ToUpper() == upperUrl).ToList().Count() > 0
            };
        }
    }
}
