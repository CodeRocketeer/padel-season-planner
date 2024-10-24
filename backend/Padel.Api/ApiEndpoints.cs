namespace Padel.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    

    public static class Seasons
    {
        private const string Base = $"{ApiBase}/seasons";

        public const string Create = Base;
        public const string Get = $"{Base}/{{idOrSlug}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
        public const string Confirm = $"{Base}/{{id:guid}}/confirm";

        // participate, using bearer token userId
        public const string Participate = $"{Base}/{{seasonId:guid}}/participate";
        public const string Leave = $"{Base}/{{seasonId:guid}}/leave";
    }

    public static class Participants
    {
        private const string Base = $"{ApiBase}/participants";
        public const string GetAll = Base;

    }


    public static class Seeders
    {
        private const string Base = $"{ApiBase}/seeders";
        public const string SeedParticipants = $"{Base}/participants";
    
    }

}
