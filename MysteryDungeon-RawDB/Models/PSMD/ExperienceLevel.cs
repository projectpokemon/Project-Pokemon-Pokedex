using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.PSMD
{
    public class ExperienceLevel
    {
        public int ID { get; set; }
        public int ExperienceTableNumber { get; set; }
        public int Level { get; set; }
        public uint Exp { get; set; }
        public int AddedHP { get; set; }
        public int AddedAttack { get; set; }
        public int AddedSpAttack { get; set; }
        public int AddedDefense { get; set; }
        public int AddedSpDefense { get; set; }
        public int AddedSpeed { get; set; }
    }
}
