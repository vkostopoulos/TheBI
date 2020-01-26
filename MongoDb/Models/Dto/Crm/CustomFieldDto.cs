using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDb.Enums;

namespace MongoDb.Models.Dto.Crm
    {
    public class CustomFieldDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public DesignType? DesignType { get; set; }

        public IList<string> Choices { get; set; }

        public int? SortOrder { get; set; }

        [Required(ErrorMessage = "IsRequired is required")]
        public bool? IsRequired { get; set; }

        [Required(ErrorMessage = "IsEnabled is required")]
        public bool? IsEnabled { get; set; }
    }
}