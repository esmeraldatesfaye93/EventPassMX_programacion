using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using QRCoder;
using System.Drawing;
using System.IO;

namespace EventPassMX_programacion
{
    public static class GeneradorBoletos
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

            doc.Add(new Paragraph("EVENTPASS MX"));
            doc.Add(new Paragraph("----------------------"));

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
                doc.Add(new iText.Layout.Element.Image(imgData));
            }

            doc.Add(new Paragraph($"Folio: {t.QRCode}"));

            doc.Close();

            return path;
        }
    }
}