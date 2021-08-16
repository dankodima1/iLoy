using AutoMapper;

namespace Tms.Api.Mapping
{
    public static class AutoMapperConfiguration
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource src)
        {
            return Mapper.Map<TSource, TDestination>(src);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource src, TDestination dest)
        {
            return Mapper.Map(src, dest);
        }

        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// Mapper configuration
        /// </summary>
        public static MapperConfiguration MapperConfiguration { get; private set; }

        /// <summary>
        /// Initialize mapper
        /// </summary>
        /// <param name="config">Mapper configuration</param>
        public static void Init(MapperConfiguration config, IMapper mapper)
        {
            MapperConfiguration = config;
            Mapper = mapper;
        }
    }
}
