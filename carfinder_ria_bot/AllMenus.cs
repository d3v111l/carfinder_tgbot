using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using static carfinder_tgbotcon.BotUi;
using Telegram.Bot.Polling;
using Newtonsoft.Json;
using carfinder_tgbotcon.ClassModels;
using System.Reflection.Metadata.Ecma335;
using Telegram.Bot.Requests.Abstractions;
using System.Net.Http;
using Telegram.Bot.Types.Enums;
using System.Text.Json.Nodes;
using Microsoft.VisualBasic;
using System.Reflection;

namespace carfinder_tgbotcon
{
    public class AllMenus
    {
        TelegramBotClient botClient = new TelegramBotClient("6144193226:AAELnBSVaQ2vHqfYAML0kxwabKWJ4DWkWyo");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

        public string currentMenu = "Home";

        public async Task Home(ITelegramBotClient botClient, Message message)
        {
            var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[] { new KeyboardButton[] { "🔍Шукати авто", "❤️Мої вподобайки" } })
            {
                ResizeKeyboard = true
            };
            await botClient.SendTextMessageAsync(message.Chat.Id, "Оберіть пункт меню:", replyMarkup: replyKeyboardMarkup);
        }

        public async Task Search(ITelegramBotClient botClient, Message message)
        {
            var replyKeyboardMarkup =
            new ReplyKeyboardMarkup(new[]
            {
                        new KeyboardButton[] { "🔍Пошук" },
                        new KeyboardButton[] { "Дізнатись більше" },
                        new KeyboardButton[] { "📊Середня ціна за параметрами" },
                        new KeyboardButton[] { "🚘Марка", "🚙Модель" },
                        new KeyboardButton[] { "💵Ціна", "📆Роки випуску" },
                        new KeyboardButton[] { "🏠Додому" }
            })
            {
                ResizeKeyboard = true
            };
            await botClient.SendTextMessageAsync(message.Chat.Id, "Оберіть параметри авто:", replyMarkup: replyKeyboardMarkup);
        }

        public async Task Price(ITelegramBotClient botClient, Message message)
        {
            var replyKeyboardMarkup =
            new ReplyKeyboardMarkup(new[]
            {
                        new KeyboardButton[] { "💵Ціна від" },
                        new KeyboardButton[] { "💰Ціна до" },
                        new KeyboardButton[] { "✅Готово" },
            })
            {
                ResizeKeyboard = true
            };
            await botClient.SendTextMessageAsync(message.Chat.Id, "Оберіть найнижчу та найвищу ціну авто:", replyMarkup: replyKeyboardMarkup);
        }

        public async Task Years(ITelegramBotClient botClient, Message message)
        {
            var replyKeyboardMarkup =
            new ReplyKeyboardMarkup(new[]
            {
                        new KeyboardButton[] { "Роки від" },
                        new KeyboardButton[] { "Роки до" },
                        new KeyboardButton[] { "✅Готово" },
            })
            {
                ResizeKeyboard = true
            };
            await botClient.SendTextMessageAsync(message.Chat.Id, "Оберіть роки коли випускався шуканий автомобіль:", replyMarkup: replyKeyboardMarkup);
        }

