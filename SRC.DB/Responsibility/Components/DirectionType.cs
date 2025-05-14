using System;

namespace SRC.DB.Responsibility.Components
{
    /// <summary>
    /// SP 類型方向 = Dapper.ParameterDirection
    /// </summary>
    public enum DirectionType
    {
        /// <summary>
        /// 輸入
        /// </summary>
        Input = 1,

        /// <summary>
        /// 輸出
        /// </summary>
        Output = 2,

        /// <summary>
        /// 輸入出
        /// </summary>
        InputOutput = 3,

        /// <summary>
        /// 回傳 - 不提供
        /// </summary>
        [Obsolete("不提供使用", true)]
        ReturnValue = 6
    }
}
