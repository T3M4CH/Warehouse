using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Warehouse.DTOs;
using Warehouse.Token.Interfaces;
using WarehouseApi.Entities;

namespace Warehouse.Controller;

[AllowAnonymous]
[Route("v1/users")]
public class UserApiController : BaseApiController
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly ITokenService _tokenService;

    public UserApiController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost("create"), AllowAnonymous]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new UserEntity()
        {
            UserName = registerDto.UserName,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        foreach (var identityError in result.Errors)
        {
            return BadRequest(identityError.Description);
        }

        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<ActionResult> Login(RegisterDto registerDto)
    {
        var user = await _userManager.FindByNameAsync(registerDto.UserName);

        if (user != null)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, registerDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Password Wrong");

            var token = _tokenService.CreateToken(user);

            return Ok(new { Token = token });
        }

        return BadRequest("User not found");
    }
}