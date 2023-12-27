using Ara3D.Utils;

namespace Ara3D.Penguin;

public class Article
{
    public Template ArticleTemplate { get; }
}

public class Page
{
}

public class Markdown
{
    public string RawText { get; }
    public string Html { get; }
    public FilePath File { get; }
}

public class TableOfContents
{
}

public class Template 
{ }

public class ImageData
{
    public FilePath File;
    public string Title;
    public string Description;
    public string Credit;
}

public class ArticleData
{
    public string Title { get; set; }
    public string Copyright { get; set; }
    public List<string> Author { get; set; }
    public string Summary { get; set; }
    public List<string> Uris { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }
    public List<string> Tags { get; set; }
    public bool Published { get; set; }
    public string DateTime { get; set; }
    public string Id { get; set; }
    public string PostScript { get; set; }
    public string Deprecated { get; set; }
    public FilePath Thumbnail { get; set; }
    public List<string> Attributions { get; set; }

    // 
    public string TwitterCardSummary;

}

// https://developer.twitter.com/en/docs/twitter-for-websites/cards/overview/summary-card-with-large-image
public class TwitterData
{
    // https://cards-dev.twitter.com/validator
    // https://twittercommunity.com/t/card-validator-preview-removal/175006/3
    //  800px by 418px,
    // Images must be less than 5MB in size.JPG, PNG, WEBP and GIF formats are supported.
    public FilePath Image;
    public string ImageAlt;
    public string TwitterUserName;
    public string Site;
    public string Description;
}

public class AISuggestion
{
    public List<string> Tags;
    public string Description;
    public string Title;
    public string Suggestions;
    public string Rewrite; 
    public FilePath ImagePath;
    public string Summary;
    public string Abstract;
    public string ConciseRewrite;
    public string AdditionalPoints; 
    public List<string> Quotes;
}

public class Ogp
{
    public string Title;
    public FilePath Image;
    public string Url;
    public string Description;
    
    //Use og:image:width and og:image:height Open Graph tags: 
    public int ImageWidth;
    public int ImageHeight;

    // https://developers.facebook.com/tools/debug/
    // https://developers.facebook.com/docs/sharing/webmasters/
    // https://developers.facebook.com/docs/sharing/webmasters/images
    // The minimum allowed image dimension is 200 x 200 pixels.
    // The size of the image file must not exceed 8 MB.
    // Use images that are at least 1200 x 630 pixels for the best display on high resolution devices.
    // At the minimum, you should use images that are 600 x 315 pixels to display link page posts with larger images.
    // og:title 40 chars
    // og:description has 2 max lengths:
    // When the link is used in a Post, it's 300 chars. When a link is used in a Comment, it's 110 chars.

    //https://www.seoquake.com/blog/open-graph-meta-tags-for-facebook-and-twitter


    // https://www.linkedin.com/help/linkedin/answer/a521928
    // https://www.linkedin.com/post-inspector/inspect/
    // Max file size: 5 MB
    // Minimum image dimensions: 1200 (w) x 627 (h) pixels
    // Recommended ratio: 1.91:1
    // JPG, PNG, or GIF
    // Animated GIF images must be 300 frames or shorterHorizontal/landscape: 1.91:1 (recommended ratio)
    // Minimum: 640 x 360 pixels
    // Maximum: 7680 x 4320 pixels
    // Square: 1:1
    // Minimum: 360 x 360 pixels
    // Maximum: 4320 x 4320 pixels
    // Vertical: 1:1.91 (recommended ratio)
    // Minimum: 360 x 640 pixels
    // Maximum: 2340 x 4320 pixels
    // https://learn.microsoft.com/en-us/linkedin/consumer/integrations/self-serve/share-on-linkedin?context=linkedin%2Fconsumer%2Fcontext
}

public class Url
{
}

// Per author 
public class FollowLinks
{
    // https://developer.twitter.com/en/docs/twitter-for-websites/tweet-button/overview
    public Url Twitter;
    public Url MailingList;
    public Url YouTube;
    public Url Patreon;
    public Url BuyMeACoffee;
    public Url GoFundMe;
    public Url GitHub;
    public Url Facebook;
    public Url LinkedIn;
    public Url Tumblr;
    public Url Mastodon;
    public Url Instagram;
}

public class SharingLinks
{
    public Url ShortenedUri;
    public Url LinkedIn;
    public Url Email;
    public Url Reddit;
    public Url HackerNews;
    // https://developer.twitter.com/en/docs/twitter-for-websites/tweet-button/overview
    public Url Twitter;
    public Url Mastodon;
    public Url Facebook;
    public Url Pinterest;
    public Url Tumblr;
}

public class ExtraArticleData
{

}

public class Css
{
}

public class Author
{
    public string Name;
    public string ShortBio;
    public string LongBio;
    public List<string> Projects;
    public FollowLinks FollowLinks;
}

public class Blog
{
    public Css Css;
    public Page LandingPage;
    public Page Toc;
    public Page About;
    public List<Author> Authoea;
    public List<Article> Articles;
}