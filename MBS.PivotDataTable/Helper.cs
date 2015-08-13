using MBS.PivotDataTable.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DT = System.Data.DataTable;
using DR = System.Data.DataRow;

namespace MBS.PivotDataTable
{
    static class Helper
    {

        public static void MakeTable(List<Model> m, Setting.Config config, DT table,
            Func<ConvertToCellEventArgs, string> mapper)
        {

            var allColumns = m.GroupBy(a => a.ColumnTitle).ToList();
            var allRows = m.GroupBy(a => a.RowTitle).ToList();
            var allColNamesContent = allColumns.Select(a => a.Key).ToList();

            //Func<ConvertToCellEventArgs, string> tmpMapperCell = (e) =>
            //{
            //    if (mapper != null)
            //    {
            //        return mapper(e);
            //    }
            //    else
            //    {
            //        return e.Value.ToString();
            //    }
            //};

            AdditionalRow firstrow = null;
            List<AdditionalRow> beforeRows = null;

            AdditionalRow lastRow = null;
            List<AdditionalRow> afterRows = null;


            AdditionalColumn firstCol = null;
            List<AdditionalColumn> beforeColumns = null;

            AdditionalColumn lastCol = null;
            List<AdditionalColumn> afterColumns = null;
            int firstcol_index = 0;
            int firstrow_index = 0;


            #region check additional Columns
            if (config.HasAdditionalColumn)
            {
                var first = config.AdditionalColumns.Where(a => a.Order == Setting.ContentOrder.IsFirst).ToList();
                if (first != null)
                {

                    if (first.Count == 1)
                    {
                        firstCol = first.First();
                        firstcol_index++;
                    }
                    else if (first.Count > 1)
                    {
                        throw new InvalidOperationException("PivotDataTable - Column: Just One 'First' Order is Valid in a Table");
                    }
                }


                var befores = config.AdditionalColumns.Where(a => a.Order == Setting.ContentOrder.UseIndexBeforeContent).ToList();
                if (befores != null && befores.Any())
                {
                    beforeColumns = befores.OrderBy(a => a.OrderIndex).ToList();
                    firstcol_index += (beforeColumns.Count);
                }


                var last = config.AdditionalColumns.Where(a => a.Order == Setting.ContentOrder.IsLast).ToList();
                if (last != null)
                {
                    if (last.Count == 1)
                    {
                        lastCol = last.First();
                    }
                    else if (last.Count > 1)
                    {
                        throw new InvalidOperationException("PivotDataTable - Column: Just One 'Last' Order is Valid in a Table");
                    }
                }
                

                var afters = config.AdditionalColumns.Where(a => a.Order == Setting.ContentOrder.UseIndexAfterContent).ToList();
                if (afters != null && afters.Any())
                {
                    afterColumns = afters.OrderBy(a => a.OrderIndex).ToList();
                }
            }

            #endregion

            #region initialize columns


            if (firstCol != null)
            {
                table.Columns.Add(firstCol.Title);
            }

            table.Columns.Add(config.FirstRowColumnHeaderTitle);

            if (beforeColumns != null)
            {
                beforeColumns.ForEach(a => table.Columns.Add(a.Title));
            }

            allColNamesContent.ForEach(a => {
                table.Columns.Add(a);
            });

            if (afterColumns != null)
            {
                afterColumns.ForEach(a => table.Columns.Add(a.Title));
            }
            if (lastCol != null)
            {
                table.Columns.Add(lastCol.Title);
            }
            #endregion

            #region Check Additional Rows

            if (config.HasAdditionalRow)
            {
                var first = config.AdditionalRows.Where(a => a.Order == Setting.ContentOrder.IsFirst).ToList();
                if (first != null)
                {

                    if (first.Count == 1)
                    {
                        firstrow = first.First();
                        firstrow_index++;
                    }
                    else if (first.Count > 1)
                    {
                        throw new InvalidOperationException("PivotDataTable - Row: Just One 'First' Order is Valid in a Table");
                    }
                }


                var befores = config.AdditionalRows.Where(a => a.Order == Setting.ContentOrder.UseIndexBeforeContent).ToList();
                if (befores != null && befores.Any())
                {
                    beforeRows = befores.OrderBy(a => a.OrderIndex).ToList();
                    firstrow_index += (beforeColumns.Count);
                }


                var last = config.AdditionalRows.Where(a => a.Order == Setting.ContentOrder.IsLast).ToList();
                if (last != null)
                {
                    if (last.Count == 1)
                    {
                        lastRow = last.First();
                    }
                    else if (last.Count > 1)
                    {
                        throw new InvalidOperationException("PivotDataTable - Row: Just One 'Last' Order is Valid in a Table");
                    }
                }


                var afters = config.AdditionalRows.Where(a => a.Order == Setting.ContentOrder.UseIndexAfterContent).ToList();
                if (afters != null && afters.Any())
                {
                    afterRows = afters.OrderBy(a => a.OrderIndex).ToList();
                }
            }

            #endregion


            #region Add Rows - Before
            if (firstrow != null)
            {
                throw new NotImplementedException("NOT READY - firstrow");
            }

            if (beforeRows != null && beforeRows.Any())
            {
                throw new NotImplementedException("NOT READY - beforeRows");
            }

            #endregion


            #region Load Main Original Table

            int lenCols = allColNamesContent.Count;

            allRows.ForEach(a => {
                DR dr = table.NewRow();

                var tmpData = a.ToList();

                #region Load 'First Section' Table
                if (firstCol != null)
                {
                    firstCol.ColumnData = tmpData;
                    firstCol.Apply(dr);
                }
                #endregion

                #region Load 'Before Section' Table

                if (beforeColumns != null)
                {
                    beforeColumns.ForEach(b =>
                    {
                        b.ColumnData = tmpData;
                        b.Apply(dr);
                    });
                }
                #endregion


                //dr[0] = a.Key;
                dr[firstcol_index] = a.Key;

                
                for (int i = 0; i < lenCols; i++)
                {
                    string col = allColNamesContent[i];
                    if (!string.IsNullOrEmpty(col))
                    {
                        ConvertToCellEventArgs e = new ConvertToCellEventArgs
                            (a.Where(b => b.ColumnTitle == col).Select(x => x.Value).FirstOrDefault(),
                            col,i,a.Key);
                        dr[col] = mapper(e);
                    }
                }

                #region Load 'After Section' Table
                if (afterColumns != null)
                {
                    afterColumns.ForEach(b =>
                    {
                        b.ColumnData = tmpData;
                        b.Apply(dr);
                    });
                }

                #endregion


                #region Load 'Last Section' Table
                if (lastCol != null)
                {
                    lastCol.ColumnData = tmpData;
                    lastCol.Apply(dr);
                }
                #endregion

                table.Rows.Add(dr);


               




            });

            #endregion

            #region Add Rows - After

            if (afterRows != null && afterRows.Any())
            {

                for (int i = 0; i < lenCols; i++)
                {
                    string col = allColNamesContent[i];
                    if (!string.IsNullOrEmpty(col))
                    {

                        var q = (from System.Data.DataRow dr in table.Rows
                                 select new Model{  ColumnTitle = col,
                                            Value = Convert.ToSingle((string)dr[col]),
                                                    RowTitle = Convert.ToString(dr[firstcol_index])
                                            }).ToList();

                         afterRows.ForEach(r => {

                             r.ColumnData = q;
                             //r.Apply();

                         });
                    }
                }

               
            }


            if (lastRow != null)
            {
                DR drow = table.NewRow();
                
                drow[firstcol_index] = lastRow.Title;
                for (int i = 0; i < lenCols; i++)
                {
                    string col = allColNamesContent[i];
                    if (!string.IsNullOrEmpty(col))
                    {
                        var q = (from System.Data.DataRow dr in table.Rows
                                 select new Model
                                 {
                                     ColumnTitle = col,
                                     Value = Convert.ToSingle((string)dr[col]),
                                     RowTitle = lastRow.Title
                                 }).ToList();

                        lastRow.ColumnData = q;
                        lastRow.Apply(drow);


                        if (firstCol != null)
                        {
                            firstCol.ColumnData = q;
                            firstCol.Apply(drow);
                        }

                        if (beforeColumns != null)
                            beforeColumns.ForEach(w => { w.ColumnData = q; w.Apply(drow); });      


                        if (lastCol != null)
                        {
                            lastCol.ColumnData = q;
                            lastCol.Apply(drow);
                        }

                        if (afterColumns != null)
                            afterColumns.ForEach(w => { w.ColumnData = q; w.Apply(drow); });      
                    }
                }


                table.Rows.Add(drow);

            }

            

            #endregion



        }

    }
}
