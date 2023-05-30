using carfinder_tgbotcon.ClassModels;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace carfinder_tgbotcon
{
    public class BotUi
    {
        TelegramBotClient botClient = new TelegramBotClient("6144193226:AAELnBSVaQ2vHqfYAML0kxwabKWJ4DWkWyo");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };


        public async Task Start()
        {
            botClient.StartReceiving(HandlerUpdateAsync, HandlerErrorAsync, receiverOptions, cancellationToken);
            var botMe = await botClient.GetMeAsync();
            Console.WriteLine($"Бот {botMe.Username} почав працювати");
            Console.ReadKey();
        }

        private async Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Помилка в телеграм бот АПІ:\n {apiRequestException.ErrorCode}" +
                $"\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {

                    await Task.Delay(1000, cancellationToken);

                    botClient.StartReceiving(HandlerUpdateAsync, HandlerErrorAsync, receiverOptions, cancellationToken);
                }
                catch (Exception restartException)
                {
                    Console.WriteLine($"An error occurred while restarting the bot: {restartException.Message}");
                }
            }
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
        }
        AllMenus allMenus = new AllMenus();

        int userMarkId = 0;                 //6
        int userModelId = 0;                //49
        string userMarkName = "";
        string userModelName = "";
        int price_min = 0;                  //10000
        int price_max = 0;                  //30000
        int year_min = 0;               //2011
        int year_max = 0;               //2023
        int i = 0;
        int price = 0;
        List<string> idsList = new List<string>();
        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {

            switch (allMenus.currentMenu)
            {
                case "mark":
                    userModelName = "";
                    var MarkDictionary = allMenus.MarkParamsRes(botClient, message).Result;
                    userMarkName = message.Text;
                    userMarkId = MarkDictionary[message.Text];
                    await allMenus.Search(botClient, message);
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Обрані параметри зараз\nМарка: {userMarkName}\nМодель: {userModelName}\nЦіна від: {price_min}\nЦіна до: {price_max}\nМінімальний рік: {year_min}\nМаксимальний рік: {year_max}");
                    allMenus.currentMenu = "";
                    break;

                case "model":
                    if (userMarkName != "")
                    {
                        var ModelDictionary = allMenus.ModelParamsRes(botClient, message, userMarkId.ToString()).Result;
                        userModelName = message.Text;
                        userModelId = ModelDictionary[message.Text];
                        await allMenus.Search(botClient, message);
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Обрані параметри зараз\nМарка: {userMarkName}\nМодель: {userModelName}\nЦіна від: {price_min}\nЦіна до: {price_max}\nМінімальний рік: {year_min}\nМаксимальний рік: {year_max}");
                        allMenus.currentMenu = "";
                    }
                    break;

                case "price_min":
                    price_min = Convert.ToInt32(message.Text);

                    if (Convert.ToInt32(price_max) < Convert.ToInt32(price_min) && Convert.ToInt32(price_max) != 0)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Мінімальна ціна не може бути вищою за максимальну!");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Обрані параметри зараз\nМарка: {userMarkName}\nМодель: {userModelName}\nЦіна від: {price_min}\nЦіна до: {price_max}\nМінімальний рік: {year_min}\nМаксимальний рік: {year_max}");
                    }
                    allMenus.currentMenu = "";
                    break;

                case "price_max":
                    price_max = Convert.ToInt32(message.Text);

                    if (Convert.ToInt32(price_max) < Convert.ToInt32(price_min))
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Максимальна ціна не може бути нижчою за мінімальну!");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Обрані параметри зараз\nМарка: {userMarkName}\nМодель: {userModelName}\nЦіна від: {price_min}\nЦіна до: {price_max}\nМінімальний рік: {year_min}\nМаксимальний рік: {year_max}");
                    }
                    allMenus.currentMenu = "";
                    break;

                case "year_min":
                    year_min = Convert.ToInt32(message.Text);
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Обрані параметри зараз\nМарка: {userMarkName}\nМодель: {userModelName}\nЦіна від: {price_min}\nЦіна до: {price_max}\nМінімальний рік: {year_min}\nМаксимальний рік: {year_max}");
                    allMenus.currentMenu = "";
                    break;

                case "year_max":
                    year_max = Convert.ToInt32(message.Text);
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Обрані параметри зараз\nМарка: {userMarkName}\nМодель: {userModelName}\nЦіна від: {price_min}\nЦіна до: {price_max}\nМінімальний рік: {year_min}\nМаксимальний рік: {year_max}");
                    allMenus.currentMenu = "";
                    break;

                case "done":
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Обрані параметри зараз\nМарка: {userMarkName}\nМодель: {userModelName}\nЦіна від: {price_min}\nЦіна до: {price_max}\nМінімальний рік: {year_min}\nМаксимальний рік: {year_max}");
                    allMenus.currentMenu = "";
                    break;

                case "search":

                    break;
            }

            switch (message.Text)
            {
                case "/start":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Привіт! Щоб розпочати, оберіть команду /keyboard");
                    allMenus.currentMenu = "start";
                    break;

                case "/keyboard":
                    await allMenus.Home(botClient, message);
                    allMenus.currentMenu = "home";
                    break;

                case "🏠Додому":
                    allMenus.currentMenu = "home";
                    break;

                case "🔍Шукати авто":
                    await allMenus.Search(botClient, message);
                    allMenus.currentMenu = "search";
                    break;


                case "💵Ціна":
                    await allMenus.Price(botClient, message);
                    allMenus.currentMenu = "price";
                    break;

                case "💵Ціна від":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть найнижчу ціну:");
                    allMenus.currentMenu = "price_min";
                    break;

                case "💰Ціна до":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть найвищу ціну:");
                    allMenus.currentMenu = "price_max";
                    break;

                case "✅Готово":
                    await allMenus.Search(botClient, message);
                    allMenus.currentMenu = "done";
                    break;

                case "📆Роки випуску":
                    await allMenus.Years(botClient, message);
                    allMenus.currentMenu = "year";
                    break;

                case "Роки від":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть мінімальний рік випуску");
                    allMenus.currentMenu = "year_min";
                    break;

                case "Роки до":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть максимальний рік випуску");
                    allMenus.currentMenu = "year_max";
                    break;

                case "🚘Марка":
                    await allMenus.MarkParams(botClient, message);
                    allMenus.currentMenu = "mark";
                    break;

                case "🚙Модель":
                    await allMenus.ModelParams(botClient, message, userMarkId.ToString());
                    allMenus.currentMenu = "model";
                    break;
                case "🔍Пошук":
                    await allMenus.AdvButtons(botClient, message);
                    //await allMenus.AdvById(botClient, message, result);
                    allMenus.currentMenu = "getids";
                    break;
                case "📊Середня ціна за параметрами":
                    allMenus.currentMenu = "avrprice";
                    break;

                case "⬅️":
                    allMenus.currentMenu = "previous";
                    break;

                case "➡️":
                    allMenus.currentMenu = "next";
                    break;
                case "Дізнатись більше":
                    allMenus.currentMenu = "moreabout";
                    break;
                case "❤️":
                    allMenus.currentMenu = "like";
                    break;
                case "❤️Мої вподобайки":
                    allMenus.currentMenu = "openlikes";
                    break;
                case "❌Видалити вподобайки":
                    allMenus.currentMenu = "deletelikes";
                    break;
                default:
                    break;
            }

            switch (allMenus.currentMenu)
            {
                case "home":
                    i = 0;
                    idsList.Clear();
                    await allMenus.Home(botClient, message);
                    break;

                case "getids":
                    if (userMarkId == 0 || userModelId == 0 || price_min == 0 || price_max == 0 || year_min == 0 || year_max == 0 || year_max < year_min)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть всі дані коректно!");
                        await allMenus.Search(botClient, message);
                        allMenus.currentMenu = "";
                    }
                    else
                    {
                        idsList = allMenus.GetIds(botClient, message, userMarkId, userModelId, price_min, price_max, year_min, year_max).Result;
                        await allMenus.AdvById(botClient, message, idsList, i);
                    }
                    break;

                case "previous":
                    i--;
                    if (i < 0)
                        i++;
                    price = await allMenus.AdvById(botClient, message, idsList, i);
                    break;
                case "next":
                    i++;
                    if (i >= idsList.Count)
                        i--;
                    price = await allMenus.AdvById(botClient, message, idsList, i);
                    break;
                case "like":
                    await allMenus.PutLikes(botClient, message, idsList, i, price);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Ваш лайк збережено");
                    allMenus.currentMenu = "";
                    break;

                case "avrprice":
                    var avrprice = allMenus.AvrPrice(botClient, message, userMarkId, userModelId, year_min, year_max).Result;
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Середня ціна обраної моделі наразі становить {avrprice[1]}$.\nПроаналізовано {avrprice[0]} оголошень.");
                    allMenus.currentMenu = "";
                    break;
                case "moreabout":
                    if (userMarkName != "" && userModelName != "")
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Зачекайте, інформація генерується...");
                        var moreabout = allMenus.GetMoreAbout(botClient, message, userMarkName, userModelName).Result;
                        await botClient.SendTextMessageAsync(message.Chat.Id, moreabout);
                        allMenus.currentMenu = "";
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть всі дані!");
                        allMenus.currentMenu = "";
                        await allMenus.Search(botClient, message);
                    }

                    break;
                case "openlikes":
                    await allMenus.GetLikes(botClient, message, allMenus);
                    allMenus.currentMenu = "";
                    break;
                case "deletelikes":
                    await allMenus.DeleteLikes(botClient, message);
                    allMenus.currentMenu = "";
                    break;
            }


            return;
        }
    }
}
