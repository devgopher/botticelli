# Botticelli
A framework for creation/deployment and administration of Telegram/WhatsApp/WhatEverElse bots

This framework simplifies bot construction for wide range of purposes


# Getting Started

## Short summary
Botticelli consists of 2 parts: 
- Bots
- Server

On server side we can add new bots, change and check their state (active/non-active).
On a bot side we can implement any business logic we want.

## Requirements
1. .Net Core 6
2. Visual Studio 2022 and higher/Rider

## Building from source

1. Clone a git repository: ``` git clone https://github.com/devgopher/botticelli.git ```
2. Open Botticelli.sln in VS'2022/Rider
3. Examples of usage are in Samples subfolder

## Registering a Telegram bot
1. Register a bot account: https://core.telegram.org/bots/tutorial#introduction
2. Save your Bot Token

## Starting a sample project
1. In Visual Studio you should enable multi-project starting: https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=vs-2022

2. In our case, we need to run 2 project simultaneously: Botticelli.Server and Samples\TelegramBotSample
3. After the first run of a bot-project(Samples\TelegramBotSample in pour case), you should find Data\botId file with a generanted botId
4. Copy a generated botId into a clipboard
5. In your browser run http://localhost:5050/swagger (Admin panel functions)
6. Find an AddNewBot([FromBody] RegisterBotRequest request) method with parameters:
	- botId (put your botId here)
	- botKey (put your Telegram Bot key from a BotFather)
	- botType (0 - Telegram)
7. Execute AddNewBot with your parameters
8. Create some new chat/group with your bot (Notice! In order to read messages from a group, your bot account needs admin priveleges!)
9. Enjoy!