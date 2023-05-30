using System.ComponentModel.DataAnnotations;

namespace MagicVillaApi.Models.Dto
{
    public class NumeroVillaUpdateDto
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string? DetalleEspecial { get; set; }
    }
}
