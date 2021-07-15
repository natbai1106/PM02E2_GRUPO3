using SQLite;
using System;
using System.Collections.Generic;
using System.Text;


namespace PM02E2_GRUPO3
{
    class Sitios
    {
        [AutoIncrement, PrimaryKey]
        public int Id { set; get; }

        public double Latitud { get; set; }
        public double Longitud { get; set; }

        [MaxLength(100)]
        public string DescripcionLarga { get; set; }

        [MaxLength(30)]
        public string DescripcionCorta { get; set; }

        public Byte Fotografia { get; set; }
    }
}
