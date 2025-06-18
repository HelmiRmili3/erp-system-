using System.Reflection;

namespace Backend.Application.Common.Mappings;

public static class CustomMapper
{
    private static readonly Lazy<IMapper> Lazy = new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.ShouldMapProperty = p => p.GetMethod != null && (p.GetMethod.IsPublic || p.GetMethod.IsAssembly);
            var profiles = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract)
                .Select(type => (Profile)Activator.CreateInstance(type)!)
                .Where(profile => profile != null);

            foreach (var profile in profiles)
            {
                cfg.AddProfile(profile);
            }
        });

        var mapper = config.CreateMapper();
        return mapper;
    });

    public static IMapper Mapper => Lazy.Value;
}
