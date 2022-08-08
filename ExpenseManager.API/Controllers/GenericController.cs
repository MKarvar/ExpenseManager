using AutoMapper.QueryableExtensions;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos;
using ExpenseManager.Domain.SeedWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebUtilities;

namespace ExpenseManager.API.Controllers
{
    [ApiVersion("1")]
    public class GenericController<TDto, TEntity, TKey> : BaseApiController
       where TDto : BaseDto<TDto, TEntity, TKey>, new()
       where TEntity : BaseEntity<TKey>, IAggregateRoot
    {
        private readonly IRepository<TEntity> _repository;

        public GenericController(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public virtual async Task<ActionResult<List<TDto>>> Get(CancellationToken cancellationToken)
        {
            var list = await _repository.TableNoTracking.ProjectTo<TDto>()
                .ToListAsync(cancellationToken);

            return Ok(list);
        }

        [HttpGet("{id}")]
        public virtual async Task<ApiResult<TDto>> Get(TKey id, CancellationToken cancellationToken)
        {
            var dto = await _repository.TableNoTracking.ProjectTo<TDto>()
                .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }

        [HttpDelete("{id:guid}")]
        public virtual async Task<ApiResult> Delete(TKey id, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(cancellationToken, id);

            await _repository.DeleteAsync(model, cancellationToken);

            return Ok();
        }
    }

    public class CrudController<TDto, TEntity> : GenericController<TDto, TEntity, int>
         where TDto : BaseDto<TDto, TEntity, int>, new()
       where TEntity : BaseEntity<int>, IAggregateRoot
    {
        public CrudController(IRepository<TEntity> repository)
            : base(repository)
        {
        }
    }
}
