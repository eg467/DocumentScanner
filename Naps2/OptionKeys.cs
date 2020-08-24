using System.Runtime.CompilerServices;

namespace DocumentScanner.NapsOptions.Keys
{
    /// <summary>
    /// From: https://www.naps2.com/doc-command-line.html
    /// </summary>
    public static class Main
    {
        // MAIN OPTIONS
        public const string Output = "-o";

        public const string Profile = "-p";
        public const string Verbose = "-v";
        public const string Progress = "--progress";
        public const string NumScans = "-n";
        public const string Delay = "-d";
        public const string ForceOverwrite = "--force";
        public const string Wait = "--wait";

        /// <summary>
        ///  Installs the specified component. Possible values: "genericimport", "ocr-eng" (and other language codes - see reference)
        ///  http://www.loc.gov/standards/iso639-2/php/code_list.php
        /// </summary>
        public const string Install = "--install";

        public const string Help = "--help";
        public const string Email = "-e";
        public const string Autosave = "-a";
        public const string Import = "-i";
        public const string Combine = "-c";
    }

    public static class Order
    {
        /// <summary>
        ///  Specifies that pages should be interleaved. For example, if you scan pages in the order "1, 3, 5, 2, 4, 6", using this option will result in the order "1, 2, 3, 4, 5, 6".
        /// </summary>
        public const string Interleave = "--interleave";

        /// <summary>
        /// Specifies that pages should be interleaved in an alternative way. For example, if you scan pages in the order "1, 3, 5, 6, 4, 2", using this option will result in the order "1, 2, 3, 4, 5, 6".
        ///
        /// </summary>
        public const string AltInterleave = "--altinterleave";

        /// <summary>
        /// Specifies that pages should be deinterleaved. For example, if you have pages in the order "1, 4, 2, 5, 3, 6", using this option will result in the order "1, 2, 3, 4, 5, 6".
        /// </summary>
        public const string Deinterleave = "--deinterleave";

        /// <summary>
        ///  Specifies that pages should be deinterleaved in an alternative way. For example, if you have pages in the order "1, 6, 2, 5, 3, 4", using this option will result in the order "1, 2, 3, 4, 5, 6".
        /// </summary>
        public const string AltDeinterleave = "--altdeinterleave";

        /// <summary>
        /// Specifies that pages should be reversed. For example, if you scan pages in the order "6, 5, 4, 3, 2, 1", using this option will result in the order "1, 2, 3, 4, 5, 6"
        /// </summary>
        public const string Reverse = "--reverse";

        /// <summary>
        ///  Specifies that each page should be saved to its own PDF or TIFF file.
        /// </summary>
        public const string Split = "--split";

        /// <summary>
        ///  Specifies that the pages from each scan should be saved to their own file. This only makes sense with the -n option.
        /// </summary>
        public const string SplitScans = "--splitscans";

        /// <summary>
        ///  Specifies that pages should be saved to multiple files, separated by Patch-T pages.
        /// </summary>
        public const string SplitPatcht = "--splitpatcht";

        /// <summary>
        ///  Specifies that pages should be saved to multiple files, with the given number of pages per file.
        /// </summary>
        public const string SplitSize = "--splitsize";
    }

    public static class Quality
    {
        // QUALITY
        /// <summary>
        /// 0-100
        /// </summary>
        public const string JpegQuality = "--jpegquality ";

        /// <summary>
        /// Specifies the type of compression used for TIFF files ("auto", "lzw", "ccitt4", or "none").
        /// </summary>
        public const string TiffCompression = "--tiffcomp  ";
    }

    public static class EmailFlags
    {
        public const string To = "--to";
        public const string Cc = "--cc";
        public const string Bcc = "--bcc";
        public const string Subject = "--subject";
        public const string Body = "--body";
        public const string AutoSend = "--autosend";
        public const string SilentSend = "--silentsend";
    }

    public static class PdfFlags
    {
        public const string Title = "--pdftitle";
        public const string Author = "--pdfauthor";
        public const string Subject = "--pdfsubject";
        public const string Keywords = "--pdfkeywords";
        public const string UseSavedMetadata = "--usesavedmetadata";
        public const string EncryptConfig = "--encryptconfig";
        public const string UseSavedEncryptConfig = "--usesavedencryptconfig";
    }

    public static class OcrFlags
    {
        /// <summary>
        /// Specifies the three-letter code for the language used for OCR (e.g. 'eng' for English, 'fra' for French, etc.). Multiple codes can be separated by the '+' symbol. Implies --enableocr.
        /// </summary>
        public const string OcrLang = "--ocrlang";

        public const string OcrEnable = "--enableocr";
        public const string OcrDisable = "--disableocr";
    }
}