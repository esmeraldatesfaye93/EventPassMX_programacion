using iText.Kernel.Pdf;
using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace EventPassMX_programacion
{
    public static class GeneradorBoletos
    {
        public static string GeneratePDF(Ticket t, int cantidad = 1, string asientos = "N/A")
        {
            string fileName = $"Boleto_{t.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string path = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
                fileName
            );

            try
            {
                
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfWriter writer = new PdfWriter(fs);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document doc = new Document(pdf);

                    doc.Add(new Paragraph("EVENTPASS MX")
                        .SetFontSize(24)
                        .SetFontColor(ColorConstants.BLUE)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(10));

                    doc.Add(new Paragraph("BOLETO DE ENTRADA")
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(20));

                    
                    doc.Add(CrearEncabezadoSeccion("INFORMACIÓN DEL EVENTO"));
                    doc.Add(CrearLinea("Evento:", t.Evento.Nombre));
                    doc.Add(CrearLinea("Artista:", t.Evento.Artista?.Nombre ?? "N/A"));
                    doc.Add(CrearLinea("Fecha:", t.Evento.Fecha.ToString("dddd, dd/MM/yyyy HH:mm")));
                    doc.Add(CrearLinea("Lugar:", t.Evento.Ciudad));
                    doc.Add(CrearLinea("Categoría:", t.Evento.Categoria));
                    doc.Add(new Paragraph(" "));

                    
                    doc.Add(CrearEncabezadoSeccion("DETALLES DE COMPRA"));
                    doc.Add(CrearLinea("Comprador:", t.Usuario));
                    doc.Add(CrearLinea("Tipo de Acceso:", t.Access.ToString()));
                    doc.Add(CrearLinea("Cantidad de Boletos:", cantidad.ToString()));
                    doc.Add(CrearLinea("Asientos:", asientos));
                    doc.Add(CrearLinea("Fecha de Compra:", t.FechaCompra.ToString("dd/MM/yyyy")));
                    doc.Add(new Paragraph(" "));

                    
                    doc.Add(CrearEncabezadoSeccion("INFORMACIÓN DE PAGO"));
                    doc.Add(CrearLinea("Precio Unitario:", $"${t.Precio}"));

                    doc.Add(new Paragraph($"TOTAL PAGADO: ${t.Precio * cantidad}")
                        .SetFontSize(16)
                        .SetFontColor(ColorConstants.GREEN)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(20));

                    
                    doc.Add(new Paragraph("_________________________________________________________________")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(10));

                    doc.Add(new Paragraph("FOLIO DE VALIDACIÓN")
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.DARK_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER));

                    doc.Add(new Paragraph(t.QRCode)
                        .SetFontSize(14)
                        .SetFontColor(ColorConstants.BLUE)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(20));

                    
                    try
                    {
                        QRCodeGenerator qrGen = new QRCodeGenerator();
                        QRCodeData qrData = qrGen.CreateQrCode(t.QRCode, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrData);
                        Bitmap qrImage = qrCode.GetGraphic(20);

                        
                        string tempImagePath = Path.Combine(Path.GetTempPath(), $"qr_{t.Id}.png");
                        qrImage.Save(tempImagePath);

                        try
                        {
                            var imgData = iText.IO.Image.ImageDataFactory.Create(tempImagePath);
                            doc.Add(new iText.Layout.Element.Image(imgData)
                                .SetWidth(100)
                                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                                .SetMarginBottom(15));
                        }
                        finally
                        {
                            
                            if (File.Exists(tempImagePath))
                            {
                                try { File.Delete(tempImagePath); } catch { }
                            }
                        }
                    }
                    catch (Exception qrEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Advertencia: Error generando QR: {qrEx.Message}");
                    }

                    
                    doc.Add(new Paragraph("_________________________________________________________________")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(10));

                    doc.Add(new Paragraph("Gracias por tu compra.")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(10)
                        .SetFontColor(ColorConstants.GRAY));

                    doc.Add(new Paragraph("Presenta este boleto en la entrada del evento.")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(10)
                        .SetFontColor(ColorConstants.GRAY));

                    doc.Add(new Paragraph("Conserva el folio para validación.")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(9)
                        .SetFontColor(ColorConstants.GRAY));

                    doc.Add(new Paragraph($"Generado: {DateTime.Now:dd/MM/yyyy}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(8)
                        .SetFontColor(ColorConstants.LIGHT_GRAY));

                    doc.Close();
                }

                return path;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generando PDF: {ex.Message}", ex);
            }
        }

        private static Paragraph CrearEncabezadoSeccion(string titulo)
        {
            return new Paragraph(titulo)
                .SetFontSize(11)
                .SetFontColor(ColorConstants.BLUE)
                .SetMarginTop(5)
                .SetMarginBottom(5);
        }

        private static Paragraph CrearLinea(string etiqueta, string valor)
        {
            var p = new Paragraph();
            p.Add(new Text(etiqueta + " ").SetFontColor(ColorConstants.DARK_GRAY));
            p.Add(new Text(valor).SetFontColor(ColorConstants.BLACK));
            p.SetMarginBottom(3);
            return p;
        }
    }
}
