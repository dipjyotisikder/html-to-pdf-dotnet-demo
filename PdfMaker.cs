using DinkToPdf;
using DinkToPdf.Contracts;
using PdfSharpCore.Pdf.IO;

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
        var htmlContent = GetHTML();

        var document = GetObjectSettings(htmlContent);

        return _converter.Convert(document);
    }

    public byte[] CreateChunkedPDF()
    {
        var pdfChunks = PaginateAndConvertPDF(20, 100);

        using MemoryStream finalPdfStream = JoinChunkedPDF(pdfChunks);

        return finalPdfStream.ToArray();
    }



    private static string GetHTML()
    {
        string html = @"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Multi-Layer Column Table</title>
            <style>
                table {
                    border-collapse: collapse;
                    width: 100%;
                }
                th, td {
                    border: 1px solid black;
                    padding: 8px;
                    text-align: center;
                }
                th {
                    background-color: #f2f2f2;
                }
            </style>
        </head>
        <body>
            <table>
                <thead>
                    <tr>
                        <th rowspan=""2"">General Info</th>
                        <th colspan=""2"">Performance</th>
                        <th colspan=""3"">Scores</th>
                    </tr>
                    <tr>
                        <th>Math</th>
                        <th>Science</th>
                        <th>English</th>
                        <th>History</th>
                        <th>Geography</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Student 1</td>
                        <td>85</td>
                        <td>90</td>
                        <td>88</td>
                        <td>76</td>
                        <td>89</td>
                    </tr>
                    <tr>
                        <td>Student 2</td>
                        <td>78</td>
                        <td>92</td>
                        <td>85</td>
                        <td>80</td>
                        <td>91</td>
                    </tr>
                    <tr>
                        <td>Student 3</td>
                        <td>82</td>
                        <td>88</td>
                        <td>79</td>
                        <td>84</td>
                        <td>87</td>
                    </tr>
                </tbody>
            </table>
        </body>
        </html>
        ";

        return html;
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

    private List<byte[]> PaginateAndConvertPDF(int rowsPerPage, int totalRows)
    {
        if (rowsPerPage <= 0)
        {
            throw new ArgumentException("Rows per page must be greater than zero.", nameof(rowsPerPage));
        }

        var pdfChunks = new List<byte[]>();
        int totalPageCount = (int)Math.Ceiling((double)totalRows / rowsPerPage);

        for (int currentPage = 0; currentPage < totalPageCount; currentPage++)
        {
            var htmlContent = GetHTML();

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
            DocumentTitle = "User",
        };

        var objectSettings = new ObjectSettings
        {
            HtmlContent = htmlContent,
            WebSettings =
            {
                DefaultEncoding = "utf-8",
                LoadImages = true,
                EnableIntelligentShrinking = true
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
