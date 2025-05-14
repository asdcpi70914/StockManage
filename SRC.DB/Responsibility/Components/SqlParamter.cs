namespace SRC.DB.Responsibility.Components
{
    /// <summary>
    /// 設定SP參數
    /// </summary>
    public interface ISqlParamter { }

    /// <summary>
    /// 設定SP參數
    /// </summary>
    public class SqlParamter : ISqlParamter
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name">SP參數名稱</param>
        /// <param name="direction">SP 類型方向</param>
        protected internal SqlParamter(string name, DirectionType direction)
        {
            Name = name;
            Direction = direction;
        }

        /// <summary>
        /// Gets or sets gets SP參數名稱
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets gets SP 類型方向
        /// </summary>
        public DirectionType Direction { get; } = DirectionType.Input;
    }

    /// <summary>
    /// 設定SP參數
    /// </summary>
    /// <typeparam name="T">物件</typeparam>
    public class SqlParamter<T> : SqlParamter
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name">SP參數名稱</param>
        public SqlParamter(string name)
            : base(name, DirectionType.Output)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name">SP參數名稱</param>
        /// <param name="value">物件</param>
        /// <param name="bothput"> SP 類型方向
        /// <para>預設值為input，反之為inputoutput</para>
        /// </param>
        protected internal SqlParamter(string name, T value, bool bothput = false)
            : base(name, bothput == false ? DirectionType.Input : DirectionType.InputOutput)
        {
            Value = value;
        }

        /// <summary>
        /// Gets 物件
        /// </summary>
        public T Value { get; }
    }

    /// <summary>
    /// 擴充
    /// </summary>
    public static class SqlParamterExtension
    {
        /// <summary>
        /// 回傳值
        /// </summary>
        /// <param name="sqlParamter"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SqlParamter<object> Model(this ISqlParamter sqlParamter, string name)
            => new SqlParamter<object>(name);

        /// <summary>
        /// 綁定之物件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlParamter"></param>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <param name="bothput"></param>
        /// <returns></returns>
        public static SqlParamter<T> Model<T>(this ISqlParamter sqlParamter, string name, T model, bool bothput = false)
            => new SqlParamter<T>(name, model, bothput);
    }
}
