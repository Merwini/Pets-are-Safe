using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace Nuff.PetsAreSafe
{
    public class CompPetsAreSafe : ThingComp
    {

        PetsAreSafeSettings settings = PetsAreSafe.pasSettings;

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            absorbed = false;
            bool flag;

            if (dinfo.Instigator != null)
            {
                Pawn pawn = dinfo.Instigator as Pawn;
                if (pawn != null)
                {
                    if (settings.wildOrFact == PetsAreSafeSettings.WildOrFact.All_Animals)
                    {
                        flag = DoAnEscape(parent);
                        absorbed = true;
                    }
                    else if (settings.wildOrFact == PetsAreSafeSettings.WildOrFact.Just_Your_Animals
                                                 && parent.Faction != null
                                                 && parent.Faction.IsPlayer)
                    {
                        flag = DoAnEscape(parent);
                        absorbed = true;
                    }
                }
            }
            return; 
        }

        public bool DoAnEscape(ThingWithComps thingWithComps)
        {
            bool success = false;
            Pawn animal = thingWithComps as Pawn;

            if (settings.poofOrPlay == PetsAreSafeSettings.PoofOrPlay.Play_Dead)
            {
                success = TakeANap(animal);
            }
            else if (settings.poofOrPlay == PetsAreSafeSettings.PoofOrPlay.Poof_To_Safety)
            {
                success = DoAPoof(animal);
            }
            else
                success = true;
            return success;
        }

        public bool DoAPoof(Pawn animal)
        {
            Map map = animal.MapHeld;
            bool success = false;
            Random rnd = new Random();
            IntVec3 currentPosition = animal.Position;
            int randX = rnd.Next(-10, 10);
            int randZ = rnd.Next(-10, 10);
            int newX = currentPosition.x + randX;
            int newZ = currentPosition.z + randZ;
            if (newX < 0 || newX >= map.Size.x)
            {
                if (newX < map.Center.x)
                {
                    newX = currentPosition.x + 10;
                }
                else
                {
                    newX = currentPosition.x - 10;
                }    
            }
            if (newZ < 0 || newZ >= map.Size.z)
            {
                if (newZ < map.Center.z)
                {
                    newZ = currentPosition.z + 10;
                }
                else
                {
                    newZ = currentPosition.z - 10;
                }
            }

            IntVec3 targetVec = new IntVec3(newX, 0, newZ);
            LocalTargetInfo target = new LocalTargetInfo(targetVec);
            animal.pather.StartPath(target, Verse.AI.PathEndMode.OnCell);
            animal.pather.PatherTick();

            try
            {
                animal.SetPositionDirect(animal.pather.Destination.Cell);
                animal.ClearMind();
                success = true;
            }
            catch(Exception ex)
            {
                ;
            }

            return success;
        }

        public bool TakeANap(Pawn animal)
        {
            bool success = false;
            animal.health.AddHediff(PAS_HediffDefOf.PASPlayDead);
            success = true;

            return success;
        }

    }
}