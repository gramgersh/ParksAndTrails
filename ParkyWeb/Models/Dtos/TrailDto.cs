using System;
using System.ComponentModel.DataAnnotations;
using static ParkyWeb.Models.Trail;

namespace ParkyWeb.Models.Dtos
{
    public class TrailDto
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }
        public enum DifficultyType { Easy, Moderate, Difficult, Expert }

        public DifficultyType Difficulty { get; set; }

        public int NationalParkId { get; set; }

        public NationalPark NationalPark { get; set; }
    }
}