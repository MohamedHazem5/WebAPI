using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Dtos.Character
{
    public class AddCharacterRequestDto
    {
        public string Name { get; set; } = "Hamo Elgamed";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; }=10;
        public int Defense { get; set; }=10;
        public int Intelligence { get; set; }=10;

        public RgpClass Class {get;set;} = RgpClass.Knight;
    }
}