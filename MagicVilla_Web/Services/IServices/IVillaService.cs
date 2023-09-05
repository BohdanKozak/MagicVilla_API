using MagicVilla_Web.Models.Dto.VillaDTO;
using MagicVilla_Web.Models.Dto.VillaDTO.VillaDTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> CreateAsync<T>(VillaCreateDTO dto);
        Task<T> UpdateAsync<T>(VillaUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);

        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);



    }
}
