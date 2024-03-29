﻿using Botticelli.Shared.Utils;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.Shared.API.Client.Requests;

public class SendMessageRequest : BaseRequest<SendMessageRequest>
{
    public SendMessageRequest(string uid) : base(uid)
    {
        Message = new Message(uid);
    }

    public Message Message { get; set; }

    public static SendMessageRequest GetInstance() => new(BotIdUtils.GenerateShortBotId());
}