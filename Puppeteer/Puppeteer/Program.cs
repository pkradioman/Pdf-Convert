using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace Puppeteer
{
    class Program
    {
        /// <summary>
        /// https://kiltandcode.com/puppeteer-sharp-crawl-the-web-using-csharp-and-headless-chrome/
        /// </summary>


        private static readonly string[] urls = new[]
        {
            //"https://www.mea.or.th/profile/2989/2991",
            "http://siamchart.com/stock-chart/FSMART",
            "https://ese-customer-front-end.firebaseapp.com/today"
        };

        static async Task Main(string[] args)
        {
            // Download browser if not exist
            Console.Write("Download chrome browser if required...");
            var bsFetcher = new BrowserFetcher();
            await bsFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);
            Console.WriteLine("Done");

            // Launch
            Console.WriteLine("Launch browser");
            var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            try
            {
                Console.WriteLine("Create new page");
                var page = await browser.NewPageAsync();

                var url = urls.First();
                Console.WriteLine($"Go to {url}");
                await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);

                // Save html
                var htmlFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    $"htmlExport-{DateTime.Now.Minute}-{DateTime.Now.Second}.html");
                Console.Write($"Save to html at {htmlFile}...");
                var html = await page.GetContentAsync();
                File.WriteAllText(htmlFile, html);
                Console.WriteLine("Done");

                // Save as png
                var pngFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    $"pngExport-{DateTime.Now.Minute}-{DateTime.Now.Second}.png");
                Console.Write($"Save to png at {pngFile}...");
                await page.ScreenshotAsync(pngFile);
                Console.WriteLine("Done");

                // Save as Pdf
                var pdfFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    $"pdfExport-{DateTime.Now.Minute}-{DateTime.Now.Second}.pdf");
                Console.Write($"Save to Pdf at {pdfFile}...");
                await page.PdfAsync(pdfFile);
                Console.WriteLine("Done");


            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERR] {e.Message}");
            }
            finally
            {
                await browser?.CloseAsync();
            }

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
