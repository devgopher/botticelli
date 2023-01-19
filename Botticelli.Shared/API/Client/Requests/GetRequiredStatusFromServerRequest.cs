﻿namespace Botticelli.Shared.API.Client.Requests;

/// <summary>
///     Requests a needed status from a server (started/stopped)
/// </summary>
public class GetRequiredStatusFromServerRequest
{
    public string? BotId { get; set; }
}