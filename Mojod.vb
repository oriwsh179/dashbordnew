Imports DevExpress.XtraCharts
Imports FinanceDashboard.qrDataSetTableAdapters
Imports Guna.UI2.AnimatorNS

Public Class Mojod

    Private hoveredAffrgument As String = ""
    Private currentPageIndex As Integer = 0 ' البدء من الصفحة الأولى
    Private pages As DevExpress.XtraBars.Navigation.NavigationPage()
    Private WithEvents navTimer As New Timer()
    Private Sub ConfigureSplineChart(series As Series, chart As ChartControl, title As String,
                                    mainColor As Color, designType As String)

        ' إعداد التسميات
        series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True
        series.Label.TextPattern = "{V:n0}"
        series.Label.Font = New Font("Tajawal", 11, FontStyle.Bold)
        series.Label.BackColor = Color.FromArgb(240, 248, 255)
        series.Label.Border.Color = mainColor
        series.Label.Border.Thickness = 2
        series.Label.TextColor = Color.FromArgb(25, 25, 25)

        Dim splineView = CType(series.View, DevExpress.XtraCharts.SplineSeriesView)

        ' خط منحنى ناعم وسميك
        splineView.Color = mainColor
        splineView.LineStyle.Thickness = 5
        splineView.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Solid

        ' نقاط دائرية كبيرة
        splineView.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True
        splineView.LineMarkerOptions.Kind = DevExpress.XtraCharts.MarkerKind.Circle
        splineView.LineMarkerOptions.Size = 35
        splineView.LineMarkerOptions.BorderColor = Color.White
        splineView.LineMarkerOptions.BorderVisible = True
        splineView.ColorEach = True

        ' ألوان متدرجة زرقاء
        Dim blueColors() As Color = {
            Color.FromArgb(33, 150, 243),   ' أزرق فاتح
            Color.FromArgb(25, 118, 210),   ' أزرق متوسط
            Color.FromArgb(21, 101, 192),   ' أزرق داكن
            Color.FromArgb(13, 71, 161),    ' أزرق غامق
            Color.FromArgb(26, 35, 126)     ' أزرق ليلي
        }

        For i As Integer = 0 To Math.Min(series.Points.Count - 1, blueColors.Length - 1)
            series.Points(i).Color = blueColors(i)
        Next

        ' تأثير حركي ناعم
        Dim smoothEasing As New DevExpress.XtraCharts.CubicEasingFunction With {
            .EasingMode = DevExpress.XtraCharts.EasingMode.InOut
        }

        splineView.SeriesAnimation = New DevExpress.XtraCharts.XYSeriesUnwindAnimation With {
            .Direction = DevExpress.XtraCharts.AnimationDirection.FromLeft,
            .EasingFunction = smoothEasing,
            .BeginTime = TimeSpan.FromMilliseconds(200),
            .Duration = TimeSpan.FromMilliseconds(2200)
        }

        ConfigureChart(chart, series, title, mainColor, Color.FromArgb(240, 248, 255))
    End Sub
    Private Sub ConfigureChart(chart As ChartControl, series As Series, title As String,
                              mainColor As Color, bgColor As Color)

        chart.Series.Add(series)

        ' إعداد المحاور
        Dim diagram = TryCast(chart.Diagram, DevExpress.XtraCharts.XYDiagram)
        If diagram IsNot Nothing Then
            With diagram.AxisX
                .Label.Font = New Font("Tajawal", 10, FontStyle.Bold)
                .Label.Angle = -45
                .Label.TextPattern = "{A}"
                .Label.TextColor = Color.FromArgb(60, 60, 60)
                .Title.Text = "مكتب/قسم/فرع"
                .Title.Visibility = DevExpress.Utils.DefaultBoolean.True
                .Title.Font = New Font("Tajawal", 11, FontStyle.Bold)
                .Title.TextColor = mainColor
                .GridLines.Visible = True
                .GridLines.Color = Color.FromArgb(230, 230, 230)
                .GridLines.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dot
            End With

            With diagram.AxisY
                .Label.Font = New Font("Tajawal", 10, FontStyle.Bold)
                .Label.TextPattern = "{V:n0}"
                .Label.TextColor = Color.FromArgb(60, 60, 60)
                '.Title.Text = "💰 إجمالي الرواتب"
                .Title.Visibility = DevExpress.Utils.DefaultBoolean.True
                .Title.Font = New Font("Tajawal", 11, FontStyle.Bold)
                .Title.TextColor = mainColor
                .GridLines.Visible = True
                .GridLines.Color = Color.FromArgb(240, 240, 240)
                .GridLines.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dot
            End With
        End If

        ' عنوان أنيق
        chart.Titles.Clear()
        chart.Titles.Add(New DevExpress.XtraCharts.ChartTitle With {
            .Text = title,
            .Font = New Font("Tajawal", 13, FontStyle.Bold),
            .TextColor = Color.FromArgb(25, 25, 25),
            .Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Top,
            .Alignment = StringAlignment.Center
        })

        ' خلفية مميزة
        chart.BackColor = bgColor
        chart.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False
        chart.AnimationStartMode = DevExpress.XtraCharts.ChartAnimationMode.OnLoad

        ' Crosshair محسن
        With chart.CrosshairOptions
            .ShowArgumentLabels = True
            .ShowValueLabels = True
            .ShowCrosshairLabels = True
            .CrosshairLabelMode = DevExpress.XtraCharts.CrosshairLabelMode.ShowForNearestSeries
            .GroupHeaderPattern = "📊 {A}"
        End With

        ' تلميحات الأدوات
        chart.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True
        chart.ToolTipOptions.ShowForPoints = True
    End Sub
    Private Sub Chart_Bind()
        Try

            ' 1. تحميل البيانات من الجدول
            Dim dt As New qrDataSet.taDataTable()
            Dim adapter As New qrDataSetTableAdapters.taTableAdapter()
            adapter.Fill(dt)

            ' 2. استخراج آخر 5 أشهر حسب code_2
            Dim uniqueMonths = dt.AsEnumerable().
                Where(Function(r) Not IsDBNull(r("che1")) AndAlso Not IsDBNull(r("che")) AndAlso Not IsDBNull(r("m"))).
                GroupBy(Function(r) CInt(r("m"))).
                Select(Function(g) New With {
                    .code = g.Key,
                    .total = g.Sum(Function(x) CDec(x("S32")))
                }).
                OrderByDescending(Function(x) x.code).
                Take(5).
                OrderBy(Function(x) x.code).ToList()

            ' 3. مسح أي بيانات سابقة
            ChartControl1.Series.Clear()

            ' 4. إنشاء Series - خط منحنى ناعم
            Dim series = New DevExpress.XtraCharts.Series("رواتب الحركة", DevExpress.XtraCharts.ViewType.Spline)
            series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative

            ' 5. إضافة النقاط
            For Each item In uniqueMonths
                Dim codeStr As String = item.code.ToString()
                If codeStr.Length = 5 Then codeStr = codeStr.Insert(4, "0")
                If codeStr.Length <> 6 Then Continue For

                Dim year As Integer = CInt(codeStr.Substring(0, 4))
                Dim month As Integer = CInt(codeStr.Substring(4, 2))
                Dim label As String = $"{month:D2}/{year}"

                series.Points.Add(New DevExpress.XtraCharts.SeriesPoint(label, item.total))
            Next

            ' 6. تصميم مميز للسلسلة الأولى - ألوان زرقاء متدرجة
            ConfigureSplineChart(series, ChartControl1, "📊 مؤشر رواتب الحركة - آخر 5 أشهر",
                               Color.FromArgb(33, 150, 243), "التصميم الأول")

        Catch ex As Exception
            MessageBox.Show($"خطأ في ربط بيانات رواتب الحركة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Async Sub Malomtee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Try
        '    ' تحديث البيانات الأساسية
        '    RefreshData()
        '    Await Task.Delay(100)
        '    ShowWithAnimation(panelVehicles, AnimationType.Mosaic, 1500, 0.03, 20)

        '    Await Task.Delay(2600)
        '    Me.Prymare_EjarTableAdapter.Fill(Me.FinanceDataSet.Prymare_Ejar)
        '    ShowWithAnimation(PanelControl14, AnimationType.Mosaic, 1500, 0.03, 20)

        '    Await Task.Delay(2600)
        '    Me.AsstesTableAdapter.Fill(Me.FinanceDataSet.Asstes)
        '    ShowWithAnimation(PanelControl88, AnimationType.Mosaic, 1500, 0.03, 20)

        '    Await Task.Delay(2600)
        '    ShowWithAnimation(PanelControl95, AnimationType.Mosaic, 1500, 0.03, 20)

        '    ' إعداد الصفحات
        '    pages = {NavigationPage1, NavigationPage2, NavigationPage3}

        '    ' بدء تشغيل الصفحة الأولى فوراً
        '    NavigationFrame1.SelectedPage = NavigationPage1
        '    Chart_Bind() ' تحميل بيانات الصفحة الأولى

        '    ' إعداد المؤقت للتنقل التلقائي
        '    navTimer.Interval = 8000 ' 8 ثواني لكل صفحة
        '    navTimer.Start()

        '    ' مراقبة حركة الماوس
        '    AddHandler NavigationFrame1.MouseEnter, AddressOf NavigationFrame1_MouseEnter
        '    AddHandler NavigationFrame1.MouseLeave, AddressOf NavigationFrame1_MouseLeave

        '    ' تفعيل أحداث التتبع للرسوم البيانية
        '    AddHandler ChartControl1.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
        '    AddHandler ChartControl5.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
        '    AddHandler ChartControl2.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
        '    AddHandler ChartControl9.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
        '    AddHandler ChartControl7.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
        '    AddHandler ChartControl35.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
        '    AddHandler ChartControl34.ObjectHotTracked, AddressOf Chart_ObjectHotTracked

        '    tblMain.AutoScroll = True

        'Catch ex As Exception
        '    MessageBox.Show($"خطأ في تحميل النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
        Await Task.Delay(50)
        Me.Table_1TableAdapter.Fill(Me.QrDataSet.Table_1)
        Me.TaTableAdapter.Fill(Me.QrDataSet.ta)
        Mainte.PanelControl2.Visible = False
        Mainte.panelVisible = False
        Mainte.isAnimating = False
        AddHandler ChartControl9.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
    End Sub


    Private Sub ShowWithAnimation(targetControl As Control,
                             animType As AnimationType,
                             Optional duration As Integer = 800,
                             Optional timeStep As Double = 0.02,
                             Optional interval As Integer = 1)

        With Guna2Transition1
            .AnimationType = animType
            .MaxAnimationTime = duration
            .TimeStep = timeStep
            .Interval = interval
            .ShowSync(targetControl, False, Nothing)
        End With
    End Sub

    ''' <summary>
    ''' تتبع حركة الماوس على الرسوم البيانية
    ''' </summary>
    Private Sub Chart_ObjectHotTracked(sender As Object, e As HotTrackEventArgs)
        Dim chart = CType(sender, ChartControl)
        If chart.Series.Count = 0 Then Return

        Dim series = chart.Series(0)
        Dim view = TryCast(series.View, PieSeriesView)
        If view Is Nothing Then Return

        ' تغيير المؤشر حسب الموقع
        chart.Cursor = If(e.HitInfo.SeriesPoint IsNot Nothing, Cursors.Hand, Cursors.Default)

        Dim currentArg As String = If(e.HitInfo.SeriesPoint?.Argument, "")

        ' إذا خرج الماوس من كل القطع
        If String.IsNullOrEmpty(currentArg) Then
            hoveredAffrgument = ""
            view.ExplodeMode = PieExplodeMode.All
            view.ExplodedDistancePercentage = 1
            view.ExplodedPointsFilters.Clear()
            Exit Sub
        End If

        ' إذا نفس القطعة، لا تعيد
        If currentArg = hoveredAffrgument Then Exit Sub

        ' إذا قطعة جديدة
        hoveredAffrgument = currentArg
        view.ExplodeMode = PieExplodeMode.UseFilters
        view.ExplodedDistancePercentage = 10 ' تأثير انفجار أكثر وضوحاً
        view.ExplodedPointsFilters.Clear()
        view.ExplodedPointsFilters.Add(
        New SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.Equal, currentArg)
    )
    End Sub

    ''' <summary>
    ''' إغلاق التطبيق مع تأكيد
    ''' </summary>
    Private Sub LabelControl18_Click(sender As Object, e As EventArgs)
        ' تأكيد الإغلاق
        Dim result = MessageBox.Show("هل تريد إغلاق التطبيق؟", "تأكيد الإغلاق",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' إيقاف المؤقت قبل الإغلاق
            If navTimer IsNot Nothing Then
                navTimer.Stop()
                navTimer.Dispose()
            End If

            Application.Exit()
        End If
    End Sub


End Class