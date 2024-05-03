using SendCrypto.Domain.Models;

namespace SendCrypto.Application;

public static class Constants
{
    public const int MaxRandomNumber = 255;
    public const int MinRandomNumber = 0;

    public const string RandomGeneratorUrl = "https://rpssl.olegbelousov.online"; //must be in config
    public const int MaxRetryCount = 3;

    public static readonly int NumberOfSignalTypes = Enum.GetValues(typeof(SignalType)).Length;
    public static readonly int RangeOfSignalTypes = MaxRandomNumber / NumberOfSignalTypes;
}