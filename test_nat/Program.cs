using System.Diagnostics;
using System.Text.Json;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

class Program
{

    static void Main(string[] args)
    {
        string env;

        #if DEBUG
                env = "debug";
        #else
                env = "release";
        #endif

        string jsonFilePath = GetJsonFilePath(env);

        if (!File.Exists(jsonFilePath))
        {
            Console.WriteLine("File tidak ditemukan.");
            close();
        }

        string jsonData = File.ReadAllText(jsonFilePath);
        var employeeReview = JsonSerializer.Deserialize<EmployeeReview>(jsonData);

        if (employeeReview == null)
        {
            Console.WriteLine("Data tidak valid.");
            close();
        }

        CalculateTotalScore(employeeReview);
        GeneratePDFReport(employeeReview, env);
    }

    static void CalculateTotalScore(EmployeeReview review)
    {
        review.TotalScore = 0;
        foreach (var criterion in review.Criteria)
        {
            review.TotalScore += criterion.Score;
        }
        review.TotalScore /= review.Criteria.Count;
    }

    static void GeneratePDFReport(EmployeeReview review, string env)
    {
        string pdfFilePath = GetPdfFilePath(env);
        using (PdfWriter writer = new PdfWriter(pdfFilePath))
        {
            using (PdfDocument pdf = new PdfDocument(writer))
            {
                Document document = new Document(pdf);

                document.Add(new Paragraph("Performance Review")
                    .SetFontSize(20)
                    .SimulateBold());

                document.Add(new Paragraph($"Nama : {review.EmployeeName}"));
                document.Add(new Paragraph($"Jabatan : {review.Position}"));
                document.Add(new Paragraph(""));

                document.Add(new Paragraph("Detail Kriteria :")
                    .SimulateBold());

                foreach (var criterion in review.Criteria)
                {
                    document.Add(new Paragraph($"- {criterion.Name}: {criterion.Score}/5"));
                    document.Add(new Paragraph($"  Umpan Balik : {criterion.Feedback}"));
                }

                document.Add(new Paragraph(""));
                document.Add(new Paragraph($"Total Skor: {review.TotalScore:F2}"));
                document.Add(new Paragraph($"Umpan Balik Keseluruhan: {review.GeneralFeedback}"));

                document.Close();
            }
        }

        Console.WriteLine($"Laporan berhasil dibuat : {pdfFilePath}");
        close();

    }

    static void close()
    {
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        Environment.Exit(0);
    }

    static string GetJsonFilePath(string env)
    {
        string filePath;

        if(env == "debug")
        {
            filePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "data.json");
        } else
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "data.json");
        }

        return filePath;
    }

    static string GetPdfFilePath(string env)
    {
        string filePath;

        if(env == "debug")
        {
            filePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "PerformanceReview.pdf");
        } else
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "PerformanceReview.pdf");
        }

        return filePath;
    }
}
