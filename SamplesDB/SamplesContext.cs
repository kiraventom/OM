using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace SamplesDB
{
    public class SamplesContext : DbContext
    {
        public DbSet<Sample> Samples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite("Data Source=samples.db");
    }

    public class Sample
    {
        [Key]
        public long Id { get; set; }
        public double ReactionMassConsumption { get; set; } // расход реакционной массы, кг/ч
        public double ReactorPressure { get; set; } // давление в реакторе, Кпа
        public int HeatExchangersAmount { get; set; } // количество теполобменных устройств
    }
}
