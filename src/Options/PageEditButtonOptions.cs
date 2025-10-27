namespace XperienceCommunity.PageEditButton.Options
{
    /// <summary>
    /// Configuration options for the Page Edit Button package.
    /// </summary>
    public class PageEditButtonOptions
    {
        /// <summary>
        /// The domain where the Xperience administration interface is hosted.
        /// If specified, the edit button will generate an absolute URL to this domain.
        /// If not specified, the button will use a relative URL (current domain).
        /// </summary>
        /// <example>
        /// For a dedicated admin domain: "admin.domain.com"
        /// For main domain: "domain.com"
        /// </example>
        public string? AdminDomain { get; set; }
    }
}
