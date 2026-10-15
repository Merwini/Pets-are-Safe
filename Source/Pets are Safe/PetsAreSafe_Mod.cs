using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace Nuff.PetsAreSafe;

[StaticConstructorOnStartup]
public class PetsAreSafe_Mod : Mod
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

    public static PetsAreSafe_Settings Settings;

    public PetsAreSafe_Mod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<PetsAreSafe_Settings>();
    }

    public override string SettingsCategory()
    {
        return "PAS_Name".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard list = new Listing_Standard();
        list.Begin(inRect);
        Text.Font = GameFont.Medium;

        list.EnumSelector(ref Settings.settingsPage, "", "PAS_", "");

        if (Settings.settingsPage == PetsAreSafe_Settings.SettingsPage.General)
        {
            list.Label("PAS_General".Translate());
            Text.Font = GameFont.Small;
            list.Gap();

            list.Label("PAS_Settings1".Translate());
            list.EnumSelector(ref PetsAreSafe_Settings.animalsAffected, "", "PAS_");
            list.Label("PAS_Settings2".Translate());
            list.Gap();

            list.Label("PAS_Settings3".Translate());
            list.EnumSelector(ref PetsAreSafe_Settings.damageSources, "", "PAS_");
            list.Gap();

            list.Label("PAS_Settings4".Translate());
            list.EnumSelector(ref PetsAreSafe_Settings.safetyBehavior, "", "PAS_");
            list.Gap();

            list.CheckboxLabeled("PAS_DisableSlaughterer".Translate(), ref PetsAreSafe_Settings.disableSlaughtererBreak);
            list.Gap();

            list.CheckboxLabeled("PAS_DisableInsanity".Translate(), ref PetsAreSafe_Settings.disableInsanityIncident);
            list.Gap();

            list.CheckboxLabeled("PAS_DisableManhunterIncident".Translate(), ref PetsAreSafe_Settings.disableManhunterPackIncident);
            list.Gap();

            list.CheckboxLabeled("PAS_DisableManhunterOnFailedTame".Translate(), ref PetsAreSafe_Settings.disableManhunterOnFailedTame);
            list.Gap();
        }
        else if (Settings.settingsPage == PetsAreSafe_Settings.SettingsPage.ExcludeList)
        {
            list.Label("PAS_Settings5".Translate());
            Text.Font = GameFont.Small;
            list.Gap();

            list.ListControl(inRect, ref PetsAreSafe_Settings.allAnimals, ref PetsAreSafe_Settings.excludedAnimalsList, ref Settings.searchTerm, ref Settings.leftScrollPosition, ref Settings.rightScrollPosition,
                    ref Settings.leftSelectedObject, ref Settings.rightSelectedObject, "PAS_Settings5".Translate(), rectPCT: 0.85f);
        }

        list.End();
    }

    public override void WriteSettings()
    {
        PetsAreSafe_Controller.UpdateExcludedHash();
        base.WriteSettings();
    }
}
