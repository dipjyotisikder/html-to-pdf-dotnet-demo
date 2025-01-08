using Microsoft.AspNetCore.Mvc;

namespace HTPDF.Controllers;

/// <summary>
/// Controller for handling PDF related requests.
/// </summary>
[ApiController]
[Route("pdf")]
public class PDFController(ILogger<PDFController> logger, IPdfMaker pdfMaker) : ControllerBase
{
    private readonly ILogger<PDFController> _logger = logger;
    private readonly IPdfMaker _pdfMaker = pdfMaker;

    /// <summary>
    /// Endpoint to get a PDF document.
    /// </summary>
    /// <returns>PDF file as a byte array.</returns>
    [HttpGet]
    public ActionResult Get()
    {
        var pdfBytes = _pdfMaker.CreatePDF();

        return File(pdfBytes, Constants.PDF, "Test.pdf");
    }

    /// <summary>
    /// Endpoint to get a chunked PDF document.
    /// </summary>
    /// <returns>Chunked PDF file as a byte array.</returns>
    [HttpGet("chunked")]
    public ActionResult GetChunked()
    {
        var pdfBytes = _pdfMaker.CreateChunkedPDF();

        return File(pdfBytes, Constants.PDF, "Test.pdf");
    }
}
