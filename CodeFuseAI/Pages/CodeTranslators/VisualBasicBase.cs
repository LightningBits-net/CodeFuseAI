using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;
using CodeFuseAI_Apps.Service.IService;
using ColorCode;

namespace CodeFuseAI_Apps.Pages.CodeTranslators
{

    public class VisualBasicBase : ComponentBase
    {
        [Inject]
        protected ICodeTranslationService? _codeTranslationService { get; set; }

        [Inject]
        public ISnackbar? _snackBar { get; set; }

        public TranslationDirection selectedOption { get; set; } = TranslationDirection.VbNetToCSharp;

        public string CodeReviewResult { get; set; } = string.Empty;
        public string inputCode { get; set; } = string.Empty;
        public string outputCode { get; set; } = string.Empty;

        public bool IsInactiveContent { get; set; } = true;
        public bool isSwitched { get; set; } = false;
        public bool IsProcessing { get; set; } = false;
        public bool IsProcessingReview { get; set; } = false;


        public string ErrorMessage { get; set; } = string.Empty;
        public string title { get; set; } = "";
        public string InputLabel => isSwitched ? "Enter your C# code" : "Enter your VB.NET code";

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

        public enum TranslationDirection
        {
            VbNetToCSharp,
            CSharpToVbNet
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            IsInactiveContent = true;
        }

        protected async Task TranslateCode()
        {
            IsInactiveContent = false;
            StateHasChanged();
            ErrorMessage = string.Empty;

            if (isSwitched && !IsCSharpCodeSnippet(inputCode))
            {
                ErrorMessage = "Please provide a valid C# code snippet.";
                _snackBar.Add(ErrorMessage, Severity.Error);
                IsProcessing = false;
                return;
            }
            else if (!isSwitched && !IsVBNetCodeSnippet(inputCode))
            {
                ErrorMessage = "Please provide a valid VB.NET code snippet.";
                _snackBar.Add(ErrorMessage, Severity.Error);
                IsProcessing = false;
                return;
            }

            IsProcessing = true;

            if (isSwitched)
            {
                outputCode = await _codeTranslationService.TranslateCSharpToVBNetAsync(inputCode);
            }
            else
            {
                outputCode = await _codeTranslationService.TranslateVBNetToCSharpAsync(inputCode);
            }

            IsProcessing = false;

           

            IsProcessingReview = true;

            StateHasChanged();

            CodeReviewResult = await _codeTranslationService.ExplainCodeDifferencesAsync(inputCode, outputCode, isSwitched ? "C#" : "VB.NET", isSwitched ? "VB.NET" : "C#");

            IsProcessingReview = false;

        }

        #region Helper methods
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
            IsInactiveContent = true;
            StateHasChanged();
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
        #endregion
    }
}

