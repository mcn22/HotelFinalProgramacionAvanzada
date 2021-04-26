
namespace HotelFinalProgramacionAvanzada.Utility
{
    public static class SD
    {
        public static class Roles
        {
            public const string Administrador = "Administrador";
            public const string Empleado = "Recepcionista";
            public const string Cliente = "Cliente";
        }

        public static class EstadosReserva
        { 
            public const string Adelanto = "Adelanto pagado";
            public const string Total = "Totalmente pagado";
            public const string Suspendida = "Suspendida";
        }

        public static class TiposHabitacion
        {
            public const string Individual = "Individual";
            public const string IndividualDesc = "Nuestras habitaciones individuales gozan de vistas a la calle o al patio y disponen de ventanas de doble acristalamiento. Cuentan con una cama de 120 cm, baño con bañera e inodoro privado. Gozan de vistas al patio, lo que garantiza una calma y una tranquilidad totales.";
            public const string IndividualImg = "individual.jpg";

            public const string Doble = "Doble";
            public const string DobleDesc = "La habitación con dos camas individuales permite alojarse a dos personas y cuenta con dos camas individuales de 90 cm de ancho colocadas una junto a la otra. Gozan de vistas al patio, lo que garantiza tranquilidad total. Disponen de baño con bañera e inodoro privado.";
            public const string DobleImg = "doble.jpg";

            public const string DobleSuperior = "Doble superior";
            public const string DobleSuperiorDesc = "La habitación doble, perfecta para una o dos personas, ofrece vistas al patio o a la calle y cuenta con ventanas de doble acristalamiento para que disfrute de un ambiente tranquilo y relajante. Dispone de una cama de 140 cm de ancho, baño con bañera e inodoro privado.";
            public const string DobleSuperiorImg = "dobleSuperior.jpg";
        }

        public static class Hoteles
        {
            public const string Hotel1 = "Hotel Arenal Lodge";
            public const string Hotel1Desc = "Está rodeado de un bosque de 810 hectáreas repleto de vegetación tropical, pájaros exóticos y animales salvajes. Vistas panorámicas inmejorables al volcán y lago.";
            public const string Hotel1Img = "hotel1.jpg";
            public const string Hotel1Direc = "200 metros norte del dique del lago Arenal, la Fortuna, 21007 Fortuna, Costa Rica.";
            public const string Hotel1Ciu = "Alajuela";
            public const string Hotel1Tel = "26512555";

            public const string Hotel2 = "Hotel Fonda Vela";
            public const string Hotel2Desc = "El Hotel Fonda Vela se encuentra en Monteverde, Costa Rica, además en este hotel se ofrece piscina cubierta y restaurante. Se facilita WiFi gratuita y aparcamiento privado.";
            public const string Hotel2Img = "hotel2.jpg";
            public const string Hotel2Direc = "600 m sur este de la fabrica de quesos Monteverde, Puntarenas, Costa Rica, 60109 Monteverde.";
            public const string Hotel2Ciu = "Puntarenas";
            public const string Hotel2Tel = "26697215";

            public const string Hotel3 = "Hotel Belmar";
            public const string Hotel3Desc = "Se encuentra en Monteverde, Costa Rica, y alberga un restaurante exclusivo con cervecería y bar. Cuenta con jardín ecológico y certificado de sostenibilidad. ";
            public const string Hotel3Img = "hotel3.jpg";
            public const string Hotel3Direc = "300 metros este de la estacion de gasolina 5655, 00111 Monteverde, Costa Rica.";
            public const string Hotel3Ciu = "Puntarenas";
            public const string Hotel3Tel = "27541399";
        }

    }
}
