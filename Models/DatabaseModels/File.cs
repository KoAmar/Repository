using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Repository.Models.DatabaseModels
{
    public class File
    {
        [MaxLength(450)]
        [NotNull]
        public string Id { get; set; }
        public string Name { get; set; }
        [NotNull]
        public string FilePath { get; set; }
        [NotNull]
        [MaxLength(450)]
        public string ProjectId { get; set; }
    }
}