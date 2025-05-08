using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserAuthAPI.Data;
using UserAuthAPI.DTOs;
using UserAuthAPI.Models;
using UserAuthAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace UserAuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;

    public UserController(AppDbContext context, IMapper mapper, TokenService tokenService)
    {
        _context = context;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public ActionResult Register(UserRegisterDTO dto)
    {
        if (_context.Users.Any(u => u.Username == dto.Username))
            return BadRequest("Usuário já existe");

        var user = _mapper.Map<User>(dto);

        _context.Users.Add(user);
        _context.SaveChanges();

        return CreatedAtAction("GetById", new { id = user.Id }, user);
    }

    [HttpPost("login")]
    public ActionResult Login(UserLoginDTO dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
        if (user == null)
            return Unauthorized("Usuário inválido");

        bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!validPassword)
            return Unauthorized("Senha inválida");

        var token = _tokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
    {
        var users = await _context.Users.ToListAsync();
        if (users == null)
            return NotFound("Usuários não encontrados");
        var usersDto = _mapper.Map<IEnumerable<UserDTO>>(users);
        return Ok(usersDto);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserDTO>> GetById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound("Usuário não encontrado");
        var userDto = _mapper.Map<UserDTO>(user);
        return Ok(userDto);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<UserDTO>> UpdateUser(int id, UserDTO userDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound("Usuário não encontrado");

        _mapper.Map(userDto, user);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound($"Usuário não encontrado com o id {id}");

        _context.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
