using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MTACodersLicence.Models;
using MTACodersLicence.Services;

namespace MTACodersLicence.Services
{ 
    public class CodeRunner2
    {
        public static CodeRunnerResult RunCode(string code, string input, ProgrammingLanguageModel programmingLanguage)
        {
            const string url = "https://api.judge0.com/submissions?base64_encoded=true&wait=${WAIT}";
            var data = "{\"source_code\": \"" + code + "\" , \"language_id\": 5, \"stdin\": \"" + input + "}";
            
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
