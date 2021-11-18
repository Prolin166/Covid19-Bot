using CovidBot.Services;
using System;
using System.Timers;
using Telegram.Bot;

namespace CovidBot
{
    class Program
    {
        //Send once a day (86400000 ms)
        const long time = 86400000;

        private static Covid19Service _covid19Service;
        private static TelegramService _telegramService;

        static void Main(string[] args)
        {
            Console.WriteLine("Bot gestartet");
            _covid19Service = new Covid19Service("https://api.covid19api.com/");
            _telegramService = new TelegramService(new TelegramBotClient("addtelegrambottokenhere"));

            while (DateTime.Now.Hour != 10 || DateTime.Now.Minute != 00 || DateTime.Now.Second != 00)
            {

            }

            var chatIds = TextFileWorker.ReadLongToList("ChatIds.txt");
            foreach (var chatid in chatIds)
                _telegramService.SendMessage(chatid, "Covid19 Service gestartet");

            Console.WriteLine("Senderythmus gestartet");

            SendData();

            var timer = new Timer(time);
            timer.Enabled = true;

            timer.Elapsed += OnTimedEvent;
            _telegramService.OnMessage += TelegramService_OnMessage;
            _telegramService.StartService();

            Console.ReadKey();
        }

        private static void TelegramService_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            bool isValidName = false;

            var chatIds = TextFileWorker.ReadLongToList("ChatIds.txt");

            var chatId = e.Message.Chat.Id;
            if (!chatIds.Contains(chatId))
            {
                TextFileWorker.AppendTextFileWithLong("ChatIds.txt", chatId);

                chatIds.Add(chatId);
                _telegramService.SendMessage(e.Message.Chat.Id,
                    "Hallo, du hast dich auf den Telegram Newsletter zu den Covid 19 Fallzahlen registriert. " +
                    "Du bekommst nun alle 24 Stunden die aktuellen Fallzahlen aus Deutschland gesendet.");

                Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + " ChatId " + e.Message.Chat.Id + " registriert.");

            }
            else
            {
                var countries = _covid19Service.GetCountries();

                if (countries.Count != 0)
                {
                    if (e.Message.Text == "Help")
                    {
                        foreach (var country in countries)
                        {
                            _telegramService.SendMessage(e.Message.Chat.Id, country.CountryName);
                        }
                    }

                    foreach (var country in countries)
                    {
                        if (country.CountryName == e.Message.Text)
                        {
                            var result = _covid19Service.GetCases(e.Message.Text);

                            _telegramService.SendMessage(e.Message.Chat.Id,
                                    @"Aktuelle Zahlen für " + e.Message.Text + ": " + Environment.NewLine +
                                    "Absolute Zahlen:" + Environment.NewLine +
                                    "Bestätigte Fälle: " + result.TotalConfirmed + Environment.NewLine +
                                    "Aktive Infektionen: " + result.TotalActive + Environment.NewLine +
                                    "Tote mit Covid19: " + result.TotalDeaths + Environment.NewLine +
                                    "Anstieg zum Vortag: " + Environment.NewLine +
                                    "Bestätigte Fälle: " + result.NewConfirmed + Environment.NewLine +
                                    "Aktive Infektionen: " + result.NewActive + Environment.NewLine +
                                    "Tote mit Covid19: " + result.NewDeaths
                                );
                            isValidName = true;

                            Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + " Daten für " + e.Message.Text + " gesendet.");
                        }
                    }
                }


                if (!isValidName)
                {
                    _telegramService.SendMessage(e.Message.Chat.Id, "Entschuldige, dass war kein gültiger Name. Gültige Namen sind z.B. Germany, France, Indonesia." + Environment.NewLine +
                        "Es muss immer die englische/internationale Bezeichnung eingegeben werden. Weiterhin muss jeder Name am Anfang groß geschrieben werden." + Environment.NewLine +
                        "Mit oder Eingabe 'Help', bekommst du alle Länder angezeigt.");
                }
            }
        }

        public static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            SendData();            
        }

        public static void SendData()
        {
            var chatIds = TextFileWorker.ReadLongToList("ChatIds.txt");

            try
            {
                foreach (var chatId in chatIds)
                {
                    var result = _covid19Service.GetCases("Germany");
                    _telegramService.SendMessage(chatId,
                            @"Aktuelle Zahlen für Deutschland:" + Environment.NewLine +
                            "Absolute Zahlen:" + Environment.NewLine +
                            "Bestätigte Fälle: " + result.TotalConfirmed + Environment.NewLine +
                            "Aktive Infektionen: " + result.TotalActive + Environment.NewLine +
                            "Tote mit Covid19: " + result.TotalDeaths + Environment.NewLine + Environment.NewLine +
                            "Anstieg zum Vortag: " + Environment.NewLine +
                            "Bestätigte Fälle: " + result.NewConfirmed + Environment.NewLine +
                            "Aktive Infektionen: " + result.NewActive + Environment.NewLine +
                            "Tote mit Covid19: " + result.NewDeaths
                        );
                    Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + " Daten durch Timer an " + chatId + " gesendet.");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

}