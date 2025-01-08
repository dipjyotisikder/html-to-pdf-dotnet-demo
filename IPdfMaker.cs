namespace HTPDF;

public interface IPdfMaker
{
    /// <summary>
    /// Method to create PDF.
    /// </summary>
    /// <param name="htmlContent"></param>
    /// <returns></returns>
    public byte[] CreatePDF();

    /// <summary>
    /// Method to create PDF.
    /// </summary>
    /// <param name="htmlContent"></param>
    /// <returns></returns>
    public byte[] CreateChunkedPDF();
}
