using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using HarmonyLib;

namespace Pets_are_Safe
{
    [StaticConstructorOnStartup]
    public partial class PetsAreSafe : Mod
    {
        /*
         * TODO FEATURE LIST
         * If an animal would be harmed, it instead disappears until the threat has passed.
         *      option to enable this on all animals, maybe non-pet animals don't come back
         *  OR: animals can be injured, but not killed, and their vital body parts can't be destroyed
         * Optional: if an animal would die from nonviolent means, it instead doesn't, and the relevant hediff is removed
         *  OR: "Crossing the rainbow bridge" event happens instead. Togglable
         * 
         * Option to disable animals from raids
         * Option to disable predators spawning on your map
         */

        PetsAreSafeSettings pasSettings;

        public PetsAreSafe(ModContentPack content) : base(content)
        {
            this.pasSettings = GetSettings<PetsAreSafeSettings>();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            //listingStandard.CheckboxLabeled("Change Player Faction Icon", ref IdeoFactIconSettings.changePlayerIcon, "Changes the player's faction settlement icon on the world map to match their primary ideoligion's icon.");
            //listingStandard.CheckboxLabeled("Change Player Faction Color", ref IdeoFactIconSettings.changePlayerIconColor, "Change the player's faction settlement icon color to match the primary ideoligion's color.");
            //listingStandard.CheckboxLabeled("Change Nonplayer Faction Icons", ref IdeoFactIconSettings.changeNonplayerIcons, "Changes all other factions' settlement icons to match their primary ideoligion's icon.");
            //listingStandard.CheckboxLabeled("Change Nonplayer Faction Colors", ref IdeoFactIconSettings.changeNonplayerColors, "Change all other factions' settlement icons to match their primary ideoligion's color.");
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }
    }

    public class PetsAreSafeSettings : ModSettings
    {
        public static bool petsPoof = true; // pets that are attacked disappear and then reappear when it's safe
        public static bool petsDown = false; // pets can be injured, but vital body parts are never destroyed and they don't bleed out
        public static bool wildSafe; // apply the above option to all animals, if either is active
        public static bool animalsHeal; // if a hediff would kill an animal, it instead is removed
        public static bool animalsBridge; // if a hediff would kill an animal, it instead crosses the rainbow bridge
        public static bool noRaiders; // raiders will not spawn with animals
        public static bool noPredators; // predatory animals will not spawn

        public override void ExposeData()
        {
            Scribe_Values.Look(ref petsPoof, "petsPoof");
            Scribe_Values.Look(ref petsDown, "petsDown");
            Scribe_Values.Look(ref wildSafe, "wildSafe");
            Scribe_Values.Look(ref animalsHeal, "animalsHeal");
            Scribe_Values.Look(ref animalsBridge, "animalsBridge");
            Scribe_Values.Look(ref noRaiders, "noRaiders");
            Scribe_Values.Look(ref noPredators, "noPredators");
            base.ExposeData();
        }
    }

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);
        static HarmonyPatches()
        {
            Harmony harmony = new Harmony(id: "rimWorld.nuff.petsaresafe");
            // TODO patches go here
            // here's an example from the other mod
            // harmony.Patch(AccessTools.Method(typeof(WorldInterface), nameof(WorldInterface.WorldInterfaceUpdate)), prefix: new HarmonyMethod(patchType, nameof(WorldInterfaceUpdatePrefix)));
        }

        public static void ExamplePrefix()
        {

        }
    }
}
