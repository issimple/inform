Imports ShaderEffectLibrary
Imports Transitionals
Imports System.IO
Imports System.Windows.Media.Animation
Imports System.Windows.Threading

Public Class SheffSlider : Inherits Grid
    Public SliderImageNew As New Image
    Public SliderImageOld As New Image

    Public ContentDirectory As String = System.AppDomain.CurrentDomain.BaseDirectory() + "data/"
    Public FilePrefix As String = ""
    Public FileExtention As String = ".jpg"
    Public EffectIndex As Integer = 0
    Public SlideDelay As Double = 3
    Public EffectDuration As Double = 2
    Public EffectAmount As Double = 1

    Public Play As Boolean = True

    Dim sheff_set(30) As Object
    Dim sheff_set2(30) As Object
    Dim sheff_amt As Double = 0.5
    Dim slide_ind As Integer = 0

    Dim SlideImages() As BitmapImage

    Dim header_loop_timer As New DispatcherTimer

    Public Sub New(ByVal _root As String, ByVal _prefix As String, ByVal _effect As Integer, ByVal _delay As Double, _
                            ByVal _duration As Double, Optional _amount As Double = -1, Optional _ext As String = ".jpg", Optional _play As Boolean = True)
        Me.ContentDirectory = _root
        Me.FilePrefix = _prefix
        Me.EffectIndex = _effect
        Me.SlideDelay = _delay
        Me.EffectDuration = _duration
        Me.EffectAmount = _amount
        Me.FileExtention = _ext
        Me.Play = _play

        'LAYOUT
        Me.Children.Add(SliderImageOld)
        Me.Children.Add(SliderImageNew)
        Me.VerticalAlignment = VerticalAlignment.Top

        'FILES
        For i = 1 To 19
            If File.Exists(ContentDirectory + FilePrefix + CStr(i) + FileExtention) Then
                ReDim Preserve SlideImages(i - 1)
                SlideImages(i - 1) = New BitmapImage(New Uri(ContentDirectory + FilePrefix + CStr(i) + FileExtention, UriKind.Absolute))
            End If
        Next

        'EFFECTS
        SetShaders()

        'TIMER
        AddHandler header_loop_timer.Tick, AddressOf header_loop_timer_tick
        header_loop_timer.Interval = TimeSpan.FromSeconds(SlideDelay)
        header_loop_timer.Start()
    End Sub

    Public Sub ReloadSource(ByVal _root As String, ByVal _prefix As String, ByVal _effect As Integer, ByVal _delay As Double, _
                        ByVal _duration As Double, Optional _amount As Double = -1, Optional _ext As String = ".jpg", Optional _play As Boolean = True)
        Me.ContentDirectory = _root
        Me.FilePrefix = _prefix
        Me.EffectIndex = _effect
        Me.SlideDelay = _delay
        Me.EffectDuration = _duration
        Me.EffectAmount = _amount
        Me.FileExtention = _ext
        Me.Play = _play

        header_loop_timer.Interval = TimeSpan.FromSeconds(SlideDelay)

        For i = 1 To 19
            If File.Exists(ContentDirectory + FilePrefix + CStr(i) + FileExtention) Then
                'FOR SHADER TRANS-S
                ReDim Preserve SlideImages(i - 1)
                SlideImages(i - 1) = New BitmapImage(New Uri(ContentDirectory + FilePrefix + CStr(i) + FileExtention, UriKind.Absolute))
            End If
        Next

        SetShaders()
    End Sub

    Public Sub SetPlay(ByVal _state As Boolean)
        Play = _state
        SetShaders()
        header_loop_timer_tick()
    End Sub

    Private Sub SetShaders()
        sheff_set(0) = New TransitionEffects.BandedSwirlTransitionEffect
        sheff_set(1) = New TransitionEffects.BlindsTransitionEffect
        sheff_set(2) = New TransitionEffects.BloodTransitionEffect

        sheff_set(3) = New TransitionEffects.CircleRevealTransitionEffect
        sheff_set(4) = New TransitionEffects.CircleStretchTransitionEffect
        sheff_set(5) = New TransitionEffects.CloudRevealTransitionEffect
        sheff_set(6) = New TransitionEffects.CrumbleTransitionEffect

        sheff_set(7) = New TransitionEffects.DisolveTransitionEffect
        sheff_set(8) = New TransitionEffects.DropFadeTransitionEffect

        sheff_set(9) = New TransitionEffects.FadeTransitionEffect

        sheff_set(10) = New TransitionEffects.LeastBrightTransitionEffect
        sheff_set(11) = New TransitionEffects.LineRevealTransitionEffect

        sheff_set(12) = New TransitionEffects.MostBrightTransitionEffect

        sheff_set(13) = New TransitionEffects.PixelateInTransitionEffect
        sheff_set(14) = New TransitionEffects.PixelateOutTransitionEffect
        sheff_set(15) = New TransitionEffects.PixelateTransitionEffect

        sheff_set(16) = New TransitionEffects.RadialBlurTransitionEffect
        sheff_set(17) = New TransitionEffects.RadialWiggleTransitionEffect
        sheff_set(18) = New TransitionEffects.RandomCircleRevealTransitionEffect

        sheff_set(19) = New TransitionEffects.RippleTransitionEffect
        sheff_set(20) = New TransitionEffects.RotateCrumbleTransitionEffect

        sheff_set(21) = New TransitionEffects.SaturateTransitionEffect
        sheff_set(22) = New TransitionEffects.ShrinkTransitionEffect
        sheff_set(23) = New TransitionEffects.SlideInTransitionEffect
        sheff_set(24) = New TransitionEffects.SmoothSwirlGridTransitionEffect
        sheff_set(25) = New TransitionEffects.SwirlGridTransitionEffect
        sheff_set(26) = New TransitionEffects.SwirlTransitionEffect

        sheff_set(27) = New TransitionEffects.WaterTransitionEffect
        sheff_set(28) = New TransitionEffects.WaveTransitionEffect


        If File.Exists(ContentDirectory + FilePrefix + "1" + FileExtention) Then
            SliderImageNew.Source = New BitmapImage(New Uri(ContentDirectory + FilePrefix + "1" + FileExtention))
        Else
            Play = False
        End If

        SliderImageNew.Effect = sheff_set(EffectIndex)
        If Not IsNothing(SlideImages) Then
            'If Not IsNothing(SlideImages(0)) Then sheff_set(EffectIndex).OldImage = New ImageBrush With {.ImageSource = SlideImages(0)}
            'If SlideImages.Length > 1 Then sheff_set(EffectIndex).Input = New ImageBrush With {.ImageSource = SlideImages(1)}
        End If


        sheff_set2(0) = New TransitionEffects.BandedSwirlTransitionEffect
        sheff_set2(1) = New TransitionEffects.BlindsTransitionEffect
        sheff_set2(2) = New TransitionEffects.BloodTransitionEffect

        sheff_set2(3) = New TransitionEffects.CircleRevealTransitionEffect
        sheff_set2(4) = New TransitionEffects.CircleStretchTransitionEffect
        sheff_set2(5) = New TransitionEffects.CloudRevealTransitionEffect
        sheff_set2(6) = New TransitionEffects.CrumbleTransitionEffect

        sheff_set2(7) = New TransitionEffects.DisolveTransitionEffect
        sheff_set2(8) = New TransitionEffects.DropFadeTransitionEffect

        sheff_set2(9) = New TransitionEffects.FadeTransitionEffect

        sheff_set2(10) = New TransitionEffects.LeastBrightTransitionEffect
        sheff_set2(11) = New TransitionEffects.LineRevealTransitionEffect

        sheff_set2(12) = New TransitionEffects.MostBrightTransitionEffect

        sheff_set2(13) = New TransitionEffects.PixelateInTransitionEffect
        sheff_set2(14) = New TransitionEffects.PixelateOutTransitionEffect
        sheff_set2(15) = New TransitionEffects.PixelateTransitionEffect

        sheff_set2(16) = New TransitionEffects.RadialBlurTransitionEffect
        sheff_set2(17) = New TransitionEffects.RadialWiggleTransitionEffect
        sheff_set2(18) = New TransitionEffects.RandomCircleRevealTransitionEffect

        sheff_set2(19) = New TransitionEffects.RippleTransitionEffect
        sheff_set2(20) = New TransitionEffects.RotateCrumbleTransitionEffect

        sheff_set2(21) = New TransitionEffects.SaturateTransitionEffect
        sheff_set2(22) = New TransitionEffects.ShrinkTransitionEffect
        sheff_set2(23) = New TransitionEffects.SlideInTransitionEffect
        sheff_set2(24) = New TransitionEffects.SmoothSwirlGridTransitionEffect
        sheff_set2(25) = New TransitionEffects.SwirlGridTransitionEffect
        sheff_set2(26) = New TransitionEffects.SwirlTransitionEffect

        sheff_set2(27) = New TransitionEffects.WaterTransitionEffect
        sheff_set2(28) = New TransitionEffects.WaveTransitionEffect

        If File.Exists(ContentDirectory + FilePrefix + "1" + FileExtention) Then
            SliderImageOld.Source = New BitmapImage(New Uri(ContentDirectory + FilePrefix + "1" + FileExtention))
        Else
            Play = False
        End If

        SliderImageOld.Effect = sheff_set2(EffectIndex)
        If Not IsNothing(SlideImages) Then
            'If Not IsNothing(SlideImages(0)) Then sheff_set2(EffectIndex).OldImage = New ImageBrush With {.ImageSource = SlideImages(0)}
            'If SlideImages.Length > 1 Then sheff_set2(EffectIndex).Input = New ImageBrush With {.ImageSource = SlideImages(1)}
        End If
    End Sub

    Private Sub header_loop_timer_tick()
        If Play Then


            sheff_set(EffectIndex).OldImage = New ImageBrush With {.ImageSource = SlideImages(slide_ind)}
            If slide_ind + 1 = SlideImages.Length Then slide_ind = -1
            sheff_set2(EffectIndex).OldImage = New ImageBrush With {.ImageSource = SlideImages(slide_ind + 1)}
            sheff_set(EffectIndex).Input = New ImageBrush
            sheff_set2(EffectIndex).Input = New ImageBrush

            slide_ind += 1

            If Me.EffectAmount <> -1 Then
                With sheff_set(EffectIndex)
                    Select Case EffectIndex
                        Case 0 : .TwistAmount = EffectAmount
                        Case 2 : .RandomSeed = EffectAmount
                        Case 3 : .FuzzyAmount = EffectAmount

                        Case 5 : .RandomSeed = EffectAmount
                        Case 6 : .RandomSeed = EffectAmount
                            '...
                    End Select
                End With
            End If

            Dim sheff_anim As New DoubleAnimation
            With sheff_anim
                .From = 0
                .To = 1
                .Duration = TimeSpan.FromSeconds(Me.EffectDuration)
                .AutoReverse = False
                .EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseInOut}
            End With
            sheff_set(EffectIndex).BeginAnimation(sheff_set(EffectIndex).ProgressProperty, sheff_anim)

            Dim sheff_anim2 As New DoubleAnimation
            With sheff_anim2
                .From = 1
                .To = 0
                .Duration = TimeSpan.FromSeconds(Me.EffectDuration)
                .AutoReverse = False
                .EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseInOut}
            End With
            sheff_set2(EffectIndex).BeginAnimation(sheff_set2(EffectIndex).ProgressProperty, sheff_anim2)

        End If
    End Sub
End Class
