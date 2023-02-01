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

        public static bool animalsHeal; // if a hediff would kill an animal, it instead is removed
        public static bool animalsBridge; // if a hediff would kill an animal, it instead crosses the rainbow bridge
        public static bool noRaiders; // raiders will not spawn with animals
        public static bool noPredators; // predatory animals will not spawn
        public static bool noSlaughterer;
        public static bool noInsanity;
        public static bool noManhunterPack;

        public override void ExposeData()
        {
            //Scribe_Values.Look(ref petsPoof, "petsPoof");
            //Scribe_Values.Look(ref petsPlayDead, "petsPlayDead");
            //Scribe_Values.Look(ref wildSafe, "factSafe");
            //Scribe_Values.Look(ref wildSafe, "wildSafe");
            Scribe_Values.Look<WildOrFact>(ref wildOrFact, "wildOrFact", WildOrFact.Just_Your_Animals, true);
            Scribe_Values.Look<PoofOrPlay>(ref poofOrPlay, "poofOrPlay", PoofOrPlay.Play_Dead, true);
            Scribe_Values.Look(ref animalsHeal, "animalsHeal");
            Scribe_Values.Look(ref animalsBridge, "animalsBridge");
            Scribe_Values.Look(ref noRaiders, "noRaiders");
            Scribe_Values.Look(ref noPredators, "noPredators");
            Scribe_Values.Look(ref noSlaughterer, "noSlaughterer");
            Scribe_Values.Look(ref noInsanity, "noInsanity");
            Scribe_Values.Look(ref noManhunterPack, "noManhunterPack");


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

            listingStandard.CheckboxLabeled("Disable Slaughterer mental break? (requires restart)", ref noSlaughterer);
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("Disable single animal insanity event? (requires restart)", ref noInsanity);
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("Disable manhunter pack event? (requires restart)", ref noManhunterPack);
            listingStandard.Gap();

            listingStandard.End();
        }
    }
}