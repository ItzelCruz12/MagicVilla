using System.ComponentModel.DataAnnotations;

namespace MagicVillaApi.Models.Dto
{
    public class VillaUpdateDto
    {
        [Required]
        public int Id { get; set; }
        //////El required es un DataAnnotation
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        [Required]
        public double Tarifa { get; set; }
        [Required]
        public int Ocupantes { get; set; }
        [Required]
        public int MetrosCuadrados { get; set; }
        [Required]
        public string ImagenUrl { get; set; }
        [Required]
        public string Amenidad { get; set; }
    }
}
