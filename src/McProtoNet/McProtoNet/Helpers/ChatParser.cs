using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace McProtoNet;

public static class ChatParser
{
    private static readonly Regex regex = new("§[0-9a-fr]", RegexOptions.Compiled);

    /// <summary>
    ///     Specify whether translation rules have been loaded
    /// </summary>
    private static bool RulesInitialized;

    /// <summary>
    ///     Set of translation rules for formatting text
    /// </summary>
    private static readonly Dictionary<string, string> TranslationRules = new();

    public static string ParseText(string json, List<string> links = null, bool Without = true)
    {
        var test = JSONData2String(Json.ParseJson(json), "", links);
        if (Without)
            test = regex.Replace(test, "");

        return test;
    }

    /// <summary>
    ///     Get the classic color tag corresponding to a color name
    /// </summary>
    /// <param name="colorname">Color Name</param>
    /// <returns>Color code</returns>
    private static string Color2tag(string colorname)
    {
        switch (colorname.ToLower())
        {
            /* MC 1.7+ Name           MC 1.6 Name           Classic tag */
            case "black": /*  Blank if same  */ return "§0";
            case "dark_blue": return "§1";
            case "dark_green": return "§2";
            case "dark_aqua":
            case "dark_cyan": return "§3";
            case "dark_red": return "§4";
            case "dark_purple":
            case "dark_magenta": return "§5";
            case "gold":
            case "dark_yellow": return "§6";
            case "gray": return "§7";
            case "dark_gray": return "§8";
            case "blue": return "§9";
            case "green": return "§a";
            case "aqua":
            case "cyan": return "§b";
            case "red": return "§c";
            case "light_purple":
            case "magenta": return "§d";
            case "yellow": return "§e";
            case "white": return "§f";
            default: return "";
        }
    }

    /// <summary>
    ///     Initialize translation rules.
    ///     Necessary for properly printing some chat messages.
    /// </summary>
    public static void InitTranslations()
    {
        if (!RulesInitialized)
        {
            InitRules();
            RulesInitialized = true;
        }
    }

    /// <summary>
    ///     Internal rule initialization method. Looks for local rule file or download it from Mojang asset servers.
    /// </summary>
    private static void InitRules()
    {
        //Small default dictionnary of translation rules
        TranslationRules["chat.type.admin"] = "[%s: %s]";
        TranslationRules["chat.type.announcement"] = "§d[%s] %s";
        TranslationRules["chat.type.emote"] = " * %s %s";
        TranslationRules["chat.type.text"] = "<%s> %s";
        TranslationRules["multiplayer.player.joined"] = "§e%s joined the game.";
        TranslationRules["multiplayer.player.left"] = "§e%s left the game.";
        TranslationRules["commands.message.display.incoming"] = "§7%s whispers to you: %s";
        TranslationRules["commands.message.display.outgoing"] = "§7You whisper to %s: %s";
    }

    /// <summary>
    ///     Format text using a specific formatting rule.
    ///     Example : * %s %s + ["ORelio", "is doing something"] = * ORelio is doing something
    /// </summary>
    /// <param name="rulename">Name of the rule, chosen by the server</param>
    /// <param name="using_data">Data to be used in the rule</param>
    /// <returns>Returns the formatted text according to the given data</returns>
    private static string TranslateString(string rulename, List<string> using_data)
    {
        if (!RulesInitialized)
        {
            InitRules();
            RulesInitialized = true;
        }

        if (TranslationRules.ContainsKey(rulename))
        {
            var using_idx = 0;
            var rule = TranslationRules[rulename];
            var result = new StringBuilder();
            for (var i = 0; i < rule.Length; i++)
            {
                if (rule[i] == '%' && i + 1 < rule.Length)
                {
                    //Using string or int with %s or %d
                    if (rule[i + 1] == 's' || rule[i + 1] == 'd')
                    {
                        if (using_data.Count > using_idx)
                        {
                            result.Append(using_data[using_idx]);
                            using_idx++;
                            i += 1;
                            continue;
                        }
                    }

                    //Using specified string or int with %1$s, %2$s...
                    else if (char.IsDigit(rule[i + 1])
                             && i + 3 < rule.Length && rule[i + 2] == '$'
                             && (rule[i + 3] == 's' || rule[i + 3] == 'd'))
                    {
                        var specified_idx = rule[i + 1] - '1';
                        if (using_data.Count > specified_idx)
                        {
                            result.Append(using_data[specified_idx]);
                            using_idx++;
                            i += 3;
                            continue;
                        }
                    }
                }

                result.Append(rule[i]);
            }

            return result.ToString();
        }

        return "[" + rulename + "] " + string.Join(" ", using_data);
    }

