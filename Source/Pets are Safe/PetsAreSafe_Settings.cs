using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;
using UnityEngine;

namespace Nuff.PetsAreSafe;

public class PetsAreSafe_Settings : ModSettings
{
    public enum SettingsPage
    {
        General,
        ExcludeList,
    }

    public enum AnimalsAffected
    {
        YourAnimals,
        AllAnimals,
        SpecificAnimals
    }

    public enum DamageBlocked
    {
        AllSources,
        PawnSources
    }

    public enum SafetyBehavior
    {
        Ignore,
        PlayDead,
        PoofToSafety,
    }

    public SettingsPage settingsPage = SettingsPage.General;
    public static AnimalsAffected animalsAffected = AnimalsAffected.YourAnimals;
    public static SafetyBehavior safetyBehavior = SafetyBehavior.PlayDead;
    public static DamageBlocked damageSources = DamageBlocked.AllSources;

    //public static bool animalsHeal; // if a hediff would kill an animal, it instead is removed
    //public static bool animalsBridge; // if a hediff would kill an animal, it instead crosses the rainbow bridge
    //public static bool noRaiders; // raiders will not spawn with animals
    //public static bool noPredators; // predatory animals will not spawn
    public static bool disableSlaughtererBreak;
    public static bool disableInsanityIncidents;
    public static bool disableManhunterPackIncident;
    public static bool disableAnimalDiseaseIncidents;
    public static bool disableManhunterOnFailedTame;


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
        Scribe_Values.Look(ref animalsAffected, "animalsAffected", AnimalsAffected.YourAnimals, true);
        Scribe_Values.Look(ref safetyBehavior, "poofOrPlay", SafetyBehavior.PlayDead, true);
        Scribe_Values.Look(ref damageSources, "allDamageOrEnemies", DamageBlocked.AllSources, true);
        //Scribe_Values.Look(ref animalsHeal, "animalsHeal");
        //Scribe_Values.Look(ref animalsBridge, "animalsBridge");
        //Scribe_Values.Look(ref noRaiders, "noRaiders");
        //Scribe_Values.Look(ref noPredators, "noPredators");
        Scribe_Values.Look(ref disableSlaughtererBreak, "disableSlaughtererBreak");
        Scribe_Values.Look(ref disableInsanityIncidents, "disableInsanityIncidents");
        Scribe_Values.Look(ref disableAnimalDiseaseIncidents, "disableAnimalDiseaseIncidents");
        Scribe_Values.Look(ref disableManhunterPackIncident, "disableManhunterPackIncident");
        Scribe_Values.Look(ref disableManhunterOnFailedTame, "disableManhunterOnFailedTame");

        if (Scribe.mode == LoadSaveMode.Saving && excludedAnimalsList != null)
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
}
