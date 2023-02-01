using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Nuff.PetsAreSafe
{
    [StaticConstructorOnStartup]
    public static class DefChecks
    {
        static DefChecks()
        {
            if (PetsAreSafeSettings.noSlaughterer)
            {
                MentalBreakDef slaughterer = PAS_MentalBreakDefOf.Slaughterer;
                slaughterer.baseCommonality = 0;
            }

            if (PetsAreSafeSettings.noInsanity)
            {
                IncidentDef insanity = PAS_IncidentDefOf.AnimalInsanitySingle;
                insanity.baseChance = 0;
                insanity.baseChanceWithRoyalty = 0;
            }

            if (PetsAreSafeSettings.noManhunterPack)
            {
                IncidentDef manhunterPack = IncidentDefOf.ManhunterPack;
                manhunterPack.baseChance = 0;
                manhunterPack.baseChanceWithRoyalty = 0;
            }
        }
    }

}
