using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Models.Dto.Villa_NumberDTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IMapper _mapper;
        protected APIResponce _responce;
        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IMapper mapper)
        {
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            _responce = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponce>> GetAllVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumbersList = await _dbVillaNumber.GetAllAsync();
                _responce.StatusCode = HttpStatusCode.OK;
                _responce.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumbersList);
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.ErrorMessages = new List<string>() { ex.ToString() };
                _responce.IsSuccess = false;
            }

            return _responce;
        }
        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponce>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(c => c.VillaNo == id);
                if (villaNumber == null)
                {
                    _responce.StatusCode = HttpStatusCode.NotFound;

                    return NotFound(_responce);
                }
                _responce.StatusCode = HttpStatusCode.OK;
                _responce.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.ErrorMessages = new List<string>() { ex.ToString() };
                _responce.IsSuccess = false;

            }

            return _responce;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponce>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumber)
        {
            try
            {
                if (villaNumber.VillaNo == 0)
                {
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.Result = villaNumber;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }
                if (await _dbVillaNumber.GetAsync(u => u.VillaNo == villaNumber.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomError", "VillaNumber already Exists");
                    return BadRequest(ModelState);
                }
                if (villaNumber == null)
                {
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.Result = villaNumber;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }

                var villaNum = _mapper.Map<VillaNumber>(villaNumber);

                await _dbVillaNumber.CreateAsync(villaNum);
                _responce.Result = villaNum;
                _responce.IsSuccess = true;
                _responce.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber", new { id = villaNum.VillaNo }, _responce);
            }
            catch (Exception ex)
            {
                _responce.ErrorMessages = new List<string>() { ex.ToString() };

                _responce.IsSuccess = false;
            }
            return _responce;
        }

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponce>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }

                var villaNumToBeDeleted = await _dbVillaNumber.GetAsync(c => c.VillaNo == id);
                if (villaNumToBeDeleted == null)
                {
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    _responce.IsSuccess = false;
                    return NotFound(_responce);
                }

                await _dbVillaNumber.RemoveAsync(villaNumToBeDeleted);
                _responce.IsSuccess = true;
                _responce.StatusCode = HttpStatusCode.NoContent;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.ErrorMessages = new List<string>() { ex.ToString() };
                _responce.IsSuccess = false;
            }
            return _responce;
        }

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponce>> UpdateVilla(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO.VillaNo != id || updateDTO == null)
                {
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }

                var model = _mapper.Map<VillaNumber>(updateDTO);
                await _dbVillaNumber.UpdateAsync(model);
                _responce.IsSuccess = true;
                _responce.StatusCode = HttpStatusCode.NoContent;
                return Ok(_responce);
            }
            catch (Exception ex)
            {

                _responce.ErrorMessages = new List<string>() { ex.ToString() };
                _responce.IsSuccess = false;
            }

            return _responce;


        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVillaNum")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<APIResponce>> UpdatePartialVilla(int id,
            JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            try
            {
                if (id == 0 || patchDTO == null)
                {
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }

                var vilNum = await _dbVillaNumber.GetAsync(c => c.VillaNo == id, tracked: false);
                VillaNumberUpdateDTO numToUpdate = _mapper.Map<VillaNumberUpdateDTO>(vilNum);
                if (numToUpdate == null)
                {
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }

                patchDTO.ApplyTo(numToUpdate, ModelState);
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(numToUpdate);
                await _dbVillaNumber.UpdateAsync(villaNumber);


                if (!ModelState.IsValid)
                {
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }

                _responce.StatusCode = HttpStatusCode.NoContent;
                _responce.IsSuccess = true;

                return Ok(_responce);

            }
            catch (Exception ex)
            {
                _responce.ErrorMessages = new List<string>() { ex.ToString() };
                _responce.IsSuccess = false;
            }

            return _responce;
        }
    }
}
