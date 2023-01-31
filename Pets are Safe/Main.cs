using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using HarmonyLib;
using PeteTimesSix.CompactHediffs.Rimworld.UI;

namespace Nuff.PetsAreSafe
{
    [StaticConstructorOnStartup]
    public class PetsAreSafe : Mod
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

        public static PetsAreSafeSettings pasSettings;
        public PetsAreSafe(ModContentPack content) : base(content)
        {
            pasSettings = GetSettings<PetsAreSafeSettings>();
        }

        public override string SettingsCategory()
        {
            return "Pets Are Safe";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            pasSettings.DoSettingsWindowContents(inRect);
            base.DoSettingsWindowContents(inRect);
        }
    }
}
