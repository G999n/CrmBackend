using CustomerRelationshipManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CustomerRelationshipManagement.Data;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly CrmDbContext _dbContext;

    public LoginController(JwtService jwtService, CrmDbContext dbContext)
    {
        _jwtService = jwtService;
        _dbContext = dbContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        // Find the user in the database
        var user = await _dbContext.Users
            .Where(u => u.Username == loginRequest.Username && u.IsActive)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return Unauthorized("Invalid username or account is inactive.");
        }

        // Directly compare the password
        if (loginRequest.Password != user.PasswordHash)
        {
            return Unauthorized("Invalid password.");
        }

        // Generate the token
        var token = _jwtService.GenerateToken(user.Username, user.Role);

        return Ok(new { Token = token });
    }

    [HttpGet("isLoggedIn")]
    //[Authorize(Roles = [])]  // Require authentication to access this endpoint
    public IActionResult IsLoggedIn()
    {
        // The token is valid if this method is called successfully
        return Ok(new { Message = "You are logged in!" });
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
