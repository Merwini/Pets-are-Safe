using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace Nuff.PetsAreSafe
{
    public class CompPetsAreSafe : ThingComp
    {

        PetsAreSafeSettings settings = PetsAreSafe.pasSettings;

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            absorbed = false;
            bool flag;

            if (dinfo.Instigator != null)
            {
                Pawn pawn = dinfo.Instigator as Pawn;
                if (pawn != null)
                {
                    if (settings.wildOrFact == PetsAreSafeSettings.WildOrFact.All_Animals)
                    {
                        flag = DoAnEscape(parent);
                        absorbed = true;
                    }
                    else if (settings.wildOrFact == PetsAreSafeSettings.WildOrFact.Just_Your_Animals
                                                 && parent.Faction != null
                                                 && parent.Faction.IsPlayer)
                    {
                        flag = DoAnEscape(parent);
                        absorbed = true;
                    }
                }
            }
            return; 
        }

        public bool DoAnEscape(ThingWithComps thing)
        {
            bool success = false;

            if (settings.poofOrPlay == PetsAreSafeSettings.PoofOrPlay.Play_Dead)
            {
                success = TakeANap(thing);
            }
            else if (settings.poofOrPlay == PetsAreSafeSettings.PoofOrPlay.Poof_To_Safety)
            {
                success = DoAPoof(thing);
            }
            else
                success = true;
            return success;
        }

        public bool DoAPoof(ThingWithComps thing)
        {
            bool success = false;

            return success;
        }

        public bool TakeANap(ThingWithComps thingWithComps)
        {
            bool success = false;
            Pawn pawn = thingWithComps as Pawn;
            //pawn.health.AddHediff(HediffDefOf.Anesthetic);
            pawn.health.AddHediff(PAS_HediffDefOf.PASPlayDead);
            success = true;

            return success;
        }

    }
}