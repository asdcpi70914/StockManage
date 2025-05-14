namespace SRC.Backend.Models.System
{
    public class RandomText
    {
        public string GetRandomText(int len)
        {
            Random r = new Random();
            string txt = "1234567890ABCDEFGHIJK@!";

            string key = "";
            for (int i = 0; i < len; i++)
            {
                key += txt[r.Next(0, txt.Length - 1)];
            }

            return key;
        }
    }
}
