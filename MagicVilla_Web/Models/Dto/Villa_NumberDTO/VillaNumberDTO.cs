using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto.Villa_NumberDTO
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set; }

        public Villa_DTO.VillaDTO Villa { get; set; }
    }
}
