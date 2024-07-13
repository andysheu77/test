using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

string endpoint = "https://andysheu.openai.azure.com/";
string key = "e3330c0963cd46e0bb19c434fb4f45c0";

var azureOpenAIclient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));

var chatClient = azureOpenAIclient.GetChatClient("MyGPT4o");


Console.WriteLine($"飲品推薦，本店提供酒類與茶類飲品，請問今天您感覺如何？");

string? userinput, category;

ChatCompletion completion;
getCategory:
var result = GetCategory();
category= result[1];
userinput = result[0];
if (string.IsNullOrEmpty(userinput))
    return;
if (category == "酒類" || category == "茶類")
{
    var fileData = LoadDataFiles(category);
    completion = chatClient.CompleteChat(
        new List<ChatMessage>()
        {
        new SystemChatMessage("你是一位專業的飲品推薦員，會依照顧客今天所回答的感覺或需求，引導顧客挑選我們店內提供的飲品" +
                                                        "顧客挑選飲品種類:" +
                                                        $"{category}" +
                                                        "店內提供此類飲品的內容:" +
                                                        $"{fileData}" +
                                                        "輸出規則 <品名> <(英文品名)> : <解釋> ； <推薦的原因(不超過 50 字)>"),
        new UserChatMessage(result[0]),
        });
    Console.WriteLine($"{completion.Role}:{Environment.NewLine}{completion.Content[0].Text}");
    return;
}
else
{
    completion = chatClient.CompleteChat(
        new List<ChatMessage>()
        {
                new SystemChatMessage("你是一個引導顧客表達他們想喝飲品的推薦員，請使用簡潔且溫和的口吻回覆，引導顧客挑選我們店內提供的飲品。店內提供的飲品有「酒類」、「茶類」，請引導顧客回答出這 2 類的敘述"),
        }); ;

    Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");
    goto getCategory;
}
string LoadDataFiles(string fileName)
{
    var filePath = Path.Combine(Environment.CurrentDirectory, "DataFiles", $"{fileName}.txt");
    using var reader = new StreamReader(filePath);
    var data = reader.ReadToEnd();

    return data;
}

string[] GetCategory()
{
    string userinput = Console.ReadLine()?? string.Empty;
    ChatCompletion completion = chatClient.CompleteChat(
        new List<ChatMessage>()
        {
        new SystemChatMessage("你是一位專業的飲品推薦員，會依照顧客今天所回答的感覺或需求，你的主要任務是輸出對應的店內的飲品種類\r\n        根據使用者的需求，請只輸出以下其中一個飲品標籤：\r\n        酒類：當使用者提到「酒」或者顯示出對「酒類」的興趣時，選擇這個模式。例如「我想來點微醺的感覺」。\r\n        茶類：當使用者提到「茶」或者顯示出對「茶類」的興趣時，選擇這個模式。例如「我想來點回甘的飲品」。\r\n        如果使用者沒有提供明確的學習目標，請輸出「無相關資訊」。\r\n        重要規則:\r\n        你只能輸出「酒類」、「茶類」或者「無相關資訊」。不可以回答任何其他的訊息。"),
        new UserChatMessage(userinput),
        });

    //Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");
    var category = completion.Content[0].Text;
    return new string[]{ userinput, category };
}