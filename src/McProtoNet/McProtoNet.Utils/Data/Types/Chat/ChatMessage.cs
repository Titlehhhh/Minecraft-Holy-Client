using Newtonsoft.Json;

namespace McProtoNet.Utils
{
    public class ChatMessage
    {
        [JsonProperty(PropertyName = "translate", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string? Translate { get; set; }

        [JsonProperty(PropertyName = "with", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public ChatMessage? With { get; set; }

        [JsonProperty(PropertyName = "text", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "color", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string? Color { get; set; }

        [JsonProperty(PropertyName = "bold", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public bool Bold { get; set; }

        [JsonProperty(PropertyName = "italic", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public bool Italic { get; set; }

        [JsonProperty(PropertyName = "underlined", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public bool Underlined { get; set; }

        [JsonProperty(PropertyName = "strikethrough", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public bool Strikethrough { get; set; }

        [JsonProperty(PropertyName = "obfuscated", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public bool Obfuscated { get; set; }

        [JsonProperty(PropertyName = "insertion", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string? Insertion { get; set; }

        //TODO
        //[IgnoreDataMember]
        [JsonProperty(PropertyName = "clickEvent", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public ClickComponent? ClickEvent { get; set; }
        //TODO
        // [IgnoreDataMember]
        [JsonProperty(PropertyName = "hoverEvent", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public HoverComponent? HoverEvent { get; set; }

        [JsonProperty(PropertyName = "extra", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public List<ChatMessage>? Extra { get; set; }
        public IEnumerable<ChatMessage> Extras => GetExtras();

        public IEnumerable<ChatMessage> GetExtras()
        {
            if (Extra == null)
                yield break;

            foreach (var extra in Extra)
            {
                yield return extra;
            }
        }

        public static implicit operator ChatMessage(string text) => Simple(text);

        public static ChatMessage operator +(ChatMessage a, ChatMessage b) => a.AddExtra(b);

        public static ChatMessage operator +(ChatMessage a, ChatColor b) => a.AppendColor(b);

        public static ChatMessage Simple(string text) => new() { Text = text };

        public static ChatMessage SimpleLegacy(string text) => new() { Text = ReformatAmpersandPrefixes(text) };

        public static ChatMessage Simple(string text, ChatColor color) => new()
        {
            Text = $"{color}{text}"
        };

        public static ChatMessage SimpleLegacy(string text, ChatColor color) => new()
        {
            Text = $"{color}{ReformatAmpersandPrefixes(text)}"
        };

        public static ChatMessage Click(ChatMessage message, EClickAction action, string value, string translate = "")
        {
            message.ClickEvent = new ClickComponent(action, value, translate);
            return message;
        }

        public static ChatMessage Hover(ChatMessage message, EHoverAction action, object contents, string translate = "")
        {
            message.HoverEvent = new HoverComponent(action, contents, translate);
            return message;
        }

        public static string ReformatAmpersandPrefixes(string originalText)
        {
            return string.Create(originalText.Length, originalText, (span, text) =>
            {
                for (int i = 0; i < span.Length; i++)
                {
                    char c = text[i];
                    span[i] = c;

                    if (c == '&' && i + 1 < text.Length)
                    {
                        c = text[i + 1];
                        if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'e') || (c >= 'k' && c <= 'o') || c == 'r')
                        {
                            span[i] = '§';
                        }
                    }
                }
            });
        }

        public ChatMessage AddExtra(ChatMessage message)
        {
            Extra ??= new List<ChatMessage>();
            Extra.Add(message);

            return this;
        }

        public ChatMessage AddExtra(List<ChatMessage> messages)
        {
            Extra ??= new List<ChatMessage>(capacity: messages.Count);
            Extra.AddRange(messages);

            return this;
        }

        public ChatMessage AddExtra(IEnumerable<ChatMessage> messages)
        {
            foreach (var message in messages)
            {
                AddExtra(message);
            }

            return this;
        }

        public ChatMessage AppendText(string text)
        {
            if (Text is null)
            {
                Text = text;
            }
            else
            {
                Text += text;
            }
            return this;
        }

        public ChatMessage AppendColor(ChatColor color)
        {
            if (Text is null)
            {
                Text = color.ToString();
            }
            else
            {
                Text += color.ToString();
            }

            return this;
        }

        public ChatMessage AppendText(string text, ChatColor color)
        {
            if (Text is null)
            {
                Text = $"{color}{text}";
            }
            else
            {
                Text += $"{color}{text}";
            }
            return this;
        }
        public ChatMessage()
        {
            // Extra = new List<ChatMessage>();
        }

        public static ChatMessage Parse(string json)
        {
            return (ChatMessage)JsonConvert.DeserializeObject(json);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }


        public static ChatMessage Empty => Simple(string.Empty);


    }
}
