using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBS.PivotDataTable
{
    public sealed class ConvertToCellEventArgs : EventArgs
    {
        public int ColumnIndex { get; private set; }
        public string Column { get; private set; }
        public string Row { get; private set; }
        public float Value { get; private set; }
        public ConvertToCellEventArgs(float value, string columnTitle, int colIndex,string rowTitle)
        {
            ColumnIndex = colIndex;
            Column = columnTitle;
            Row = rowTitle;
            Value = value;
        }

    }
}
