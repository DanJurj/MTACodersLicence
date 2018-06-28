using System;
using System.IO;
using System.Net;
using System.Text;
using MTACodersLicence.Models;
using MTACodersLicence.Models.ResultModels;
using MTACodersLicence.Models.TestModels;
using Newtonsoft.Json.Linq;

namespace MTACodersLicence.Services
{
    public class CodeRunner
    {
        public static string ConvertToBase64(string text)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes);
        }

        public static string ConvertFromBase64(string base64Text)
        {
            var textBytes = Convert.FromBase64String(base64Text);
            return Encoding.UTF8.GetString(textBytes);
        }

        public static TestResultModel RunCode(string code, TestModel test, int languageCode)
        {

            var codeBase64 = ConvertToBase64(code);
            var inpuBase64 = ConvertToBase64(test.Input);
            var expectedOutputBase64 = ConvertToBase64(test.ExpectedOutput);
            const string url = "https://api.judge0.com/submissions?base64_encoded=true&wait=true";
            var data =
                "{\r\n  \"source_code\": \"" + codeBase64 + "\"," +
                "\r\n  \"language_id\": \"" + languageCode + "\"," +
                "\r\n  \"stdin\": \"" + inpuBase64 + "\"," +
                "\r\n  \"expected_output\": \"" + expectedOutputBase64 + "\"\r\n}";

            WebRequest myReq = WebRequest.Create(url);
            myReq.Method = "POST";
            myReq.ContentLength = data.Length;
            myReq.ContentType = "application/json; charset=UTF-8";
            UTF8Encoding enc = new UTF8Encoding();
            using (Stream ds = myReq.GetRequestStream())
            {
                ds.Write(enc.GetBytes(data), 0, data.Length);
            }

            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            var content = reader.ReadToEnd();

            var result = JObject.Parse(content);
            var memoryUsed = result.Value<float>("memory");
            var executionTime = result.Value<decimal>("time");
            var stdoutBase64 = result.Value<string>("stdout");
            string stdout = "";
            if (stdoutBase64!=null)
            {
                stdout = ConvertFromBase64(stdoutBase64);
            }
            return new TestResultModel()
            {
                ResultedOutput = stdout,
                ExecutionTime = executionTime,
                Memory = memoryUsed
            };
        }
    }
}
