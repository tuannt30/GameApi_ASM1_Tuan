using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameApi.DTOs;
using GameApi.Services;

namespace GameApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/characters")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        private int GetAccountId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<IActionResult> GetCharacters()
        {
            var accountId = GetAccountId();
            var chars = await _characterService.GetCharactersByAccountAsync(accountId);
            return Ok(chars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacterById(int id)
        {
            var accountId = GetAccountId();
            var character = await _characterService.GetCharacterByIdAsync(accountId, id);
            if (character == null)
            {
                return NotFound(new { message = "Không tìm thấy nhân vật" });
            }
            return Ok(character);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacter([FromBody] CreateCharacterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountId = GetAccountId();
            var character = await _characterService.CreateCharacterAsync(accountId, request);
            return StatusCode(StatusCodes.Status201Created, character);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacter(int id, [FromBody] UpdateCharacterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountId = GetAccountId();
            var character = await _characterService.UpdateCharacterAsync(accountId, id, request);
            if (character == null)
            {
                return NotFound(new { message = "Không tìm thấy nhân vật hoặc không có quyền" });
            }

            return Ok(character);
        }
    }
}
