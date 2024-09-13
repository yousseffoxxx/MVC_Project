namespace PresentationLayer.Utilities
{
    public static class DocumentSittings
    {
        public static string UploadFile(IFormFile file , string folderName)
        {
            // Create Folder Path
            //  //E:/NewFolder/MvcDemoSln/Demo.PL/www.root/Files/folderName

            /*string folderPath = Directory.GetCurrentDirectory() + @"\wwwroot\Files";*/

            string folderPath = Path.Combine(Directory.GetCurrentDirectory() , @"\wwwroot\Files" , folderName);

            // Create Unique Name for the File

            string fileName = $"{Guid.NewGuid}-{file.FileName}";

            // create file Path
            //  //E:/NewFolder/MvcDemoSln/Demo.PL/www.root/Files/folderName/Image.Png

            string filePath = Path.Combine(folderPath, fileName);

            // Create FileStream to save the File

            using var stream = new FileStream(filePath , FileMode.Create);

            // Copy File to FileStream

            file.CopyTo(stream);

            return fileName;
        }

        public static void DeleteFile(string folderName , string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"\wwwroot\Files", folderName, fileName);

            if (File.Exists(filePath)) 
            {
                File.Delete(filePath);
            }
        }
    }
}
