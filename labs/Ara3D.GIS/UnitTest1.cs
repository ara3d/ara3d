using System.Net;
using System;
using System.Net;
using System.IO;

namespace Ara3D.GIS
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDownloadGisData()
        {
            DownloadData();
            Assert.Pass();
        }

        public static CredentialCache GetCredentials()
        {
            var urs = "https://urs.earthdata.nasa.gov";
            var apiKeyFile = @"C:\Users\cdigg\api-keys\earthdata.txt";
            var lines = File.ReadAllLines(apiKeyFile);
            var username = lines[0];
            var password = lines[1];
            // Create a credential cache for authenticating when redirected to Earthdata Login
            var cache = new CredentialCache();
            cache.Add(new Uri(urs), "Basic", new NetworkCredential(username, password));
            return cache;
        }

        [Test]
        public static void DownloadOneFile()
        {
            var resource = "https://e4ftl01.cr.usgs.gov/MEASURES/SRTMGL1.003/2000.02.11/N03E027.SRTMGL1.hgt.zip";
            DownloadFile(resource, GetCredentials(), new CookieContainer());
        }

        public static void WriteStreamToFile(Stream inputStream, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                inputStream.CopyTo(fileStream);
            }
        }

        public static void DownloadFile(string resource, CredentialCache cache, CookieContainer cookies)
        {
            var baseName = Path.GetFileName(resource);
            var filePath = $@"c:\tmp\{baseName}";
            if (File.Exists(filePath))
                return;
            try
            {
                Console.WriteLine($"Downloading {resource}");
                var request = (HttpWebRequest)WebRequest.Create(resource);
                request.Method = "GET";
                request.Credentials = cache;
                request.CookieContainer = cookies;
                request.PreAuthenticate = false;
                request.AllowAutoRedirect = true;
                var response = (HttpWebResponse)request.GetResponse();

                // Now access the data
                var length = response.ContentLength;
                var type = response.ContentType;
                using (var stream = response.GetResponseStream())
                {
                    WriteStreamToFile(stream, filePath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        // https://www.opentopodata.org/datasets/srtm/#public-api
        // https://urs.earthdata.nasa.gov/documentation/for_users/data_access/c_sharp
        public static void DownloadData()
        {
            //var resource = "https://e4ftl01.cr.usgs.gov/MEASURES/SRTMGL1.003/2000.02.11/N03E027.SRTMGL1.hgt.zip";
            var urls = File.ReadAllLines(@"C:\Users\cdigg\Downloads\srtm30m_urls.txt"); 

            try
            {
                // Ideally the cookie container will be persisted to/from file
                var myContainer = new CookieContainer();

                // Create a credential cache for authenticating when redirected to Earthdata Login
                var cache = GetCredentials();
                Parallel.For(0, urls.Length, i =>
                {
                    DownloadFile(urls[i], cache, myContainer);
                });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}