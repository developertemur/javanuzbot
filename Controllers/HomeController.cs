using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Args;
using WikiDotNet;

namespace FIrstBotASP.Controllers
{
    public class HomeController : Controller
    {
        string func(string search){
        string searchString = search;
        WikiSearchSettings searchSettings = new WikiSearchSettings
	    {RequestId = "Request ID", ResultLimit = 5, ResultOffset = 2, Language = "uz"};

        WikiSearchResponse response = WikiSearcher.Search(searchString, searchSettings);
        string res="";
        res=res+"\n"+$"\n Natija ({searchString}):\n";
        foreach (WikiSearchResult result in response.Query.SearchResults)
            {           
	    res=res+"\n"+
		$"\t{result.Title} ({result.WordCount} words, {result.Size} bytes, id {result.PageId}):\t{result.Preview}...\n\tAt {result.Url(searchSettings.Language)}\n\tLast edited at {result.LastEdited}\n";
        }
        return res;
        }

        
        
       
        // TelegramBotClient sinfidan obyekt hosil qilamiz
        // va unga Botfather orqali olgan tokenni
        // bog'laymiz
        private TelegramBotClient client = new TelegramBotClient("1779025628:AAGL4ZtzpWpehT10M5wH_MVc-hDywPDS2ps");
        static string lang="uzb";
        // HomePage
        public string Index()
        {
            // yangi event_handler yasaldi
            client.OnMessage += Xabar_Kelganda;

            // xabar kelishini tasdiqlash
            client.StartReceiving();
            
            // string qaytaradi  
            return "Bot hozr ishlamoqda";
        }

        // foydalanuvchu xabar yuborganda ishlaydi
        private async void Xabar_Kelganda(object sender, MessageEventArgs e)
        {
            // foydalanuvchi idsi
            long userId = e.Message.Chat.Id;
            
            // kelgan xabar idsi
            int msgId = e.Message.MessageId;

            if(e.Message.Text == "/start")
            {
                // xabar yuborish
                await client.SendTextMessageAsync(userId, "Assalomu alaykum from javanuzbot", replyToMessageId: msgId);
            }else
            if(e.Message.Text == "/til uzb")
            {
               lang="uz";
            }else
            if(e.Message.Text == "/til ru")
            {
               lang="ru";
            }else
            if(e.Message.Text == "/til en")
            {
               lang="en";
            }else
            if(e.Message.Text[0]!='/')
             await client.SendTextMessageAsync(userId, func(e.Message.Text), replyToMessageId: msgId); 
           
        }
    }
}
