using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto.Villa_DTO;
using MagicVilla_VillaAPI.Models.Dto.VillaDTO;
using MagicVilla_VillaAPI.Models.Dto.VillaDTO.VillaDTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    //[Route("api/[controller]")]
    [ApiController]

    public class VillaAPIController : ControllerBase
    {
        protected APIResponce _responce;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;

        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            _dbVilla = dbVilla;
            _mapper = mapper;
            this._responce = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponce>> GetAllVillas()
        {
            try
            {
                IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
                _responce.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _responce.StatusCode = HttpStatusCode.OK;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages
                    = new List<string>() { ex.ToString() };

            }

            return _responce;
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponce>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }
                _responce.Result = _mapper.Map<VillaDTO>(villa);
                _responce.StatusCode = HttpStatusCode.OK;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages
                    = new List<string>() { ex.ToString() };

            }
            return _responce;


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponce>> CreteVilla([FromBody] VillaCreateDTO createDTO)
        {

            try
            {
                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa already Exists");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {

                    return BadRequest(createDTO);
                }
                Villa villa = _mapper.Map<Villa>(createDTO);
                await _dbVilla.CreateAsync(villa);
                _responce.Result = _mapper.Map<VillaDTO>(villa);
                _responce.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _responce);
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages
                    = new List<string>() { ex.ToString() };

            }
            return _responce;


        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<APIResponce>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var VillaToBeDeleted = await _dbVilla.GetAsync(u => u.Id == id);
                if (VillaToBeDeleted == null)
                {
                    return NotFound();
                }

                await _dbVilla.RemoveAsync(VillaToBeDeleted);
                _responce.StatusCode = HttpStatusCode.NoContent;
                _responce.IsSuccess = true;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages
                    = new List<string>() { ex.ToString() };

            }
            return _responce;
        }



        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponce>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                Villa model = _mapper.Map<Villa>(updateDTO);
                await _dbVilla.UpdateAsync(model);
                _responce.StatusCode = HttpStatusCode.NoContent;
                _responce.IsSuccess = true;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.IsSuccess = false;
                _responce.ErrorMessages
                    = new List<string>() { ex.ToString() };

            }
            return _responce;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);


            if (villaDTO == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = _mapper.Map<Villa>(villaDTO);

            await _dbVilla.UpdateAsync(model);


            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
