// See https://aka.ms/new-console-template for more information
using VortexScript.Core;
using VortexScript.Core.Lang;

Console.WriteLine("Hello, World!");
var code = File.ReadAllText("F:\\GIT\\VortexScriptX\\script.vs");
var t = new Tokenizer(code, LangContext.Instance);
var tokens = t.Tokenize();
var parser = new Parser(tokens);
parser.ParseStatement();
var x = 1;