using Microsoft.AspNetCore.Mvc;

namespace HTPDF.Controllers;

[ApiController]
[Route("pdf")]
public class PDFController(ILogger<PDFController> logger, IPdfMaker pdfMaker) : ControllerBase
{
    private readonly ILogger<PDFController> _logger = logger;
    private readonly IPdfMaker _pdfMaker = pdfMaker;

    [HttpGet]
    public ActionResult Get()
    {
        var pdfBytes = _pdfMaker.CreatePDF();

        return File(pdfBytes, Constants.PDF, "Test.pdf");
    }

    [HttpGet("chunked")]
    public ActionResult GetChunked()
    {
        var pdfBytes = _pdfMaker.CreateChunkedPDF();

        return File(pdfBytes, Constants.PDF, "Test.pdf");
    }
}
