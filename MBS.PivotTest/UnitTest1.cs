using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBS.PivotDataTable;
using System.Collections.Generic;
using MBS.PivotDataTable.Setting;
using System.Linq;

namespace MBS.PivotTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<Model> data = new List<Model> { 
                new Model{ Value = 4.6F , ColumnTitle="تهران", RowTitle="شهرداری"},
                new Model{ Value = 7 , ColumnTitle="تهران", RowTitle="وزارت راه"},
                new Model{ Value = 3.6F , ColumnTitle="سمنان", RowTitle="شهرداری"},
                new Model{ Value = 1 , ColumnTitle="سمنان", RowTitle="وزارت راه"},
                new Model{ Value = 11.3F , ColumnTitle="کرمان", RowTitle="شهرداری"},
                new Model{ Value = 8 , ColumnTitle="کرمان", RowTitle="وزارت راه"},
                new Model{  ColumnTitle="قم", RowTitle="شهرداری"},
                new Model{ Value = 5 , ColumnTitle="قم", RowTitle="وزارت راه"},

            };

            DataTable dt = new DataTable(data);
            dt.Options.FirstRowColumnHeaderTitle = "ارگان / شهر";

            var addCol = new List<AdditionalColumn>();
            int counter = 1;
            addCol.Add(new AdditionalColumn
            {
                Function = AggregateFunction.Custom,
                CustomAggregateFunction = (d) => { return (counter++).ToString(); }, 
                Title = "ردیف"
                ,
                Order = ContentOrder.IsFirst
            });

            addCol.Add(new AdditionalColumn{ Function = AggregateFunction.Sum , Title ="مجموع"
                ,
                                             Order = ContentOrder.UseIndexAfterContent,
                                             OrderIndex = 1,
            });

            addCol.Add(new AdditionalColumn
            {
                Function = AggregateFunction.Custom,
                Title = "تعداد"
                ,
                CustomAggregateFunction = (model) => {

                    return (model.Select(s=> s.Value).Sum() * 1000).ToString("##.##");
                }
                ,
                OrderIndex = 0,
                Order = ContentOrder.UseIndexAfterContent
            });

            dt.Options.AdditionalColumns = addCol;

            var addRow = new List<AdditionalRow>();
            addRow.Add(new AdditionalRow { Title="میانگین شهر" ,  Order = ContentOrder.IsLast, 
            Function = AggregateFunction.Avg });

            dt.Options.AdditionalRows = addRow;

            dt.onConvertToCell += (e) => {
                return e.Value.ToString("##.0");

                //return e.Row+" : "+ e.Value.ToString("##.#");
            };

            //Ready to Test and Use
            var x = dt.Generate();

            var y = x;

        }
    }
}
