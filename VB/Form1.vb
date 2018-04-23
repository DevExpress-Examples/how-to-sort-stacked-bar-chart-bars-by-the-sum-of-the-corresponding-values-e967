Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraCharts

Namespace ChartStackedSorting
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			chartControl1.Series.Clear()

			Dim s1, s2, s3 As Series

			s1 = New Series("Serie1", ViewType.StackedBar)
			s2 = New Series("Serie2", ViewType.StackedBar)
			s3 = New Series("Serie3", ViewType.StackedBar)
			s1.ArgumentScaleType = ScaleType.Qualitative
			s2.ArgumentScaleType = ScaleType.Qualitative
			s3.ArgumentScaleType = ScaleType.Qualitative
			Dim r As New Random()

			For i As Integer = 0 To 9
				s1.Points.Add(New SeriesPoint(i, Math.Round(r.NextDouble() * 100)))
				s2.Points.Add(New SeriesPoint(i, Math.Round(r.NextDouble() * 100)))
				s3.Points.Add(New SeriesPoint(i, Math.Round(r.NextDouble() * 100)))
			Next i

			s3.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False
			s2.LabelsVisibility = s3.LabelsVisibility
			s1.LabelsVisibility = s2.LabelsVisibility

			chartControl1.Series.AddRange(New Series() { s1, s2, s3 })
		End Sub

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			Dim css As New ChartStackedSorting(chartControl1)

			css.SortChartSeries()
			chartControl1.RefreshData()
		End Sub
	End Class

  Public Class ChartStackedSorting
	Private chart As ChartControl

	Public Sub New(ByVal chart As ChartControl)
		Me.chart = chart
	End Sub

	Public Sub SortChartSeries()
		For i As Integer = 0 To chart.Series(0).Points.Count - 1
			Dim p1 As SeriesPoint = chart.Series(0).Points(i)
			Dim pointMax As SeriesPoint = p1
			Dim indexMax As Integer = i

			Dim secondItemsMax As Double = GetAdditionalValues(p1)

			For j As Integer = i + 1 To chart.Series(0).Points.Count - 1
				Dim p2 As SeriesPoint = chart.Series(0).Points(j)

				Dim secondItems As Double = GetAdditionalValues(p2)

				If p2.Values(0) + secondItems > pointMax.Values(0) + secondItemsMax Then
					pointMax = p2
					secondItemsMax = secondItems
					indexMax = j
				End If
			Next j

			chart.Series(0).Points.Swap(p1, pointMax)
			SwapAdditionalPoints(p1, pointMax)
		Next i
	End Sub

	Private Function GetAdditionalValues(ByVal point As SeriesPoint) As Double
		Dim result As Double = 0.0

		For i As Integer = 1 To chart.Series.Count - 1
			For j As Integer = 0 To chart.Series(i).Points.Count - 1
				If Convert.ToDouble(chart.Series(i).Points(j).Argument) = Convert.ToDouble(point.Argument) AndAlso chart.Series(i).Points(j) IsNot point Then
					result += chart.Series(i).Points(j).Values(0)
				End If
			Next j
		Next i

		Return result
	End Function

	Private Sub SwapAdditionalPoints(ByVal pi As SeriesPoint, ByVal pj As SeriesPoint)
		For s As Integer = 1 To chart.Series.Count - 1

			Dim swp1 As SeriesPoint = Nothing
			Dim swp2 As SeriesPoint = Nothing

			For i As Integer = 0 To chart.Series(s).Points.Count - 1
				If Convert.ToDouble(chart.Series(s).Points(i).Argument) = Convert.ToDouble(pi.Argument) Then
					swp1 = chart.Series(s).Points(i)
				End If
				If Convert.ToDouble(chart.Series(s).Points(i).Argument) = Convert.ToDouble(pj.Argument) Then
					swp2 = chart.Series(s).Points(i)
				End If
			Next i

			If swp1 IsNot Nothing AndAlso swp2 IsNot Nothing Then
				chart.Series(s).Points.Swap(swp1, swp2)
			End If
		Next s

	End Sub

  End Class

End Namespace