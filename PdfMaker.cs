using DinkToPdf;
using DinkToPdf.Contracts;
using PdfSharpCore.Pdf.IO;
using System.Text;

namespace HTPDF;

public class PdfMaker : IPdfMaker
{
    private readonly IConverter _converter;

    public PdfMaker(IConverter converter)
    {
        _converter = converter;
    }

    public byte[] CreatePDF()
    {
        List<dynamic> data = GetData();

        string htmlTable = GenerateTable(data);
        var document = GetObjectSettings(htmlTable);

        return _converter.Convert(document);
    }

    public byte[] CreateChunkedPDF()
    {
        var data = GetData();
        var pdfChunks = PaginateAndConvertPDF(data, 5);

        using MemoryStream finalPdfStream = JoinChunkedPDF(pdfChunks);

        return finalPdfStream.ToArray();
    }

    private static List<dynamic> GetData()
    {
        return
        [
            new { Info = "Student 1", Math = 85, Science = 90, English = 88, History = 76, Geography = 89 },
            new { Info = "Student 2", Math = 78, Science = 92, English = 85, History = 80, Geography = 91 },
            new { Info = "Student 3", Math = 82, Science = 88, English = 79, History = 84, Geography = 87 },
            new { Info = "Student 3", Math = 82, Science = 88, English = 79, History = 84, Geography = 87 },
            new { Info = "Student 3", Math = 82, Science = 88, English = 79, History = 84, Geography = 87 },
            new { Info = "Student 3", Math = 82, Science = 88, English = 79, History = 84, Geography = 87 },
            new { Info = "Student 3", Math = 82, Science = 88, English = 79, History = 84, Geography = 87 },
            new { Info = "Student 3", Math = 82, Science = 88, English = 79, History = 84, Geography = 87 },
        ];
    }

    private static string GenerateTable(List<dynamic> data)
    {
        StringBuilder html = new();

        // Start the table
        html.Append(@"
        <table border='1' cellspacing='0' cellpadding='5'>
            <thead>
                <tr>
                    <th rowspan='2'>General Info</th>
                    <th colspan='2'>Performance</th>
                    <th colspan='3'>Scores</th>
                </tr>
                <tr>
                    <th>Math</th>
                    <th>Science</th>
                    <th>English</th>
                    <th>History</th>
                    <th>Geography</th>
                </tr>
            </thead>
            <tbody>");

        // Loop through the data to create rows
        foreach (var row in data)
        {
            html.Append($@"
            <tr>
                <td>{row.Info}</td>
                <td>{row.Math}</td>
                <td>{row.Science}</td>
                <td>{row.English}</td>
                <td>{row.History}</td>
                <td>{row.Geography}</td>
            </tr>");
        }

        // End the table
        html.Append(@"
            </tbody>
        </table>");

        return html.ToString();
    }

    private static MemoryStream JoinChunkedPDF(List<byte[]> pdfChunks)
    {
        var finalPdfStream = new MemoryStream();
        using (var finalPdfDocument = new PdfSharpCore.Pdf.PdfDocument())
        {
            foreach (var chunkChunk in pdfChunks)
            {
                using var tempDocument = PdfReader.Open(new MemoryStream(chunkChunk), PdfDocumentOpenMode.Import);
                foreach (var page in tempDocument.Pages)
                {
                    finalPdfDocument.AddPage(page);
                }
            }

            finalPdfDocument.Save(finalPdfStream, false);
        }

        return finalPdfStream;
    }

    private List<byte[]> PaginateAndConvertPDF(List<dynamic> data, int rowsPerPage)
    {
        if (rowsPerPage <= 0)
        {
            throw new ArgumentException("Rows per page must be greater than zero.", nameof(rowsPerPage));
        }

        var pdfChunks = new List<byte[]>();
        int totalRows = data.Count;
        int totalPageCount = (int)Math.Ceiling((double)totalRows / rowsPerPage);

        for (int currentPage = 0; currentPage < totalPageCount; currentPage++)
        {
            var pageData = data.Skip(currentPage * rowsPerPage).Take(rowsPerPage).ToList();
            var htmlContent = GenerateTable(pageData);

            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                break;
            }

            var document = GetObjectSettings(htmlContent);

            var bytes = _converter.Convert(document);

            pdfChunks.Add(bytes);
        }

        return pdfChunks;
    }

    private static HtmlToPdfDocument GetObjectSettings(string htmlContent)
    {
        var globalSettings = new GlobalSettings
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 },
            DocumentTitle = "TABLE",
        };

        var objectSettings = new ObjectSettings
        {
            HtmlContent = htmlContent,
            WebSettings =
            {
                DefaultEncoding = "utf-8",
            }
        };

        var document = new HtmlToPdfDocument()
        {
            GlobalSettings = globalSettings,
            Objects = { objectSettings }
        };

        return document;
    }
}
