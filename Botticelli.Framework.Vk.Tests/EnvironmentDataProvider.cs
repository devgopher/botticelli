namespace Botticelli.Framework.Vk.Tests;

internal static class EnvironmentDataProvider
{
    public static string GetApiKey() => Environment.GetEnvironmentVariable("TEST_VK_API_KEY") ?? "test_empty_key";
    public static int GetTargetUserId() => int.Parse(Environment.GetEnvironmentVariable("TEST_VK_TARGET_USER_ID") ?? "-1");
}