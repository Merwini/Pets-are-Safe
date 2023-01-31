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
    public class CompProperties_PetsAreSafe : CompProperties
    {
        public CompProperties_PetsAreSafe()
        {
            this.compClass = typeof(CompPetsAreSafe);
        }
        public CompProperties_PetsAreSafe(Type compClass) : base(compClass)
        {
            this.compClass = compClass;
        }
    }
}