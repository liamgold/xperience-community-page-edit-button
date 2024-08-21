using Kentico.Content.Web.Mvc;
using Kentico.Membership;
using Kentico.Web.Mvc;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Websites.UIPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XperienceCommunity.PageEditButton.ViewComponents
{
    public class PageEditButtonViewComponent : ViewComponent
    {
        private readonly IPageUrlGenerator _pageUrlGenerator;
        private readonly IWebPageDataContextRetriever _webPageDataContextRetriever;

        public PageEditButtonViewComponent(IPageUrlGenerator pageUrlGenerator, IWebPageDataContextRetriever webPageDataContextRetriever)
        {
            _pageUrlGenerator = pageUrlGenerator;
            _webPageDataContextRetriever = webPageDataContextRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_webPageDataContextRetriever.TryRetrieve(out var data))
            {
                return Content(string.Empty);
            }

            if (HttpContext.Kentico().Preview().Enabled)
            {
                return Content(string.Empty);
            }

            var authenticateResult = await HttpContext.AuthenticateAsync(AdminIdentityConstants.APPLICATION_SCHEME);

            if (authenticateResult.Succeeded &&
                authenticateResult.Principal?.Identity != null &&
                authenticateResult.Principal.Identity.IsAuthenticated)
            {
                var pageUrl = _pageUrlGenerator.GenerateUrl<PageBuilderTab>($"webpages-{data.WebPage.WebsiteChannelID}", $"{data.WebPage.LanguageName}_{data.WebPage.WebPageItemID}");

                var model = $"/admin{pageUrl}";
                return View(model: model);
            }

            return Content(string.Empty);
        }
    }
}
