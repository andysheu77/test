// See https://aka.ms/new-console-template for more information
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Microsoft.SemanticKernel.Plugins.Web;
using SKConsoleApp.NavFuncs;
using System.Diagnostics;
using System.Text;
using Google.Apis.CustomSearchAPI.v1.Data;
using System.Text.Json;
using System.IO;
using System.Reflection.PortableExecutable;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Net.Http;
using System;

var builder = Kernel.CreateBuilder();

builder.AddAzureOpenAIChatCompletion(
    "andy",
    "https://andysheu.openai.azure.com/",
    "e3330c0963cd46e0bb19c434fb4f45c0",
    "gpt-4o");
#pragma warning disable SKEXP0050 // 類型僅供評估之用，可能會在未來更新中變更或移除。抑制此診斷以繼續。
builder.Plugins.AddFromType<ConversationSummaryPlugin>();
#pragma warning restore SKEXP0050 // 類型僅供評估之用，可能會在未來更新中變更或移除。抑制此診斷以繼續。

var kernel = builder.Build();
#region

//var result = await kernel.InvokePromptAsync("請推薦含有雞蛋與起司的早餐清單給我");
//var input = "請推薦含有雞蛋與起司的早餐清單給我";
//var result = await kernel.InvokeAsync("ConversationSummaryPlugin", "GetConversationActionItems"
//    ,new() {{"input" , input }});
/* KernelArguments
string nativeLang = "台灣";
string nativeLang2 = "日本";

string prompt = @"建立一系列的實用的詞彙是{{$language}}與{{$language2}}對照，讓{{$language}}旅行者在{{$language2}}旅行途中需要的{{$language2}}詞語時，可以有完善的表達。
                                    並且依照下列格式產生:  {{$language}} - {{$language2}} [羅馬拼音]";
var result = await kernel.InvokePromptAsync(prompt, new KernelArguments() {
    { "language", nativeLang },
    { "language2", nativeLang2 }
});
*/
/* InvokePromptAsync
string input = "正在計劃和配偶一起的周年旅行。你們喜歡徒步旅行、山脈和海灘。你們的旅行預算是15000新台幣。";

string prompt = @$"<message role=""system"">Instructions: 識別從 出發地 到 目的地  與 使用者給予的旅行日期</message>
                                        <message role=""user"">可以給我相關的列車資訊從台北到高雄? 我將在7月30日到8月5日進行旅行</message>
                                        <message role=""assistant"">台北|高雄|07/30/2024|08/05/2024</message>
                                        <message role=""user"">${input}</message>";

var result = await kernel.InvokePromptAsync(prompt);
*/
/* ImportPluginFromPromptDirectory 

var pluginsDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "MyPlugin", "MySkPrompt");

var prompts = kernel.ImportPluginFromPromptDirectory(pluginsDirectory);
Console.WriteLine("bot: 你想聽什麼主題的故事呢? \n");
Console.Write("you: ");
string storySubject = Console.ReadLine();

Console.Write("\n");
Console.WriteLine("bot: 故事的角色是什麼呢? \n");
Console.Write("you: ");
string storyRole = Console.ReadLine();
Console.WriteLine("bot: 故事中使用到的數字會是多少呢? \n");
Console.Write("how much: ");
string storyMoney = Console.ReadLine();
var result = await kernel.InvokeAsync<string>(prompts["GetStory"],
    new() {
        { "story_subject", storySubject },
        { "story_role", storyRole },
{ "story_money",  storyMoney },    }
);
*/
/* CreateFunctionFromPrompt
string skprompt = @"現在你是一位童話故事創作高手，請根據下列主題
""""""
{{$story_subject}}
"""""" 

以及以下角色，使用繁體中文撰寫童話故事給 3 到 8 歲小朋友看，
""""""
{{$story_role}}
"""""" ";
var kernelFunction = kernel.CreateFunctionFromPrompt(skprompt, executionSettings:
                                                                                                                                                        new OpenAIPromptExecutionSettings
                                                                                                                                                        { MaxTokens = 2000, Temperature = 0.8 },
                                                                                                                                  description: "根據主題及角色創造童話故事給小朋友聽");
Console.WriteLine("bot: 你想聽什麼主題的故事呢? \n");
Console.Write("you: ");
string storySubject = Console.ReadLine();

Console.Write("\n");
Console.WriteLine("bot: 故事的角色是什麼呢? \n");
Console.Write("you: ");
string storyRole = Console.ReadLine();
*/
/* ImportPluginFromType
KernelPlugin myPlugins = kernel.ImportPluginFromType<SKConsoleApp.NavFuncs.MyPlugins>();
Console.Write("請輸入想轉換的阿拉伯數字：");
var userinput = Console.ReadLine();
string? result=string.Empty;
if (long.TryParse(userinput, out long number))
{
    result = await kernel.InvokeAsync<string>(myPlugins.Name,"ConvertToChineseNumber"
        , arguments: new KernelArguments() { ["number"] = number });

}
else
{
    Console.WriteLine("無效的輸入。");
}
*/

#endregion
var pluginsDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "QAPlugins");

var prompts = kernel.ImportPluginFromPromptDirectory(pluginsDirectory);
//Console.WriteLine(Console.OutputEncoding);
#pragma warning disable SKEXP0050 // 類型僅供評估之用，可能會在未來更新中變更或移除。抑制此診斷以繼續。
var bingConnector = new BingConnector("c69f4a306f224dacaf9c2ed851421507");
var plugin = new WebSearchEnginePlugin(bingConnector);
#pragma warning restore SKEXP0050 // 類型僅供評估之用，可能會在未來更新中變更或移除。抑制此診斷以繼續。
var bingplugin = kernel.ImportPluginFromObject(plugin);

var result = await kernel.InvokeAsync<string>(bingplugin.Name, "GetSearchResults"
   , arguments: new KernelArguments() { ["query"] = "https://tw.news.yahoo.com/" });
//var pages = JsonSerializer.Deserialize<IEnumerable<WebPage>>(result).ToArray();
HttpClient client=new HttpClient();
HttpResponseMessage response = await client.GetAsync("https://tw.news.yahoo.com");

string htmlContent = await response.Content.ReadAsStringAsync();
HtmlDocument htmlDocument = new HtmlDocument();
htmlDocument.LoadHtml(htmlContent);

// Use Fizzler to select the stock element using a CSS selector
var stockElement = htmlDocument.DocumentNode.QuerySelectorAll("div.Cf > div > h3 , p");

var stockAmount = string.Concat( stockElement.Where(a=>a.Name=="h3").Select(a => 
    $"標題 = {a.InnerText},來源 = {a.FirstChild.Attributes["href"].Value},內容 = a.NextSibling.InnerText")
    );
//#Col1-10-NewsCollectionStream-0-Stream > ul > li:nth-child(4) > div > div
result = await kernel.InvokeAsync<string>(prompts["AssistantResults"],
    new() {
        { "ans_result",  stockAmount},
        { "query_input", "台灣選手奧運" } 
    }
);
Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine(result);
//Debug.WriteLine(result);