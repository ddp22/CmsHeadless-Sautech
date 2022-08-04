using CmsHeadless.Models;

namespace CmsHeadlessApi.Controllers.SupportClassContent
{
    public class ContentQrCodeJson
    {
        public Content contentParent { get; set; }
        public List<Content> contentChild { get; set; }
        public ContentQrCodeJson(Content contentParent, List<Content> contentChild)
        {
            this.contentParent = contentParent;
            this.contentChild = contentChild;
        }
    }
}
