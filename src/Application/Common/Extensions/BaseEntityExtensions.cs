using Backend.Application.Common.Mappings;
using Backend.Domain.Common;

namespace Backend.Application.Common.Extensions;
public static class BaseEntityExtensions
{
    public static T ToDto<T>(this BaseEntity entity) where T : class
    {
        return CustomMapper.Mapper.Map<T>(entity);
    }

}
