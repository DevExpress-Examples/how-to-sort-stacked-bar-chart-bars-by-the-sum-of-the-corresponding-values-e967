using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraCharts;

namespace ChartStackedSorting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chartControl1.Series.Clear();

            Series s1, s2, s3;

            s1 = new Series("Serie1", ViewType.StackedBar);
            s2 = new Series("Serie2", ViewType.StackedBar);
            s3 = new Series("Serie3", ViewType.StackedBar);

            Random r = new Random();

            for(int i = 0; i < 10; i++) {
                s1.Points.Add(new SeriesPoint(i, Math.Round(r.NextDouble() * 100)));
                s2.Points.Add(new SeriesPoint(i, Math.Round(r.NextDouble() * 100)));
                s3.Points.Add(new SeriesPoint(i, Math.Round(r.NextDouble() * 100)));
            }

            s1.Label.Visible = s2.Label.Visible = s3.Label.Visible = false;

            chartControl1.Series.AddRange(new Series[] { s1, s2, s3 });
        }

        private void button1_Click(object sender, EventArgs e) {
            ChartStackedSorting css = new ChartStackedSorting(chartControl1);

            css.SortChartSeries();
        }
    }

  public class ChartStackedSorting {
    private ChartControl chart;

    public ChartStackedSorting(ChartControl chart) {
        this.chart = chart;
    }

    public void SortChartSeries() {
        for(int i = 0; i < chart.Series[0].Points.Count; i++) {
            SeriesPoint p1 = chart.Series[0].Points[i];
            SeriesPoint pointMax = p1;
            int indexMax = i;

            double secondItemsMax = GetAdditionalValues(p1);

            for(int j = i + 1; j < chart.Series[0].Points.Count; j++) {
                SeriesPoint p2 = chart.Series[0].Points[j];

                double secondItems = GetAdditionalValues(p2);

                if(p2.Values[0] + secondItems > pointMax.Values[0] + secondItemsMax) {
                    pointMax = p2;
                    secondItemsMax = secondItems;
                    indexMax = j;
                }
            }

            chart.Series[0].Points.Swap(p1, pointMax);
            SwapAdditionalPoints(p1, pointMax);
        }
    }

    private double GetAdditionalValues(SeriesPoint point) {
        double result = 0.0;

        for(int i = 1; i < chart.Series.Count; i++)
            for(int j = 0; j < chart.Series[i].Points.Count; j++)
                if(Convert.ToDouble(chart.Series[i].Points[j].Argument) == Convert.ToDouble(point.Argument) && chart.Series[i].Points[j] != point)
                    result += chart.Series[i].Points[j].Values[0];

        return result;
    }

    private void SwapAdditionalPoints(SeriesPoint pi, SeriesPoint pj) {
        for(int s = 1; s < chart.Series.Count; s++) {

            SeriesPoint swp1 = null;
            SeriesPoint swp2 = null;
            
            for(int i = 0; i < chart.Series[s].Points.Count; i++) {
                if(Convert.ToDouble(chart.Series[s].Points[i].Argument) == Convert.ToDouble(pi.Argument))
                    swp1 = chart.Series[s].Points[i];
                if(Convert.ToDouble(chart.Series[s].Points[i].Argument) == Convert.ToDouble(pj.Argument))
                    swp2 = chart.Series[s].Points[i];
            }

            if(swp1 != null && swp2 != null)
                chart.Series[s].Points.Swap(swp1, swp2);
        }

    }

  }

}