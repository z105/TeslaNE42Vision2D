using FreeSql.DataAnnotations;
using System;

namespace TeslaNE42Vision2D.Entity
{
    [Table(Name = "detect_records")]
    public class DetectRecord
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }
        public DateTime DetectTime { get; set; }
        public bool OkNg { get; set; }
        public double PhysicalX { get; set; }
        public double PhysicalY { get; set; }
        public double PhysicalAngle { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string Polarity { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }
}
