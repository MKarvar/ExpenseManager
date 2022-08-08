using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

public class HttpContextService : IHttpContextService
{
    private IHttpContextAccessor _context;

    public HttpContextService(IHttpContextAccessor context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public int GetUserId()
    {
        return int.Parse(_context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

    }

    public string GetUserName()
    {
        return _context.HttpContext.User.Identity.Name;
    }
}