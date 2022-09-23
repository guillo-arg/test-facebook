using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_facebook
{

    public class RequestDTO
    {
        public string @object { get; set; }
        public Entry[] entry { get; set; }
    }

    public class Entry
    {
        public string id { get; set; }
        public long time { get; set; }
        public Messaging[] messaging { get; set; }
        public Hop_Context[] hop_context { get; set; }
        public Change[] changes { get; set; }
    }

    public class Messaging
    {
        public Sender sender { get; set; }
        public Recipient recipient { get; set; }
        public long timestamp { get; set; }
        public Message message { get; set; }
        public Postback postback { get; set; }
    }

    public class Sender
    {
        public string id { get; set; }
    }

    public class Recipient
    {
        public string id { get; set; }
    }

    public class Message
    {
        public string mid { get; set; }
        public string text { get; set; }
        public Attachment[] attachments { get; set; }
    }

    public class Hop_Context
    {
        public long app_id { get; set; }
        public string metadata { get; set; }
    }

    public class Attachment
    {
        public string type { get; set; }
        public Payload payload { get; set; }
    }

    public class Payload
    {
        public string url { get; set; }
    }

    public class Change
    {
        public Value value { get; set; }
        public string field { get; set; }
    }

    public class Value
    {
        public From from { get; set; }
        public string post_id { get; set; }
        public int created_time { get; set; }
        public string item { get; set; }
        public string parent_id { get; set; }
        public string reaction_type { get; set; }
        public string verb { get; set; }
        public Post post { get; set; }
        public string message { get; set; }
        public string comment_id { get; set; }
    }

    public class From
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Postback
    {
        public string title { get; set; }
        public string payload { get; set; }
    }

    public class Post
    {
        public string status_type { get; set; }
        public bool is_published { get; set; }
        public string updated_time { get; set; }
        public string permalink_url { get; set; }
        public string promotion_status { get; set; }
        public string id { get; set; }

    }
}
