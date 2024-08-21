namespace HolyClient.Core;

public static class Extensions
{
    
    public static int ProtocolVersion(this string MCVersion)
    {
        switch (MCVersion.Split(' ')[0].Trim())
        {
            case "1.12.2":
                return 340;
            case "1.13":
            case "1.13.0":
                return 393;
            case "1.13.1":
                return 401;
            case "1.13.2":
                return 404;
            case "1.14":
            case "1.14.0":
                return 477;
            case "1.14.1":
                return 480;
            case "1.14.2":
                return 485;
            case "1.14.3":
                return 490;
            case "1.14.4":
                return 498;
            case "1.15":
            case "1.15.0":
                return 573;
            case "1.15.1":
                return 575;
            case "1.15.2":
                return 578;
            case "1.16":
            case "1.16.0":
                return 735;
            case "1.16.1":
                return 736;
            case "1.16.2":
                return 751;
            case "1.16.3":
                return 753;
            case "1.16.4":
            case "1.16.5":
                return 754;
            case "1.17":
            case "1.17.0":
                return 755;
            case "1.17.1":
                return 756;
            case "1.18":
            case "1.18.0":
            case "1.18.1":
                return 757;
            case "1.18.2":
                return 758;
            case "1.19":
            case "1.19.0":
                return 759;
            case "1.19.1":
            case "1.19.2":
                return 760;
            default:
                if (int.TryParse(MCVersion, out var r))
                    return r;
                return 0;
        }
    }
}