using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;
using UnityEngine;

namespace Nuff.PetsAreSafe
{
    public class PetsAreSafeSettings : ModSettings
    {
        public enum SettingsPage
        {
            General_Settings,
            Exclude_List,
        }

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

        public static SettingsPage settingsPage = SettingsPage.General_Settings;
        public static WildOrFact wildOrFact = WildOrFact.Just_Your_Animals;
        public static PoofOrPlay poofOrPlay = PoofOrPlay.Play_Dead;

        public static bool animalsHeal; // if a hediff would kill an animal, it instead is removed
        public static bool animalsBridge; // if a hediff would kill an animal, it instead crosses the rainbow bridge
        public static bool noRaiders; // raiders will not spawn with animals
        public static bool noPredators; // predatory animals will not spawn
        public static bool noSlaughterer;
        public static bool noInsanity;
        public static bool noManhunterPack;


        #region ListControlSettings
        internal static List<ThingDef> allAnimals = new List<ThingDef>();
        internal static List<ThingDef> excludedAnimalsList = new List<ThingDef>();
        internal static HashSet<ThingDef> excludedAnimalsHash = new HashSet<ThingDef>();
        internal static List<string> animalsByDefName = new List<string>();
        internal static ThingDef selectedDef;

        internal string searchTerm = "";
        internal Vector2 leftScrollPosition = new Vector2();
        internal Vector2 rightScrollPosition = new Vector2();
        internal ThingDef leftSelectedObject = null;
        internal ThingDef rightSelectedObject = null;
        #endregion

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

            if (Scribe.mode == LoadSaveMode.Saving && !excludedAnimalsList.NullOrEmpty())
            {
                animalsByDefName = new List<string>();
                for (int i = 0; i < excludedAnimalsList.Count; i++)
                {
                    animalsByDefName.Add(excludedAnimalsList[i].defName);
                }
            }

            Scribe_Collections.Look(ref animalsByDefName, "animalsByDefName", LookMode.Value);

            if (Scribe.mode == LoadSaveMode.LoadingVars && animalsByDefName == null)
            {
                animalsByDefName = new List<string>();
            }

            base.ExposeData();
        }

        ///  <param name = "inRect" >
        internal void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard list = new Listing_Standard();
            list.Begin(inRect);
            Text.Font = GameFont.Medium;


            list.EnumSelector(ref settingsPage, "", "", "");

            if (settingsPage == SettingsPage.General_Settings)
            {
                list.Label("General Settings");
                Text.Font = GameFont.Small;
                list.Gap();

                list.Label("Which animals should be kept safe?");
                list.EnumSelector(ref wildOrFact, "", "", "");
                list.Label("Warning: protecting all animals may cause issues with raiders and predators.");
                list.Gap();

                list.Label("How should animals be protected?");
                list.EnumSelector(ref poofOrPlay, "", "", "");
                list.Gap();

                list.CheckboxLabeled("Disable Slaughterer mental break? (requires restart)", ref noSlaughterer);
                list.Gap();

                list.CheckboxLabeled("Disable single animal insanity event? (requires restart)", ref noInsanity);
                list.Gap();

                list.CheckboxLabeled("Disable manhunter pack event? (requires restart)", ref noManhunterPack);
                list.Gap();
            }
            else if (settingsPage == SettingsPage.Exclude_List)
            {
                list.Label("Animals to be excluded");
                Text.Font = GameFont.Small;
                list.Gap();

                list.ListControl(inRect, ref PetsAreSafeSettings.allAnimals, ref PetsAreSafeSettings.excludedAnimalsList, ref searchTerm, ref leftScrollPosition, ref rightScrollPosition,
                        ref leftSelectedObject, ref rightSelectedObject, "Animals to exclude", rectPCT: 0.85f);
            }


            list.End();
        }
    }
}