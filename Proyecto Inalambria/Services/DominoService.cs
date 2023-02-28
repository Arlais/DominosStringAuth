namespace Proyecto_Inalambria.Services
{
    public static class DominoService
    {
        public static List<(int, int)> ConstruirCadena(List<(int, int)> fichas)
        {
            var cadena = new LinkedList<(int, int)>();
            var disponibles = new HashSet<(int, int)>(fichas);
            // Primera ficha
            var primera = disponibles.First();
            disponibles.Remove(primera);
            cadena.AddLast(primera);
            // Resto de fichas
            while (disponibles.Count > 0)
            {
                var ultima = cadena.Last.Value.Item2;
                var siguiente = disponibles.FirstOrDefault(f => f.Item1 == ultima && f.Item2 == ultima);
                if (siguiente == default)
                {
                    siguiente = disponibles.FirstOrDefault(f => f.Item1 == ultima || f.Item2 == ultima);
                }
                if (siguiente == default)
                {
                    return null;
                }
                disponibles.Remove(siguiente);
                if (siguiente.Item1 == ultima)
                {
                    cadena.AddLast(siguiente);
                }
                else
                {
                    cadena.AddLast((siguiente.Item2, siguiente.Item1));
                }
            }
            if (cadena.First.Value.Item1!=cadena.Last.Value.Item2)
            {
                return null;
            }
            return cadena.ToList();
        }

    }
}
