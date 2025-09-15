Imports System.Drawing.Imaging
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Public Class SinglePlayGifPlayer
    Private ReadOnly _pictureEdit As PictureEdit
    Private ReadOnly _gif As Image
    Private ReadOnly _frameCount As Integer
    Private ReadOnly _totalDuration As Integer
    Private ReadOnly _timer As Timer

    ''' <summary>
    ''' أنشئ مشغل GIF يعمل لمرة واحدة ثم يجمد عند الإطار الأخير.
    ''' </summary>
    ''' <param name="pictureEdit">عنصر PictureEdit الذي سيعرض الصورة</param>
    ''' <param name="imagePath">المسار إلى ملف GIF</param>
    Public Sub New(pictureEdit As PictureEdit, imagePath As String)
        _pictureEdit = pictureEdit
        ' 1. حمّل الـ GIF
        _gif = Image.FromFile(imagePath)
        ' 2. احسب عدد الإطارات والمدة الإجمالية
        Dim dimen = New FrameDimension(_gif.FrameDimensionsList(0))
        _frameCount = _gif.GetFrameCount(dimen)
        Dim delayProp = _gif.GetPropertyItem(&H5100) ' PropertyTagFrameDelay = 20736 (0x5100)
        Dim totalMs As Integer = 0
        For i As Integer = 0 To _frameCount - 1
            Dim delay = BitConverter.ToInt32(delayProp.Value, i * 4)
            totalMs += delay * 10  ' من 1/100s إلى ms
        Next
        _totalDuration = totalMs

        ' 3. جهّز المؤقت
        _timer = New Timer()
        AddHandler _timer.Tick, AddressOf OnTimerTick

        ' 4. ابدأ التشغيل
        Start()
    End Sub

    Private Sub Start()
        ' عرض الـ GIF وتشغيله مرة واحدة
        _pictureEdit.Properties.SizeMode = PictureSizeMode.Stretch
        '_pictureEdit.Properties.AllowAnimation = True
        _pictureEdit.Image = _gif
        _timer.Interval = _totalDuration
        _timer.Start()
    End Sub

    Private Sub OnTimerTick(sender As Object, e As EventArgs)
        _timer.Stop()
        ' بعد انتهاء اللعبة، جمد عند الإطار الأخير
        Dim dimen = New FrameDimension(_gif.FrameDimensionsList(0))
        _gif.SelectActiveFrame(dimen, _frameCount - 1)
        Dim lastFrame As New Bitmap(_gif)
        '_pictureEdit.Properties.AllowAnimation = False
        _pictureEdit.Image = lastFrame
    End Sub
End Class
