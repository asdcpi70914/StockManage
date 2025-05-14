using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.Components
{
    public interface IAdapter
    {
        /// <summary>
        /// 狀態代碼
        /// </summary>
        string ResCode { get; set; }

        /// <summary>
        /// 狀態代碼資訊
        /// </summary>
        string ResMsg { get; set; }
    }
    /// <summary>
    /// 回傳基底
    /// </summary>
    public class Adapter : IAdapter
    {
        /// <summary>
        /// 狀態代碼
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 狀態代碼資訊
        /// </summary>
        public string ResMsg { get; set; }
    }

    /// <summary>
    /// 回傳基底 (含基底)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Adapter<T> : Adapter
    {
        /// <summary>
        /// 物件
        /// </summary>
        public T Data { get; set; }
    }
}
