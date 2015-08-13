using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBS.PivotDataTable.Setting
{
    public sealed class Config
    {

        public Config()
        {

        }
        public string FirstRowColumnHeaderTitle { get; set; }

        public bool HasAdditionalColumn
        {
            get { return AdditionalColumns != null && AdditionalColumns.Any(); }
        }
        public bool HasAdditionalRow
        {
            get { return AdditionalRows != null && AdditionalRows.Any(); }
        }
        public List<AdditionalColumn> AdditionalColumns { get; set; }
        public List<AdditionalRow> AdditionalRows { get; set; }


    }
}