        public async Task MarkParams(ITelegramBotClient botClient, Message message)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "https://autoriaapi.azurewebsites.net/GetMarks?category_id1=1");
            var response = await httpClient.SendAsync(request);
            var answer = await response.Content.ReadAsStringAsync();
            List<Marks> carDataList = JsonConvert.DeserializeObject<List<Marks>>(answer);
            Dictionary<string, int> carDictionary = new Dictionary<string, int>();
            foreach (var carData in carDataList)
            {
                carDictionary.Add(carData.Name, carData.Value);
            }
            KeyboardButton[][] keyboardButtons =
            carDictionary.Keys.Select(key => new KeyboardButton[] { new KeyboardButton(key) }).ToArray();
            ReplyKeyboardMarkup replyMarkup = new ReplyKeyboardMarkup(keyboardButtons);
            await botClient.SendTextMessageAsync(message.Chat.Id, "Оберіть марку", replyMarkup: replyMarkup);
        }
        public async Task<Dictionary<string, int>> MarkParamsRes(ITelegramBotClient botClient, Message message)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "https://autoriaapi.azurewebsites.net/GetMarks?category_id1=1");
            var response = await httpClient.SendAsync(request);
            var answer = await response.Content.ReadAsStringAsync();
            List<Marks> carDataList = JsonConvert.DeserializeObject<List<Marks>>(answer);
            Dictionary<string, int> carDictionary = new Dictionary<string, int>();
            foreach (var carData in carDataList)
            {
                carDictionary.Add(carData.Name, carData.Value);
            }
            return carDictionary;
        }

        public async Task ModelParams(ITelegramBotClient botClient, Message message, string mark)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://autoriaapi.azurewebsites.net/GetModels?mark={mark}");
            var response = await httpClient.SendAsync(request);
            var answer = await response.Content.ReadAsStringAsync();
            List<Marks> carDataList = JsonConvert.DeserializeObject<List<Marks>>(answer);
            Dictionary<string, int> carDictionary = new Dictionary<string, int>();
            foreach (var carData in carDataList)
            {
                carDictionary.Add(carData.Name, carData.Value);
            }
            KeyboardButton[][] keyboardButtons =
            carDictionary.Keys.Select(key => new KeyboardButton[] { new KeyboardButton(key) }).ToArray();
            ReplyKeyboardMarkup replyMarkup = new ReplyKeyboardMarkup(keyboardButtons);
            await botClient.SendTextMessageAsync(message.Chat.Id, "Оберіть модель", replyMarkup: replyMarkup);
        }
        public async Task<Dictionary<string, int>> ModelParamsRes(ITelegramBotClient botClient, Message message, string mark)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://autoriaapi.azurewebsites.net/GetModels?mark={mark}");
            var response = await httpClient.SendAsync(request);
            var answer = await response.Content.ReadAsStringAsync();
            List<Marks> carDataList = JsonConvert.DeserializeObject<List<Marks>>(answer);
            Dictionary<string, int> carDictionary = new Dictionary<string, int>();
            foreach (var carData in carDataList)
            {
                carDictionary.Add(carData.Name, carData.Value);
            }

            return carDictionary;
        }

        public async Task<List<string>> GetIds(ITelegramBotClient botClient, Message message, int userMarkId, int userModelId, int price_min, int price_max, int year_min, int year_max)
        {
            var httpClient = new HttpClient();
            string url = "https://autoriaapi.azurewebsites.net/GetIdsBySearch?";          
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"{url}marka_id={userMarkId}&model_id={userModelId}&price_ot={price_min}&price_do={price_max}&year_ot={year_min}&year_do={year_max}");
            var response = await httpClient.SendAsync(request);
            var answer = await response.Content.ReadAsStringAsync();
            var ids = JsonConvert.DeserializeObject<Root>(answer);
            List<string> idsList = ids.search_Result.ids;
            return idsList;
        }

        public async Task<int> AdvById(ITelegramBotClient botClient, Message message, List<string> idsList, int i)
        {

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://autoriaapi.azurewebsites.net/GetAdvById?auto_id={idsList[i]}");
            var response = await httpClient.SendAsync(request);
            var answer = await response.Content.ReadAsStringAsync();
            var adv = JsonConvert.DeserializeObject<Adv>(answer);
            //List<InputMediaPhoto> media = new List<InputMediaPhoto>                       тут можна зробити вивід кількох фотографій, якщо зробити правильний гет запит на фотографії
            //{
            //    new InputMediaPhoto(InputFile.FromUri(adv.PhotoData.SeoLinkF)),
            //    new InputMediaPhoto(InputFile.FromUri(adv.PhotoData.SeoLinkB)),
            //    new InputMediaPhoto(InputFile.FromUri(adv.PhotoData.SeoLinkSX)),
            //    new InputMediaPhoto(InputFile.FromUri(adv.PhotoData.SeoLinkM))
            //};
            //await botClient.SendMediaGroupAsync(message.Chat.Id, media);

            
            string output = $" {adv.MarkName} {adv.ModelName} {adv.AutoData.Year}\r\nМісто: {adv.LocationCityName}\r\nДвигун: {adv.AutoData.FuelName}\r\nКПП: {adv.AutoData.GearboxName}\r\nПробіг: {adv.AutoData.Race}\r\nЦіна: {adv.Usd}$\r\nОпис: {adv.AutoData.Description}";
            await botClient.SendPhotoAsync(message.Chat.Id, InputFile.FromUri(adv.PhotoData.SeoLinkF), caption: output);
            //await botClient.SendTextMessageAsync(message.Chat.Id, output);
            return adv.Usd;
        }

        public async Task<(string, int)> LikedAdvById(ITelegramBotClient botClient, Message message, int advId)
        {

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://autoriaapi.azurewebsites.net/GetAdvById?auto_id={advId}");
            var response = await httpClient.SendAsync(request);
            var answer = await response.Content.ReadAsStringAsync();
            var adv = JsonConvert.DeserializeObject<Adv>(answer);

            string output = $"{adv.MarkName} {adv.ModelName} {adv.AutoData.Year}\r\nМісто: {adv.LocationCityName}\r\nДвигун: {adv.AutoData.FuelName}\r\nКПП: {adv.AutoData.GearboxName}\r\nПробіг: {adv.AutoData.Race}\r\nЦіна: {adv.Usd}";
            int oldPrice = adv.Usd;
            return (output, oldPrice);
        }


        public async Task AdvButtons(ITelegramBotClient botClient, Message message)
        {
            var replyKeyboardMarkup =
            new ReplyKeyboardMarkup(new[]
            {
                                                 new KeyboardButton[] {"⬅️", "❤️", "➡️"},
                                                 new KeyboardButton[] { "🏠Додому" }
            })
            {
                ResizeKeyboard = true
            };
            await botClient.SendTextMessageAsync(message.Chat.Id, "Пошук за обраними параметрами...", replyMarkup: replyKeyboardMarkup);
        }

        public async Task<double[]> AvrPrice(ITelegramBotClient botClient, Message message, int userMarkId, int userModelId, int year_min, int year_max)
        {
            var httpClient = new HttpClient();
            string url = "https://localhost:7275/GetIdsBySearch?";
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://autoriaapi.azurewebsites.net/GetAvrPrice?marka_id2={userMarkId}&model_id={userModelId}");
            var response = await httpClient.SendAsync(request);
            var answer = await response.Content.ReadAsStringAsync();
            var avr = JsonConvert.DeserializeObject<Avr>(answer);
            var total = avr.Total;
            var average = avr.ArithmeticMean;
            double[] result = new double[2];
            result[0] = total;
            result[1] = average;
            return result;
        }





        public async Task<string> GetMoreAbout(ITelegramBotClient botClient, Message message, string userMarkName, string userModelName)
        {
            var httpClient = new HttpClient();
            var url = "https://autoriaapi.azurewebsites.net/GetMoreAbout/chat/brand";
            var carInfo = new
            {
                mark = userMarkName,
                model = userModelName
            };

            // Серіалізація об'єкту в JSON
            var jsonContent = JsonConvert.SerializeObject(carInfo);

            // Створення HttpContent з JSON
            var content = new StringContent(jsonContent, Encoding.Unicode);

            // Відправка POST-запиту
            var response = await httpClient.PostAsync(url, content);

            // Перевірка, чи був запит успішним
            response.EnsureSuccessStatusCode();

            // Повернення відповіді як рядка
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }


        public async Task PutLikes(ITelegramBotClient botClient, Message message, List<string> idsList, int i, int price)
        {
            var httpClient = new HttpClient();
            var url = "https://autoriaapi.azurewebsites.net/DB/PutLikes";
            var carInfo = new
            {
                user_id = message.Chat.Id,
                adv_id = idsList[i],
                price = price
            };
            var jsonContent = JsonConvert.SerializeObject(carInfo);
            var content = new StringContent(jsonContent, Encoding.Unicode, "application/json");
            var response = await httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();

        }

        public async Task<string> FormingOut(Message message, MyLikes myLikes, AllMenus allMenus)
        {
            var result = allMenus.LikedAdvById(botClient, message, myLikes.adv_id).Result;

            var output = result.Item1;
            var oldPrice = result.Item2;
            int difference = 0;
            string priceChanged = "";
            if (oldPrice < myLikes.price)
            {
                difference = myLikes.price - oldPrice;
                priceChanged = $"Ціна збільшилась на {difference}";
            }
            else if (oldPrice > myLikes.price)
            {
                difference = oldPrice - myLikes.price;
                priceChanged = $"Ціна зменшилась на {difference}";
            }
            else if (oldPrice == myLikes.price)
            {
                priceChanged = $"Ціна не змінилась";
            }

            string formed = $"{output}\n{priceChanged}\n\n";

            return formed;
        }
        public async Task GetLikes(ITelegramBotClient botClient, Message message, AllMenus allMenus)
        {
            var replyKeyboardMarkup =
            new ReplyKeyboardMarkup(new[]
            {
                                                 new KeyboardButton[] {"❌Видалити вподобайки"},
                                                 new KeyboardButton[] { "🏠Додому" }
            })
            {
                ResizeKeyboard = true
            };
            var httpClient = new HttpClient();
            var url = $"https://autoriaapi.azurewebsites.net/DB/GetLikes/{message.Chat.Id}";
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            var response = await httpClient.SendAsync(request);
            var answer = await response.Content.ReadAsStringAsync();
            var myLikes = JsonConvert.DeserializeObject<List<MyLikes>>(answer);
            string formedFinal = "";
            for (int i = 0; i < myLikes.Count; i++) 
            {
                formedFinal += await allMenus.FormingOut(message, myLikes[i], allMenus);
            }
            await botClient.SendTextMessageAsync(message.Chat.Id, formedFinal, replyMarkup: replyKeyboardMarkup);
        }
        public async Task DeleteLikes(ITelegramBotClient botClient, Message message)
        {
            var httpClient = new HttpClient();
            var url = $"https://autoriaapi.azurewebsites.net/DB/DeleteLike?user_id={message.Chat.Id}";
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), url);
            var response = await httpClient.SendAsync(request);
            await botClient.SendTextMessageAsync(message.Chat.Id, "✅Вподобайки видалені");
        }

    }  
}

