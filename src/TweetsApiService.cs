using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace CodeScreen.Assessments.TweetsApi
{
    public class TweetData
    {
        public string id { get; set; }
        public DateTime createdAt { get; set; }
        public string text { get; set; }
        public TweetUser user { get; set; }
    }
    public class TweetUser
    {
        public string id { get; set; }
        public string userName { get; set; }
    }
    /**
    * Service that retrieves data from the CodeScreen Tweets API.
    */
    class TweetsApiService
    {
        private static readonly string TweetsEndpointURL = "https://app.codescreen.com/api/assessments/tweets";

        //Your API token. Needed to successfully authenticate when calling the tweets endpoint.
        //This needs to be included in the Authorization header (using the Bearer authentication scheme) in the request you send to the tweets endpoint.
        private static readonly string ApiToken = "8c5996d5-fb89-46c9-8821-7063cfbc18b1";
        
        /**
         * Retrieves the data for all tweets, for the given user,
         * by calling the https://app.codescreen.com/api/assessments/tweets endpoint.
         *
         * The userName should be passed in the request as a query parameter called userName.
         *
         * @param userName the name of the user
         * @return a list containing the data for all tweets for the given user
        */
        public List<TweetData> GetTweets(string userName) {
           
            string apiurl = TweetsEndpointURL + "?userName=" + userName;
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, apiurl);
                        
            using HttpResponseMessage response = client.Send(req);
            using HttpContent content = response.Content;
            string myContent = content.ReadAsStringAsync().Result;
            List<TweetData> result = JsonConvert.DeserializeObject<List<TweetData>>(myContent); 
            
            return result;
        }

    }
}
