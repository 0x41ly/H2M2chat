namespace H2M2chat.Models
{
    public class ObjFile
{
    public IEnumerable<IFormFile> files { get; set; } // definition for the base class of all classes 
                                                               // that provide access to individual files that have been uploaded by client
    public string File { get; set; } //files upload and download
    public long Size { get; set; }
    public string Type { get; set; }
}
}