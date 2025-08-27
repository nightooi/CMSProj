namespace CMSProj.SubSystems.RouteResolvers
{
    public static class FindPageByGuidWebroot
    {
        public static Stream? FindPageByGuid(this Guid guid, IWebHostEnvironment env, FileMode filemode)
        {
            if (!Directory.Exists(guid.GetPathByBuid(env)))
                return null;

            return new FileStream(Path.Combine(guid.GetPathByBuid(env), "Index.html"), filemode);
        }
        public static string GetPathByBuid(this Guid guid, IWebHostEnvironment env)
        {
            return Path.Combine(env.WebRootPath, guid.ToString());
        }
    }
}

