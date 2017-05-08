using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryDungeon_RawDB.Models.EOS
{
    public class Pokemon
    {
        public class GenderInfo {
            public int Unk_02 { get; set; }
            public int Unk_06 { get; set; }
            public int EvolveFrom { get; set; }
            public int EvolveMethod { get; set; }
            public int EvolveParam { get; set; }
            public string EvolveItemName { get; set; }
            public int SpriteIndex { get; set; }
            public string GenderName { get; set; }
            public int BodySize { get; set; }
            public int MainType { get; set; }
            public int AltType { get; set; }
            public string MovementTypeName { get; set; }
            public int IqGroup { get; set; }
            public int Ability1 { get; set; }
            public int Ability2 { get; set; }
            public int Unk_1a { get; set; }
            public int ExpYield { get; set; }
            public int RecruitRate { get; set; }
            public int BaseHP { get; set; }
            public int RecruitRate2 { get; set; }
            public int BaseATK { get; set; }
            public int BaseSPATK { get; set; }
            public int BaseDEF { get; set; }
            public int BaseSPDEF { get; set; }
            public int Weight { get; set; }
            public int Size { get; set; }
            public int Unk_29 { get; set; }
            public int BaseFormIndex { get; set; }
            public int ExclusiveItem1 { get; set; }
            public int ExclusiveItem2 { get; set; }
            public int ExclusiveItem3 { get; set; }
            public int ExclusiveItem4 { get; set; }
            public int Unk3C { get; set; }
            public int Unk3E { get; set; }
            public int Unk40 { get; set; }
            public int Unk42 { get; set; }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int DexNumber { get; set; }

        public GenderInfo Male { get; set; }
        public GenderInfo Female { get; set; }

    }
}
