using System.Collections.Generic;

namespace ExcelMergeV7
{
    public static class Usr
    {
        //K=IdUsuario,Rick=IdArea,Joi=IdRol
        public static int K { get; set; }

        public static int Rick { get; set; }
        public static int Joi { get; set; }
        public static string Nombre { get; set; }
        public static bool Flag { get; set; }
        private static bool isValidator;

        private static List<string> incertidumbre = new List<string>();
        private static List<string> incertidumbreH = new List<string>();
        public static string offset { get; set; }
        public static string proporcion { get; set; }
        public static string offsetH { get; set; }
        public static string proporcionH { get; set; }
        private static List<string[,]> uni = new List<string[,]>();
        public static List<string[,]> Uni { get => uni; set => uni = value; }
        public static int DatePos { get; set; }
        public static double DateDif { get; set; }
        public static List<string> Incertidumbre { get => incertidumbre; set => incertidumbre = value; }
        public static List<string> IncertidumbreH { get => incertidumbreH; set => incertidumbreH = value; }
        public static bool IsValidator { get => isValidator; set => isValidator = value; }
    }
}