    /// <summary>
    ///     Use a JSON Object to build the corresponding string
    /// </summary>
    /// <param name="data">JSON object to convert</param>
    /// <param name="colorcode">Allow parent color code to affect child elements (set to "" for function init)</param>
    /// <param name="links">Container for links from JSON serialized text</param>
    /// <returns>returns the Minecraft-formatted string</returns>
    private static string JSONData2String(Json.JSONData data, string colorcode, List<string> links)
    {
        var extra_result = "";
        switch (data.Type)
        {
            case Json.JSONData.DataType.Object:
                if (data.Properties.ContainsKey("color"))
                    colorcode = Color2tag(JSONData2String(data.Properties["color"], "", links));

                if (data.Properties.ContainsKey("clickEvent") && links != null)
                {
                    var clickEvent = data.Properties["clickEvent"];
                    if (clickEvent.Properties.ContainsKey("action")
                        && clickEvent.Properties.ContainsKey("value")
                        && clickEvent.Properties["action"].StringValue == "open_url"
                        && !string.IsNullOrEmpty(clickEvent.Properties["value"].StringValue))
                        links.Add(clickEvent.Properties["value"].StringValue);
                }

                if (data.Properties.ContainsKey("extra"))
                {
                    var extras = data.Properties["extra"].DataArray.ToArray();
                    foreach (var item in extras)
                        extra_result = extra_result + JSONData2String(item, colorcode, links) + "§r";
                }

                if (data.Properties.ContainsKey("text"))
                    return colorcode + JSONData2String(data.Properties["text"], colorcode, links) + extra_result;

                if (data.Properties.ContainsKey("translate"))
                {
                    var using_data = new List<string>();
                    if (data.Properties.ContainsKey("using") && !data.Properties.ContainsKey("with"))
                        data.Properties["with"] = data.Properties["using"];
                    if (data.Properties.ContainsKey("with"))
                    {
                        var array = data.Properties["with"].DataArray.ToArray();
                        for (var i = 0; i < array.Length; i++)
                            using_data.Add(JSONData2String(array[i], colorcode, links));
                    }

                    return colorcode +
                           TranslateString(JSONData2String(data.Properties["translate"], "", links), using_data) +
                           extra_result;
                }

                return extra_result;

            case Json.JSONData.DataType.Array:
                var result = "";
                foreach (var item in data.DataArray) result += JSONData2String(item, colorcode, links);

                return result;

            case Json.JSONData.DataType.String:
                return colorcode + data.StringValue;
        }

        return "";
    }

    /// <summary>
    ///     Do a HTTP request to get a webpage or text data from a server file
    /// </summary>
    /// <param name="url">URL of resource</param>
    /// <returns>Returns resource data if success, otherwise a WebException is raised</returns>
    private static string DownloadString(string url)
    {
        var myRequest = (HttpWebRequest)WebRequest.Create(url);
        myRequest.Method = "GET";
        var myResponse = myRequest.GetResponse();
        var sr =
            new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
        var result = sr.ReadToEnd();
        sr.Close();
        myResponse.Close();
        return result;
    }
}