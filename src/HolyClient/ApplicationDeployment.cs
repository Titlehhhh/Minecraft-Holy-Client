using System;

namespace HolyClient;

public class ApplicationDeployment
{
    private static ApplicationDeployment currentDeployment;
    private static bool currentDeploymentInitialized;

    private static bool isNetworkDeployed;
    private static bool isNetworkDeployedInitialized;

    private ApplicationDeployment()
    {
        // As an alternative solution, we could initialize all properties here
    }

    public static bool IsNetworkDeployed
    {
        get
        {
            if (!isNetworkDeployedInitialized)
            {
                bool.TryParse(Environment.GetEnvironmentVariable("ClickOnce_IsNetworkDeployed"), out isNetworkDeployed);
                isNetworkDeployedInitialized = true;
            }

            return isNetworkDeployed;
        }
    }

    public static ApplicationDeployment CurrentDeployment
    {
        get
        {
            if (!currentDeploymentInitialized)
            {
                currentDeployment = IsNetworkDeployed ? new ApplicationDeployment() : null;
                currentDeploymentInitialized = true;
            }

            return currentDeployment;
        }
    }

    public Uri ActivationUri
    {
        get
        {
            Uri.TryCreate(Environment.GetEnvironmentVariable("ClickOnce_ActivationUri"), UriKind.Absolute, out var val);
            return val;
        }
    }

    public Version CurrentVersion
    {
        get
        {
            Version.TryParse(Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion"), out var val);
            return val;
        }
    }

    public string DataDirectory => Environment.GetEnvironmentVariable("ClickOnce_DataDirectory");

    public bool IsFirstRun
    {
        get
        {
            bool.TryParse(Environment.GetEnvironmentVariable("ClickOnce_IsFirstRun"), out var val);
            return val;
        }
    }

    public DateTime TimeOfLastUpdateCheck
    {
        get
        {
            DateTime.TryParse(Environment.GetEnvironmentVariable("ClickOnce_TimeOfLastUpdateCheck"), out var value);
            return value;
        }
    }

    public string UpdatedApplicationFullName =>
        Environment.GetEnvironmentVariable("ClickOnce_UpdatedApplicationFullName");

    public Version UpdatedVersion
    {
        get
        {
            Version.TryParse(Environment.GetEnvironmentVariable("ClickOnce_UpdatedVersion"), out var val);
            return val;
        }
    }

    public Uri UpdateLocation
    {
        get
        {
            Uri.TryCreate(Environment.GetEnvironmentVariable("ClickOnce_UpdateLocation"), UriKind.Absolute,
                out var val);
            return val;
        }
    }

    public Version LauncherVersion
    {
        get
        {
            Version.TryParse(Environment.GetEnvironmentVariable("ClickOnce_LauncherVersion"), out var val);
            return val;
        }
    }
}