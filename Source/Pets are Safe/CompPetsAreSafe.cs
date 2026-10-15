using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace Nuff.PetsAreSafe;

public class CompPetsAreSafe : ThingComp
{
    public CompProperties_PetsAreSafe Props => (CompProperties_PetsAreSafe)props;

    const int escapeRadius = 10;
    const int hostileClearanceSquared = 25;

    private bool protect = false;
    
    public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
    {
        absorbed = false;

        if (ShouldAbsorb(dinfo))
        {
            absorbed = true;
            DoAnEscape();
        }
    }

    public bool ShouldAbsorb(DamageInfo dinfo)
    {
        // Animal is designated for slaughter, and being slaughtered
        if (parent is Pawn parentPawn
            && parentPawn.ShouldBeSlaughtered()
            && dinfo.defInt == DamageDefOf.ExecutionCut)
        {
            return false;
        }

        // Damage source shouldn't be blocked
        if (!ShouldAbsorbSource(dinfo))
            return false;

        // Currently set to protect specific animals, return based on current toggle
        if (PetsAreSafe_Settings.animalsAffected == PetsAreSafe_Settings.AnimalsAffected.SpecificAnimals)
            return protect;
        
        // This type of animal is excluded from protection
        if (PetsAreSafe_Settings.excludedAnimalsHash.Contains(parent.def))
        {
            return false;
        }

        // Currently set to only protect colony animals, and this animal is not one
        if (PetsAreSafe_Settings.animalsAffected == PetsAreSafe_Settings.AnimalsAffected.YourAnimals
            && parent.Faction?.IsPlayer == true)
        {
            return false;
        }

        return true;
    }

    private bool ShouldAbsorbSource(DamageInfo dinfo)
    {
        return PetsAreSafe_Settings.damageSources == PetsAreSafe_Settings.DamageBlocked.AllSources || dinfo.Instigator is Pawn;
    }

    public void DoAnEscape()
    {
        if (PetsAreSafe_Settings.safetyBehavior == PetsAreSafe_Settings.SafetyBehavior.PlayDead)
        {
            TakeANap();
        }
        else if (PetsAreSafe_Settings.safetyBehavior == PetsAreSafe_Settings.SafetyBehavior.PoofToSafety)
        {
            DoAPoof();
        }
    }

    public bool DoAPoof()
    {
        Pawn animal = parent as Pawn;
        if (animal == null || animal.Dead || animal.Destroyed || !animal.Spawned || animal.Map == null)
            return false;

        try
        {
            Map map = animal.Map;
            IntVec3 currentPosition = animal.Position;
            if (!currentPosition.InBounds(map))
                return false;

            // Validator of a safe position
            bool IsSafeDestination(IntVec3 cell)
            {
                return cell != currentPosition
                    && cell.InBounds(map)
                    && cell.StandableBy(map, animal)
                    && !cell.Fogged(map)
                    && cell.GetFirstPawn(map) == null
                    && cell.GetFirstThing<Fire>(map) == null
                    && cell.GetFirstThing<Building_Trap>(map) == null;
            }

            // Try to find a safe position to poof to
            if (!CellFinder.TryFindRandomCellNear(currentPosition, map, escapeRadius,
                    IsSafeDestination, out IntVec3 destination, maxTries: 100))
                return false;

            animal.Position = destination;
            animal.Notify_Teleported(endCurrentJob: true, resetTweenedPos: true);
            animal.ClearMind_NewTemp();

            return true;
        }
        catch (Exception ex)
        {
            Log.ErrorOnce($"[Pets are Safe] Failed to teleport {animal}: {ex}", 194735021);
            return false;
        }
    }

    public void TakeANap()
    {
        Pawn animal = parent as Pawn;
        Hediff hediff = animal.health.hediffSet.GetFirstHediffOfDef(PetsAreSafe_DefOf.PAS_PlayDead);
        if (hediff != null)
        {
            HediffComp_Disappears comp = hediff.TryGetComp<HediffComp_Disappears>();
            if (comp != null)
            {
                comp.ticksToDisappear = 10000;
            }
        }
        else
        {
            animal.health.AddHediff(PetsAreSafe_DefOf.PAS_PlayDead);
        }
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        foreach (var g in base.CompGetGizmosExtra())
        {
            yield return g;
        }

        if (PetsAreSafe_Settings.animalsAffected == PetsAreSafe_Settings.AnimalsAffected.SpecificAnimals)
        {
            yield return new Command_Toggle
            {
                defaultLabel = "PAS_KeepSafe".Translate(),
                //icon = TODO
                isActive = () => protect,
                toggleAction = () => protect = !protect
            };
        }
    }

    public override void PostExposeData()
    {
        Scribe_Values.Look(ref protect, "protect");

        base.PostExposeData();
    }
}
