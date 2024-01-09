using System.Security.Claims;

namespace api;

public static class ClaimsPrincipalExtensions
{
  public static string? GetUsername(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}