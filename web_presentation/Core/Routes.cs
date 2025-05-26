namespace web_presentation.Core
{
    public static class Routes
    {
        // Authentication routes
        public const string Home = "/Index";
        public const string Login = "/Login";
        public const string Register = "/Register";
        public const string Logout = "/Logout";

        // Dictionary for dynamic access
        public static readonly Dictionary<string, string> RouteMap = new()
        {
            { "Home", Home },
            { "Login", Login },
            { "Register", Register },
            { "Logout", Logout }
        };

        // Route validation
        public static bool IsValidRoute(string routeName)
        {
            return RouteMap.ContainsKey(routeName);
        }
    }
}
