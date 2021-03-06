﻿using System;
using System.Globalization;
using System.IO;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Hujilion.Console
{
    public static class Telegramer
    {
        public static void Post(Purchase newMostExpensivePurchase, bool debug = false)
        {
            var chatId = "@hujilion";
            if (Program.Debug || debug)
                chatId = "201466217";
            Log.Information($"Will post to chat: [{chatId}]");

            var telegram = new TelegramBotClient(File.ReadAllText(@"C:\hujilion-api-key.txt"));
            telegram.OnMessage += async (_, eventArgs) =>
                                  {
                                      await telegram.SendTextMessageAsync(eventArgs.Message.Chat.Id, eventArgs.Message.Chat.Id.ToString());
                                  };
            telegram.OnReceiveError += (_, e) => Log.Error($"shit: [{e.ApiRequestException.Message}]");
            telegram.OnReceiveGeneralError += (_, e) => Log.Error($"shit: [{e.Exception.Message}]");
            telegram.StartReceiving();
            if (!telegram.IsReceiving)
                throw new InvalidOperationException("can't start Telegram");

            telegram.SendTextMessageAsync(chatId,
                                          Format(newMostExpensivePurchase),
                                          parseMode: ParseMode.Markdown)
                    .Wait();
        }

        public static string Format(Purchase newMostExpensivePurchase) => $"Это стоит примерно хаджиллион:\r\n" +
                                                                          $"[{newMostExpensivePurchase.Title}]({newMostExpensivePurchase.Uri.AbsoluteUri})\r\n" +
                                                                          $"за {FormatPrice(newMostExpensivePurchase.Price)} ₽";

        public static string FormatPrice(decimal price)
        {
            var nfi = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            var formatted = price.ToString("#,0.00", nfi); // "1 234 897.11"
            return formatted;
        }
    }
}