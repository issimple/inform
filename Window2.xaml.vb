Imports System.ComponentModel
Imports System.IO
Imports ShaderEffectLibrary
Imports Transitionals
Imports System.Windows.Media.Animation

Public Class Window2

    Implements INotifyPropertyChanged
    Private StateControlValue As String = "0"
    Public Property StateControl As String
        Get
            Return Me.StateControlValue
        End Get
        Set(ByVal value As String)
            If Not (value = StateControlValue) Then
                Me.StateControlValue = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("bStatus"))
                'LbStat.Content = Me.StateControl
                StateUpdated()
            End If
        End Set
    End Property
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Dim wnd2_dir As String = System.AppDomain.CurrentDomain.BaseDirectory() + "2window\"
    Dim wnd1_dir As String = System.AppDomain.CurrentDomain.BaseDirectory() + "data\"

    Dim trans_txt() As String = {"Checkerboard", "Diagonal Wipe", "Diamonds", "Door", "Dots",
                                 "Double Rotate Wipe", "Explosion", "Fade And Blur", "Fade And Grow", "Fade",
                                 "Flip", "Horizontal Blinds", "Horizontal Wipe", "Melt", "Page",
                                 "Roll", "Rotate", "Rotate Wipe", "Star", "Translate",
                                 "Vertical Blinds", "Vertical Wipe"}
    Dim sel_trans As Integer = 8
    Dim transel(trans_txt.Length - 1) As Transitionals.Transition

    Dim filenames() As String

    Dim sel_sheff As Integer = 0
    Dim sheff_anim As New DoubleAnimation
    Dim sheff_txt() As String = {"Banded Swirl", "Blinds", "Blood",
                             "Circle Reveal", "Circle Stretch", "Cloud Reveal", "Crumble",
                             "Disolve", "DropFade", "Fade", "Least Bright", "Line Reveal",
                             "Most Bright", "Pixelate In", "Pixelate Out", "Pixelate",
                             "RadialBlur", "RadialWiggle", "RandomCircle", "Ripple", "RotateCrumble",
                             "Saturate", "Shrink", "SlideIn", "SmoothSwirlGrid", "SwirlGrid", "Swirl",
                             "Water", "Wave"}
    Dim slide_ind As Integer = 0
    Dim sheff(sheff_txt.Length - 1) As Object
    Dim sheff_dur As Double = 1
    Dim sheff_amt As Double = 0.5
    Dim sheff_prog As Double = 1
    Dim sheff_delay As Double = 0

    Dim pic_names(99) As String
    Dim pic_count As Integer
    Dim slides_noresize As Boolean = True

    Private Sub StateUpdated()
        If StateControlValue = "0" Then
            'ADS LOOP
            BgSlShow.Visibility = Windows.Visibility.Visible
            stop_sheff = False
            RunSheff()
            'HIDE SLIDES
            Dim transf2 = New TranslateTransform
            BorderHomeSlides.RenderTransform = transf2
            Dim anim2 As New DoubleAnimation(0, -BorderHomeSlides.ActualWidth * 2, TimeSpan.FromSeconds(0.5))
            Dim eleasy2 As New CubicEase
            eleasy2.EasingMode = EasingMode.EaseIn
            anim2.EasingFunction = eleasy2

            transf2.BeginAnimation(TranslateTransform.XProperty, anim2)
        Else
            'WND1 SLIDES
            BgSlShow.Visibility = Windows.Visibility.Hidden
            'SHOW SLIDES AFTER DELAY
            If Not stop_sheff Then
                Dim transf2 = New TranslateTransform
                transf2.X = -BorderHomeSlides.ActualWidth * 2
                BorderHomeSlides.RenderTransform = transf2
                Dim anim2 As New DoubleAnimation(-BorderHomeSlides.ActualWidth * 2, 0, TimeSpan.FromSeconds(1))
                Dim eleasy As New CubicEase
                eleasy.EasingMode = EasingMode.EaseOut
                anim2.EasingFunction = eleasy
                anim2.BeginTime = TimeSpan.FromSeconds(0.5)
                transf2.BeginAnimation(TranslateTransform.XProperty, anim2)
            End If       
            stop_sheff = True
        End If
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) Handles ThatWindow.Loaded
        'BG
        If File.Exists(wnd2_dir + "bg.jpg") Then
            ImageBg.Source = New BitmapImage(New Uri(wnd2_dir + "bg.jpg"))
        End If

        'TRANS
        transel(0) = New Transitionals.Transitions.CheckerboardTransition
        transel(1) = New Transitionals.Transitions.DiagonalWipeTransition
        transel(2) = New Transitionals.Transitions.DiamondsTransition
        transel(3) = New Transitionals.Transitions.DoorTransition
        transel(4) = New Transitionals.Transitions.DotsTransition
        transel(5) = New Transitionals.Transitions.DoubleRotateWipeTransition
        transel(6) = New Transitionals.Transitions.ExplosionTransition
        transel(7) = New Transitionals.Transitions.FadeAndBlurTransition
        transel(8) = New Transitionals.Transitions.FadeAndGrowTransition
        transel(9) = New Transitionals.Transitions.FadeTransition
        transel(10) = New Transitionals.Transitions.FlipTransition
        transel(11) = New Transitionals.Transitions.HorizontalBlindsTransition
        transel(12) = New Transitionals.Transitions.HorizontalWipeTransition
        transel(13) = New Transitionals.Transitions.MeltTransition
        transel(14) = New Transitionals.Transitions.PageTransition
        transel(15) = New Transitionals.Transitions.RollTransition
        transel(16) = New Transitionals.Transitions.RotateTransition
        transel(17) = New Transitionals.Transitions.RotateWipeTransition
        transel(18) = New Transitionals.Transitions.StarTransition
        transel(19) = New Transitionals.Transitions.TranslateTransition
        transel(20) = New Transitionals.Transitions.VerticalBlindsTransition
        transel(21) = New Transitionals.Transitions.VerticalWipeTransition
        BgSlShow.Transition = transel(sel_trans)

        For i = 1 To 99
            If File.Exists(wnd2_dir + CStr(i) + ".png") Then
                Dim SlShowItem As New Transitionals.Controls.SlideshowItem
                Dim ImageItem As New Image
                ReDim Preserve filenames(i - 1)
                filenames(i - 1) = wnd2_dir + CStr(i) + ".png"
                ImageItem.Source = New BitmapImage(New Uri((filenames(i - 1))))
                SlShowItem.Content = ImageItem
                BgSlShow.Items.Add(SlShowItem)
            End If
        Next
        'SlShow.AutoAdvance = True
        'SlShow.AutoAdvanceDuration = TimeSpan.FromSeconds(2)
        BgSlShow.TransitionNext()


        'HOME PAGE SLIDES
        BorderHomeSlides.Visibility = Visibility.Hidden
        If Directory.Exists(wnd1_dir + "slides\") Then
            pic_count = 0
            pic_count = Directory.GetFiles(wnd1_dir + "slides\", "*.png").Count()
            If pic_count > 0 And Not File.Exists(wnd1_dir + "slides\video.avi") Then
                BorderHomeSlides.Visibility = Visibility.Visible
                For i = 1 To pic_count
                    If File.Exists(wnd1_dir + "slides\" + CStr(i) + ".png") Then
                        pic_names(i) = wnd1_dir + "slides\" + CStr(i) + ".png"
                    End If
                Next i
                'tick = 0
                For i = 1 To pic_count
                    Dim ImageSlide As New Image
                    ImageSlide.Source = New BitmapImage(New Uri(pic_names(i)))
                    ImageSlide.Stretch = Stretch.Uniform
                    Dim SlideShowItem As New Transitionals.Controls.SlideshowItem
                    SlideShowItem.Content = ImageSlide
                    SlideShow.Items.Add(SlideShowItem)
                Next i
                ImgRS1.Source = New BitmapImage(New Uri(pic_names(1)))
                ImgRS1.Visibility = Visibility.Hidden
                ImgRS1.UpdateLayout()
                SlideShow.Width = ImgRS1.ActualWidth
                SlideShow.Height = ImgRS1.ActualHeight
                If slides_noresize Then
                    SlideShow.Width = ImgRS1.Source.Width
                    SlideShow.Height = ImgRS1.Source.Height
                    BorderHomeSlides.Width = ImgRS1.Source.Width
                    BorderHomeSlides.Height = ImgRS1.Source.Height
                End If

                'sstimer.Interval = New TimeSpan(0, 0, 0, slides_timer_preset)
                'AddHandler sstimer.Tick, AddressOf sstimer_Tick
                'sstimer.Start()

                SlideShow.Transition = transel(12)
                SlideShow.AutoAdvanceDuration = TimeSpan.FromSeconds(5)
                SlideShow.AutoAdvance = True
                SlideShow.TransitionNext()
            End If
        End If


        'SHEFF TRANS
        sheff(0) = New TransitionEffects.BandedSwirlTransitionEffect
        sheff(1) = New TransitionEffects.BlindsTransitionEffect
        sheff(2) = New TransitionEffects.BloodTransitionEffect

        sheff(3) = New TransitionEffects.CircleRevealTransitionEffect
        sheff(4) = New TransitionEffects.CircleStretchTransitionEffect
        sheff(5) = New TransitionEffects.CloudRevealTransitionEffect
        sheff(6) = New TransitionEffects.CrumbleTransitionEffect

        sheff(7) = New TransitionEffects.DisolveTransitionEffect
        sheff(8) = New TransitionEffects.DropFadeTransitionEffect

        sheff(9) = New TransitionEffects.FadeTransitionEffect

        sheff(10) = New TransitionEffects.LeastBrightTransitionEffect
        sheff(11) = New TransitionEffects.LineRevealTransitionEffect

        sheff(12) = New TransitionEffects.MostBrightTransitionEffect

        sheff(13) = New TransitionEffects.PixelateInTransitionEffect
        sheff(14) = New TransitionEffects.PixelateOutTransitionEffect
        sheff(15) = New TransitionEffects.PixelateTransitionEffect

        sheff(16) = New TransitionEffects.RadialBlurTransitionEffect
        sheff(17) = New TransitionEffects.RadialWiggleTransitionEffect
        sheff(18) = New TransitionEffects.RandomCircleRevealTransitionEffect

        sheff(19) = New TransitionEffects.RippleTransitionEffect
        sheff(20) = New TransitionEffects.RotateCrumbleTransitionEffect

        sheff(21) = New TransitionEffects.SaturateTransitionEffect
        sheff(22) = New TransitionEffects.ShrinkTransitionEffect
        sheff(23) = New TransitionEffects.SlideInTransitionEffect
        sheff(24) = New TransitionEffects.SmoothSwirlGridTransitionEffect
        sheff(25) = New TransitionEffects.SwirlGridTransitionEffect
        sheff(26) = New TransitionEffects.SwirlTransitionEffect

        sheff(27) = New TransitionEffects.WaterTransitionEffect
        sheff(28) = New TransitionEffects.WaveTransitionEffect

        sel_sheff = 15
        BgSlShow.Effect = sheff(sel_sheff)

        AddHandler sheff_anim.Completed, AddressOf Sheff_Completed

        RunSheff()
    End Sub

    Dim stop_sheff As Boolean = False

    Private Sub Sheff_Completed()
        If Not stop_sheff Then RunSheff()
    End Sub

    Private Sub RunSheff()

        If Me.IsVisible Then

            sheff(sel_sheff).OldImage = New ImageBrush With {.ImageSource = New BitmapImage(New Uri(filenames(slide_ind), UriKind.Absolute))}

            If slide_ind + 1 = filenames.Length Then
                slide_ind = -1
            End If

            sheff(sel_sheff).Input = New ImageBrush With {.ImageSource = New BitmapImage(New Uri(filenames(slide_ind + 1), UriKind.Absolute))}

            slide_ind += 1

            Dim sheff_prop As New TransitionEffects.CircleRevealTransitionEffect
            sheff_prop.FuzzyAmount = 0.5

            Select Case sel_sheff
                Case 0 : sheff(sel_sheff).TwistAmount = sheff_amt
                Case 2 : sheff(sel_sheff).RandomSeed = sheff_amt
                Case 3 : sheff(sel_sheff).FuzzyAmount = sheff_amt
            End Select

            With sheff_anim
                .From = 0
                .To = sheff_prog
                .Duration = TimeSpan.FromSeconds(sheff_dur)
                .BeginTime = TimeSpan.FromSeconds(sheff_delay)
                '.AccelerationRatio = 0.15
                '.DecelerationRatio = 0.15
                'If CheckBoxBounce.IsChecked Then
                '    .AutoReverse = True
                '    .RepeatBehavior = RepeatBehavior.Forever
                'Else
                '    .AutoReverse = False
                'End If
            End With

            sheff(sel_sheff).BeginAnimation(sheff(sel_sheff).ProgressProperty, sheff_anim)

        End If
    End Sub

End Class
