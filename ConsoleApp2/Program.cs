using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

static byte[] FlateDecode(byte[] inp, bool strict)
{
    MemoryStream stream = new MemoryStream(inp);
    InflaterInputStream zip = new InflaterInputStream(stream);
    MemoryStream outp = new MemoryStream();
    byte[] b = new byte[strict ? 4092 : 1];
    try
    {
        int n;
        while ((n = zip.Read(b, 0, b.Length)) > 0)
        {
            outp.Write(b, 0, n);
        }
        zip.Close();
        outp.Close();
        return outp.ToArray();
    }
    catch (Exception)
    {
        if (strict)
            return null;
        return outp.ToArray();
    }
}

//string filePath = System.IO.Directory.GetCurrentDirectory();
//Console.WriteLine($"Program is running from: {filePath}");

//string filePath1 = AppContext.BaseDirectory;
//Console.WriteLine($"Program is running from: {filePath1}");

//var testFile = IOUtility.GetAssetsPath("1.BestFileToTest.pdf");
//Console.WriteLine($"Program is running from: {testFile}");

//var filename = PdfFileUtility.GetTempPdfFullFileName("11");

// Load the input PDF
string inputPdfPath = "1.BestFileToTest.pdf";
PdfDocument inputDocument = PdfReader.Open(inputPdfPath, PdfDocumentOpenMode.Modify);

// Create a new PDF document for output
PdfDocument outputDocument = new PdfDocument();

// Ensure the page is A4 size
XSize a4Size = PageSizeConverter.ToSize(PageSize.A4);

for (int i = 0; i < inputDocument.Pages.Count; i++)
{
    PdfPage page = inputDocument.Pages[i];
    Console.WriteLine($"Program is running from: {page.Width}-{page.Height}");

    // Get the original dimensions
    var mediaBox = page.MediaBox;
    double originalWidth = mediaBox.Width;
    double originalHeight = mediaBox.Height;

    // Get the rotation value
    int rotation = page.Rotate;

    // Calculate the actual width and height after rotation
    double actualWidth = originalWidth;
    double actualHeight = originalHeight;

    if (rotation == 90 || rotation == 270)
    {
        // Swap width and height for 90 or 270 degrees rotation
        actualWidth = originalHeight;
        actualHeight = originalWidth;
        XSize size = PageSizeConverter.ToSize(PdfSharp.PageSize.A4);
        page.MediaBox = new PdfRectangle(new XPoint(0, 0), new XPoint(size.Height, size.Width)); // Magic: swap width and height
        //page.TrimMargins.Top = XUnit.FromMillimeter(5);
        //page.TrimMargins.Right = XUnit.FromMillimeter(1);
        //page.TrimMargins.Bottom = XUnit.FromMillimeter(5);
        //page.TrimMargins.Left = XUnit.FromMillimeter(1);

        //page.Rotate = (page.Rotate + 90) % 360;

        //page.Orientation = PageOrientation.Landscape;
    }

    Console.WriteLine($"Page {i + 1}: Rotation = {rotation} degrees, Actual Size = {actualWidth} x {actualHeight}");


    //// Check for rotation
    //int rotation = page1.Rotate;
    //// Check for crop box if applicable
    //var cropBox = page1.Elements.GetRectangle("/CropBox");

    //Console.WriteLine($"Page {page1}: Width = {page1.Width}, Height = {page1.Height}, Rotation = {rotation}");
    //if (cropBox != null)
    //{
    //    Console.WriteLine($"Crop Box: {cropBox.Width} x {cropBox.Height}");
    //}

}
string newfilename = @"corrected.pdf";

inputDocument.Save(newfilename);

outputDocument.PageLayout = PdfPageLayout.SinglePage;

XGraphics gfx;
XRect box;
// Open the file to resize
XPdfForm form = XPdfForm.FromFile(newfilename);

XFont font = new XFont("Verdana", 8, XFontStyleEx.Bold);
XStringFormat format = new XStringFormat();
format.Alignment = XStringAlignment.Center;
format.LineAlignment = XLineAlignment.Far;

