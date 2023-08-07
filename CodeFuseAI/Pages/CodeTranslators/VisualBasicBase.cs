using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;
using CodeFuseAI.Service.IService;
using ColorCode;

namespace CodeFuseAI.Pages.CodeTranslators
{

    public class VisualBasicBase : ComponentBase
    {
        [Inject]
        protected ICodeTranslationService _codeTranslationService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected ISnackbar snackBar { get; set; }

        public TranslationDirection selectedOption { get; set; } = TranslationDirection.VbNetToCSharp;
        public string CodeReviewResult { get; set; } = string.Empty;
        public string inputCode { get; set; } = string.Empty;
        public string outputCode { get; set; } = string.Empty;
        public bool IsLoading { get; set; } = false;
        public bool isSwitched { get; set; } = false;
        public string title { get; set; } = "";
        public string ErrorMessage { get; set; } = string.Empty;

        protected void UpdateTitle()
        {
            if (isSwitched)
            {
                title = "VB.NET to C#";
            }
            else
            {
                title = "C# to VB.NET";
            }
        }

        public string InputLabel => isSwitched ? "Enter your C# code" : "Enter your VB.NET code";
        public bool IsProcessing { get; set; } = false;

        public enum TranslationDirection
        {
            VbNetToCSharp,
            CSharpToVbNet
        }

        protected override void OnInitialized()
        {
            IsProcessing = true;
            base.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            IsProcessing = false;
        }

        protected async Task TranslateCode()
        {
            ErrorMessage = string.Empty;

            if (isSwitched && !IsCSharpCodeSnippet(inputCode))
            {
                ErrorMessage = "Please provide a valid C# code snippet.";
                snackBar.Add(ErrorMessage, Severity.Error);
                IsLoading = false;
                return;
            }
            else if (!isSwitched && !IsVBNetCodeSnippet(inputCode))
            {
                ErrorMessage = "Please provide a valid VB.NET code snippet.";
                snackBar.Add(ErrorMessage, Severity.Error);
                IsLoading = false;
                return;
            }

            IsLoading = true;

            if (isSwitched)
            {
                outputCode = await _codeTranslationService.TranslateCSharpToVBNetAsync(inputCode);
                CodeReviewResult = await _codeTranslationService.ExplainCodeDifferencesAsync(inputCode, outputCode, "C#", "VB.NET");
                //inputCode = HighlightCode(inputCode, Languages.CSharp.Id);
                //outputCode = HighlightCode(outputCode, Languages.VbDotNet.Id);
            }
            else
            {
                outputCode = await _codeTranslationService.TranslateVBNetToCSharpAsync(inputCode);
                CodeReviewResult = await _codeTranslationService.ExplainCodeDifferencesAsync(inputCode, outputCode, "VB.NET", "C#");
                //inputCode = HighlightCode(inputCode, Languages.VbDotNet.Id);
                //outputCode = HighlightCode(outputCode, Languages.CSharp.Id);
            }


            IsLoading = false;
        }

        protected string HighlightCode(string code, string language)
        {
            var formatter = new HtmlFormatter();
            return formatter.GetHtmlString(code, Languages.FindById(language));
        }



        protected void ResetPage()
        {
            selectedOption = TranslationDirection.VbNetToCSharp;
            inputCode = string.Empty;
            outputCode = string.Empty;
            CodeReviewResult = string.Empty;
            isSwitched = false;
            title = "";
            IsProcessing = false;
        }

        protected bool IsCSharpCodeSnippet(string text)
        {
            if (text == null)
            {
                return false;
            }
            return text.Contains(";") || (text.Contains("{") && text.Contains("}")) || (text.Contains("<") && text.Contains(">"));
        }

        protected bool IsVBNetCodeSnippet(string text)
        {
            if (text == null)
            {
                return false;
            }

            return text.Contains("Sub") || text.Contains("End Sub") || text.Contains("Dim");
        }
    }


}

