﻿namespace Botticelli.Shared.API.Client.Requests;

/// <summary>
///     Keep alive notification to an admin server from a bot
/// </summary>
public class KeepAliveNotificationRequest
{
    public string? BotId { get; set; }
}