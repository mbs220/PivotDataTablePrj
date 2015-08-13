using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DT = System.Data.DataTable;
namespace MBS.PivotDataTable
{
    public sealed class DataTable
    {
        public DataTable(List<Model> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("PivotDataTable: DataSource is Null");
            }
            DataSource = data;
        }
        public delegate string ConvertToCell(ConvertToCellEventArgs e);
        public event ConvertToCell onConvertToCell;

        string ToCell(ConvertToCellEventArgs e)
        {
            if (onConvertToCell ==null)
            {
                return e.Value.ToString();
            }
            return onConvertToCell(e);
        }

        private Setting.Config _Options;
        public Setting.Config Options
        {
            get
            {
                if (_Options == null)
                    _Options = new Setting.Config();

                return _Options ;
            }
            //set
            //{
            //    _Options = value;
            //}
        }

        public List<Model> DataSource { get; private set; }

        public DT Generate()
        {
            if (DataSource == null)
            {
                throw new ArgumentNullException("PivotDataTable: DataSource is Null");
            }
            DT tmp = new DT("PivotDataTable");

            Helper.MakeTable(DataSource, Options, tmp, ToCell);

            return tmp;
        }

    }
}
