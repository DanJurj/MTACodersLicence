using System.IO;
using System.Net;
using System.Text;
using MTACodersLicence.Models;

namespace MTACodersLicence.Services
{
    public class CodeRunnerResult
    {
        public string Stdout { get; set; }
        public string Stderr { get; set; }
        public string Error { get; set; }
        public bool HasError { get; set; }
    }

    public class CodeRunner
    {
        public static CodeRunnerResult RunCode(string code, string input, ProgrammingLanguageModel programmingLanguage)
        {
            string url = "https://run.glot.io/languages/" + programmingLanguage.Type + "/latest";
            string data;
            if (input != null)
            {
                data = "{\"stdin\": \"" + input + "\" , \"files\": [{\"name\": \"" + programmingLanguage.Filename + "\", \"content\": \"" + code + "\"}]}";
            }
            else
            {
                data = "{\"files\": [{\"name\": \"" + programmingLanguage.Filename + "\", \"content\": \"" + code + "\"}]}";
            }
            WebRequest myReq = WebRequest.Create(url);
            myReq.Method = "POST";
            myReq.ContentLength = data.Length;
            myReq.ContentType = "application/json; charset=UTF-8";

            UTF8Encoding enc = new UTF8Encoding();

            myReq.Headers.Add("Authorization: Token 17a385d3-6c5e-4b76-8821-8e83b83352ff");


            using (Stream ds = myReq.GetRequestStream())
            {
                ds.Write(enc.GetBytes(data), 0, data.Length);
            }

            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            var content = reader.ReadToEnd();

            var stdout = content.Split("stdout\":\"")[1].Split("\"")[0];
            var stderr = content.Split("stderr\":\"")[1].Split(",\"error")[0];
            var error = content.Split("error\":\"")[1].Split("\"}")[0];

            var codeRunnerResult = new CodeRunnerResult
            {
                Stdout = stdout,
                Stderr = stderr,
                Error = error,
                HasError = error.Length > 3
            };

            return codeRunnerResult;
        }
    }
}
