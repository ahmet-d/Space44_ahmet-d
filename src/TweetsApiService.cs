using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace CodeScreen.Assessments.TweetsApi
{
   
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
        public List<dynamic> GetTweets(string userName) {
            //TODO Implement
            //Note that the type of the returned list should be something that better represents tweet data.

            string apiurl = "https://app.codescreen.com/api/assessments/tweets?userName=" + userName;
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, apiurl);
                        
            using HttpResponseMessage response = client.Send(req);
            using HttpContent content = response.Content;
            string myContent = content.ReadAsStringAsync().Result;
            List<dynamic> result = JsonConvert.DeserializeObject<List<dynamic>>(myContent); 
            
            return result;
        }

    }
}
