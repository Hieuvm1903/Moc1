using System.Net;
using System.Runtime.CompilerServices;
using System.Linq.Dynamic.Core;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moc.DTO;
using Moc.Entities;
using Moc.Models;
using Moc.Repos;
using Moc.Data;
using System.Linq;

namespace Moc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class VillaController : ControllerBase
    {
        // private readonly ApplicationContext ac;
        //private readonly ILogging logger;
        protected APIResponse response;
        private readonly IVillaRepository villaRepository;
        private readonly IMapper mapper;
        public VillaController(IVillaRepository villaRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.villaRepository = villaRepository;
            response = new();
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("~/diff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ResponseCache]
        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery] string sortby = null
            ,string Name = null
            )
        {
            try
            {
                
                IEnumerable<Villa> villas = await villaRepository.GetAllAsync();
                response.Result = mapper.Map<List<VillaDTO>>(villas);
                response.StatusCode = HttpStatusCode.OK;
                var villa = villas.AsQueryable();

                if (Name != null)
                {
                    villa = villa.Where(s => s.Name == Name);
                }
                if (sortby!= null)
                {
                    try
                    {                       
                        return Ok(mapper.Map<List<VillaDTO>>(villa.OrderBy(sortby)));
                    }
                    catch
                    {
                        throw;

                    }
                }

               

                return Ok(mapper.Map<List<VillaDTO>>(villas));
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            // logger.Log("Get all villas", "");
            return response;
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [Authorize(Roles = "custom,admin")]       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    // logger.Log("Get Villa Error with id " + id,"error");
                    return BadRequest();
                }
                var villa = await villaRepository.GetAsync(s => s.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                response.Result = mapper.Map<VillaDTO>(villa);
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await villaRepository.GetAsync(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already existed!");
                return BadRequest(ModelState);
            }


            if (villaDTO == null)
            {
                return BadRequest(villaDTO);

            }
            if (villaDTO.ID > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa model = mapper.Map<Villa>(villaDTO);
            //Villa model = new()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id = villaDTO.ID,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft,
            //    CreatedDate = DateTime.Now
            //};
            model.CreatedDate = DateTime.Now;
            await villaRepository.CreateAsync(model);
            return
                CreatedAtRoute("GetVilla", new { id = villaDTO.ID }, villaDTO);

        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "custom,admin")]
        
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await villaRepository.GetAsync(s => s.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            await villaRepository.RemoveAsync(villa);
            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            Villa model = mapper.Map<Villa>(villaDTO);
            //Villa model = new()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id = villaDTO.ID,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft,
            //    UpdatedDate = DateTime.Now
            //};
            model.UpdatedDate = DateTime.Now;
            await villaRepository.UpdateAsync(model);
            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{id:int}", Name = "PatchVilla")]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await villaRepository.GetAsync(s => s.Id == id, false);

            if (villa == null)
            {
                return BadRequest();
            }
            VillaUpdateDTO villaDTO = mapper.Map<VillaUpdateDTO>(villa);

            //VillaDTO villaDTO = new()
            //{
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //    ID = villa.Id,
            //    ImageUrl = villa.ImageUrl,
            //    Name = villa.Name,
            //    Occupancy  = villa.Occupancy,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft           
            //};

            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = mapper.Map<Villa>(villaDTO);
            //Villa model = new()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id = villaDTO.ID,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft,
            //    UpdatedDate = DateTime.Now
            //};
            model.UpdatedDate = DateTime.Now;
            await villaRepository.UpdateAsync(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
