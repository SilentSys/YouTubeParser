using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace YouTubeParser
{
    public class VideoParser
    {
        private readonly string url;

        public VideoParser(string url)
        {
            this.url = url;
        }

        public async Task<Video> parse()
        {
            string html;

            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                //client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.ReasonPhrase);

                html = await response.Content.ReadAsStringAsync();

            }

            var video = new Video();

            var document = new AngleSharp.Parser.Html.HtmlParser().Parse(html);

            video.title = document.QuerySelectorAll("meta[property='og:title']").First().GetAttribute("content");

            video.thumbnailUrl = document.QuerySelectorAll("meta[property='og:image']").First().GetAttribute("content");

            video.description = document.QuerySelectorAll("p[id='eow-description']").First().TextContent;



            return video;
        }
    }

    public class Video
    {
        public string title { get; set; }
        public string description { get; set; }
        public string thumbnailUrl { get; set; }
    }
}
