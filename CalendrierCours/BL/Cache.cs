using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entites;

namespace BL
{
    internal static class Cache
    {
        internal static Cohorte Cohorte { get; set; }
        internal static List<Cours> Cours { get; set; }
    }
}
