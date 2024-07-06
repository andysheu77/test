using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

string endpoint = "https://myaoaiclassdemo.openai.azure.com";
string key = "daf7070cfbc04099a6317d393f3a9d2d";

var azureOpenAIclient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));

var chatClient = azureOpenAIclient.GetChatClient("MyGPT4o");


ChatCompletion completion = chatClient.CompleteChat(
    new List<ChatMessage>()
    {
        new SystemChatMessage("你是一位電影評論員，請根據使用者的評論好壞，並針對輸入的 <評論> 判斷與進行反駁，反駁的文字應在 20 字內"),

        new UserChatMessage("這部真可怕"),
        new AssistantChatMessage("好，這是鬼片當然可怕"),

        new UserChatMessage("這部真有趣"),
        new AssistantChatMessage("好，真是反串這部鬼片"),

        new UserChatMessage("這部真美妙"),
        new AssistantChatMessage("壞，鬼片怎會很美妙"),

        new UserChatMessage("這部看的很討厭"),
        new AssistantChatMessage("壞，鬼片是應該被討厭"),

        new UserChatMessage("這部看的很討喜"),
    });


Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");

