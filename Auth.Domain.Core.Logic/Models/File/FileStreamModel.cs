namespace Auth.Domain.Core.Logic.Models.File
{
    public class FileStreamModel
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; init; }
    }
}
