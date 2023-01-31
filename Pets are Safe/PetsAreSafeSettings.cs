using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;
using UnityEngine;
using PeteTimesSix.CompactHediffs.Rimworld.UI;

namespace Nuff.PetsAreSafe
{
    public class PetsAreSafeSettings : ModSettings
    {
        public enum WildOrFact
        {
            Just_Your_Animals,
            All_Animals,
        }

        public enum PoofOrPlay
        {
            Do_Nothing,
            Play_Dead,
            Poof_To_Safety,
        }

        public WildOrFact wildOrFact = WildOrFact.Just_Your_Animals;
        public PoofOrPlay poofOrPlay = PoofOrPlay.Play_Dead;

        //public static bool petsPoof = true; // pets that are attacked disappear and then reappear when it's safe
        //public static bool petsPlayDead = false; // pets can be injured, but vital body parts are never destroyed and they don't bleed out
        //public static bool factSafe = true;
        //public static bool wildSafe = false; // apply the above option to all animals, if either is active

        public static bool animalsHeal; // if a hediff would kill an animal, it instead is removed
        public static bool animalsBridge; // if a hediff would kill an animal, it instead crosses the rainbow bridge
        public static bool noRaiders; // raiders will not spawn with animals
        public static bool noPredators; // predatory animals will not spawn

        public override void ExposeData()
        {
            //Scribe_Values.Look(ref petsPoof, "petsPoof");
            //Scribe_Values.Look(ref petsPlayDead, "petsPlayDead");
            //Scribe_Values.Look(ref wildSafe, "factSafe");
            //Scribe_Values.Look(ref wildSafe, "wildSafe");
            Scribe_Values.Look(ref animalsHeal, "animalsHeal");
            Scribe_Values.Look(ref animalsBridge, "animalsBridge");
            Scribe_Values.Look(ref noRaiders, "noRaiders");
            Scribe_Values.Look(ref noPredators, "noPredators");
            base.ExposeData();
        }

        ///  <param name = "inRect" >
        internal void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("Which animals should be kept safe?");
            listingStandard.EnumSelector(ref wildOrFact, "", "", "");
            listingStandard.Label("Warning: protecting all animals may cause issues with raiders and predators.");
            listingStandard.Gap();

            listingStandard.Label("How should animals be protected?");
            listingStandard.EnumSelector(ref poofOrPlay, "", "", "");
            listingStandard.Gap();

            listingStandard.End();
        }
    }
}