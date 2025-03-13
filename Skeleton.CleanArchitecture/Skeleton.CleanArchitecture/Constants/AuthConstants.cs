namespace Skeleton.CleanArchitecture.Constants
{
    public static class AuthConstants
    {
        public static class Policies
        {
            public const string Read = nameof(Read);

            public const string ReadWrite = nameof(ReadWrite);
        }

        public static class Roles
        {
            public const string AppRead = "App.Read";

            public const string AppReadWrite = "App.ReadWrite";

            public const string ScpRead = "Scp.Read";

            public const string ScpReadWrite = "Scp.ReadWrite";
        }
    }
}
