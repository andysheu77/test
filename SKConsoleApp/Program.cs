// See https://aka.ms/new-console-template for more information
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;

var builder = Kernel.CreateBuilder();

builder.AddAzureOpenAIChatCompletion(
    "MyGPT4o",
    "https://myaoaiclassdemo.openai.azure.com",
    "daf7070cfbc04099a6317d393f3a9d2d",
    "gpt-4o");
#pragma warning disable SKEXP0050 // 類型僅供評估之用，可能會在未來更新中變更或移除。抑制此診斷以繼續。
builder.Plugins.AddFromType<ConversationSummaryPlugin>();
#pragma warning restore SKEXP0050 // 類型僅供評估之用，可能會在未來更新中變更或移除。抑制此診斷以繼續。

var kernel = builder.Build();
//var result = await kernel.InvokePromptAsync("請推薦含有雞蛋與起司的早餐清單給我");
//var input = "請推薦含有雞蛋與起司的早餐清單給我";
//var result = await kernel.InvokeAsync("ConversationSummaryPlugin", "GetConversationActionItems"
//    ,new() {{"input" , input }});
string background = @"在我忙碌的廚房裡，我努力滿足家人多樣的口味，應對他們獨特的喜好。
                                    我家有些人挑食，有些人有過敏問題，這讓我的烹飪菜單需要探索豐富的素食食譜。
                                    我的其中一個孩子對任何綠色食物都有抗拒，其中一個對有花生過敏，這使得菜譜的計劃變得更加複雜。
                                    但我仍是對健康烹飪充滿熱情，相信我會發現不僅能滿足挑剔口味，而且又健康美味的植物性菜餚。";

string prompt = @"這是一些有關使用者的背景知識: {{$history}} 根據使用者的背景，提供一些相關的食譜，滿足他們的需求。";

var result = await kernel.InvokePromptAsync(prompt, new KernelArguments() { { "history", background } });
Console.WriteLine(result);