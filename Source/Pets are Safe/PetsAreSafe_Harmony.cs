using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;
using Verse.AI;
using UnityEngine;
using HarmonyLib;

namespace Nuff.PetsAreSafe;

[StaticConstructorOnStartup]
public class PetsAreSafe_Harmony
{
    static PetsAreSafe_Harmony()
    {
        Harmony harmony = new Harmony("nuff.rimworld.petsaresafe");
        harmony.PatchAll();
    }

    [HarmonyPatch(typeof(MentalStateWorker_Slaughterer), nameof(MentalStateWorker_Slaughterer.StateCanOccur))]
    public static class MentalStateWorker_Slaughterer_SlaughterTargetAvailable_Patch
    {
        public static bool Prefix(ref bool __result)
        {
            if (PetsAreSafe_Settings.disableSlaughtererBreak)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(IncidentWorker), nameof(IncidentWorker.CanFireNow))]
    public static class IncidentWorker_CanFireNow_Patch
    {
        public static bool Prefix(IncidentWorker __instance, ref bool __result)
        {
            if (__instance is IncidentWorker_AggressiveAnimals && PetsAreSafe_Settings.disableManhunterPackIncident)
            {
                __result = false;
                return false;
            }

            if ((__instance is IncidentWorker_AnimalInsanityMass || __instance is IncidentWorker_AnimalInsanitySingle) && PetsAreSafe_Settings.disableInsanityIncident)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(PawnUtility), nameof(PawnUtility.GetManhunterOnTameFailChance), new Type[] { typeof(Pawn) })]
    public static class PawnUtility_GetManhunterOnTameFailChance_Patch
    {
        public static void Postfix(ref float __result)
        {
            if (PetsAreSafe_Settings.disableManhunterOnFailedTame)
            {
                __result = 0;
            }
        }
    }
}
