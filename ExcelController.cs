using Hangfire;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace ExcelUploadApi
{
    [ApiController]
    [Route("api/[controller]")]

    public class ExcelUploadController : ControllerBase
    {

        private readonly ExcelUploaderDbContext _context;
        public ExcelUploadController(ExcelUploaderDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello, World!");
        }


        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            var extension = Path.GetExtension(file.FileName);
            if (extension != ".xlsx" && extension != ".xls")
            {
                return BadRequest("Invalid format. Only .xlsx or .xls supported");
            }

            var filePath = Path.Combine(Path.GetTempPath(), file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //  background job 
            var jobId = BackgroundJob.Enqueue(() => ProcessExcelFile(filePath));

            return Ok(new { message = "File uploaded successfully.", jobId });
        }



        #region Private Methods
        private void ProcessExcelFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            var entities = new List<ExcelUploaderModel>();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string col1 = worksheet.Cells[row, 1].Text;
                    string col2 = worksheet.Cells[row, 2].Text;

                    entities.Add(new ExcelUploaderModel { Column1 = col1, Column2 = col2 });
                }
            }

            if (entities.Count > 0)
            {
                _context.Data.AddRange(entities);
                _context.SaveChanges();
            }

        }
        #endregion




    }



}
