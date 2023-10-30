using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Nuff.PetsAreSafe
{
    [StaticConstructorOnStartup]
    public static class PASController
    {
        static PASController()
        {
            //Populate list of all animals - Dryads make this weird, so doing it by finding my comp
            PetsAreSafeSettings.allAnimals = DefDatabase<ThingDef>.AllDefsListForReading.Where(def => def.GetCompProperties<CompProperties_PetsAreSafe>() != null).ToList();

            //Populate excludedAnimals list by using the animalsByDefName string list to look up the ThingDefs
            PetsAreSafeSettings.excludedAnimalsList = RebuildThingDefList();

            UpdateExcludedHash();
        }

        internal static List<ThingDef> RebuildThingDefList()
        {
            Dictionary<string, ThingDef> defDict = new Dictionary<string, ThingDef>();
            List<ThingDef> thingDefs = new List<ThingDef>();

            if (PetsAreSafeSettings.animalsByDefName.NullOrEmpty())
            {
                return new List<ThingDef>();
            }

            for (int i = 0; i < PetsAreSafeSettings.allAnimals.Count; i++)
            {
                defDict[PetsAreSafeSettings.allAnimals[i].defName] = PetsAreSafeSettings.allAnimals[i];
            }

            for (int i = PetsAreSafeSettings.animalsByDefName.Count - 1; i >= 0; i--)
            {
                string defName = PetsAreSafeSettings.animalsByDefName[i];
                if (defDict.TryGetValue(defName, out ThingDef def) && def != null)
                {
                    thingDefs.Add(def);
                }
                else
                {
                    PetsAreSafeSettings.animalsByDefName.RemoveAt(i);
                }
            }

            return thingDefs;
        }

        internal static void UpdateExcludedHash()
        {
            PetsAreSafeSettings.excludedAnimalsHash = new HashSet<ThingDef>();

            if (PetsAreSafeSettings.excludedAnimalsList.NullOrEmpty())
            {
                PetsAreSafeSettings.excludedAnimalsList = new List<ThingDef>();
                return;
            }

            for (int i = 0; i < PetsAreSafeSettings.excludedAnimalsList.Count; i++)
            {
                PetsAreSafeSettings.excludedAnimalsHash.Add(PetsAreSafeSettings.excludedAnimalsList[i]);
            }
        }
    }
}
