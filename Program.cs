using Database;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Responses;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserProfileContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var users = app.MapGroup("/users");

users.MapGet("/", async (IUserService _userService) =>
{
    var result = new CommonResponse();
    try {
        var data = await _userService.GetAllUsers();
        result.Code = 200;
        result.Message = "Success Get User List";
        result.Data = data;
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        result.Code = 400;
        result.Message = $"Failed to get user";
        result.Data = ex.Message;
        return Results.BadRequest(result);
    }
});

users.MapGet("/{id}", async (IUserService _userService, int id) =>
{
    var result = new CommonResponse();
    try {
        var data = await CheckUser(id, _userService);
        if (data == null)
        {
            result.Code = StatusCodes.Status404NotFound;
            result.Message = "User Not Found";
            result.Data = MessageNotFound(id);
            return Results.NotFound(result);
        }
        result.Code = 200;
        result.Message = "Success Get User Detail";
        result.Data = data;
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        result.Code = 400;
        result.Message = $"Failed to get user";
        result.Data = ex.Message;
        return Results.BadRequest(result);
    }    
});

users.MapPost("/", async (IUserService _userService, [FromBody] UserModel model) =>
{
    var result = new CommonResponse();
    try {
        var data = await _userService.CreateUser(model);
        result.Code = 200;
        result.Message = $"Success Added User {model.UserName}";
        result.Data = data;
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        result.Code = 400;
        result.Message = $"Failed to add user";
        result.Data = ex.Message;
        return Results.BadRequest(result);
    }    
});

users.MapPut("/", async (IUserService _userService, [FromBody] UserModel model) =>
{
    var result = new CommonResponse();
    try {
        var checkUser = await CheckUser(model.Id, _userService);
        if (checkUser == null)
        {
            result.Code = StatusCodes.Status404NotFound;
            result.Message = "User Not Found";
            result.Data = MessageNotFound(model.Id);
            return Results.NotFound(result);
        }
        var data = await _userService.UpdateUser(model);
        result.Code = 200;
        result.Message = $"Success Updated User {model.UserName}";
        result.Data = data;
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        result.Code = 400;
        result.Message = $"Failed to update user";
        result.Data = ex.Message;
        return Results.BadRequest(result);
    }    
});

users.MapDelete("/{id}", async (IUserService _userService, int id) =>
{
    var result = new CommonResponse();
    try {
        var checkUser = await CheckUser(id, _userService);
        if (checkUser == null)
        {
            result.Code = StatusCodes.Status404NotFound;
            result.Message = "User Not Found";
            result.Data = MessageNotFound(id);
            return Results.NotFound(result);
        }
        var data = await _userService.DeleteUser(id);
        result.Code = 200;
        result.Message = $"Success Deleted User";
        result.Data = data;
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        result.Code = 400;
        result.Message = $"Failed to delete user";
        result.Data = ex.Message;
        return Results.BadRequest(result);
    }    
});

static async Task<UserModel?> CheckUser(int id, IUserService _userService)
{
    var data = await _userService.GetUserDetail(id);
    return data;
}

static string MessageNotFound(int id)
{
    return $"User with id {id} not found";
}

app.Run();
