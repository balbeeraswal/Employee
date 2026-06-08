using System.IO;
using System.Text;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    [HttpGet("generatePDF")]
    public IActionResult GeneratePdf()
    {
        // Create a new PDF document
        PdfDocument document = new PdfDocument();
        document.Info.Title = "Sample PDF";

        // Add a page
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

        // Draw text
        gfx.DrawString("Hello, this is a PDF generated with PDFsharp!",
            font, XBrushes.Black,
            new XRect(0, 0, page.Width, page.Height),
            XStringFormats.Center);

        // Save to memory stream
        using var stream = new MemoryStream();
        document.Save(stream, false);
        byte[] pdfBytes = stream.ToArray();

        // Return as API response
        return File(pdfBytes, "application/pdf", "sample.pdf");
    }

    [HttpGet("generateCSV")]
    public IActionResult GenerateCSV()
    {

        var employees = new List<Employee>
        {
            new Employee { EmpId = 1, EmpName = "John Doe"},
              new Employee { EmpId = 1, EmpName = "John Doe"},
        };

        var csv = new StringBuilder();
        csv.AppendLine("Id,Name,Department");

        foreach (var emp in employees)
        {
            csv.AppendLine($"{emp.EmpId},{emp.EmpName}");
        }
        byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());
        return File(buffer, "text/csv", "employees.csv");
    }
}

