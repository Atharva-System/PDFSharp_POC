// PDFsharp - A .NET library for processing PDF
// See the LICENSE file in the solution root for more information.

/*
  This sample demonstrates how to create and open a PDF 1.6 document with 
  AES 128 bit encryption.
*/

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Signatures;
using PdfSharp.Quality;
using System.Security.Cryptography.X509Certificates;



// Create a new PDF document.
//var document = new PdfDocument();
//document.Info.Title = "AES 128 bit encryption demonstration";
//document.PageLayout = PdfPageLayout.SinglePage;


var testFile = IOUtility.GetAssetsPath("1.BestFileToTest.pdf");
var document = PdfReader.Open(testFile, PdfDocumentOpenMode.Modify);

//ResizeDocument(document);

//// Create an empty page in this document.
//var page = document.AddPage();

//// Draw some text.
//var gfx = XGraphics.FromPdfPage(page);
//var font = new XFont("Times New Roman", 20, XFontStyleEx.BoldItalic);
//gfx.DrawString("SignCare Permission By TM", font, XBrushes.Black,
//    new XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.Center);

//AppendPages(document);
Sign(document);

//var filename = PdfFileUtility.GetTempPdfFileName("Rental");
//document.Save(filename);

//using FileStream filestream = AppendPage(filename);
//using var finalDoc = PdfReader.Open(Path.Combine(Path.GetTempPath(), filename), PdfDocumentOpenMode.Modify);


//var cert = new X509Certificate2(@"C:\Test Digital Certificate Password is 123456.pfx", "123456");
////for (var i = 1; i <= 2; i++)
////{
//var options = new DigitalSignatureOptions
//{
//    //Certificate = cert,
//    //FieldName = "Signature-" + Guid.NewGuid().ToString("N"),
//    //PageIndex = 0,
//    //Rectangle = new XRect(120 * i, 40, 100, 60),
//    //Location = "My PC",
//    //Reason = "Approving Rev #" + i,

//    ContactInfo = "SignCare",
//    Location = "India",
//    Reason = "License Agreement",
//    Rectangle = new XRect(120, 40, 100, 60),
//    AppearanceHandler = new DefaultSigner.SignatureAppearanceHandler()

//    // Signature appearances can also consist of an image (Rectangle should be adapted to image's aspect ratio)
//    //Image = XImage.FromFile(@"C:\Data\stamp.png")
//};

//// Specify the URI of a timestamp server if you want a signature with timestamp.
//var pdfSignatureHandler = DigitalSignatureHandler.ForDocument(finalDoc,
//    new PdfSharpDefaultSigner(cert, PdfMessageDigestType.SHA256, new Uri("http://timestamp.apple.com/ts01")),
//    options);



//}

// =====================================
// Part 1 - Create an encrypted PDF file
// =====================================

// Set document encryption.
SecureDoc(document);

var filename = PdfFileUtility.GetTempPdfFileName("Rental");
document.Save(filename);
// Save the document...
//var filename = PdfFileUtility.GetTempPdfFullFileName("samples-PDFsharp/AES128");
//finalDoc.Save(filename);
PdfFileUtility.ShowDocument(filename);




