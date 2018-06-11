using System;
using System.IO;
using System.Net;
using System.Text;
using MTACodersLicence.Models;

namespace MTACodersLicence.Services
{
    public class CodeRunner2
    {
        public static CodeRunnerResult RunCode(string code, string input, ProgrammingLanguageModel programmingLanguage)
        {
            var plainText =
                "#include<stdio.h>\n void main()\n{\n    int x;\n    scanf(\"%d\",&x);\n    printf(\"%d\",x);\n    return 0;\n}";
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var plain2 =  Convert.ToBase64String(plainTextBytes);
            const string url = "https://api.judge0.com/submissions?base64_encoded=true&wait=true";
            var data =
                "{\r\n  \"source_code\": \"" + plain2 + "\"," +
                "\r\n  \"language_id\": \"4\"," +
                "\r\n  \"stdin\": \"" + input + "\"," +
                "\r\n  \"expected_output\": \"hello, Judge0\"\r\n}";
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

            var base64EncodedBytes = Convert.FromBase64String(content);
            var decRes = Encoding.UTF8.GetString(base64EncodedBytes);

            var codeRunnerResult = new CodeRunnerResult();

            return codeRunnerResult;
        }
    }
}
