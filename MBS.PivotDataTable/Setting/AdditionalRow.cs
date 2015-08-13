using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DT = System.Data.DataTable;

namespace MBS.PivotDataTable.Setting
{
    public  class AdditionalRow : BaseAdditionalContent
    {

        public override void Apply(DataRow dr)
        {

            ColumnData.ForEach(c => {
                switch (Function)
                {
                    case AggregateFunction.None:
                    default:
                        break;
                    case AggregateFunction.Sum:
                        dr[c.ColumnTitle] = ColumnData.Sum(a => a.Value);
                        break;
                    case AggregateFunction.Max:
                        dr[c.ColumnTitle] = ColumnData.Max(a => a.Value);
                        break;
                    case AggregateFunction.Min:
                        dr[c.ColumnTitle] = ColumnData.Min(a => a.Value);
                        break;
                    case AggregateFunction.Count:
                        dr[c.ColumnTitle] = ColumnData.Count();
                        break;
                    case AggregateFunction.Avg:
                        dr[c.ColumnTitle] = ColumnData.Average(a => a.Value);
                        break;
                    case AggregateFunction.Custom:
                        dr[c.ColumnTitle] = CustomAggregateFunction(ColumnData);
                        break;

                }
            });

            
        }

        public override ContentType ContentType
        {
            get { return ContentType.Row; }
        }
    }
}
