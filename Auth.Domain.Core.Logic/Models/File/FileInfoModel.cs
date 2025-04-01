namespace Auth.Domain.Core.Logic.Models.File
{
    public class FileInfoModel
    {
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public bool EnableRangeProcessing { get; set; } = true;
        public string FileDownloadName { get; set; }
    }
}
