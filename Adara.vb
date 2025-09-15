Imports DevExpress.XtraCharts
Imports Guna.UI2.AnimatorNS

Public Class Adara

    Private hoveredAffrgument As String = ""
    Private currentPageIndex As Integer = 0 ' البدء من الصفحة الأولى
    Private pages As DevExpress.XtraBars.Navigation.NavigationPage()
    Private WithEvents navTimer As New Timer()

    ''' <summary>
    ''' رسم بياني لرواتب الحركة - التصميم الأول
    ''' </summary>
    'Private Sub Chart_Bind()
    '    Try
    '        ' 1. تحميل البيانات من الجدول
    '        Dim dt As New FinanceDataSet.SALARE_HDataTable()
    '        Dim adapter As New FinanceDataSetTableAdapters.SALARE_HTableAdapter()
    '        adapter.Fill(dt)

    '        ' 2. استخراج آخر 5 أشهر حسب code_2
    '        Dim uniqueMonths = dt.AsEnumerable().
    '            Where(Function(r) Not IsDBNull(r("S32")) AndAlso Not IsDBNull(r("code_2"))).
    '            GroupBy(Function(r) CInt(r("code_2"))).
    '            Select(Function(g) New With {
    '                .code = g.Key,
    '                .total = g.Sum(Function(x) CDec(x("S32")))
    '            }).
    '            OrderByDescending(Function(x) x.code).
    '            Take(5).
    '            OrderBy(Function(x) x.code).ToList()

    '        ' 3. مسح أي بيانات سابقة
    '        ChartControl36.Series.Clear()

    '        ' 4. إنشاء Series - خط منحنى ناعم
    '        Dim series = New DevExpress.XtraCharts.Series("رواتب الحركة", DevExpress.XtraCharts.ViewType.Spline)
    '        series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative

    '        ' 5. إضافة النقاط
    '        For Each item In uniqueMonths
    '            Dim codeStr As String = item.code.ToString()
    '            If codeStr.Length = 5 Then codeStr = codeStr.Insert(4, "0")
    '            If codeStr.Length <> 6 Then Continue For

    '            Dim year As Integer = CInt(codeStr.Substring(0, 4))
    '            Dim month As Integer = CInt(codeStr.Substring(4, 2))
    '            Dim label As String = $"{month:D2}/{year}"

    '            series.Points.Add(New DevExpress.XtraCharts.SeriesPoint(label, item.total))
    '        Next

    '        ' 6. تصميم مميز للسلسلة الأولى - ألوان زرقاء متدرجة
    '        ConfigureSplineChart(series, ChartControl36, "📊 مؤشر رواتب الحركة - آخر 5 أشهر",
    '                           Color.FromArgb(33, 150, 243), "التصميم الأول")

    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في ربط بيانات رواتب الحركة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' رسم بياني لرواتب القناة - التصميم الثاني
    '''' </summary>
    'Private Sub Chart_Bind_AHAD()
    '    Try
    '        ' 1. تحميل البيانات من الجدول
    '        Dim dt As New FinanceDataSet.SALARE_H_AHADDataTable
    '        Dim adapter As New FinanceDataSetTableAdapters.SALARE_H_AHADTableAdapter()
    '        adapter.Fill(dt)

    '        ' 2. استخراج آخر 5 أشهر حسب code_1
    '        Dim uniqueMonths = dt.AsEnumerable().
    '            Where(Function(r) Not IsDBNull(r("FINAL_SALARE")) AndAlso Not IsDBNull(r("code_1"))).
    '            GroupBy(Function(r) CInt(r("code_1"))).
    '            Select(Function(g) New With {
    '                .code = g.Key,
    '                .total = g.Sum(Function(x) CDec(x("FINAL_SALARE")))
    '            }).
    '            OrderByDescending(Function(x) x.code).
    '            Take(5).
    '            OrderBy(Function(x) x.code).ToList()

    '        ' 3. مسح أي بيانات سابقة
    '        ChartControl37.Series.Clear()

    '        ' 4. إنشاء Series - خط مستقيم بزوايا
    '        Dim series = New DevExpress.XtraCharts.Series("رواتب القناة", DevExpress.XtraCharts.ViewType.Line)
    '        series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative

    '        ' 5. إضافة النقاط
    '        For Each item In uniqueMonths
    '            Dim codeStr As String = item.code.ToString()
    '            If codeStr.Length = 5 Then codeStr = codeStr.Insert(4, "0")
    '            If codeStr.Length <> 6 Then Continue For

    '            Dim year As Integer = CInt(codeStr.Substring(0, 4))
    '            Dim month As Integer = CInt(codeStr.Substring(4, 2))
    '            Dim label As String = $"{month:D2}/{year}"

    '            series.Points.Add(New DevExpress.XtraCharts.SeriesPoint(label, item.total))
    '        Next

    '        ' 6. تصميم مميز للسلسلة الثانية - ألوان خضراء
    '        ConfigureLineChart(series, ChartControl37, "🏢 مؤشر رواتب القناة - آخر 5 أشهر",
    '                          Color.FromArgb(76, 175, 80), "التصميم الثاني")

    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في ربط بيانات رواتب القناة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' رسم بياني لرواتب الإعلامي - التصميم الثالث
    '''' </summary>
    'Private Sub Chart_Bind_IELAM()
    '    Try
    '        ' 1. تحميل البيانات من الجدول
    '        Dim dt As New FinanceDataSet.F_SALARE_HDataTable
    '        Dim adapter As New FinanceDataSetTableAdapters.F_SALARE_HTableAdapter()
    '        adapter.Fill(dt)

    '        ' 2. استخراج آخر 5 أشهر حسب code_1
    '        Dim uniqueMonths = dt.AsEnumerable().
    '            Where(Function(r) Not IsDBNull(r("FINAL_SALARE")) AndAlso Not IsDBNull(r("code_1"))).
    '            GroupBy(Function(r) CInt(r("code_1"))).
    '            Select(Function(g) New With {
    '                .code = g.Key,
    '                .total = g.Sum(Function(x) CDec(x("FINAL_SALARE")))
    '            }).
    '            OrderByDescending(Function(x) x.code).
    '            Take(5).
    '            OrderBy(Function(x) x.code).ToList()

    '        ' 3. مسح أي بيانات سابقة
    '        ChartControl38.Series.Clear()

    '        ' 4. إنشاء Series - خط منحنى ناعم
    '        Dim series = New DevExpress.XtraCharts.Series("رواتب الاعلامي", DevExpress.XtraCharts.ViewType.Spline)
    '        series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative

    '        ' 5. إضافة النقاط
    '        For Each item In uniqueMonths
    '            Dim codeStr As String = item.code.ToString()
    '            If codeStr.Length = 5 Then codeStr = codeStr.Insert(4, "0")
    '            If codeStr.Length <> 6 Then Continue For

    '            Dim year As Integer = CInt(codeStr.Substring(0, 4))
    '            Dim month As Integer = CInt(codeStr.Substring(4, 2))
    '            Dim label As String = $"{month:D2}/{year}"

    '            series.Points.Add(New DevExpress.XtraCharts.SeriesPoint(label, item.total))
    '        Next

    '        ' 6. تصميم مميز للسلسلة الثالثة - خط منحنى بنفسجي
    '        ConfigureSplineChartPurple(series, ChartControl38, "📺 مؤشر رواتب الإعلامي - آخر 5 أشهر",
    '                          Color.FromArgb(156, 39, 176), "التصميم الثالث")

    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في ربط بيانات رواتب الإعلامي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' إعداد الرسم البياني المنحنى - التصميم الأول
    '''' </summary>
    'Private Sub ConfigureSplineChart(series As Series, chart As ChartControl, title As String,
    '                                mainColor As Color, designType As String)

    '    ' إعداد التسميات
    '    series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True
    '    series.Label.TextPattern = "{V:n0}"
    '    series.Label.Font = New Font("Tajawal", 11, FontStyle.Bold)
    '    series.Label.BackColor = Color.FromArgb(240, 248, 255)
    '    series.Label.Border.Color = mainColor
    '    series.Label.Border.Thickness = 2
    '    series.Label.TextColor = Color.FromArgb(25, 25, 25)

    '    Dim splineView = CType(series.View, DevExpress.XtraCharts.SplineSeriesView)

    '    ' خط منحنى ناعم وسميك
    '    splineView.Color = mainColor
    '    splineView.LineStyle.Thickness = 5
    '    splineView.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Solid

    '    ' نقاط دائرية كبيرة
    '    splineView.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True
    '    splineView.LineMarkerOptions.Kind = DevExpress.XtraCharts.MarkerKind.Circle
    '    splineView.LineMarkerOptions.Size = 35
    '    splineView.LineMarkerOptions.BorderColor = Color.White
    '    splineView.LineMarkerOptions.BorderVisible = True
    '    splineView.ColorEach = True

    '    ' ألوان متدرجة زرقاء
    '    Dim blueColors() As Color = {
    '        Color.FromArgb(33, 150, 243),   ' أزرق فاتح
    '        Color.FromArgb(25, 118, 210),   ' أزرق متوسط
    '        Color.FromArgb(21, 101, 192),   ' أزرق داكن
    '        Color.FromArgb(13, 71, 161),    ' أزرق غامق
    '        Color.FromArgb(26, 35, 126)     ' أزرق ليلي
    '    }

    '    For i As Integer = 0 To Math.Min(series.Points.Count - 1, blueColors.Length - 1)
    '        series.Points(i).Color = blueColors(i)
    '    Next

    '    ' تأثير حركي ناعم
    '    Dim smoothEasing As New DevExpress.XtraCharts.CubicEasingFunction With {
    '        .EasingMode = DevExpress.XtraCharts.EasingMode.InOut
    '    }

    '    splineView.SeriesAnimation = New DevExpress.XtraCharts.XYSeriesUnwindAnimation With {
    '        .Direction = DevExpress.XtraCharts.AnimationDirection.FromLeft,
    '        .EasingFunction = smoothEasing,
    '        .BeginTime = TimeSpan.FromMilliseconds(200),
    '        .Duration = TimeSpan.FromMilliseconds(2200)
    '    }

    '    ConfigureChart(chart, series, title, mainColor, Color.FromArgb(240, 248, 255))
    'End Sub

    '''' <summary>
    '''' إعداد الرسم البياني الخطي - التصميم الثاني
    '''' </summary>
    'Private Sub ConfigureLineChart(series As Series, chart As ChartControl, title As String,
    '                              mainColor As Color, designType As String)

    '    ' إعداد التسميات
    '    series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True
    '    series.Label.TextPattern = "{V:n0}"
    '    series.Label.Font = New Font("Tajawal", 11, FontStyle.Bold)
    '    series.Label.BackColor = Color.FromArgb(232, 245, 233)
    '    series.Label.Border.Color = mainColor
    '    series.Label.Border.Thickness = 2
    '    series.Label.TextColor = Color.FromArgb(27, 94, 32)

    '    Dim lineView = CType(series.View, DevExpress.XtraCharts.LineSeriesView)

    '    ' خط مستقيم بزوايا حادة
    '    lineView.Color = mainColor
    '    lineView.LineStyle.Thickness = 6
    '    lineView.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Solid

    '    ' نقاط مربعة
    '    lineView.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True
    '    lineView.LineMarkerOptions.Kind = DevExpress.XtraCharts.MarkerKind.Square
    '    lineView.LineMarkerOptions.Size = 32
    '    lineView.LineMarkerOptions.BorderColor = Color.White
    '    lineView.LineMarkerOptions.BorderVisible = True
    '    lineView.ColorEach = True

    '    ' ألوان متدرجة خضراء
    '    Dim greenColors() As Color = {
    '        Color.FromArgb(76, 175, 80),    ' أخضر فاتح
    '        Color.FromArgb(67, 160, 71),    ' أخضر متوسط
    '        Color.FromArgb(56, 142, 60),    ' أخضر داكن
    '        Color.FromArgb(46, 125, 50),    ' أخضر غامق
    '        Color.FromArgb(27, 94, 32)      ' أخضر ليلي
    '    }

    '    For i As Integer = 0 To Math.Min(series.Points.Count - 1, greenColors.Length - 1)
    '        series.Points(i).Color = greenColors(i)
    '    Next

    '    ' تأثير حركي قوي
    '    Dim bounceEasing As New DevExpress.XtraCharts.BounceEasingFunction With {
    '        .EasingMode = DevExpress.XtraCharts.EasingMode.Out
    '    }

    '    lineView.SeriesAnimation = New DevExpress.XtraCharts.XYSeriesUnwindAnimation With {
    '        .Direction = DevExpress.XtraCharts.AnimationDirection.FromRight,
    '        .EasingFunction = bounceEasing,
    '        .BeginTime = TimeSpan.FromMilliseconds(300),
    '        .Duration = TimeSpan.FromMilliseconds(2000)
    '    }

    '    ConfigureChart(chart, series, title, mainColor, Color.FromArgb(232, 245, 233))
    'End Sub

    '''' <summary>
    '''' إعداد الرسم البياني المنحنى البنفسجي - التصميم الثالث
    '''' </summary>
    'Private Sub ConfigureSplineChartPurple(series As Series, chart As ChartControl, title As String,
    '                                      mainColor As Color, designType As String)

    '    ' إعداد التسميات
    '    series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True
    '    series.Label.TextPattern = "{V:n0}"
    '    series.Label.Font = New Font("Tajawal", 11, FontStyle.Bold)
    '    series.Label.BackColor = Color.FromArgb(243, 229, 245)
    '    series.Label.Border.Color = mainColor
    '    series.Label.Border.Thickness = 2
    '    series.Label.TextColor = Color.FromArgb(74, 20, 140)

    '    Dim splineView = CType(series.View, DevExpress.XtraCharts.SplineSeriesView)

    '    ' خط منحنى ناعم بنفسجي
    '    splineView.Color = mainColor
    '    splineView.LineStyle.Thickness = 5
    '    splineView.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Solid

    '    ' نقاط معينية
    '    splineView.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True
    '    splineView.LineMarkerOptions.Kind = DevExpress.XtraCharts.MarkerKind.Diamond
    '    splineView.LineMarkerOptions.Size = 30
    '    splineView.LineMarkerOptions.BorderColor = Color.White
    '    splineView.LineMarkerOptions.BorderVisible = True
    '    splineView.ColorEach = True

    '    ' ألوان متدرجة بنفسجية
    '    Dim purpleColors() As Color = {
    '        Color.FromArgb(156, 39, 176),   ' بنفسجي فاتح
    '        Color.FromArgb(142, 36, 170),   ' بنفسجي متوسط
    '        Color.FromArgb(123, 31, 162),   ' بنفسجي داكن
    '        Color.FromArgb(106, 27, 154),   ' بنفسجي غامق
    '        Color.FromArgb(74, 20, 140)     ' بنفسجي ليلي
    '    }

    '    For i As Integer = 0 To Math.Min(series.Points.Count - 1, purpleColors.Length - 1)
    '        series.Points(i).Color = purpleColors(i)
    '    Next

    '    ' تأثير حركي مطاطي
    '    Dim elasticEasing As New DevExpress.XtraCharts.ElasticEasingFunction With {
    '        .EasingMode = DevExpress.XtraCharts.EasingMode.Out
    '    }

    '    splineView.SeriesAnimation = New DevExpress.XtraCharts.XYSeriesUnwindAnimation With {
    '        .Direction = DevExpress.XtraCharts.AnimationDirection.FromBottom,
    '        .EasingFunction = elasticEasing,
    '        .BeginTime = TimeSpan.FromMilliseconds(400),
    '        .Duration = TimeSpan.FromMilliseconds(2500)
    '    }

    '    ConfigureChart(chart, series, title, mainColor, Color.FromArgb(243, 229, 245))
    'End Sub

    '''' <summary>
    '''' إعداد مشترك للرسوم البيانية
    '''' </summary>
    'Private Sub ConfigureChart(chart As ChartControl, series As Series, title As String,
    '                          mainColor As Color, bgColor As Color)

    '    chart.Series.Add(series)

    '    ' إعداد المحاور
    '    Dim diagram = TryCast(chart.Diagram, DevExpress.XtraCharts.XYDiagram)
    '    If diagram IsNot Nothing Then
    '        With diagram.AxisX
    '            .Label.Font = New Font("Tajawal", 10, FontStyle.Bold)
    '            .Label.Angle = -45
    '            .Label.TextPattern = "{A}"
    '            .Label.TextColor = Color.FromArgb(60, 60, 60)
    '            .Title.Text = "📅 الشهر / السنة"
    '            .Title.Visibility = DevExpress.Utils.DefaultBoolean.True
    '            .Title.Font = New Font("Tajawal", 11, FontStyle.Bold)
    '            .Title.TextColor = mainColor
    '            .GridLines.Visible = True
    '            .GridLines.Color = Color.FromArgb(230, 230, 230)
    '            .GridLines.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dot
    '        End With

    '        With diagram.AxisY
    '            .Label.Font = New Font("Tajawal", 10, FontStyle.Bold)
    '            .Label.TextPattern = "{V:n0}"
    '            .Label.TextColor = Color.FromArgb(60, 60, 60)
    '            .Title.Text = "💰 إجمالي الرواتب"
    '            .Title.Visibility = DevExpress.Utils.DefaultBoolean.True
    '            .Title.Font = New Font("Tajawal", 11, FontStyle.Bold)
    '            .Title.TextColor = mainColor
    '            .GridLines.Visible = True
    '            .GridLines.Color = Color.FromArgb(240, 240, 240)
    '            .GridLines.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dot
    '        End With
    '    End If

    '    ' عنوان أنيق
    '    chart.Titles.Clear()
    '    chart.Titles.Add(New DevExpress.XtraCharts.ChartTitle With {
    '        .Text = title,
    '        .Font = New Font("Tajawal", 13, FontStyle.Bold),
    '        .TextColor = Color.FromArgb(25, 25, 25),
    '        .Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Top,
    '        .Alignment = StringAlignment.Center
    '    })

    '    ' خلفية مميزة
    '    chart.BackColor = bgColor
    '    chart.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False
    '    chart.AnimationStartMode = DevExpress.XtraCharts.ChartAnimationMode.OnLoad

    '    ' Crosshair محسن
    '    With chart.CrosshairOptions
    '        .ShowArgumentLabels = True
    '        .ShowValueLabels = True
    '        .ShowCrosshairLabels = True
    '        .CrosshairLabelMode = DevExpress.XtraCharts.CrosshairLabelMode.ShowForNearestSeries
    '        .GroupHeaderPattern = "📊 {A}"
    '    End With

    '    ' تلميحات الأدوات
    '    chart.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True
    '    chart.ToolTipOptions.ShowForPoints = True
    'End Sub

    ''' <summary>
    ''' تحديث البيانات
    ''' </summary>
    Public Sub RefreshData()
        Try
            ' إعادة تحميل البيانات
            Me.Db_sadrTableAdapter.Fill(Me.FinanceDataSet2.db_sadr)
            Me.CAR_ADWAT_ATableAdapter.Fill(Me.FinanceDataSet.CAR_ADWAT_A)
            Me.CAR_HKOME_ATableAdapter.Fill(Me.FinanceDataSet.CAR_HKOME_A)

            ' إعادة تعيين الحالة
            hoveredAffrgument = ""

            ' تحديث الرسوم البيانية
            'ChartControl5.RefreshData()
            ChartControl1.RefreshData()
            ChartControl2.RefreshData()

        Catch ex As Exception
            MessageBox.Show("خطأ في تحديث البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحميل النموذج
    ''' </summary>
    Private Async Sub Malomtee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            navTimer.Stop()
            'NavigationFrame1.Visible = False

            ' تحديث البيانات الأساسية
            RefreshData()
            'Mainte.StartSafeHideTimer()
            Await Task.Delay(100)
            ShowWithAnimation(panelVehicles, AnimationType.Mosaic, 1500, 0.03, 20)

            Await Task.Delay(2400)
            Me.Db_wardTableAdapter.Fill(Me.FinanceDataSet2.db_ward)
            ShowWithAnimation(PanelControl14, AnimationType.Mosaic, 1500, 0.03, 20)

            Await Task.Delay(2400)
            Me.Db_outTableAdapter.Fill(Me.FinanceDataSet2.db_out)
            ShowWithAnimation(PanelControl88, AnimationType.Mosaic, 1500, 0.03, 20)

            Await Task.Delay(2400)
            Me.ArchivesTableAdapter.Fill(Me.FinanceDataSet2.Archives)
            ShowWithAnimation(PanelControl95, AnimationType.Mosaic, 1500, 0.03, 20)



            ' تفعيل أحداث التتبع للرسوم البيانية
            AddHandler ChartControl1.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
            'AddHandler ChartControl5.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
            AddHandler ChartControl2.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
            AddHandler ChartControl9.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
            AddHandler ChartControl7.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
            AddHandler ChartControl35.ObjectHotTracked, AddressOf Chart_ObjectHotTracked
            AddHandler ChartControl34.ObjectHotTracked, AddressOf Chart_ObjectHotTracked


            tblMain.AutoScroll = True
            'Mainte.Padding = New Padding(0, 5, 0, 0)
        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' إيقاف التنقل عند دخول الماوس
    ''' </summary>
    Private Sub NavigationFrame1_MouseEnter(sender As Object, e As EventArgs)
        navTimer.Stop()
    End Sub

    ''' <summary>
    ''' استئناف التنقل عند خروج الماوس
    ''' </summary>
    Private Sub NavigationFrame1_MouseLeave(sender As Object, e As EventArgs)
        navTimer.Start()
    End Sub

    ''' <summary>
    ''' التنقل التلقائي بين الصفحات مع تأثيرات مختلفة
    ''' </summary>
    'Private Sub navTimer_Tick(sender As Object, e As EventArgs) Handles navTimer.Tick
    '    currentPageIndex += 1
    '    If currentPageIndex >= pages.Length Then currentPageIndex = 0

    '    ' تأثيرات تنقل مختلفة لكل صفحة
    '    Select Case currentPageIndex
    '        Case 0
    '            ' الصفحة الأولى - تأثير الانزلاق من اليسار
    '            ShowPageWithTransition(NavigationPage1, AnimationType.Mosaic)
    '            Chart_Bind()

    '        Case 1
    '            ' الصفحة الثانية - تأثير التدوير
    '            ShowPageWithTransition(NavigationPage2, AnimationType.Rotate)
    '            Chart_Bind_AHAD()

    '        Case 2
    '            ' الصفحة الثالثة - تأثير التكبير
    '            ShowPageWithTransition(NavigationPage3, AnimationType.Scale)
    '            Chart_Bind_IELAM()
    '    End Select
    'End Sub

    ''' <summary>
    ''' عرض الصفحة مع تأثير انتقالي
    ''' </summary>
    Private Sub ShowPageWithTransition(targetPage As DevExpress.XtraBars.Navigation.NavigationPage,
                                      animationType As AnimationType)

        'NavigationFrame1.SelectedPage = targetPage

        ' تطبيق تأثير بصري على محتوى الصفحة
        If targetPage.Controls.Count > 0 Then
            ShowWithAnimation(targetPage.Controls(0), animationType, 1200, 0.02, 15)
        End If
    End Sub



    ''' <summary>
    ''' تتبع حركة الماوس على الرسوم البيانية
    ''' </summary>
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
    Private Sub LabelControl18_Click(sender As Object, e As EventArgs) Handles LabelControl18.Click
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