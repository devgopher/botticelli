﻿using Botticelli.Shared.Utils;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.Shared.API.Client.Responses;

public class SendMessageResponse : BaseResponse<SendMessageResponse>
{
    public SendMessageResponse(string uid, string? techMessage = null) : base(uid, techMessage)
    {
    }

    public string? MessageUid { get; set; }

    public MessageSentStatus MessageSentStatus { get; set; }

    public Message Message { get; set; }

    public static SendMessageResponse GetInstance(string? techMessage) =>
        new(BotIdUtils.GenerateShortBotId(), techMessage);

    public static SendMessageResponse GetInstance(string uid, string? techMessage) => new(uid, techMessage);
}