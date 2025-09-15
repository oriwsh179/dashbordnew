
Imports Guna.UI2.AnimatorNS
Imports Guna.UI2.WinForms


Public Class MainDHss
    Private Async Sub MainDH_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Await Task.Delay(300)
        'Guna2Transition1.ShowSync(Guna2Button2, False, Animation.Leaf)
        'Await Task.Delay(400)
        'Guna2Transition1.ShowSync(Guna2Button1, False, Animation.Mosaic)

        'Await Task.Delay(200)
        'Guna2Transition1.ShowSync(Guna2Button3, False, Animation.Mosaic)
        Guna2Button2.Visible = False


        Await Task.Delay(200)
        ShowWithAnimation(Guna2Button2, AnimationType.Leaf, 1500, 0.03, 20)

        Await Task.Delay(300)
        ShowWithAnimation(Guna2Button1, AnimationType.Mosaic, 1500, 0.03, 20)
        Await Task.Delay(300)
        ShowWithAnimation(Guna2PictureBox1, AnimationType.Leaf, 1500, 0.03, 20)
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

    Private Sub PanelControl2_Paint(sender As Object, e As PaintEventArgs) Handles PanelControl2.Paint

    End Sub




    'Private Sub TileBarItem1_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs)
    '    '✔️ أولاً: حدد نوع التأثير
    '    NavigationFrame1.TransitionType = DevExpress.Utils.Animation.Transitions.Comb

    '    '✔️ ثانيًا: عدل خصائص الأنميشن (إذا تحتاج)
    '    NavigationFrame1.TransitionAnimationProperties.FrameCount = 4000
    '    NavigationFrame1.TransitionAnimationProperties.FrameInterval = 1000

    '    '✔️ أخيرًا: نفّذ التنقل
    '    NavigationFrame1.SelectedPage = NavigationPage1

    'End Sub

    'Private Sub TileBarItem4_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs)
    '    '✔️ أولاً: حدد نوع التأثير
    '    NavigationFrame1.TransitionType = DevExpress.Utils.Animation.Transitions.Cover

    '    '✔️ ثانيًا: عدل خصائص الأنميشن (إذا تحتاج)
    '    NavigationFrame1.TransitionAnimationProperties.FrameCount = 4000
    '    NavigationFrame1.TransitionAnimationProperties.FrameInterval = 1000

    '    '✔️ أخيرًا: نفّذ التنقل
    '    NavigationFrame1.SelectedPage = NavigationPage2

    'End Sub





End Class
