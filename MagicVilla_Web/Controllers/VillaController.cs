using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto.Villa_DTO;
using MagicVilla_Web.Models.Dto.VillaDTO;
using MagicVilla_Web.Models.Dto.VillaDTO.VillaDTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();

            var response = await _villaService.GetAllAsync<APIResponce>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO villa)
        {

            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponce>(villa);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Created Successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error encountered";
            return View(villa);
        }
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponce>(villaId);
            if (response != null && response.IsSuccess)
            {
                VillaDTO villa = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));

                return View(_mapper.Map<VillaUpdateDTO>(villa));
            }
            TempData["error"] = "Error encountered";
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villa)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<VillaUpdateDTO>(villa);
                TempData["success"] = "Villa updated Successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Error encountered";
            return View(villa);

        }


        public async Task<IActionResult> DeleteVilla(int id)
        {
            var response = await _villaService.DeleteAsync<APIResponce>(id);
            TempData["success"] = "Villa deleted Successfully";
            return RedirectToAction(nameof(IndexVilla));
        }
    }
}
