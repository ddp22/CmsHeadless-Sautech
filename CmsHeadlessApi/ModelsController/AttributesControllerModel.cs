using CmsHeadless.Models;
using System.ComponentModel.DataAnnotations;

namespace CmsHeadlessApi.ModelsController
{
    public class AttributesControllerModel
    {
        public int AttributesId { get; set; }
        [MaxLength(50)]
        public string AttributeName { get; set; }
        [MaxLength(150)]
        public string AttributeValue { get; set; }
        public List<string> Typology { get; set; }
        public AttributesControllerModel(int AttributesId, string AttributeName, string AttributeValue, List<string> Typology)
        {
            this.AttributesId = AttributesId;
            this.AttributeName = AttributeName;
            this.AttributeValue = AttributeValue;
            this.Typology = Typology;
        }
    }
}
