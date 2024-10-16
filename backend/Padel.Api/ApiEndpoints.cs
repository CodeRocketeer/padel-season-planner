namespace Padel.Api
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Seasons
        {
            private const string Base = $"{ApiBase}/seasons";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
            // confirm season
            public const string ConfirmSeason = $"{Base}/{{id:guid}}/confirm";
        }

        public static class Matches
        {
            private const string Base = $"{ApiBase}/matches";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";

        }

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
            // seeder for testing
            public const string Seed = $"{Base}/{{seasonId:guid}}/seed";
        }



    }
}
