using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Repository.Models.DatabaseModels
{
    public class FileModel
    {
        [MaxLength(450)]
        // [NotNull]
        public string Id { get; set; }
        public string Name { get; set; }
        [NotNull]
        public string FilePath { get; set; }
        //todo uncomment this
        // [NotNull]
        [MaxLength(450)]
        public string ProjectId { get; set; }
    }
}