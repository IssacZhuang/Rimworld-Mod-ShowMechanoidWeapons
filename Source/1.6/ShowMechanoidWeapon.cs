using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;

namespace ShowMechanoidWeapon
{

    public class ShowMechanoidWeapon : Mod
    {
        public ShowMechanoidWeapon(ModContentPack content) : base(content)
        {
            LongEventHandler.QueueLongEvent(ShowMechanoidWeapon.InjectHyperLink, "SMW_InjectHyperLink", false, null, true);
        }

        public static void InjectHyperLink()
        {
            IEnumerable<PawnKindDef> mechs = DefDatabase<PawnKindDef>.AllDefsListForReading.AsParallel().Where(x => x.race?.race?.FleshType == FleshTypeDefOf.Mechanoid);
            foreach (var mech in mechs)
            {
                ThingDef race = mech.race;
                if (race.descriptionHyperlinks == null) race.descriptionHyperlinks = new List<DefHyperlink>();

                CompProperties_TurretGun comp = (CompProperties_TurretGun)race.comps?.FirstOrDefault(x => x is CompProperties_TurretGun);
                if (comp?.turretDef != null) race.descriptionHyperlinks.Add(new DefHyperlink { def = comp.turretDef });

                if (mech.weaponTags.NullOrEmpty()) continue;

                IEnumerable<ThingDef> mechWeapons = DefDatabase<ThingDef>.AllDefsListForReading.AsParallel().Where(x => x.weaponTags != null && x.weaponTags.Contains(mech.weaponTags.First()));

                foreach(ThingDef weapon in mechWeapons)
                {
                    race.descriptionHyperlinks.Add(new DefHyperlink
                    {
                        def = weapon
                    });
                }

               
            }
        }
    }
}
