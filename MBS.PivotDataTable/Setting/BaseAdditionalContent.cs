using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DT = System.Data.DataTable;

namespace MBS.PivotDataTable.Setting
{
    public abstract class BaseAdditionalContent
    {
        public BaseAdditionalContent()
        {
            Function = AggregateFunction.None;
            Order = ContentOrder.UseIndexAfterContent;
            OrderIndex = 0;
        }
        public virtual List<Model> ColumnData { get; set; }

        public abstract ContentType ContentType { get;}
        public virtual string Title { get; set; }
        public virtual AggregateFunction Function { get; set; }
        public virtual ContentOrder Order { get; set; }
        public virtual uint OrderIndex { get; set; }
        public virtual Func<List<Model>, string> CustomAggregateFunction { get; set; }

        public abstract void Apply(DataRow d);

    }
}
