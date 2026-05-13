using Cognex.VisionPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaNE42Vision2D.Utils
{
    public class VisionProTool
    {
        public static ICogRecord GetRecord(ICogRecord cogRecord, string key)
        {
            for (int i = 0; i < cogRecord.SubRecords.Count; i++)
            {
                if (cogRecord.SubRecords[i].RecordKey == key)
                {
                    return cogRecord.SubRecords[i];
                }
            }
            return null;
        }

        public static ICogRecord GetRecordByIndex(ICogRecord cogRecord, int index)
        {
            return cogRecord.SubRecords[index];
        }


    }
}
