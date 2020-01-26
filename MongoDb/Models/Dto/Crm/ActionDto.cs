using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDb.Enums;

namespace MongoDb.Models.Dto.Crm
{
    public class ActionDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public DesignType? DesignType { get; set; }

        public IList<string> Choices { get; set; }

        public int? SortOrder { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "IsEnabled is required")]
        public bool? IsEnabled { get; set; }
    }
}