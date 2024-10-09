namespace ModbusRtuLib.Models.Enums;

/// <summary>
/// 三菱MC-Qna3E 读写类型
/// </summary>
public enum MitsubishiMCType : byte
{
    None = 0x00,
    X = 0x9C,
    Y = 0x9D,
    M = 0x90,
    L = 0x92,
    S = 0x98,
    B = 0xA0,
    F = 0x93,
    TS = 0xC1,
    TC = 0xC0,
    TN = 0xC2,
    CS = 0xC4,
    CC = 0xC3,
    CN = 0xC5,
    D = 0xA8,
    W = 0xB4,
    R = 0xAF,
}
