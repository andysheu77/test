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
        new UserChatMessage("Hi, 你能夠幫助我嗎?"),
    });


Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");

