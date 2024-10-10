namespace Padel.Api
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Teams
        {
            private const string Base = $"{ApiBase}/teams";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }

        public static class Players
        {
            private const string Base = $"{ApiBase}/players";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";

        }

    }
}
