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

			Dim r As New Random()

			For i As Integer = 0 To 9
				s1.Points.Add(New SeriesPoint(i, Math.Round(r.NextDouble() * 100)))
				s2.Points.Add(New SeriesPoint(i, Math.Round(r.NextDouble() * 100)))
				s3.Points.Add(New SeriesPoint(i, Math.Round(r.NextDouble() * 100)))
			Next i

			s3.Label.Visible = False
			s2.Label.Visible = s3.Label.Visible
			s1.Label.Visible = s2.Label.Visible

			chartControl1.Series.AddRange(New Series() { s1, s2, s3 })
		End Sub

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			Dim css As New ChartStackedSorting(chartControl1)

			css.SortChartSeries()
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

				Dim secondItemsMax As Double = GetAdditionalValues(i)

				For j As Integer = i + 1 To chart.Series(0).Points.Count - 1
					Dim p2 As SeriesPoint = chart.Series(0).Points(j)

					Dim secondItems As Double = GetAdditionalValues(j)

					If p2.Values(0) + secondItems > pointMax.Values(0) + secondItemsMax Then
						pointMax = p2
						secondItemsMax = secondItems
						indexMax = j
					End If
				Next j

				chart.Series(0).Points.Swap(p1, pointMax)
				SwapAdditionalPoints(i, indexMax)
			Next i
		End Sub

		Private Function GetAdditionalValues(ByVal pointIndex As Integer) As Double
			Dim result As Double = 0.0

			For i As Integer = 1 To chart.Series.Count - 1
				result += chart.Series(i).Points(pointIndex).Values(0)
			Next i

			Return result
		End Function

		Private Sub SwapAdditionalPoints(ByVal i As Integer, ByVal j As Integer)
			For s As Integer = 1 To chart.Series.Count - 1
				chart.Series(s).Points.Swap(chart.Series(s).Points(i), chart.Series(s).Points(j))
			Next s
		End Sub

	End Class

End Namespace