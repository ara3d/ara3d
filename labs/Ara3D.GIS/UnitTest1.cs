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

        public static void WriteStreamToFile(Stream inputStream, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                inputStream.CopyTo(fileStream);
            }
        }

        // https://www.opentopodata.org/datasets/srtm/#public-api
        // https://urs.earthdata.nasa.gov/documentation/for_users/data_access/c_sharp
        public static void DownloadData()
        {
            //var resource = "https://e4ftl01.cr.usgs.gov/MEASURES/SRTMGL1.003/2000.02.11/N03E027.SRTMGL1.hgt.zip";
            var urs = "https://urs.earthdata.nasa.gov";
            var username = "cdiggins";
            var password = "ydXtRa-c_aA5he5";
            var urls = File.ReadAllLines(@"C:\Users\cdigg\Downloads\srtm30m_urls.txt"); 

            try
            {
                // Ideally the cookie container will be persisted to/from file

                var myContainer = new CookieContainer();


                // Create a credential cache for authenticating when redirected to Earthdata Login

                var cache = new CredentialCache();
                cache.Add(new Uri(urs), "Basic", new NetworkCredential(username, password));


                foreach (var resource in urls)
                {
                    try
                    {
                        Console.WriteLine($"Downaloading {resource}");
                        // Execute the request

                        var request = (HttpWebRequest)WebRequest.Create(resource);
                        request.Method = "GET";
                        request.Credentials = cache;
                        request.CookieContainer = myContainer;
                        request.PreAuthenticate = false;
                        request.AllowAutoRedirect = true;
                        var response = (HttpWebResponse)request.GetResponse();


                        // Now access the data

                        var length = response.ContentLength;
                        var type = response.ContentType;
                        using (var stream = response.GetResponseStream())
                        {
                            var baseName = Path.GetFileName(resource);
                            WriteStreamToFile(stream, $@"c:\tmp\{baseName}");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}