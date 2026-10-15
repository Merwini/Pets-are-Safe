using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Nuff.PetsAreSafe;

[StaticConstructorOnStartup]
public static class PetsAreSafe_Controller
{
    static PetsAreSafe_Controller()
    {
        //Populate list of all animals - Dryads make this weird, so doing it by finding my comp
        PetsAreSafe_Settings.allAnimals = DefDatabase<ThingDef>.AllDefsListForReading.Where(def => def.GetCompProperties<CompProperties_PetsAreSafe>() != null).ToList();

        //Populate excludedAnimals list by using the animalsByDefName string list to look up the ThingDefs
        PetsAreSafe_Settings.excludedAnimalsList = RebuildThingDefList();

        UpdateExcludedHash();
    }

    internal static List<ThingDef> RebuildThingDefList()
    {
        Dictionary<string, ThingDef> defDict = new Dictionary<string, ThingDef>();
        List<ThingDef> thingDefs = new List<ThingDef>();

        if (PetsAreSafe_Settings.animalsByDefName.NullOrEmpty())
        {
            return new List<ThingDef>();
        }

        for (int i = 0; i < PetsAreSafe_Settings.allAnimals.Count; i++)
        {
            defDict[PetsAreSafe_Settings.allAnimals[i].defName] = PetsAreSafe_Settings.allAnimals[i];
        }

        for (int i = PetsAreSafe_Settings.animalsByDefName.Count - 1; i >= 0; i--)
        {
            string defName = PetsAreSafe_Settings.animalsByDefName[i];
            if (defDict.TryGetValue(defName, out ThingDef def) && def != null)
            {
                thingDefs.Add(def);
            }
            else
            {
                PetsAreSafe_Settings.animalsByDefName.RemoveAt(i);
            }
        }

        return thingDefs;
    }

    internal static void UpdateExcludedHash()
    {
        PetsAreSafe_Settings.excludedAnimalsHash = new HashSet<ThingDef>();

        if (PetsAreSafe_Settings.excludedAnimalsList.NullOrEmpty())
        {
            PetsAreSafe_Settings.excludedAnimalsList = new List<ThingDef>();
            return;
        }

        for (int i = 0; i < PetsAreSafe_Settings.excludedAnimalsList.Count; i++)
        {
            PetsAreSafe_Settings.excludedAnimalsHash.Add(PetsAreSafe_Settings.excludedAnimalsList[i]);
        }
    }
}
