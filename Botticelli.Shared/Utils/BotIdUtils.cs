﻿using System.Text.RegularExpressions;

namespace Botticelli.Shared.Utils;

public static class BotIdUtils
{
    private static readonly Random Rand = new(DateTime.Now.Nanosecond);

    private static byte[] GenerateSalt(int size)
    {
        var salt = new byte[size];

        for (var i = 0; i < size; i++) salt[i] = (byte) (Rand.Next() % (byte.MaxValue + 1));

        return salt;
    }

    private static byte[] BitWiseSum(byte[] a1, byte[] a2)
    {
        var min = Math.Min(a1.Length, a2.Length);
        var shortest = a1.Length < a2.Length ? a1 : a2;
        var longest = a1.Length > a2.Length ? a1 : a2;

        for (var i = 0; i < min; ++i) longest[i] |= shortest[i];


        return longest;
    }

    public static string GenerateShortBotId()
        => Regex.Replace(Convert.ToBase64String(BitWiseSum(Guid.NewGuid().ToByteArray(),
                                                           GenerateSalt(32))),
                         "[/+=]",
                         string.Empty);
}