static PdfDocument Sign(PdfDocument doc)
{

    var cert = new X509Certificate2(@"C:\Test Digital Certificate Password is 123456.pfx", "123456");
    var index = 0;
    var testFile = IOUtility.GetAssetsPath("1.BestFileToTest.pdf");

    // Create a new PDF document for output
    PdfDocument outputDocument = new PdfDocument();
    // Open the file to resize
    XPdfForm form = XPdfForm.FromFile(testFile);
    XGraphics gfx;
    XRect box;

    XFont font = new XFont("Verdana", 8, XFontStyleEx.Bold);
    XStringFormat format = new XStringFormat();
    format.Alignment = XStringAlignment.Center;
    format.LineAlignment = XLineAlignment.Far;

    for (int idx = 0; idx < form.PageCount; idx++)
    {

        form.PageNumber = idx + 1;
        // Add a new page to the output document
        PdfPage page = outputDocument.AddPage();

        gfx = XGraphics.FromPdfPage(page);

        //double Scale = form.Page.Height / 842.0;
        Console.WriteLine($"Program is running from: {form?.Page?.Width}-{form?.Page?.Height}");


        box = new XRect(0, 0, (int)page.Width.Point, (int)page.Height.Point);
        //box = new XRect(0, 0, (int)(form.Page.Width / Scale), (int)(form.Page.Height / Scale));
        gfx.DrawImage(form, box);

        // Write document file name and page number on each page

        box.Inflate(0, -10);

        gfx.DrawString(String.Format("-{1} -", string.Empty, idx + 1), font, XBrushes.Red, box, format);


        var options = new DigitalSignatureOptions
        {
            //Certificate = cert,
            //FieldName = "Signature-" + Guid.NewGuid().ToString("N"),
            //PageIndex = 0,
            //Rectangle = new XRect(120 * i, 40, 100, 60),
            //Location = "My PC",
            //Reason = "Approving Rev #" + i,

            ContactInfo = "SignCare Address",
            AppName = "SignCare Pro",
            Location = "India",
            Reason = "License Agreement" + index,
            PageIndex = index,
            Rectangle = new XRect(200, 40, 100, 60),
            AppearanceHandler = new DefaultSigner.SignatureAppearanceHandler()

            // Signature appearances can also consist of an image (Rectangle should be adapted to image's aspect ratio)
            //Image = XImage.FromFile(@"C:\Data\stamp.png")
        };
        // Specify the URI of a timestamp server if you want a signature with timestamp.
        var pdfSignatureHandler = DigitalSignatureHandler.ForDocument(outputDocument,
            new PdfSharpDefaultSigner(cert, PdfMessageDigestType.SHA256, null),
            options);

    }

    string newfilename = @"resized11.pdf";
    outputDocument.Save(newfilename);

    foreach (PdfPage page in doc.Pages)
    {

        var options = new DigitalSignatureOptions
        {
            //Certificate = cert,
            //FieldName = "Signature-" + Guid.NewGuid().ToString("N"),
            //PageIndex = 0,
            //Rectangle = new XRect(120 * i, 40, 100, 60),
            //Location = "My PC",
            //Reason = "Approving Rev #" + i,

            ContactInfo = "SignCare Address",
            AppName = "SignCare Pro",
            Location = "India",
            Reason = "License Agreement" + index,
            PageIndex = index,
            Rectangle = new XRect(120 * index, 40, 100, 60),
            AppearanceHandler = new DefaultSigner.SignatureAppearanceHandler()

            // Signature appearances can also consist of an image (Rectangle should be adapted to image's aspect ratio)
            //Image = XImage.FromFile(@"C:\Data\stamp.png")
        };
        // Specify the URI of a timestamp server if you want a signature with timestamp.
        var pdfSignatureHandler = DigitalSignatureHandler.ForDocument(doc,
            new PdfSharpDefaultSigner(cert, PdfMessageDigestType.SHA256, null),
            options);
        index++;
    }




    return doc;
}



static PdfDocument AppendPages(PdfDocument doc)
{
    //var fs = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
    //var doc = PdfReader.Open(fs, PdfDocumentOpenMode.Modify);
    //var page2 = doc.AddPage();

    // Draw some text.
    //using var gfx2 = XGraphics.FromPdfPage(page2);
    //var font = new XFont("Times New Roman", 20, XFontStyleEx.BoldItalic);
    //gfx2.DrawString("I am 2nd page", font, XBrushes.Black,
    //    new XRect(0, 0, page2.Width.Point, page2.Height.Point), XStringFormats.Center);



    for (int i = 0; i < 5; i++)
    {
        doc.AddPage();
    }
    var numPages = doc.PageCount;
    var numContentsPerPage = new List<int>();

    foreach (PdfPage page in doc.Pages)
    {
        // remember count of existing contents
        numContentsPerPage.Add(page.Contents.Elements.Count);
        // add new content
        using var gfx = XGraphics.FromPdfPage(page);
        gfx.DrawString("I was added", new XFont("Arial", 16), new XSolidBrush(XColors.Red), 40, 40);
    }
    return doc;

    //doc.Save(fs, true);
    //return fs;
}


