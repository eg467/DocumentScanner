using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentScanner.PdfProcessing
{
    public static class TextExtractor
    {
        public static string[] Read(string path)
        {
            string[] contents = null;
            using (var reader = new PdfReader(path))
            using (var doc = new PdfDocument(reader))
            {
                var numPages = doc.GetNumberOfPages();
                contents = contents ?? new string[numPages];
                var its = new LocationTextExtractionStrategy();
                for (var p = 0; p < numPages; p++)
                {
                    var page = doc.GetPage(p + 1);
                    contents[p] =
                        iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(
                            page,
                            its);
                }
            }
            return contents;
        }
    }
}