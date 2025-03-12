namespace Botticelli.Framework.Vk.Tests;

internal static class EnvironmentDataProvider
{
    public static string GetApiKey()
    {
        return Environment.GetEnvironmentVariable("TEST_VK_API_KEY") ?? "test_empty_key";
    }

    public static int GetTargetUserId()
    {
        return int.Parse(Environment.GetEnvironmentVariable("TEST_VK_TARGET_USER_ID") ?? "-1");
    }
}