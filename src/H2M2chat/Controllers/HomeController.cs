using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System; //Contains fundamental classes and base classes that define commonly-used value and reference data types
 //contains classes and properties that are used to create HTML elements. This class is used to write helper
using H2M2chat.Models;

namespace H2M2chat.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        string webRootPath = _webHostEnvironment.WebRootPath;
        List<ObjFile> ObjFiles = new List<ObjFile>();
        foreach (string strfile in Directory.GetFiles(Path.Combine(webRootPath,"Files")))
        {
            FileInfo fi = new FileInfo(strfile);
            ObjFile obj = new ObjFile();
            obj.File = fi.Name;
            obj.Size = fi.Length;
            obj.Type = GetFileTypeByExtension(fi.Extension);
            ObjFiles.Add(obj);
        }

        return View(ObjFiles);
    }
    public FileResult Download(string fileName)
    {
        string webRootPath = _webHostEnvironment.WebRootPath+"/Files";
        string fullPath = Path.Combine(webRootPath, fileName);
        byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
    }

    private string GetFileTypeByExtension(string fileExtension)

    {
        switch (fileExtension.ToLower())  // check the file type and return it
        {
            case ".docx":
            case ".doc":
                return "Microsoft Word Document";
            case ".xlsx":
            case ".xls":
                return "Microsoft Excel Document";
            case ".txt":
                return "Text Document";
            case ".jpg":
            case ".png":
                return "Image";
            default:
                return "Unknown";
        }
    }
    [HttpPost]
    public async Task<ActionResult> IndexAsync(ObjFile doc)
    {
        foreach (var file in doc.files)
        {

            if (file.Length > 0)
            {
                string webRootPath = _webHostEnvironment.WebRootPath+"/Files";
                var fileName = Path.GetFileName(file.FileName);  //returns the file name and extision of the file path                                                      
                string? filePath = Path.Combine(webRootPath,
                                                fileName);   //gives us the ability to add more than one file (it compines two strings into a path)
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                  // saves the content of the uploaded files
            }
        }
        TempData["Message"] = "files uploaded successfully";
        return RedirectToAction("Index");
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}