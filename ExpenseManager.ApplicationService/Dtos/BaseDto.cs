using AutoMapper;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.Domain.SeedWork;

namespace ExpenseManager.ApplicationService.Dtos
{
    public abstract class BaseDto<TDto, TEntity, TKey> : IHaveMapping
     where TDto : class, new()
     where TEntity : BaseEntity<TKey>
    {
        public TKey Id { get; set; }

        //public TEntity ToEntity()
        //{
        //    return Mapper.Map<TEntity>(CastToDerivedClass(this));
        //}

        //public TEntity ToEntity(TEntity entity)
        //{
        //    return Mapper.Map(CastToDerivedClass(this), entity);
        //}

        public static TDto FromEntity(TEntity model, IMapper mapper)
        {
            return  mapper.Map<TDto>(model);
        }

        //protected TDto CastToDerivedClass(BaseDto<TDto, TEntity, TKey> baseInstance)
        //{

        //    return Mapper.Map<TDto>(baseInstance);
        //}

        public void CreateMappings(Profile profile)
        {
            var mappingExpression = profile.CreateMap<TDto, TEntity>();

            var dtoType = typeof(TDto);
            var entityType = typeof(TEntity);
            //Ignore any property of source (like Post.Author) that dose not contains in destination 
            foreach (var property in entityType.GetProperties())
            {
                if (dtoType.GetProperty(property.Name) == null)
                    mappingExpression.ForMember(property.Name, opt => opt.Ignore());
            }

            CustomMappings(mappingExpression.ReverseMap());
        }

        public virtual void CustomMappings(IMappingExpression<TEntity, TDto> mapping)
        {
        }
    }

    public abstract class BaseDto<TDto, TEntity> : BaseDto<TDto, TEntity, int>
        where TDto : class, new()
        where TEntity : BaseEntity<int>
    {
    }
}