static PdfDocument ResizeDocument(PdfDocument inputDocument)
{
    // resize this  file from A3 to A4
    //string filename = @"C:\temp\A3.pdf";
    //string filename = IOUtility.GetAssetsPath("1.BestFileToTest.pdf");
    //PdfDocument inputDocument = new PdfDocument(filename);

    // Create the new output document (A4)
    string newfilename = @"d:\resized.pdf";
    // Create a new PDF document for output (modifiable)
    PdfDocument outputDocument = new PdfDocument();
    //outputDocument.PageLayout = PdfPageLayout.SinglePage;

    // Open the file to resize
    //XPdfForm form = XPdfForm.FromFile(filename);
    // Ensure the page is A4 size
    XSize a4Size = PageSizeConverter.ToSize(PageSize.A4);


    foreach (PdfPage page in inputDocument.Pages)
    {
        // Get the dimensions of the current page
        double width = page.Width.Point;
        double height = page.Height.Point;

        // Create a new A4 page in the output document
        PdfPage newPage = outputDocument.AddPage();
        newPage.Width = a4Size.Width;
        newPage.Height = a4Size.Height;

        // Create graphics object to draw on the new A4 page
        XGraphics gfx = XGraphics.FromPdfPage(newPage);

        // Calculate the scaling factor to fit the content within the A4 page
        double scaleX = a4Size.Width / width;
        double scaleY = a4Size.Height / height;
        double scale = Math.Min(scaleX, scaleY);

        // Adjust for rotation if needed
        if (width > height)
        {
            // Rotate the page to make it A4-compatible
            newPage.Rotate = 90;

            // Translate the graphics for the rotated content
            gfx.TranslateTransform(0, a4Size.Height);
            gfx.RotateTransform(-90);
        }

        // Draw the content of the old page onto the new A4 page with scaling
        gfx.ScaleTransform(scale);
        gfx.DrawString(page.Contents.Elements.ToString(), new XFont("Arial", 12), XBrushes.Black, new XPoint(0, 0));

    }
    outputDocument.Save(newfilename);

    return outputDocument;
}


static FileStream AppendPage(string filename)
{
    var fs = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
    var doc = PdfReader.Open(fs, PdfDocumentOpenMode.Modify);
    var page2 = doc.AddPage();

    // Draw some text.
    //using var gfx2 = XGraphics.FromPdfPage(page2);
    //var font = new XFont("Times New Roman", 20, XFontStyleEx.BoldItalic);
    //gfx2.DrawString("I am 2nd page", font, XBrushes.Black,
    //    new XRect(0, 0, page2.Width.Point, page2.Height.Point), XStringFormats.Center);

    var numPages = doc.PageCount;
    var numContentsPerPage = new List<int>();
    foreach (PdfPage page in doc.Pages)
    {
        // remember count of existing contents
        numContentsPerPage.Add(page.Contents.Elements.Count);
        // add new content
        using var gfx = XGraphics.FromPdfPage(page);
        gfx.DrawString("I was added", new XFont("Arial", 16), new XSolidBrush(XColors.Red), 40, 40);
    }

    doc.Save(fs, true);
    return fs;
}

static void SecureDoc(PdfDocument finalDoc)
{
    //document.SecuritySettings.UserPassword = "test1";
    finalDoc.SecuritySettings.OwnerPassword = "ownpwd";

    //finalDoc.SecuritySettings.PermitPrint = true;
    //finalDoc.SecuritySettings.PermitExtractContent = false;
    //finalDoc.SecuritySettings.PermitFormsFill = false;

    //document.SecuritySettings.PermitAccessibilityExtractContent = false;
    finalDoc.SecuritySettings.PermitAnnotations = false;
    finalDoc.SecuritySettings.PermitAssembleDocument = false;
    finalDoc.SecuritySettings.PermitExtractContent = false;
    finalDoc.SecuritySettings.PermitFormsFill = false;
    finalDoc.SecuritySettings.PermitFullQualityPrint = false;
    finalDoc.SecuritySettings.PermitModifyDocument = false;
    finalDoc.SecuritySettings.PermitPrint = false;



    var securityHandler = finalDoc.SecurityHandler;
    securityHandler.SetEncryptionToV2With128Bits();
}




// ===================================
// Part 2 - Open an encrypted PDF file
// ===================================

// Open the PDF document.
// After opening the document with the correct password it is not protected anymore.
// You must set the security handler again to save it encrypted.
//document = PdfReader.Open(filename, userPassword, PdfDocumentOpenMode.Modify);

//// Draw some text on 2nd page.
//page = document.AddPage();
//gfx = XGraphics.FromPdfPage(page);
//gfx.DrawString("2nd page", font, XBrushes.Black,
//    new XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.Center);

//// Save the document with new name...
//filename = PdfFileUtility.GetTempPdfFullFileName("samples-PDFsharp/AES128-unprotected");
//document.Save(filename);
//PdfFileUtility.ShowDocument(filename);