for (int idx = 0; idx < form.PageCount; idx++)
{
    form.PageNumber = idx + 1;
    // Add a new page to the output document
    PdfPage page = outputDocument.AddPage();

    Console.WriteLine($"Program is running from: {form.Page.Width}-{form.Page.Height}");

    //int rotation = form.Page.Rotate;

    //Console.WriteLine($"Page {form.Page.Contents}: Rotation = {rotation} degrees");


    if (form.PageNumber == 8)
    {

        //double ppi = page.Width.Point / page.Width.Inch;
        //double coeff = XGraphics.FromHwnd(IntPtr.Zero).DpiX / ppi;
        //Rectangle rectangle = new Rectangle(0, 0, (int)(page.Width.Point * coeff), (int)(page.Height.Point * coeff));

        //page.TrimMargins.Top = 5;
        //page.TrimMargins.Right = 120;
        //page.TrimMargins.Bottom = 5;
        // page.TrimMargins.Left = 0;

        // Check for rotation
        int rotation = form.Page.Rotate;
        // Check for crop box if applicable
        //var cropBox = form.Page.Elements.GetRectangle("/CropBox");

        var mediaBox = form.Page.Elements.GetRectangle("/MediaBox");
        var cropBox = form.Page.Elements.GetRectangle("/CropBox");

        Console.WriteLine($"Page {form.PageNumber}: Width = {page.Width}, Height = {page.Height}, Rotation = {rotation}");
        if (cropBox != null)
        {
            Console.WriteLine($"Crop Box: {cropBox.Width} x {cropBox.Height}");
        }
    }
    //if (form.PixelWidth > form.PixelHeight)
    //page.Orientation = PageOrientation.Landscape;
    //else
    page.Orientation = PageOrientation.Portrait;


    double width = page.Width;
    double height = page.Height;

    gfx = XGraphics.FromPdfPage(page);

    //double Scale = form.Page.Height / 842.0;
    Console.WriteLine($"Program is running from: {form?.Page?.Width}-{form?.Page?.Height}");


    box = new XRect(0, 0, (int)page.Width.Point, (int)page.Height.Point);
    //box = new XRect(0, 0, (int)(form.Page.Width / Scale), (int)(form.Page.Height / Scale));
    gfx.DrawImage(form, box);

    // Write document file name and page number on each page

    box.Inflate(0, -10);

    gfx.DrawString(String.Format("-{1} -", string.Empty, idx + 1), font, XBrushes.Red, box, format);

}

outputDocument.Save(newfilename);
PdfDocument newfileDoc = PdfReader.Open(newfilename, PdfDocumentOpenMode.Import);
foreach (PdfPage page1 in newfileDoc.Pages)
{
    Console.WriteLine($"Program is running from: {page1.Width}-{page1.Height}");

}







//foreach (PdfPage page in inputDocument.Pages)
//{
//    // Get the dimensions of the current page
//    double width = page.Width;
//    double height = page.Height;

//    // Create a new A4 page in the output document
//    PdfPage newPage = outputDocument.AddPage();
//    newPage.Width = a4Size.Width;
//    newPage.Height = a4Size.Height;

//    // Create graphics object to draw on the new A4 page
//    XGraphics gfx = XGraphics.FromPdfPage(newPage);

//    // Calculate the scaling factor to fit the content within the A4 page
//    double scaleX = a4Size.Width / width;
//    double scaleY = a4Size.Height / height;
//    double scale = Math.Min(scaleX, scaleY);

//    // Transform the page and apply scaling
//    gfx.Save();
//    gfx.ScaleTransform(scale);


//    // Get resources dictionary
//    PdfSharp.Pdf.PdfDictionary resources = page.Elements.GetDictionary("/Resources");

//    if (resources != null)
//    {
//        // Get external objects dictionary
//        PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
//        if (xObjects != null)
//        {
//            var items = xObjects.Elements.Values;
//            foreach (PdfItem item in items)
//            {
//                PdfReference reference = item as PdfReference;
//                if (reference != null)
//                {
//                    PdfDictionary xObject = reference.Value as PdfDictionary;
//                    // Is external object an image?
//                    if (xObject != null)//&& xObject.Elements.GetString("/Subtype") == "/Image")
//                    {
//                        string filter = xObject.Elements.GetName("/Filter");
//                        byte[] stream = FlateDecode(xObject.Stream.Value, false);


//                        PdfDictionary.PdfStream stream1 = page.Contents.Elements.GetDictionary(0).Stream;

//                        XImage form = XImage.FromStream(new MemoryStream(stream1.Value, 0, 0, true, true));
//                        gfx.DrawImage(form, 0, 0);
//                        gfx.Restore();

//                    }
//                }
//            }
//        }
//    }





//var xGraphics = XGraphics.FromPdfPage(page);
// Draw the content of the old page onto the new A4 page
//XImage form = XImage.FromStream(new MemoryStream(page.Resources.Stream.Value));

//gfx.DrawImage(form, 0, 0);
//gfx.Restore();
//}

// Save the output document
//string outputPdfPath = "output.pdf";
//outputDocument.Save(outputPdfPath);
