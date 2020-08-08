using System.ComponentModel.DataAnnotations;

using System.Xml.Serialization;

using TeisterMask.Data.Models;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType(nameof(Project))]
    public class ImportProjectDto
    {
        [Required]
        [MinLength(2), MaxLength(40)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlArray("TaskDtos")]
        public ImportTasksDto[] TasksDtos { get; set; }
    }
}
