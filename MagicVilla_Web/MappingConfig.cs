using AutoMapper;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.Dto.Villa_DTO;
using MagicVilla_Web.Models.Dto.Villa_NumberDTO;
using MagicVilla_Web.Models.Dto.VillaDTO;
using MagicVilla_Web.Models.Dto.VillaDTO.VillaDTO;


namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();


        }
    }
}
