using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace CovidBot
{
    public class TelegramService
    {
        public event EventHandler<MessageEventArgs> OnMessage;

        private TelegramBotClient _telegramBotClient;

        public TelegramService(TelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;

        }

       
        public void SendMessage(long chatid, string message)
        {
            _telegramBotClient.SendTextMessageAsync(chatid, message);
        }

        public void StartService()
        {
            _telegramBotClient.OnMessage += OnMessage;
            _telegramBotClient.OnMessageEdited += OnMessage;
            _telegramBotClient.StartReceiving();
        }
    }
}

