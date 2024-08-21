using Kentico.Content.Web.Mvc;
using Kentico.Membership;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Reflection;

namespace Goldfinch.Web.TagHelpers
{
    [HtmlTargetElement("page-edit-button-styles", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PageEditButtonStylesTagHelper : TagHelper
    {
        private readonly IWebPageDataContextRetriever _webPageDataContextRetriever;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PageEditButtonStylesTagHelper(IWebPageDataContextRetriever webPageDataContextRetriever, IHttpContextAccessor httpContextAccessor)
        {
            _webPageDataContextRetriever = webPageDataContextRetriever;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var httpContext = _httpContextAccessor.HttpContext;

            var shouldRenderStyles = await ShouldRenderStyle(httpContext);

            if (!shouldRenderStyles)
            {
                return;
            }

            output.TagName = "link";
            output.TagMode = TagMode.SelfClosing;
            output.Attributes.SetAttribute("rel", "stylesheet");
            output.Attributes.SetAttribute("href", $"/_content/XperienceCommunity.PageEditButton/css/edit-page-button.css?v={GetVersion()}");
        }

        private static string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

            return version ?? "1.0";
        }

        private async Task<bool> ShouldRenderStyle(HttpContext? httpContext)
        {
            if (httpContext is null)
            {
                return false;
            }

            if (!_webPageDataContextRetriever.TryRetrieve(out var data))
            {
                return false;
            }

            if (httpContext.Kentico().Preview().Enabled)
            {
                return false;
            }

            var authenticateResult = await httpContext.AuthenticateAsync(AdminIdentityConstants.APPLICATION_SCHEME);

            return authenticateResult.Succeeded &&
                authenticateResult.Principal?.Identity != null &&
                authenticateResult.Principal.Identity.IsAuthenticated;
        }
    }
}