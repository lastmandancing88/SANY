using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace 机舱总成排程及派工
{
    public partial class Form1 : Form
    {
        System.Data.DataTable OutPut;
        System.Data.DataSet ScheduleMaps;
        System.Data.DataSet WorkingTimeMaps;
        System.Data.DataSet ProcessMaps;

        List<Worker> Workers;
        List<Schedule> Schedules;

        public void InitialStepInfo()
        {
            Worker worker;
            double maxFinishTime = 0;
            Workers = new List<Worker>();
            Schedules = new List<Schedule>();
            for (int i = 0; i < ProcessMaps.Tables[0].Rows.Count; i++)
            {
                List<string> workers = new List<string>(ProcessMaps.Tables[0].Rows[i]["责任人"].ToString().Split('/'));
                try
                {
                    maxFinishTime = (Schedules.Where(a => workers.Contains(a.Worker.Name)).Max(b => b.Steps.Last().FinishTime)) + (double)ProcessMaps.Tables[0].Rows[i]["时间"];
                }
                catch (Exception)
                {
                    maxFinishTime = (double)ProcessMaps.Tables[0].Rows[i]["时间"];
                }
                Step step = new Step(
                                                    ProcessMaps.Tables[0].Rows[i]["工序"].ToString(),
                                                    ProcessMaps.Tables[0].Rows[i]["工步编号"].ToString(),
                                                    ProcessMaps.Tables[0].Rows[i]["工步"].ToString(),
                                                    (double)ProcessMaps.Tables[0].Rows[i]["时间"],
                                                    maxFinishTime
                                                 );
                //minWaitingTime += waitTimes.Max(a => a.Time);
                //waitTimes.Clear();
                //}
                for (int j = 0; j < workers.Count; j++)
                {
                    worker = new Worker(workers[j]);
                    if (!Workers.Contains(worker))
                    {
                        Workers.Add(worker);
                        Schedules.Add(new Schedule(worker, step));
                    }
                    else
                    {
                        Schedules.Find(a => { return a.Worker.Name == worker.Name; }).Steps.Add(step);
                    }
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        public void BuildOutput()
        {
            DateTime startDateTime, dateTime;
            string time;
            Step step;
            bool addFlag = false; 
            OutPut = new System.Data.DataTable();
            int column = Schedules.Max(a => a.Steps.Count);
            for (int i = 1; i <= 3 * column + 5; i++)
            {
                OutPut.Columns.Add("列名" + i);
            }
            DataRow dr;
            for (int i = 0; i < Schedules.Count; i++)
            {
                DataRow dr1 = OutPut.NewRow();
                dr1[0] = "订单号";
                dr1[1] = "项目";
                dr1[2] = "序列号";
                dr1[3] = "责任人";
                dr1[4] = "日期";
                DataRow dr2 = OutPut.NewRow();
                for (int j = 0; j < 5; j++)
                {
                    dr2[j] = dr1[j];
                }
                for (int j = 0; j < Schedules[i].Steps.Count; j++)
                {
                    dr1[5 + 3 * j] = Schedules[i].Steps[j].StepName;
                    dr2[5 + 3 * j] = "开始";
                    dr1[5 + 3 * j + 1] = Schedules[i].Steps[j].StepName;
                    dr2[5 + 3 * j + 1] = "作业时间";
                    dr1[5 + 3 * j + 2] = Schedules[i].Steps[j].StepName;
                    dr2[5 + 3 * j + 2] = "结束";
                }
                OutPut.Rows.Add(dr1);
                OutPut.Rows.Add(dr2);
                for (int j = 0; j < ScheduleMaps.Tables[0].Rows.Count; j++)
                {
                    dr = OutPut.NewRow();
                    dr["列名1"] = ScheduleMaps.Tables[0].Rows[j]["订单号"];
                    dr["列名2"] = ScheduleMaps.Tables[0].Rows[j]["项目"];
                    dr["列名3"] = ScheduleMaps.Tables[0].Rows[j]["序列号"];
                    dr["列名4"] = Schedules[i].Worker.Name;
                    startDateTime = (DateTime)ScheduleMaps.Tables[0].Rows[j]["开始时间"];
                    dateTime = startDateTime;
                    dr["列名5"] = dateTime.ToString("yyyy/MM/dd");
                    time = dateTime.ToString("HH:mm");
                    addFlag = false;
                    for (int k = 0; k < Schedules[i].Steps.Count; k++)
                    {
                        step = Schedules[i].Steps[k];
                        if (step.ProcessName != ScheduleMaps.Tables[0].Rows[j]["工序"].ToString())
                        {
                            break;
                        }
                        dr["列名" + (6 + 3 * k)] = time;
                        dr["列名" + (6 + 3 * k + 1)] = Schedules[i].Steps[k].WorkingTime;
                        //dateTime.AddMinutes(Schedules[i].Steps[k].WorkingTime);
                        dateTime = ModifyTime(startDateTime.AddMinutes(Schedules[i].Steps[k].FinishTime));
                        time = dateTime.ToString("HH:mm");
                        dr["列名" + (6 + 3 * k + 2)] = time;
                        addFlag = true;
                    }
                    if (addFlag)
                    {
                        OutPut.Rows.Add(dr);
                    }
                }
            }
        }
        public DateTime ModifyTime(DateTime dateTime)
        {
            double restGap;
            DateTime restBeginTime1, restBeginTime2, restEndTime1, restEndTime2;
            restBeginTime1 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 11, 30, 0, 0);
            restEndTime1 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 0, 0, 0);
            if (dateTime.DayOfWeek == DayOfWeek.Friday)
            {
                restBeginTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0);
                restEndTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 18, 0, 0, 0);
            }
            else
            {
                restBeginTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 30, 0, 0);
                restEndTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 18, 30, 0, 0);
            }
            if (dateTime <= restBeginTime1)
            {
                return dateTime;
            }
            else if (dateTime > restBeginTime1 & dateTime < restEndTime2)
            {
                restGap = 150;
            }
            else
            {
                restGap = 150 + 60;
            }
            return dateTime.AddMinutes(restGap);
        }
        #region 将Excel数据绑定到dataGridView
        //public void ExcelToDGV(string path)
        //{
        //    DataSet m_ds = new DataSet();
        //    DataTable outPut = new DataTable();
        //    string strConn = @"Provider = Microsoft.Jet.OLEDB.4.0;Data Source = "
        //    + path + ";Extended Properties='Excel 8.0;IMEX=1';";
        //    string strSheetName = "sheet1"; //默认sheet1
        //    string strExcel = string.Format("select * from [{0}$]", strSheetName);
        //    using (OleDbConnection conn = new OleDbConnection(strConn))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
        //            adapter.Fill(m_ds, strSheetName);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.ToString());
        //        }
        //        finally
        //        {
        //            conn.Close();
        //        }
        //    }
        //    outPut = m_ds.Tables[strSheetName];
        //    for (int i = 0; i < outPut.Rows.Count; i++)
        //    {
        //        outPut.Rows[i]["前置时间(MIN)"] = outPut.Rows[i]["前置时间(MIN)"].Equals(DBNull.Value) ? 0 : outPut.Rows[i]["前置时间(MIN)"];
        //        outPut.Rows[i]["作业时间(MIN)"] = outPut.Rows[i]["作业时间(MIN)"].Equals(DBNull.Value) ? 0 : outPut.Rows[i]["作业时间(MIN)"];
        //        outPut.Rows[i]["后置时间(MIN)"] = outPut.Rows[i]["后置时间(MIN)"].Equals(DBNull.Value) ? 0 : outPut.Rows[i]["后置时间(MIN)"];
        //    }
        //    this.dgvMasterData.DataSource = outPut;
        //    this.dgvMasterData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        //}
        public void ExcelToDGV(string path)
        {
            ScheduleMaps = new DataSet();
            ProcessMaps = new DataSet();
            DataTable outPut = new DataTable();
            string strConn = @"Provider = Microsoft.Jet.OLEDB.4.0;Data Source = "
            + path + ";Extended Properties='Excel 8.0;IMEX=1';";
            string strSheetName = "工步信息"; //默认sheet1
            string strExcel = string.Format("select * from [{0}$]", strSheetName);
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                try
                {
                    conn.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
                    adapter.Fill(ProcessMaps, strSheetName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
            strSheetName = "排班"; //默认sheet1
            strExcel = string.Format("select * from [{0}$]", strSheetName);
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                try
                {
                    conn.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
                    adapter.Fill(ScheduleMaps, strSheetName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
            strSheetName = "班次设定"; //默认sheet1
            strExcel = string.Format("select * from [{0}$]", strSheetName);
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                try
                {
                    conn.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
                    adapter.Fill(WorkingTimeMaps, strSheetName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
            outPut = ProcessMaps.Tables["工步信息"];
            this.dgvMasterData.DataSource = outPut;
            this.dgvMasterData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        }
        #endregion
        //public void GenerateSchedule(System.Data.DataTable dt)
        //{
        //    string endDate;
        //    double time;
        //    System.Data.DataTable temp = dt.Copy();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        temp.Rows[i]["工步序号"] = dt.Rows[i]["工步序号"].ToString().Length <= 2 ? 1 : 2;
        //    }

        //    OutPut = new System.Data.DataTable();
        //    OutPut.Columns.Add("工单号");
        //    OutPut.Columns.Add("工序");
        //    OutPut.Columns.Add("开始时间");
        //    OutPut.Columns.Add("结束时间");
        //    OutPut.Columns.Add("人员");
        //    OutPut.Columns.Add("工时(MIN)");

        //    var query = from t in temp.AsEnumerable()
        //                group t by new { t1 = t.Field<string>("工序"), t2 = t.Field<double>("工步序号"), t3 = t.Field<string>("人员") } into m
        //                select new
        //                {
        //                    process = m.Key.t1,
        //                    step = m.Key.t2,
        //                    name = m.Key.t3,
        //                    prefixTime = m.Sum(n => n.Field<double>("前置时间(MIN)")),
        //                    operationTime = m.Sum(n => n.Field<double>("作业时间(MIN)")),
        //                    suffixTime = m.Sum(n => n.Field<double>("后置时间(MIN)"))
        //                };
        //    if (query.ToList().Count > 0)
        //    {
        //        query.ToList().ForEach(q =>
        //        {
        //            time = q.prefixTime + q.operationTime + q.suffixTime;
        //            endDate = ((DateTime)dtpBeginTime.Value).AddMinutes(time).ToString("yyyy-MM-dd hh:mm:ss");
        //            OutPut.Rows.Add(tbProductionOrder.Text, q.process, dtpBeginTime.Value.ToString("yyyy-MM-dd hh:mm:ss"), endDate, q.name, q.operationTime);
        //        });
        //    }
        //    String maxEndDate = OutPut.AsEnumerable().Select(t => t.Field<String>("结束时间")).Max();
        //    System.Data.DataColumn dc = OutPut.Columns["结束时间"];
        //    int index = dc.Ordinal;
        //    OutPut.Columns.Remove(dc);
        //    dc.DefaultValue = maxEndDate;
        //    OutPut.Columns.Add(dc);
        //    dc.SetOrdinal(index);
        //}
        //public void GenerateSchedule(System.Data.DataTable dt)
        //{
        //    DataRow temp;
        //    double total =0, sum = 0;
        //    DateTime startTime, endTime;
        //    dt.DefaultView.Sort = "工序 ASC, 工步序号 ASC ";
        //    OutPut = new System.Data.DataTable();
        //    OutPut.Columns.Add("工单号");
        //    OutPut.Columns.Add("工序");
        //    OutPut.Columns.Add("人员");
        //    OutPut.Columns.Add("工步");
        //    OutPut.Columns.Add("开始时间");
        //    OutPut.Columns.Add("结束时间");
        //    OutPut.Columns.Add("时长(MIN)");
        //    temp = dt.Rows[0];
        //    startTime = dtpBeginTime.Value;
        //    endTime = startTime;
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        if (dt.Rows[i]["工序"].ToString() != temp["工序"].ToString() | dt.Rows[i]["人员"].ToString() != temp["人员"].ToString())
        //        {
        //            OutPut.Rows.Add(tbProductionOrder.Text, temp["工序"], temp["人员"], "总计", endTime.AddMinutes(-total), endTime, total);
        //            sum = 0;
        //        }
        //        temp = dt.Rows[i];
        //        sum = (double)temp["前置时间(MIN)"] + (double)temp["作业时间(MIN)"] + (double)dt.Rows[i]["后置时间(MIN)"];
        //        total += sum;
        //        endTime = endTime.AddMinutes(sum);
        //        OutPut.Rows.Add(tbProductionOrder.Text, dt.Rows[i]["工序"], dt.Rows[i]["人员"], dt.Rows[i]["工步"], startTime, endTime, sum);
        //        startTime = endTime;
        //    }
        //}
        private void bGenerate_Click(object sender, EventArgs e)
        {
            //GenerateSchedule((System.Data.DataTable)dgvMasterData.DataSource);
            //OutPut.DefaultView.Sort = "工序 DESC";
            BuildOutput();
            InitFormatColumns();
            dgvOutput.DataSource = OutPut;
            this.dgvOutput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            tcMain.SelectedTab = tcMain.TabPages[0];
        }
        private void bExport_Click(object sender, EventArgs e)
        {
            if (dgvOutput.Rows.Count == 0)
            {
                MessageBox.Show("没有数据可供导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                //首先，实例化对话框类实例
                SaveFileDialog saveDialog = new SaveFileDialog();
                //然后，判断如果当前用户在对话框里点击的是OK按钮的话。
                if (DialogResult.OK == saveDialog.ShowDialog())
                {
                    //将打开文件对话框的FileName属性传递到你的字符串进行处理
                    string fileName = saveDialog.FileName;
                    try
                    {
                        DataTabletoExcelkk(OutPut, fileName);
                        MessageBox.Show("数据导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("导出数据错误！\n\r" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private string NumTochr(int Num)
        {
            int n = 64 + Num;
            return "" + (Char)n;

        }
        private string NumToExeclRowStr(int Num)
        {
            int X, Y;
            if (Num < 27)
            {
                return NumTochr(Num);
            }
            X = Num / 26;
            Y = Num - X * 26;
            return NumTochr(X) + NumTochr(Y);

        }
        /// <summary>  
        /// 将DataTable中的列名及数据导出到Excel表中  
        /// </summary>  
        /// <param name="tmpDataTable">要导出的DataTable</param>  
        /// <param name="strFileName">Excel的保存路径及名称</param>  
        public void DataTabletoExcelkk(System.Data.DataTable tmpDataTable, string strFileName)
        {
            if (tmpDataTable == null)
                return;
            int rowNum = tmpDataTable.Rows.Count;
            int columnNum = tmpDataTable.Columns.Count;
            int rowIndex = 1;
            int columnIndex = 0;

            Excel.Application xlApp = new Excel.ApplicationClass();
            xlApp.DefaultFilePath = "";
            xlApp.DisplayAlerts = true;
            xlApp.SheetsInNewWorkbook = 1;
            Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
            Excel.Worksheet ws = (Excel.Worksheet)xlBook.Worksheets[1];
            int colnum = tmpDataTable.Columns.Count;
            Excel.Range r = ws.get_Range("A1", NumToExeclRowStr(colnum) + "1");
            object[] objHeader = new object[colnum];

            //将DataTable的列名导入Excel表第一行  
            foreach (DataColumn dc in tmpDataTable.Columns)
            {
                objHeader[columnIndex] = dc.ColumnName;
                columnIndex++;

            }
            r.Value2 = objHeader;

            //将DataTable中的数据导入Excel中  
            for (int i = 0; i < rowNum; i++)
            {
                rowIndex++;
                columnIndex = 0;
                for (int j = 0; j < columnNum; j++)
                {
                    objHeader[columnIndex] = tmpDataTable.Rows[i][j].ToString();
                    columnIndex++;

                }
                r = ws.get_Range("A" + (i + 2), NumToExeclRowStr(colnum) + (i + 2));
                r.Value2 = objHeader;


            }
            r.EntireColumn.AutoFit();
            xlBook.SaveCopyAs(strFileName);
        }
        private void bImport_Click(object sender, EventArgs e)
        {
            //首先，实例化对话框类实例
            OpenFileDialog openDialog = new OpenFileDialog();
            //然后，判断如果当前用户在对话框里点击的是OK按钮的话。
            if (DialogResult.OK == openDialog.ShowDialog())
            {
                //将打开文件对话框的FileName属性传递到你的字符串进行处理
                string fileName = openDialog.FileName;
                ExcelToDGV(fileName);
                InitialStepInfo();
                tcMain.SelectedTab = tcMain.TabPages[1];
            }
        }
        //private void dgvOutput_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{
        //    // 对第1列相同单元格进行合并
        //    if (e.ColumnIndex <=2 &&e.ColumnIndex >=0 && e.RowIndex != -1)
        //    {
        //        using
        //            (
        //            Brush gridBrush = new SolidBrush(this.dgvOutput.GridColor),
        //            backColorBrush = new SolidBrush(e.CellStyle.BackColor)
        //            )
        //        {
        //            using (Pen gridLinePen = new Pen(gridBrush))
        //            {
        //                // 清除单元格
        //                e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
        //                // 画 Grid 边线（仅画单元格的底边线和右边线）
        //                //   如果下一行和当前行的数据不同，则在当前的单元格画一条底边线
        //                if (e.RowIndex < dgvOutput.Rows.Count - 1 &&
        //                dgvOutput.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString() !=
        //                e.Value.ToString())
        //                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
        //                    e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
        //                    e.CellBounds.Bottom - 1);
        //                // 画右边线
        //                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
        //                    e.CellBounds.Top, e.CellBounds.Right - 1,
        //                    e.CellBounds.Bottom);
        //                // 画（填写）单元格内容，相同的内容的单元格只填写第一个
        //                if (e.Value != null)
        //                {
        //                    if (e.RowIndex > 0 &&
        //                    dgvOutput.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString() ==
        //                    e.Value.ToString())
        //                    { }
        //                    else
        //                    {
        //                        e.Graphics.DrawString((String)e.Value, e.CellStyle.Font,
        //                            Brushes.Black, e.CellBounds.X + 2,
        //                            e.CellBounds.Y + 5, StringFormat.GenericDefault);
        //                    }
        //                }
        //                e.Handled = true;
        //            }
        //        }
        //    }
        //}
        #region"合并单元格(多行多列)"

        //需要(行、列)合并的所有列标题名
        List<String> colsHeaderText_V = new List<String>();
        List<String> colsHeaderText_H = new List<String>();

        private void InitFormatColumns()
        {
            int column = Schedules.Max(a => a.Steps.Count);
            for (int i = 0; i < 3 * column; i++)
            {
                colsHeaderText_V.Add("列名" + (i + 6));
            }

            colsHeaderText_H.Add("列名1");
            colsHeaderText_H.Add("列名2");
            colsHeaderText_H.Add("列名3");
            colsHeaderText_H.Add("列名4");
            colsHeaderText_H.Add("列名5");
        }

        //绘制单元格
        private void dgvOutput_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            foreach (string fieldHeaderText in colsHeaderText_H)
            {
                //纵向合并
                if (e.ColumnIndex >= 0 && this.dgvOutput.Columns[e.ColumnIndex].HeaderText == fieldHeaderText && e.RowIndex >= 0)
                {
                    using (
                        Brush gridBrush = new SolidBrush(this.dgvOutput.GridColor),
                        backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                    {
                        using (Pen gridLinePen = new Pen(gridBrush))
                        {
                            // 擦除原单元格背景
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                            /****** 绘制单元格相互间隔的区分线条，datagridview自己会处理左侧和上边缘的线条，因此只需绘制下边框和和右边框
                             DataGridView控件绘制单元格时，不绘制左边框和上边框，共用左单元格的右边框，上一单元格的下边框*****/

                            //不是最后一行且单元格的值不为null
                            if (e.RowIndex < this.dgvOutput.RowCount - 1 && this.dgvOutput.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null)
                            {
                                //若与下一单元格值不同
                                if (e.Value.ToString() != this.dgvOutput.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
                                {
                                    //下边缘的线
                                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
                                    e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                                    //绘制值
                                    if (e.Value != null)
                                    {
                                        e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
                                            Brushes.Crimson, e.CellBounds.X + 2,
                                            e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                    }
                                }
                                //若与下一单元格值相同 
                                else
                                {
                                    //背景颜色
                                    //e.CellStyle.BackColor = Color.LightPink;   //仅在CellFormatting方法中可用
                                    //this.dgvMasterData.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightBlue;
                                    //this.dgvMasterData.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Style.BackColor = Color.LightBlue;
                                    //只读（以免双击单元格时显示值）
                                    this.dgvOutput.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                                    this.dgvOutput.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].ReadOnly = true;
                                }
                            }
                            //最后一行或单元格的值为null
                            else
                            {
                                //下边缘的线
                                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
                                    e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

                                //绘制值
                                if (e.Value != null)
                                {
                                    e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
                                        Brushes.Crimson, e.CellBounds.X + 2,
                                        e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                }
                            }

                            ////左侧的线（）
                            //e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                            //    e.CellBounds.Top, e.CellBounds.Left,
                            //    e.CellBounds.Bottom - 1);

                            //右侧的线
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                                e.CellBounds.Top, e.CellBounds.Right - 1,
                                e.CellBounds.Bottom - 1);

                            //设置处理事件完成（关键点），只有设置为ture,才能显示出想要的结果。
                            e.Handled = true;
                        }
                    }
                }
            }

            foreach (string fieldHeaderText in colsHeaderText_V)
            {
                //横向合并
                if (e.ColumnIndex >= 0 && this.dgvOutput.Columns[e.ColumnIndex].HeaderText == fieldHeaderText && e.RowIndex >= 0)
                {
                    using (
                        Brush gridBrush = new SolidBrush(this.dgvOutput.GridColor),
                        backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                    {
                        using (Pen gridLinePen = new Pen(gridBrush))
                        {
                            // 擦除原单元格背景
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                            /****** 绘制单元格相互间隔的区分线条，datagridview自己会处理左侧和上边缘的线条，因此只需绘制下边框和和右边框
                             DataGridView控件绘制单元格时，不绘制左边框和上边框，共用左单元格的右边框，上一单元格的下边框*****/

                            //不是最后一列且单元格的值不为null
                            if (e.ColumnIndex < this.dgvOutput.ColumnCount - 1 && this.dgvOutput.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value != null)
                            {
                                if (e.Value.ToString() != this.dgvOutput.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString())
                                {
                                    //右侧的线
                                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top,
                                        e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                                    //绘制值
                                    if (e.Value != null)
                                    {
                                        e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
                                            Brushes.Crimson, e.CellBounds.X + 2,
                                            e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                    }
                                }
                                //若与下一单元格值相同 
                                else
                                {
                                    //背景颜色
                                    //e.CellStyle.BackColor = Color.LightPink;   //仅在CellFormatting方法中可用
                                    //this.dgvMasterData.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightPink;
                                    //this.dgvMasterData.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Style.BackColor = Color.LightPink;
                                    //只读（以免双击单元格时显示值）
                                    this.dgvOutput.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                                    this.dgvOutput.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].ReadOnly = true;
                                }
                            }
                            else
                            {
                                //右侧的线
                                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top,
                                    e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

                                //绘制值
                                if (e.Value != null)
                                {
                                    e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
                                        Brushes.Crimson, e.CellBounds.X + 2,
                                        e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                }
                            }
                            //下边缘的线
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
                                                        e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                            e.Handled = true;
                        }
                    }

                }
            }

        }
        #endregion

        WebBrowser wb;  //定义公共控件
        //发送消息函数
        //参数：主标题，标题，单号，内容详情，时间，发送对象（如有重名建议在车辆系统个人信息修改名字加上数字顺序号）
        void SendToWechat(string zhubiaoti, string biaoti, string danhao, string neirong, string shijian, string to)
        {
            if (wb == null)
                wb = new WebBrowser();
            zhubiaoti = escape(zhubiaoti);
            biaoti = escape(biaoti);
            danhao = escape(danhao);
            neirong = escape(neirong);
            to = escape(to);
            string urlMsg = "http://39.106.133.149:8080/interface/jihua?token=jlholWERajfalseks&biaoti=" + biaoti + "&zhubiaoti=" + zhubiaoti + "&neirong=" + neirong + "&danhao=" + danhao + "&shijian=" + shijian + "&to=" + to;
            wb.Navigate(urlMsg);
        }
        string SendToObserver(string jhid, string sxid, string xingming, string riqi, string jhneirong, string jhtiaoshu)
        {
            if (wb == null)
            {
                wb = new WebBrowser();
            }
            jhid = escape(jhid);
            sxid = escape(sxid);
            jhneirong = escape(jhneirong);
            jhtiaoshu = escape(jhtiaoshu);
            xingming = escape(xingming);
            riqi = escape(riqi);
            string urlMsg = "http://10.19.7.88:8080/getplan?jhid=" + jhid + "&sxid=" + sxid + "&jhneirong=" + jhneirong + "&jhtiaoshu=" + jhtiaoshu + "&xingming=" + xingming + "&riqi=" + riqi; 
            wb.Navigate(urlMsg);
            Thread.Sleep(2000);
            return wb.DocumentText;
        }
        //对中文进行编码
        private string escape(string s)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            byte[] byteArr = System.Text.Encoding.Unicode.GetBytes(s);

            for (int i = 0; i < byteArr.Length; i += 2)
            {
                if (byteArr.Length > i + 1)
                {
                    sb.Append("%u");
                    sb.Append(byteArr[i + 1].ToString("X2"));//把字节转换为十六进制的字符串表现形式
                    sb.Append(byteArr[i].ToString("X2"));
                }
            }
            return sb.ToString();
        }
        //调用测试, 发送给测试用户“万维”，中文和各种符号
        private void btnSend_Click(object sender, EventArgs e)
        {
            string zhubiaoti, biaoti, danhao, neirong, shijian, to;
            DataRow dr;
            int counter;
            for (int i = 0; i < Schedules.Count; i++)
            {
                biaoti = ScheduleMaps.Tables[0].Rows[0]["项目"].ToString();
                danhao = ScheduleMaps.Tables[0].Rows[0]["订单号"].ToString();
                shijian = ((DateTime)ScheduleMaps.Tables[0].Rows[0]["开始时间"]).ToString("yyyy-MM-dd HH:mm");
                //shijian = "2018-07-12 13:05";
                neirong = "";
                to = Schedules[i].Worker.Name;
                counter = (int)Math.Ceiling((double)(Schedules[i].Steps.Count / 6.0));
                dr = OutPut.NewRow();
                dr = OutPut.Rows[i * (4 + 2) + 2]; 
                for (int k = 0; k < counter; k++)
                {
                    zhubiaoti = Schedules[i].Worker.Name + ", 您明天的工作内容如下:  (" + (k+1) + "/" + counter + ")";
                    for (int j = k * 6; j < (Schedules[i].Steps.Count - (k * 6) >= 6 ? (k+1)*6 : Schedules[i].Steps.Count); j++)
                    {
                        neirong += Schedules[i].Steps[j].StepName + "[" + dr[5 + j * 3] + "~" + dr[5 + j * 3 + 2] + "];";
                    }
                    SendToWechat(zhubiaoti, biaoti, danhao, neirong, shijian, to);
                    Thread.Sleep(1000);
                    neirong = "";
                }
                //sendmessage(zhubiaoti,biaoti, danhao, "%^&*(!@#$%^&*()_+b", shijian, to);
            }
        }

        private void btnSendtoObserver_Click(object sender, EventArgs e)
        {
            string jhneirong, jhtiaoshu, xingming, riqi;
            DataRow dr;
            jhtiaoshu = Schedules.Count.ToString();
            for (int i = 0; i < Schedules.Count; i++)
            {
                jhneirong = "";
                xingming = Schedules[i].Worker.Name;
                riqi = ((DateTime)ScheduleMaps.Tables[0].Rows[0]["开始时间"]).ToString("yyyy-MM-dd");
                for (int j = 0; j < Schedules[i].Steps.Count; j++)
                {
                    dr = OutPut.NewRow();
                    dr = OutPut.Rows[i * (4 + 2) + 2];
                    jhneirong += Schedules[i].Steps[j].StepName + "[" + dr[5 + j * 3] + "~" + dr[5 + j * 3 + 2] + "];";
                }
                SendToObserver("", i.ToString(), xingming, riqi, jhneirong, jhtiaoshu);
            }
        }
    }
}
