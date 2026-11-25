using Microsoft.EntityFrameworkCore;
using GameApi.Data;
using GameApi.DTOs;
using GameApi.Models;

namespace GameApi.Services
{
    public interface ICharacterService
    {
        Task<CharacterResponse> CreateCharacterAsync(int accountId, CreateCharacterRequest request);
        Task<CharacterResponse?> UpdateCharacterAsync(int accountId, int characterId, UpdateCharacterRequest request);
        Task<List<CharacterResponse>> GetCharactersByAccountAsync(int accountId);
        Task<CharacterResponse?> GetCharacterByIdAsync(int accountId, int characterId);
    }

    public class CharacterService : ICharacterService
    {
        private readonly AppDbContext _context;

        public CharacterService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CharacterResponse> CreateCharacterAsync(int accountId, CreateCharacterRequest request)
        {
            var character = new Character
            {
                Name = request.Name,
                Class = request.Class,
                Level = 1,
                Hp = 100,
                CreatedAt = DateTime.UtcNow,
                AccountId = accountId
            };

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            return MapToResponse(character);
        }

        public async Task<CharacterResponse?> UpdateCharacterAsync(int accountId, int characterId, UpdateCharacterRequest request)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId);
            if (character == null || character.AccountId != accountId)
            {
                return null;
            }

            character.Name = request.Name;
            character.Class = request.Class;
            character.Level = request.Level ?? character.Level;
            character.Hp = request.Hp ?? character.Hp;

            await _context.SaveChangesAsync();

            return MapToResponse(character);
        }

        public async Task<List<CharacterResponse>> GetCharactersByAccountAsync(int accountId)
        {
            var chars = await _context.Characters
                .Where(c => c.AccountId == accountId)
                .ToListAsync();

            return chars.Select(MapToResponse).ToList();
        }

        public async Task<CharacterResponse?> GetCharacterByIdAsync(int accountId, int characterId)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId);
            if (character == null || character.AccountId != accountId)
            {
                return null;
            }

            return MapToResponse(character);
        }

        private static CharacterResponse MapToResponse(Character character)
        {
            return new CharacterResponse
            {
                Id = character.Id,
                Name = character.Name,
                Class = character.Class,
                Level = character.Level,
                Hp = character.Hp,
                AccountId = character.AccountId
            };
        }
    }
}
