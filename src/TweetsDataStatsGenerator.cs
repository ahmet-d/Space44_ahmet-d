using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/**
 * Generates various statistics about the tweets data set returned by the given TweetsApiService instance.
 */
namespace CodeScreen.Assessments.TweetsApi
{
    class TweetDataStatsGenerator
    {
        private readonly TweetsApiService TweetsApiService;
        private List<TweetData> _tweetResults;

        public TweetDataStatsGenerator(TweetsApiService tweetsApiService) {
            TweetsApiService = tweetsApiService;
        }

        /**
         * Retrieves the highest number of tweets that were created on any given day by the given user.
         *
         * A day's time period here is defined from 00:00:00 to 23:59:59
         * If there are no tweets for the given user, this method should return 0.
         *
         * @param userName the name of the user
         * @return the highest number of tweets that were created on a any given day by the given user
        */
        public int GetMostTweetsForAnyDay(string userName) {
            if (_tweetResults == null)
            {
                _tweetResults = TweetsApiService.GetTweets("joe_smith");
            }
            if (_tweetResults.Count == 0) return 0;
            int maxnumber = _tweetResults.GroupBy(x => x.createdAt.DayOfYear)
                    .Select(x => new { Key = x.Key, count = x.Count() })
                    .Max(x => x.count);
            return maxnumber;
        }

        /**
         * Finds the ID of longest tweet for the given user.
         *
         * You can assume there will only be one tweet that is the longest.
         * If there are no tweets for the given user, this method should return null.
         *
         * @param userName the name of the user
         * @return the ID of longest tweet for the given user
        */
        public string GetLongestTweet(string userName) {           
            if (_tweetResults == null)
            {
                _tweetResults = TweetsApiService.GetTweets("joe_smith");
            }
            if (_tweetResults.Count == 0) return null;

            var maxlength = _tweetResults.Max(x => x.text.Length);
            var longest = _tweetResults.Where(x => x.text.Length == maxlength).First();
            return longest.id;
        }

        /**
         * Retrieves the most number of days between tweets by the given user, wrapped as an OptionalInt.
         *
         * This should always be rounded down to the complete number of days, i.e. if the time is 12 days & 3 hours, this
         * method should return 12.
         * If there are no tweets for the given user, this method should return 0.
         *
         * @param userName the name of the user
         * @return the most number of days between tweets by the given user
        */
        public int FindMostDaysBetweenTweets(string userName) {
            if (_tweetResults == null)
            {
                _tweetResults = TweetsApiService.GetTweets("joe_smith");
            }
            if (_tweetResults.Count == 0) return 0;
            var orderedList = _tweetResults.OrderBy(x => x.createdAt).ToList();
            TimeSpan maxday = Enumerable.Range(1, orderedList.Count - 1)
                  .Select(i => orderedList[i].createdAt.Subtract(orderedList[i - 1].createdAt))
                  .Max();            

            return maxday.Days;
        }

        /**
         * Retrieves the most popular hash tag tweeted by the given user.
         *
         * Note that the string returned by this method should include the hashtag itself.
         * For example, if the most popular hash tag is "#Java", this method should return "#Java".
         * If there are no tweets for the given user, this method should return null.
         *
         * @param userName the name of the user
         * @return the most popular hash tag tweeted by the given user.
        */
        public string GetMostPopularHashTag(string userName) {
            if (_tweetResults == null)
            {
                _tweetResults = TweetsApiService.GetTweets("joe_smith");
            }
            if (_tweetResults.Count == 0) return null;
            var regex = new Regex(@"#\w+");
            //var matches = regex.Matches("test").Select(x => x.Value);

            var hashtags = _tweetResults.Select(x => regex.Matches(x.text))
                            .Where(x => x.Count >= 1)
                            .Select(y => y[0].Value)
                            .GroupBy(n => n).Select(x => new { Key = x.Key, count = x.Count() })
                            .OrderByDescending(x => x.count)
                            .First().Key;

            return hashtags;

        }

    }
}
