using Microsoft.Extensions.Configuration;

namespace Common.Core.Constant;

/// <summary>
/// All the common constant for whole .NET code
/// </summary>
[ExcludeFromCodeCoverage]
public partial class CommonConst
{
    public CommonConst()
    { }

    #region CONSTANTS
    public const string ACCESS_TOKEN_SESSION = "accessTokenSession";
    public const string APP_ENVIRONMENT = "DEV";
    public const string BASE_RBAC_POA_KEY = "rbacPoa";
    public const string CONTENT_TYPE = "Content-Type";
    public const string CONTENT_TYPE_APP_JSON = "application/json";
    public const string CONTENT_TYPE_APP_PNG = "image/png";
    public static readonly DateTime DEFAULT_MIN_DATE = DateTime.Parse("1900-01-01");
    public const int DEFAULT_NRP = 0;
    public const string DRBAC_LEVEL_CONTEXT_ITEM_KEY = "DRBAC_LOW";
    public const string DRBAC_PERMISSION_CONTEXT_ITEM_KEY = "DRBAC";
    public const string DRBAC_POA_REDIS_KEY = "dataRbacPoa";
    public const string DRBAC_REDIS_KEY = "dataRbac";
    public const string HEADER_AUTHORIZATION = "Authorization";
    public const string HEADER_DRBAC = "DRBAC";
    public const string HEADER_FORWARDED = "X-Forwarded-For";
    public const string HEADER_OID = "oid";
    public const string HEADER_TIMEOFFSET = "timeOffset";
    public const string HEADER_TRANSACTION_ID = "transactionId";
    public const string HEADER_UNIQUE_ROLE = "uniqueRole";
    public const string HEADER_USER_AGENT = "User-Agent";
    public const string HEADER_USER_NAME = "fullName";
    public const string HEADER_USER_ID = "userId";
    public const string HEADER_USER_NRP = "nrp";
    public const string HEADER_USER_PREF_NAME = "preferred_name";
    public const string HEADER_USER_ROLE = "userRole";
    /// <summary>
    /// Maximum service bus DLQ retry time in seconds
    /// </summary>
    public const int MAX_DLQ_RETRY = 180;
    #endregion

    #region CRON
    public const string SAMPLE_CRON = "0 * * * * *";
    #endregion

    #region HTTP METHOD CONSTANT VERSION
    public const string POST = "POST";
    public const string GET = "GET";
    public const string PUT = "PUT";
    public const string DELETE = "DELETE";
    #endregion

    #region REGEX
    public const string REGEX_CODE = "^[0-9a-zA-Z ]+$"; //Numbers and letters only
    public const string REGEX_EMAIL = "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"; //email format
    public const string REGEX_NAME = "^[0-9a-zA-Z ]?[0-9a-zA-Z_|\\.|\\,|\\s|\\-|\\(|\\)|\\/]+$"; //"Numbers, letters, space, point, dash and comma
    public const string REGEX_FULLNAME = "^[a-zA-Z ]?[a-zA-Z_|\\.|\\,|\\s|\\-|\\(|\\)|\\/]+$"; //"Letters, space, point, dash and comma
    public const string REGEX_NUMBER = "^[0-9]+$"; //Numbers only
    public const string REGEX_NUMBER_COMMA = "^[0-9,]+$"; //Numbers and commas only
    public const string REGEX_PHONE_NO = "^\\+?([ -]?\\d+)+|\\(\\d+\\)([ -]\\d+)/*]*/";
    public const string REGEX_LICENSE_PLATE = @"([a-zA-Z]{1,3})(\d{1,4})([a-zA-Z]{0,3})";
    /// <summary>
    /// Letters, numbers and hypens only
    /// </summary>
    public const string REGEX_RESERVATION = @"^[a-zA-Z0-9-]+$";
    public const string REGEX_SWAGGER_DOC_FILTER = "/v\\d+[^/]*";
    #endregion

    #region GET CONFIGURATION
    private static IConfiguration baseconfig;
    /// <summary>
    /// Get Application Settings Configuration File
    /// Please add appsettings.json to your client project (REST API, console, Web)
    /// </summary>
    public static IConfiguration BaseConfiguration
    {
        get
        {
            if (baseconfig == null)
            {
                IConfigurationBuilder configBuilder = new ConfigurationBuilder().
                                                          SetBasePath(Directory.GetCurrentDirectory()).
                                                          AddJsonFile("appsettings.json");
                baseconfig = configBuilder.Build();
            }

            return baseconfig;
        }
    }
    #endregion

    #region RETRIEVE CONFIGURATION
    private static string? glitchtip_dsn;
    public static string GLITCHTIP_DSN
    {
        get
        {
            if (string.IsNullOrWhiteSpace(glitchtip_dsn))
            {
                string path = BaseConfiguration.GetSection("AppConfigs").GetSection("SourcePath").Value;
                string name = BaseConfiguration.GetSection("AppConfigs").GetSection("SourceName").Value;
                string appConfPath = Path.Combine(Directory.GetCurrentDirectory(), path);

                IConfiguration config;
                IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(appConfPath).AddJsonFile(name);

                config = builder.Build();

                if (config != null)
                {
                    glitchtip_dsn = config["GLITCHTIP_DSN"];
                }
            }
            return glitchtip_dsn!;
        }
    }

    private static string? rbac_url;
    public static string RBAC_URL
    {
        get
        {
            if (string.IsNullOrWhiteSpace(rbac_url))
            {
                string path = BaseConfiguration.GetSection("AppConfigs").GetSection("SourcePath").Value;
                string name = BaseConfiguration.GetSection("AppConfigs").GetSection("SourceName").Value;
                string appConfPath = Path.Combine(Directory.GetCurrentDirectory(), path);

                IConfiguration config;
                IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(appConfPath).AddJsonFile(name);

                config = builder.Build();

                if (config != null)
                {
                    rbac_url = config["RBAC_URL"];
                }
            }

            return rbac_url!;
        }
    }

    private static string? user_url;
    public static string USER_URL
    {
        get
        {
            if (string.IsNullOrWhiteSpace(user_url))
            {
                string path = BaseConfiguration.GetSection("AppConfigs").GetSection("SourcePath").Value;
                string name = BaseConfiguration.GetSection("AppConfigs").GetSection("SourceName").Value;
                string appConfPath = Path.Combine(Directory.GetCurrentDirectory(), path);

                IConfiguration config;
                IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(appConfPath).AddJsonFile(name);

                config = builder.Build();

                if (config != null)
                {
                    user_url = config["USER_URL"];
                }
            }
            return user_url!;
        }
    }
    #endregion

}