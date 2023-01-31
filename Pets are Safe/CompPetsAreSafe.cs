using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace PetsAreSafe
{
    public class CompPetsAreSafe : ThingComp
    {
        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {


            absorbed = false;
            if (dinfo.Instigator != null)
            {
                Pawn pawn = dinfo.Instigator as Pawn;
                if (pawn != null)
                {
                    absorbed = true;
                }
            }
            return;
        }
    }
}