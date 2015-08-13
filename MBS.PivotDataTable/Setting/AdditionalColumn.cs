using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DT = System.Data.DataTable;


namespace MBS.PivotDataTable.Setting
{
    public  class AdditionalColumn : BaseAdditionalContent
    {

        public override void Apply(DataRow dr)
        {

            switch (Function)
            {
                case AggregateFunction.None:
                default:
                    break;
                case AggregateFunction.Sum:
                    dr[this.Title] = ColumnData.Sum(a => a.Value);
                    break;
                case AggregateFunction.Max:
                    dr[this.Title] = ColumnData.Max(a => a.Value);
                    break;
                case AggregateFunction.Min:
                    dr[this.Title] = ColumnData.Min(a => a.Value);
                    break;
                case AggregateFunction.Count:
                    dr[this.Title] = ColumnData.Count();
                    break;
                case AggregateFunction.Avg:
                    dr[this.Title] = ColumnData.Average(a => a.Value);
                    break;
                case AggregateFunction.Custom:
                    dr[this.Title] = CustomAggregateFunction(ColumnData);
                    break;
                    
            }

        }

        public override ContentType ContentType
        {
            get { return ContentType.Column; }
        }
    }
}
