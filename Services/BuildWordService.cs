using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


namespace Tamphan_BBP.Services
{
    public class BuildWordService
    {
        public static void Build(
        string templatePath,
        string outputPath,
        Dictionary<string, string> data)
        {
            var destDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template not found: {templatePath}", templatePath);

            // đổi tên nếu file đã tồn tại (KHÔNG xóa nữa)++
            outputPath = GetUniqueFilePath(outputPath);

            File.Copy(templatePath, outputPath);

            // retry mở file để tránh bị lock (Downloads)
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    using (var doc = WordprocessingDocument.Open(outputPath, true))
                    {
                        if (doc.MainDocumentPart == null)
                            throw new Exception("Invalid Word template: missing MainDocumentPart");

                        ReplaceInPart(doc.MainDocumentPart, data);

                        foreach (var header in doc.MainDocumentPart.HeaderParts)
                            ReplaceInPart(header, data);

                        foreach (var footer in doc.MainDocumentPart.FooterParts)
                            ReplaceInPart(footer, data);
                    }
                    break;
                }
                catch (IOException)
                {
                    if (i == 4) throw;
                    Thread.Sleep(200);
                }
            }
        }

        // hàm tự tạo tên file không trùng
        private static string GetUniqueFilePath(string path)
        {
            if (!File.Exists(path))
                return path;

            string? dir = Path.GetDirectoryName(path);
            if (dir == null) throw new Exception("Invalid path");
            string name = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);

            int i = 1;
            string newPath;

            do
            {
                newPath = Path.Combine(dir, $"{name} ({i}){ext}");
                i++;
            }
            while (File.Exists(newPath));

            return newPath;
        }

        private static void ReplaceInPart(OpenXmlPart part, Dictionary<string, string> data)
        {
            if (part?.RootElement == null)
                return;

            // Ưu tiên xử lý Content Control trước
            ReplaceContentControls(part, data);

            // Sau đó xử lý placeholder dạng text
            ReplaceTextPlaceholders(part, data);
        }

        private static void ReplaceTextPlaceholders(OpenXmlPart part, Dictionary<string, string> data)
        {
            if (part?.RootElement == null) return;

            var textElements = part.RootElement.Descendants<Text>();

            foreach (var text in textElements)
            {
                foreach (var item in data)
                {
                    if (text.Text?.Contains(item.Key) == true)
                    {
                        text.Text = text.Text.Replace(item.Key, item.Value);
                    }
                }
            }
        }

        private static void ReplaceContentControls(OpenXmlPart part, Dictionary<string, string> data)
        {
            if (part?.RootElement == null) return;

            var contentControls = part.RootElement
                .Descendants<SdtElement>()
                .ToList();

            foreach (var sdt in contentControls)
            {
                var tag = sdt.SdtProperties?
                    .GetFirstChild<Tag>()?
                    .Val?.Value;

                if (string.IsNullOrEmpty(tag)) continue;
                if (!data.ContainsKey(tag)) continue;

                var parent = sdt.Parent;
                if (parent == null) continue;

                // lấy paragraph gốc để copy style (indent, spacing, justify...)
                var originalParagraph = sdt.Ancestors<Paragraph>().FirstOrDefault();
                ParagraphProperties? paragraphProps = null;

                if (originalParagraph?.ParagraphProperties != null)
                    paragraphProps = (ParagraphProperties)originalParagraph.ParagraphProperties.CloneNode(true);

                string value = data[tag] ?? "";
                string[] lines = value.Split('\n');

                foreach (var line in lines)
                {
                    // Paragraph
                    var newParagraph = new Paragraph();

                    if (paragraphProps != null)
                        newParagraph.ParagraphProperties =
                            (ParagraphProperties)paragraphProps.CloneNode(true);
                    else
                        newParagraph.ParagraphProperties = new ParagraphProperties();

                    var pPr = newParagraph.ParagraphProperties;

                    // --------------------
                    // Indent 1cm
                    // --------------------
                    var indent = pPr.GetFirstChild<Indentation>();
                    if (indent == null)
                    {
                        indent = new Indentation();
                        pPr.Append(indent);
                    }
                    indent.FirstLine = "567"; //FirstLineIndent thụt vào 1cm (567 twips)

                    // --------------------
                    // Spacing
                    // --------------------
                    var spacing = pPr.GetFirstChild<SpacingBetweenLines>();
                    if (spacing == null)
                    {
                        spacing = new SpacingBetweenLines();
                        pPr.Append(spacing);
                    }

                    spacing.Before = "60";
                    spacing.After = "60";
                    spacing.Line = "276"; //LineSpacing 1.15 
                    spacing.LineRule = LineSpacingRuleValues.Auto;

                    // --------------------
                    // Justify
                    // --------------------
                    var justification = pPr.GetFirstChild<Justification>();
                    if (justification == null)
                    {
                        justification = new Justification();
                        pPr.Append(justification);
                    }
                    justification.Val = JustificationValues.Both;

                    // --------------------
                    // Run
                    // --------------------
                    var run = new Run(
                        new RunProperties(
                            new RunFonts()
                            {
                                Ascii = "Times New Roman",
                                HighAnsi = "Times New Roman",
                                EastAsia = "Times New Roman",
                                ComplexScript = "Times New Roman"
                            },
                            new FontSize() { Val = "28" }, //FontSize14 
                            new FontSizeComplexScript() { Val = "28" }
                        ),
                        new Text(line)
                        {
                            Space = SpaceProcessingModeValues.Preserve
                        }
                    );

                    newParagraph.Append(run);
                    parent.InsertBefore(newParagraph, sdt);
                }

                // xóa content control sau khi replace
                sdt.Remove();
            }
        }
    }
}

