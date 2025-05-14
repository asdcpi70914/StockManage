using SRC.DB.Interfaces.Settings;
using SRC.ST.Security;
using System;
using System.Text;

namespace SRC.Backend.Models.Brain
{
    public class UserInfoSecurity
    {
        private Serilog.ILogger _Logger { get; set; }
        private IDF_SystemCode _SystemCode { get; set; }
        public UserInfoSecurity(Serilog.ILogger logger, IDF_SystemCode systemCode)
        {
            _Logger = logger;

            _SystemCode = systemCode;
        }

        public string EncryptUserInfo(string Data)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Data))
                {


                    var GetHash = _SystemCode.List_SystemCode("USERINFO");

                    var HashKey = GetHash.Where(x => x.data == "HashKey").First();
                    var HashIV = GetHash.Where(x => x.data == "HashIV").First();

                    var BytesKey = Encoding.UTF8.GetBytes(HashKey.description);

                    var BytesIV = Encoding.UTF8.GetBytes(HashIV.description);

                    //SecuritySalt Security = new SecuritySalt(HashKey.description, HashIV.description);

                    //return Security.AES256(Data);

                    EPSSecurity security = new EPSSecurity(Encoding.UTF8, BytesKey, BytesIV);
                    return security.EncryptToString(Data);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                _Logger.Fatal(ex, $"資料加密時發生異常，{ex.Message}");
                return "";
            }
        }

        public string DecryptUserInfo(string Data)
        {
            try
            {


                if (!string.IsNullOrWhiteSpace(Data))
                {
                    var GetHash = _SystemCode.List_SystemCode("USERINFO");

                    var HashKey = GetHash.Where(x => x.data == "HashKey").First();
                    var HashIV = GetHash.Where(x => x.data == "HashIV").First();

                    var BytesKey = Encoding.UTF8.GetBytes(HashKey.description);

                    var BytesIV = Encoding.UTF8.GetBytes(HashIV.description);

                    //SecuritySalt Security = new SecuritySalt(HashKey.description, HashIV.description);

                    //return Security.AES256Decrypt(Data);

                    EPSSecurity security = new EPSSecurity(Encoding.UTF8, BytesKey, BytesIV);
                    Span<byte> buffer = new Span<byte>(new byte[Data.Length]);

                    if (Convert.TryFromBase64String(Data, buffer, out int bytesParsed))
                    {
                        byte[] bytes = Convert.FromBase64String(Data);

                        return security.Decrypt(bytes);
                    }
                    else
                    {
                        return Data;
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                _Logger.Fatal(ex, $"解析加密資料發生異常：{ex.Message}");
                return Data;
            }
        }
    }
}
