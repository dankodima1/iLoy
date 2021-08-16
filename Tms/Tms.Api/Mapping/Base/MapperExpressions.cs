using System.Reflection;

using AutoMapper;

using Tms.Data.Domain;
using Tms.Dto.Base;

namespace Tms.Api.Mapping
{
    public static class MapperExpressions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreNonExisting<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }

        public static IMappingExpression<TSource, TDestination> IgnoreBase<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mapping)
        where TSource : BaseEntity
        where TDestination : BaseEntity
        {
            mapping.ForMember(d => d.Id, c => c.UseDestinationValue());
            mapping.ForMember(d => d.CreatedOnUtc, c => c.UseDestinationValue());
            mapping.ForMember(d => d.UpdatedOnUtc, c => c.UseDestinationValue());
            return mapping;
        }

        public static IMappingExpression<TSource, TDestination> IgnoreBaseDto<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mapping)
        where TSource : BaseDto
        where TDestination : BaseEntity
        {
            mapping.ForMember(d => d.Id, c => c.UseDestinationValue());
            mapping.ForMember(d => d.CreatedOnUtc, c => c.UseDestinationValue());
            mapping.ForMember(d => d.UpdatedOnUtc, c => c.UseDestinationValue());
            return mapping;
        }
    }
}
