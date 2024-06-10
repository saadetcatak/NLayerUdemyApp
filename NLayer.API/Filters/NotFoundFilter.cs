﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T :BaseEntity
    {
        private readonly IService<T> _services;

        public NotFoundFilter(IService<T> services)
        {
            _services = services;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          var idValue=context.ActionArguments.Values.FirstOrDefault();
            if (idValue == null) 
            {
                await next.Invoke();
                return;
            }

            var id = (int)idValue;
            var anyEntity=await _services.AnyAsync(x=>x.Id==id);
            if (anyEntity == null) 
            {
                await next.Invoke();
            }

            context.Result=new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404,$"{typeof(T).Name} ({id}) not found"));
        }
    }
}
