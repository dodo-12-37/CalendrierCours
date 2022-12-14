using CalendrierCours.Entites;

namespace CalendrierCours.BL
{
    internal static class Cache
    {
        internal static Cohorte Cohorte { get; set; }
        internal static List<Cours> Cours { get; set; }
    }
}