using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;



namespace STTCRMWeb
{

    public partial class SttluisResponse
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("prediction")]
        public Prediction Prediction { get; set; }
    }

    public partial class Prediction
    {
        [JsonProperty("topIntent")]
        public string TopIntent { get; set; }

        [JsonProperty("intents")]
        public Intents Intents { get; set; }

        [JsonProperty("entities")]
        public Entities Entities { get; set; }
    }

    public partial class Entities
    {
        [JsonProperty("personName")]
        public string[] PersonName { get; set; }

        [JsonProperty("CompanyName")]
        public string[] CompanyName { get; set; }

        [JsonProperty("Solution")]
        public string[] Solution { get; set; }

        [JsonProperty("money")]
        public Money[] Money { get; set; }

        [JsonProperty("datetimeV2")]
        public DatetimeV2[] DatetimeV2 { get; set; }

        [JsonProperty("Next Step")]
        public string[] NextStep { get; set; }

        [JsonProperty("$instance")]
        public Instance Instance { get; set; }
    }

    public partial class DatetimeV2
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("values")]
        public Value[] Values { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("timex")]
        public string Timex { get; set; }

        [JsonProperty("resolution")]
        public Resolution[] Resolution { get; set; }
    }

    public partial class Resolution
    {
        [JsonProperty("start")]
        public DateTimeOffset Start { get; set; }

        [JsonProperty("end")]
        public DateTimeOffset End { get; set; }
    }

    public partial class Instance
    {
        [JsonProperty("personName")]
        public CompanyName[] PersonName { get; set; }

        [JsonProperty("CompanyName")]
        public CompanyName[] CompanyName { get; set; }

        [JsonProperty("Solution")]
        public CompanyName[] Solution { get; set; }

        [JsonProperty("money")]
        public CompanyName[] Money { get; set; }

        [JsonProperty("datetimeV2")]
        public CompanyName[] DatetimeV2 { get; set; }

        [JsonProperty("Next Step")]
        public CompanyName[] NextStep { get; set; }
    }

    public partial class CompanyName
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("startIndex")]
        public long StartIndex { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public double? Score { get; set; }

        [JsonProperty("modelTypeId")]
        public long ModelTypeId { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("recognitionSources")]
        public string[] RecognitionSources { get; set; }
    }

    public partial class Money
    {
        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("units")]
        public string Units { get; set; }
    }

    public partial class Intents
    {
        [JsonProperty("FullSentence")]
        public FullSentence FullSentence { get; set; }

        [JsonProperty("None")]
        public FullSentence None { get; set; }
    }

    public partial class FullSentence
    {
        [JsonProperty("score")]
        public double Score { get; set; }
    }


    public class CRMAuth
    {
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string expires_on { get; set; }
        public string not_before { get; set; }
        public string resource { get; set; }
        public string access_token { get; set; }
    }


    public partial class Speak : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSpeak_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(() => RecognizeSpeechAsync(txtSpeechText, txtResponse, txtCRMPost, txtJsonResponse)));
        }


        static async Task RecognizeSpeechAsync(TextBox txt, TextBox txtResponse, Label txtCRMPost, TextBox txtJsonResponse)
        {
            var config =
                SpeechConfig.FromSubscription(
                    "04be3fcaa4c444ca8cc1e64cb113b2f1",
                    "australiaeast");

            var recognizer = new SpeechRecognizer(config);

            var result = await recognizer.RecognizeOnceAsync();
            switch (result.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    Console.WriteLine($"We recognized: {result.Text}");
                    txt.Text = result.Text;
                    break;
                case ResultReason.NoMatch:
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                    break;
            }
            try
            {
                PostAPICalls(txt, txtResponse, txtCRMPost, txtJsonResponse);
            } catch (Exception e)
            {
                txtCRMPost.Text = e.Message;
            }
            

        }

        private static void PostAPICalls(TextBox txt, TextBox txtResponse, Label txtCRMPost, TextBox txtJsonResponse)
        {
            HttpClient auth_client = new HttpClient();
            auth_client.BaseAddress = new Uri("https://login.microsoftonline.com/76c75697-a17e-4fb7-884d-418d6781ba39/oauth2/token");
            var auth_url = "https://login.microsoftonline.com/76c75697-a17e-4fb7-884d-418d6781ba39/oauth2/token";
            auth_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("client_id", "a85ffc67-e1f4-4ef2-b85f-9cc8928fc7ca"));
            param.Add(new KeyValuePair<string, string>("client_secret", "4d~qyRjC_c9pC4A~DG4hg0SN3TS-k_mmM_"));
            param.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            param.Add(new KeyValuePair<string, string>("resource", "https://avaedb.crm5.dynamics.com"));

            HttpResponseMessage auth_response = auth_client.PostAsync(auth_url, new FormUrlEncodedContent(param)).Result;
            CRMAuth authObjects = null;
            if (auth_response.IsSuccessStatusCode)
            {
                string auth_result = auth_response.Content.ReadAsStringAsync().Result;
                authObjects = JsonConvert.DeserializeObject<CRMAuth>(auth_result);
            }
            else
            {
                txtCRMPost.Text = auth_response.ReasonPhrase;
            }
            auth_client.Dispose();


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://westus.api.cognitive.microsoft.com/luis/prediction/v3.0/apps/ebfae732-d138-4c74-8b8b-f8bec4b2801c/slots/staging/predict");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string urlParameters = "?subscription-key=a9a25ffa625648f59c881e7c5864c7c2&verbose=true&show-all-intents=true&log=true&query=" + txt.Text;
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            SttluisResponse dataObjects = null;
            if (response.IsSuccessStatusCode)
            {
                string sttresponse = response.Content.ReadAsStringAsync().Result;
                dataObjects = JsonConvert.DeserializeObject<SttluisResponse>(sttresponse);
                string json = JsonConvert.SerializeObject(dataObjects, Formatting.Indented);

                txtJsonResponse.Text = json;
                try
                {
                    txtResponse.Text = "";
                    txtResponse.Text += "FirstName: " + dataObjects.Prediction.Entities.PersonName[0].ToString().Split(char.Parse(" "))[0] + Environment.NewLine;
                    txtResponse.Text += "LastName: " + dataObjects.Prediction.Entities.PersonName[0].ToString().Split(char.Parse(" "))[1] + Environment.NewLine;
                    txtResponse.Text += "Companyname: " + dataObjects.Prediction.Entities.CompanyName[0].ToString() + Environment.NewLine;
                    txtResponse.Text += "Solution/Subject: " + dataObjects.Prediction.Entities.Solution[0].ToString() + Environment.NewLine;
                    txtResponse.Text += "Dealsize: " + dataObjects.Prediction.Entities.Money[0].Number.ToString() + Environment.NewLine;
                    txtResponse.Text += "RFP Issuance: " + dataObjects.Prediction.Entities.DatetimeV2[0].Values[0].Timex + "-01" + Environment.NewLine;
                    txtResponse.Text += "Expected Contract Start: " + dataObjects.Prediction.Entities.DatetimeV2[1].Values[0].Timex + "-01" + Environment.NewLine;
                    txtResponse.Text += "Next Step: " + dataObjects.Prediction.Entities.NextStep[0] + Environment.NewLine;
                } catch (Exception exp) { }                
            }
            else
            {
                txtResponse.Text = response.ReasonPhrase;
            }
            client.Dispose();

            try
            {
                var crm_url = "https://avaedb.crm5.dynamics.com/api/data/v9.1/leads";
                var accessToken = authObjects.access_token;
                JObject lead = new JObject{
                { "firstname" , dataObjects.Prediction.Entities.PersonName[0].ToString().Split(char.Parse(" "))[0]},
                { "lastname" , dataObjects.Prediction.Entities.PersonName[0].ToString().Split(char.Parse(" "))[1]},
                { "companyname" , dataObjects.Prediction.Entities.CompanyName[0].ToString()},
                { "subject" ,  dataObjects.Prediction.Entities.Solution[0].ToString()},
                { "ava_dealsize" , dataObjects.Prediction.Entities.Money[0].Number.ToString()},
                { "ava_rfpissuance" , dataObjects.Prediction.Entities.DatetimeV2[0].Values[0].Timex + "-01"},
                { "ava_contractstart" , dataObjects.Prediction.Entities.DatetimeV2[1].Values[0].Timex + "-01"}
                };


                


                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(crm_url))
                {
                    Content = new StringContent(lead.ToString(), Encoding.UTF8, "application/json")
                };
                HttpResponseMessage crm_response_2 = httpClient.SendAsync(request).Result;
                if (crm_response_2.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    txtCRMPost.Text = "<div class=\"alert alert-success\"><strong>Success!</strong> The request has successfully posted to CRM.</div>";
                }
                else
                {
                    txtCRMPost.Text ="Error: " + auth_response.ReasonPhrase;
                 
                }
               
            }
            
            catch (Exception e) {
                txtCRMPost.Text = e.Message;
            }
        }

        protected void btnFix_Click(object sender, EventArgs e)
        {
            PostAPICalls(txtSpeechText, txtResponse, txtCRMPost, txtJsonResponse);
        }
    }
}