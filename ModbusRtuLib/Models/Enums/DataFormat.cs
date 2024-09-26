namespace ModbusRtuLib.Models.Enums
{
    /// <summary>
    /// 单字字节序
    /// </summary>
    public enum DataFormat
    {
        /// <summary>
        /// 3，2，1，0
        /// </summary>
        ABCD,

        /// <summary>
        /// 1，0，3，2
        /// </summary>
        CDAB,

        /// <summary>
        /// 2，3，0，1
        /// </summary>
        BADC,

        /// <summary>
        /// 0，1，2，3
        /// </summary>
        DCBA,
    }
}
