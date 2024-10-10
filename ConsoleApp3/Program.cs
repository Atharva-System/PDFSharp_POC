// See https://aka.ms/new-console-template for more information
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

Console.WriteLine("Hello, World!");

string inputPdfPath = "1.BestFileToTest.pdf";
PdfDocument inputDocument = PdfReader.Open(inputPdfPath, PdfDocumentOpenMode.Import);


//var obj = new PdfSignatureOptions
//{
//    Renderer
//};