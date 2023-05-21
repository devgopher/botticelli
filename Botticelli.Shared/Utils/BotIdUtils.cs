using System.Text.RegularExpressions;

namespace Botticelli.Shared.Utils;

public static class Uid
{
    private static Random _rand = new(DateTime.Now.Nanosecond);

    private static byte[] GenerateSalt(int size)
    {
        var salt = new byte[size];

        for (int i = 0; i < size; i++) 
            salt[i] = (byte) (_rand.Next() % (byte.MaxValue + 1));

        return salt;
    }

    private static byte[] BitWiseSum(byte[] a1, byte[] a2)
    {
        var min = Math.Min(a1.Length, a2.Length);
        var shortest = a1.Length < a2.Length ? a1 : a2;
        var longest = a1.Length > a2.Length ? a1 : a2;

        for (int i = 0; i < min; ++i) 
            longest[i] |= shortest[i];


        return longest;
    }

    public static string GenerateShortUid()
        => Regex.Replace(Convert.ToBase64String(BitWiseSum(Guid.NewGuid().ToByteArray(), 
                                                           GenerateSalt(32))),
                         "[/+=]", 
                         string.Empty);


}