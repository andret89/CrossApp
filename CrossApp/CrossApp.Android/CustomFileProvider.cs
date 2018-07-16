using Android.App;
using Android.Content;
using Android.Support.V4.Content;

namespace CrossApp
{
    [ContentProvider(
        new[] { "${applicationId}.fileProvider" },
        Name = "com.companyname.CrossApp.FileProvider",
        Exported = false,
        GrantUriPermissions = true)]
    [MetaData(
        "android.support.FILE_PROVIDER_PATHS",
        Resource = "@xml/file_paths")]
    public class CustomProvider : Android.Support.V4.Content.FileProvider
    {
    }
}

