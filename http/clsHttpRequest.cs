using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcore2;
using tcore2.global;


namespace TJ
{
    public class clsHttpRequestModel
    {
        public string Url { get; set; }
        public string Body { get; set; } = "";
        public string Method { get; set; } = "POST";
        public string ContentType { get; set; } = "application/json";
        public clsCommand cmdForm { get; set; } = new();
        public clsCommand cmdHeader { get; set; } = new();
    }

    public static class clsHttpRequest
    {

        public static string getFormDataInString(List<clsParam> cmd)
        {
            List<string> lst = new List<string>();

            foreach (clsParam f in cmd)
            {
                string sValue = string.Format("{0}={1}", f.name, f.value);
                lst.Add(sValue);
            }
            return string.Join("&", lst.ToArray());
        }

        public static void applyHeader(HttpClient client, List<clsParam> cmdHeader)
        {
            foreach (var p in cmdHeader)
            {
                client.DefaultRequestHeaders.Add(p.name, (string)p.value);
            }
        }

        public static async Task<T> sendRequest<T>(clsHttpRequestModel requestModel)
        {
            object obj1 = null;
            return (T)obj1;
        }

        public static async Task<string> sendRequest(clsHttpRequestModel requestModel)
        {

            using (var client = new HttpClient())
            {
                applyHeader(client, requestModel.cmdHeader.prm);
                HttpResponseMessage response = null;

                if (requestModel.Method.ToLower() == "post")
                {
                    if (requestModel.cmdForm.prm.Count > 0)
                    {
                        requestModel.Body = getFormDataInString(requestModel.cmdForm.prm);
                    }

                    HttpContent _content = new StringContent(requestModel.Body, Encoding.UTF8, requestModel.ContentType);
                    response = await client.PostAsync(requestModel.Url, _content);
                }
                else if (requestModel.Method.ToLower() == "get")
                {
                    response = await client.GetAsync(requestModel.Url);
                }

                string sResponse = await response.Content.ReadAsStringAsync();

                return sResponse;

            }
        }

    }

}
