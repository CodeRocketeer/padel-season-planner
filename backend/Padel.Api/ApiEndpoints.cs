namespace Padel.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    

    public static class Seasons
    {
        private const string Base = $"{ApiBase}/seasons";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:int}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:int}}";
        public const string Delete = $"{Base}/{{id:int}}";
        public const string Confirm = $"{Base}/{{id:int}}/confirm";
        public const string CreateSeasonSchedule = $"{Base}/{{id:int}}/create-schedule";

        // participate, using bearer token userId
        public const string Join = $"{Base}/{{seasonId:int}}/join";
        public const string Leave = $"{Base}/{{seasonId:guid}}/leave";

       
    }

    public static class Players
    {
        private const string Base = $"{ApiBase}/players";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:int}}"; 
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:int}}";
        public const string Delete = $"{Base}/{{id:int}}";
    }




}
