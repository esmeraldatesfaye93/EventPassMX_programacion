using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using QRCoder;
using System.Drawing;
using System.IO;

namespace EventPassMX_programacion
{
    public static class TicketGeneratorPro
    {
        public static string GeneratePDF(Ticket t)
        {
            string path = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
                $"Ticket_{t.Id}.pdf"
            );

            var writer = new PdfWriter(path);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            doc.Add(new Paragraph("EVENTPASS MX")
                .SetFontSize(22)
                .SetFontColor(ColorConstants.BLUE)
                .SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph(" "));

            doc.Add(new Paragraph($"Evento: {t.Evento.Nombre}"));
            doc.Add(new Paragraph($"Usuario: {t.Usuario}"));
            doc.Add(new Paragraph($"Tipo: {t.Access}"));
            doc.Add(new Paragraph($"Precio: ${t.Precio}"));

            // QR
            QRCodeGenerator qrGen = new QRCodeGenerator();
            var qrData = qrGen.CreateQrCode(t.QRCode, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrData);
            Bitmap qrImage = qrCode.GetGraphic(20);

            using (var ms = new MemoryStream())
            {
                qrImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var imgData = iText.IO.Image.ImageDataFactory.Create(ms.ToArray());

                doc.Add(new iText.Layout.Element.Image(imgData)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER));
            }

            doc.Add(new Paragraph($"Folio: {t.Id}")
                .SetFontSize(10)
                .SetFontColor(ColorConstants.GRAY)
                .SetTextAlignment(TextAlignment.CENTER));

            doc.Close();

            return path;
        }
    }
}