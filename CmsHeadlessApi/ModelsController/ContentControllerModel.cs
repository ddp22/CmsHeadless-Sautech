using CmsHeadless.Models;

namespace CmsHeadlessApi.ModelsController
{
    public class ContentControllerModel : Content
    {
        public string? MediaWithPath { get; set; }
        public string emailUser { get; set; }
        public List<Attributes> Attributes { get; set; }
        public List<Tag> Tag { get; set; }
        public List<Category> Category { get; set; }
        public ContentControllerModel(Content content, string pathMedia, string email, List<Attributes> Attributes, List<Tag> Tag, List<Category> Category)
        {
            
            ContentId = content.ContentId;
            Title = content.Title;
            Description = content.Description;
            InsertionDate = content.InsertionDate;
            Text = content.Text;
            LastEdit = content.LastEdit;
            PubblicationDate = content.PubblicationDate;
            UserId = content.UserId;
            if (content.Media != null)
            {
                MediaWithPath = pathMedia + content.Media;
            }
            emailUser = email;
            this.Attributes = Attributes;
            this.Tag = Tag;
            this.Category = Category;
        }
    }
}
