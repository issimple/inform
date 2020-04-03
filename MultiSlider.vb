Imports ShaderEffectLibrary
Imports Transitionals
Imports Transitionals.Transitions
Imports System.IO
Imports System.Windows.Media.Animation
Imports System.Windows.Threading

'class=C
'type=1
'amount=1 (0..1 x100%)
'alpha=on (on/off)
'delay=0.1..100 (sec)
'transition_duration=0.1..10 (sec)

'A - 0..22 (Basic)
'B - 0..28 (Shader)
'C - 0..6 (Native WPF)
'	0 - off (to use with alpha)
'	1,2,3,4 - slide stack left-right, top-bottom
'	5,6,7,8 - overflip left, right, top, bottom
'	9,10 - scale down-up, down-up
'	11,12 - flip

Public Class MultiSliderSettings

    Public SliderClass As String = "B"
    Public SlidesSourceType As String = "SF"
    Public EffectIndex As Integer = 0
    Public SlideDelay As Double = 3
    Public EffectDuration As Double = 2
    Public EffectAmount As Double = 1
    Public UseAlpha As Boolean = False

    Public TouchScrollEnabled As Boolean = False
    Public TouchScrollDirection As String = "HOR" '/VER

    Public Sub New(ByVal _sliderclass As String, ByVal _effect As Integer, ByVal _delay As Double, ByVal _duration As Double, ByVal _useaplha As Boolean)
        Me.SliderClass = _sliderclass
        Me.EffectIndex = _effect
        Me.SlideDelay = _delay
        Me.EffectDuration = _duration
        Me.UseAlpha = _useaplha
    End Sub

End Class


Public Class MultiSlider : Inherits Grid

    'B and C
    Public SliderImageNew As New Image
    Public SliderImageOld As New Image

    Public SliderClass As String = "B"
    Public SlidesSourceType As String = "SF" 'SF = Single Folder, MF = Multiple Folders

    Public DirName As String = System.AppDomain.CurrentDomain.BaseDirectory() + "data/"
    Public Prefix As String = ""
    Public Suffix As String = ""
    Public FileName As String = ""
    Public FileExt As String = ".jpg"

    Public EffectIndex As Integer = 0
    Public SlideDelay As Double = 3
    Public EffectDuration As Double = 2
    Public EffectAmount As Double = 1

    Public UseAlpha As Boolean = False

    Public Play As Boolean = True
    Public ManualPlayback As Boolean = False

    Public CurrentSlide As Integer = -1

    'A - TRANC
    Dim TrancSlideShow As New Transitionals.Controls.Slideshow

    'B - SHEFF
    Dim sheff_set(30) As Object
    Dim sheff_set2(30) As Object
    Dim sheff_amt As Double = 0.5

    'C - TRANS SCROLLER
    Dim ScrollViewerContent As New ScrollViewer With {.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden, .VerticalScrollBarVisibility = ScrollBarVisibility.Hidden}
    Dim StackPanelContentScroller As New StackPanel
    'TRANSFORM
    Dim slide_trans_gr As New TransformGroup
    Dim slide_trans_xy As New TranslateTransform
    Dim slide_trans_z As New ScaleTransform

    'C - XY SLIDER
    Dim TransfOld As New TranslateTransform
    Dim TransfNew As New TranslateTransform
    'C - FLIP SLIDER
    Dim SkewTransfOld As New SkewTransform
    Dim SkewTransfNew As New SkewTransform
    
    Dim slide_ind As Integer = 0

    'SOURCES
    Dim SlideImages() As BitmapImage

    Dim MainTimer As New DispatcherTimer
    Dim RunWithDelayTimer As New DispatcherTimer

    Dim MainTransGroup As New TransformGroup
    Dim MainTranslateTrans As New TranslateTransform
    Dim MainScaleTrans As New ScaleTransform
    Dim MainRotateTrans As New RotateTransform

    Private Sub Me_MouseDown()

    End Sub

    Private Sub Me_MouseMove()

    End Sub

    Public Sub Me_MouseUp()
        MsgBox("")
    End Sub

    Public Sub New(ByVal _sliderclass As String, ByVal _effect As Integer)
        Me.SliderClass = _sliderclass
        Me.EffectIndex = _effect

        'MAIN TRANSFORMS
        With MainTransGroup.Children
            .Add(MainTranslateTrans)
            .Add(MainScaleTrans)
            .Add(MainRotateTrans)
        End With
        Me.RenderTransform = MainTransGroup
        'MAIN EVENTS
        AddHandler Me.MouseDown, AddressOf Me_MouseDown
        AddHandler Me.MouseMove, AddressOf Me_MouseMove
        AddHandler Me.MouseUp, AddressOf Me_MouseUp

        'LAYOUT
        'A Class - Transitionals SlideShow 
        If SliderClass = "A" Then
            Me.Children.Add(TrancSlideShow)
        End If

        'B Class - Shader eff, heavy CPU load
        If SliderClass = "B" Then
            Me.Children.Add(SliderImageOld)
            Me.Children.Add(SliderImageNew)
            Me.VerticalAlignment = VerticalAlignment.Top
        End If

        'C Class - WPF
        If SliderClass = "C" Then
            Me.Children.Clear()

            'SCROLLER
            If Me.EffectIndex = 1 Or Me.EffectIndex = 2 Or Me.EffectIndex = 3 Or Me.EffectIndex = 4 Then

                If Me.EffectIndex = 1 Or Me.EffectIndex = 2 Then
                    StackPanelContentScroller.Orientation = Orientation.Horizontal
                Else
                    StackPanelContentScroller.Orientation = Orientation.Vertical
                End If
                ScrollViewerContent.Content = StackPanelContentScroller
                Me.Children.Add(ScrollViewerContent)
                'TRANSFORMATIONS
                slide_trans_gr.Children.Add(slide_trans_xy)
                slide_trans_gr.Children.Add(slide_trans_z)
                StackPanelContentScroller.RenderTransform = slide_trans_gr
            End If

            'XY-SLIDER
            If Me.EffectIndex = 5 Or Me.EffectIndex = 6 Or Me.EffectIndex = 7 Or Me.EffectIndex = 8 Then
                Me.ClipToBounds = True
                Me.Children.Add(SliderImageOld)
                Me.Children.Add(SliderImageNew)
                SliderImageOld.RenderTransform = TransfOld
                SliderImageNew.RenderTransform = TransfNew
            End If

            'FLIP SLIDER
            If Me.EffectIndex = 11 Or Me.EffectIndex = 12 Then
                'Me.ClipToBounds = True
                Me.Children.Add(SliderImageOld)
                Me.Children.Add(SliderImageNew)
                SliderImageOld.RenderTransform = SkewTransfOld
                SliderImageNew.RenderTransform = SkewTransfNew
            End If
        End If

        'TIMER
        AddHandler MainTimer.Tick, AddressOf MainTimer_Tick
    End Sub

    Public Sub ReloadSource(ByVal _sourcetype As String, ByVal _dirname As String, ByVal _prefix As String, ByVal _suffix As String, ByVal _filename As String, ByVal _fileext As String, _
                            ByVal _delay As Double, ByVal _duration As Double, Optional _amount As Double = -1, Optional _play As Boolean = True)

        Me.SlidesSourceType = _sourcetype
        Me.DirName = _dirname
        Me.Prefix = _prefix
        Me.Suffix = _suffix
        Me.FileName = _filename
        Me.FileExt = _fileext
        Me.SlideDelay = _delay
        Me.EffectDuration = _duration
        Me.EffectAmount = _amount
        Me.Play = _play

        'FILES FROM SINGLE FOLDER
        If SlidesSourceType = "SF" Then
            For i = 1 To 99
                If File.Exists(DirName + Prefix + CStr(i) + FileExt) Then
                    ReDim Preserve SlideImages(i - 1)
                    SlideImages(i - 1) = New BitmapImage
                    With SlideImages(i - 1)
                        .BeginInit()
                        .CreateOptions = BitmapCreateOptions.IgnoreImageCache
                        .CacheOption = BitmapCacheOption.OnLoad
                        .UriSource = New Uri(DirName + Prefix + CStr(i) + Suffix + FileExt, UriKind.Absolute)
                        .EndInit()
                    End With
                End If
            Next
        End If

        'FILES FROM MULTIPLE FOLDERS
        If SlidesSourceType = "MF" Then
            'CHECK ROOT FOLDER
            If File.Exists(DirName + FileName + FileExt) Then
                ReDim Preserve SlideImages(0)
                SlideImages(0) = New BitmapImage
                With SlideImages(0)
                    .BeginInit()
                    .CreateOptions = BitmapCreateOptions.IgnoreImageCache
                    .CacheOption = BitmapCacheOption.OnLoad
                    .UriSource = New Uri(DirName + FileName + FileExt, UriKind.Absolute)
                    .EndInit()
                End With
            End If
            For i = 1 To 99
                'CHECK SUB FOLDERS
                If File.Exists(DirName + Prefix + CStr(i) + Suffix + "/" + FileName + FileExt) Then
                    ReDim Preserve SlideImages(i)
                    SlideImages(i) = New BitmapImage
                    With SlideImages(i)
                        .BeginInit()
                        .CreateOptions = BitmapCreateOptions.IgnoreImageCache
                        .CacheOption = BitmapCacheOption.OnLoad
                        .UriSource = New Uri(DirName + Prefix + CStr(i) + Suffix + "/" + FileName + FileExt, UriKind.Absolute)
                        .EndInit()
                    End With
                End If
            Next
        End If

        'INIT IMAGES
        If SliderClass = "A" Then SetTransitions()
        If SliderClass = "B" Then SetShaders()

        If SliderClass = "C" Then

            If Not IsNothing(SlideImages) Then
                If Me.EffectIndex = 1 Or Me.EffectIndex = 2 Or Me.EffectIndex = 3 Or Me.EffectIndex = 4 Then
                    For i = 0 To SlideImages.Length - 1
                        Dim ImageSlide As New Image
                        ImageSlide.Source = SlideImages(i)
                        StackPanelContentScroller.Children.Add(ImageSlide)
                    Next
                    'INIT SLIDES SHIFT
                    If Me.EffectIndex = 1 Or Me.EffectIndex = 2 Then 'HOR
                        slide_trans_xy.X = ScrollViewerContent.ActualWidth
                        slide_trans_xy.BeginAnimation(TranslateTransform.XProperty, Nothing)
                    End If
                    If Me.EffectIndex = 3 Or Me.EffectIndex = 4 Then 'VER
                        slide_trans_xy.Y = ScrollViewerContent.ActualHeight
                        slide_trans_xy.BeginAnimation(TranslateTransform.YProperty, Nothing)
                    End If
                End If
            End If

            If Me.EffectIndex = 5 Or Me.EffectIndex = 6 Or Me.EffectIndex = 7 Or Me.EffectIndex = 8 Then
                SliderImageOld.Source = SlideImages(slide_ind)
            End If
        End If

        'TIMER
        MainTimer.Interval = TimeSpan.FromSeconds(SlideDelay)
        If Play Then MainTimer.Start()
    End Sub

    Public Sub SetPlay(ByVal _state As Boolean)
        Play = _state
        'If SliderClass = "B" Then SetShaders()
        MainTimer_Tick()
    End Sub

    Private Sub SetTransitions()
        Dim trans_selector As New Transitionals.RandomTransitionSelector
        Dim ttrans(22) As Object
        ttrans(1) = New CheckerboardTransition
        ttrans(2) = New DiagonalWipeTransition
        ttrans(3) = New DiamondsTransition
        ttrans(4) = New DoorTransition
        ttrans(5) = New DotsTransition
        ttrans(6) = New DoubleRotateWipeTransition
        ttrans(7) = New ExplosionTransition
        ttrans(8) = New FadeAndBlurTransition
        ttrans(9) = New FadeAndGrowTransition
        ttrans(10) = New FadeTransition
        ttrans(11) = New FlipTransition
        ttrans(12) = New HorizontalBlindsTransition
        ttrans(13) = New HorizontalWipeTransition
        ttrans(14) = New MeltTransition
        ttrans(15) = New PageTransition
        ttrans(16) = New RollTransition
        ttrans(17) = New RotateTransition
        ttrans(18) = New RotateWipeTransition
        ttrans(19) = New StarTransition
        ttrans(20) = New TranslateTransition
        ttrans(21) = New VerticalBlindsTransition
        ttrans(22) = New VerticalWipeTransition
        For i = 1 To 22
            trans_selector.Transitions.Add(ttrans(i))
        Next i
        If EffectIndex = 0 Then
            trancSlideShow.TransitionSelector = trans_selector
        Else
            TrancSlideShow.Transition = ttrans(EffectIndex)
        End If
        'TrancSlideShow.Transition.Duration = New Duration(TimeSpan.FromSeconds(Me.EffectDuration))

        For i = 0 To SlideImages.Length - 1
            Dim ImageSlide As New Image With {.Source = SlideImages(i), .Stretch = Stretch.Uniform}
            Dim SlideShowItem As New Transitionals.Controls.SlideshowItem With {.Content = ImageSlide}
            TrancSlideShow.Items.Add(SlideShowItem)
        Next

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


        SliderImageNew.Source = SlideImages(0)
        SliderImageNew.Effect = sheff_set(EffectIndex)

        sheff_set2 = sheff_set

        SliderImageOld.Source = SlideImages(0)
        SliderImageOld.Effect = sheff_set2(EffectIndex)

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
    End Sub

    Dim prev_slide_index As Integer = -1

    Public Sub MainTimer_Tick()
        If Play And Not IsNothing(SlideImages) Then

            If SliderClass = "A" Then
                CurrentSlide += 1
                If CurrentSlide = SlideImages.Count Then CurrentSlide = 0
                Dim diff As Integer = CurrentSlide - prev_slide_index
                If diff <> 0 Then TrancSlideShow.SelectedIndex = CurrentSlide
            End If

            If SliderClass = "B" Then
                sheff_set(EffectIndex).OldImage = New ImageBrush With {.ImageSource = SlideImages(slide_ind)}
                If slide_ind + 1 = SlideImages.Length Then slide_ind = -1
                sheff_set2(EffectIndex).OldImage = New ImageBrush With {.ImageSource = SlideImages(slide_ind + 1)}
                sheff_set(EffectIndex).Input = New ImageBrush
                sheff_set2(EffectIndex).Input = New ImageBrush

                slide_ind += 1

                Dim sheff_anim As New DoubleAnimation With {.From = 0, .To = 1, .Duration = TimeSpan.FromSeconds(Me.EffectDuration),
                                                            .EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseInOut}}
                sheff_set(EffectIndex).BeginAnimation(sheff_set(EffectIndex).ProgressProperty, sheff_anim)
                Dim sheff_anim2 As New DoubleAnimation With {.From = 1, .To = 0, .Duration = TimeSpan.FromSeconds(Me.EffectDuration),
                                                            .EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseInOut}}
                sheff_set2(EffectIndex).BeginAnimation(sheff_set2(EffectIndex).ProgressProperty, sheff_anim2)
            End If

            If SliderClass = "C" Then

                'SCROLL SLIDER (1 2 3 4)
                If Me.EffectIndex = 1 Or Me.EffectIndex = 2 Or Me.EffectIndex = 3 Or Me.EffectIndex = 4 Then

                    CurrentSlide += 1
                    If CurrentSlide = SlideImages.Count Then CurrentSlide = 0

                    'SCROLLER MOVE
                    Dim w As Integer = ScrollViewerContent.ActualWidth
                    Dim h As Integer = ScrollViewerContent.ActualHeight
                    slide_trans_z.CenterX = w / 2
                    slide_trans_z.CenterY = h / 2
                    Dim anim_xy As DoubleAnimation = Nothing
                    Dim ease As New BackEase With {.Amplitude = 0.25, .EasingMode = EasingMode.EaseInOut}
                    Dim anim_duration As Double = Me.EffectDuration

                    Dim diff As Integer = CurrentSlide - prev_slide_index

                    If CurrentSlide = 0 And diff > 0 Then
                        'move to start:
                        If Me.EffectIndex = 1 Or Me.EffectIndex = 2 Then
                            slide_trans_xy.X = w
                            slide_trans_xy.BeginAnimation(TranslateTransform.XProperty, Nothing)
                            anim_xy = New DoubleAnimation(-w * (SlideImages.Count - 1), slide_trans_xy.X - w, TimeSpan.FromSeconds(anim_duration * SlideImages.Count / 3))
                        End If
                        If Me.EffectIndex = 3 Or Me.EffectIndex = 4 Then
                            slide_trans_xy.Y = h
                            slide_trans_xy.BeginAnimation(TranslateTransform.YProperty, Nothing)
                            anim_xy = New DoubleAnimation(-h * (SlideImages.Count - 1), slide_trans_xy.Y - h, TimeSpan.FromSeconds(anim_duration * SlideImages.Count / 3))
                        End If
                    Else
                        'or move to next item:
                        If Me.EffectIndex = 1 Or Me.EffectIndex = 2 Then
                            Dim target_x As Double = -prev_slide_index * w
                            'anim_xy = New DoubleAnimation(slide_trans_xy.X, slide_trans_xy.X - w * diff, TimeSpan.FromSeconds(anim_duration))
                            anim_xy = New DoubleAnimation(slide_trans_xy.X, target_x - w * diff, TimeSpan.FromSeconds(anim_duration))
                        End If
                        If Me.EffectIndex = 3 Or Me.EffectIndex = 4 Then
                            Dim target_y As Double = -prev_slide_index * h
                            'anim_xy = New DoubleAnimation(slide_trans_xy.Y, slide_trans_xy.Y - h * diff, TimeSpan.FromSeconds(anim_duration))
                            anim_xy = New DoubleAnimation(slide_trans_xy.Y, target_y - h * diff, TimeSpan.FromSeconds(anim_duration))
                        End If
                    End If

                    If Not IsNothing(anim_xy) Then
                        anim_xy.EasingFunction = ease
                        If Me.EffectIndex = 1 Or Me.EffectIndex = 2 Then slide_trans_xy.BeginAnimation(TranslateTransform.XProperty, anim_xy)
                        If Me.EffectIndex = 3 Or Me.EffectIndex = 4 Then slide_trans_xy.BeginAnimation(TranslateTransform.YProperty, anim_xy)
                    End If

                End If

                'XY-SLIDER (5 6 7 8)
                If Me.EffectIndex = 5 Or Me.EffectIndex = 6 Or Me.EffectIndex = 7 Or Me.EffectIndex = 8 Then

                    SliderImageOld.Source = SlideImages(slide_ind)
                    If slide_ind + 1 = SlideImages.Length Then slide_ind = -1
                    SliderImageNew.Source = SlideImages(slide_ind + 1)
                    slide_ind += 1

                    Dim AnimOld As New DoubleAnimation
                    With AnimOld
                        .From = 0
                        Select Case Me.EffectIndex
                            Case 5 : .To = -Me.ActualWidth
                            Case 6 : .To = -Me.ActualHeight
                            Case 7 : .To = 2 * Me.ActualWidth
                            Case 8 : .To = 2 * Me.ActualHeight
                        End Select
                        .Duration = TimeSpan.FromSeconds(Me.EffectDuration)
                        .AutoReverse = False
                        .EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseInOut}
                    End With
                    If Me.EffectIndex = 5 Or Me.EffectIndex = 7 Then
                        TransfOld.BeginAnimation(TranslateTransform.XProperty, AnimOld)
                    Else
                        TransfOld.BeginAnimation(TranslateTransform.YProperty, AnimOld)
                    End If

                    Dim AnimNew As New DoubleAnimation
                    With AnimNew
                        Select Case Me.EffectIndex
                            Case 5 : .From = -Me.ActualWidth
                            Case 6 : .From = -Me.ActualHeight
                            Case 7 : .From = 2 * Me.ActualWidth
                            Case 8 : .From = 2 * Me.ActualHeight
                        End Select
                        .To = 0
                        .Duration = TimeSpan.FromSeconds(Me.EffectDuration)
                        .AutoReverse = False
                        .EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseInOut}
                    End With
                    If Me.EffectIndex = 5 Or Me.EffectIndex = 7 Then
                        TransfNew.BeginAnimation(TranslateTransform.XProperty, AnimNew)
                    Else
                        TransfNew.BeginAnimation(TranslateTransform.YProperty, AnimNew)
                    End If
                End If

                'SCALE SLIDER (9, 10)
                '...

                'FLIP SLIDER (11, 12)
                If Me.EffectIndex = 11 Or Me.EffectIndex = 12 Then

                    SliderImageOld.Source = SlideImages(slide_ind)
                    If slide_ind + 1 = SlideImages.Length Then slide_ind = -1
                    SliderImageNew.Source = SlideImages(slide_ind + 1)
                    slide_ind += 1

                    SkewTransfOld.CenterX = 0
                    SkewTransfOld.CenterY = 0
                    SliderImageOld.Stretch = Stretch.Fill
                    SliderImageOld.HorizontalAlignment = HorizontalAlignment.Left
                    SliderImageOld.VerticalAlignment = VerticalAlignment.Top
                    Dim AnimOld As New DoubleAnimation With {.Duration = TimeSpan.FromSeconds(Me.EffectDuration), .From = 0, .To = 15}
                    SkewTransfOld.BeginAnimation(SkewTransform.AngleXProperty, AnimOld)
                    Dim WAnimOld As New DoubleAnimation With {.Duration = TimeSpan.FromSeconds(Me.EffectDuration), .From = Me.ActualHeight, .To = 3 * Me.ActualHeight / 4}
                    SliderImageOld.BeginAnimation(Image.HeightProperty, WAnimOld)

                    SkewTransfNew.CenterX = 0
                    SkewTransfNew.CenterY = 0
                    SliderImageNew.Stretch = Stretch.Fill
                    SliderImageNew.HorizontalAlignment = HorizontalAlignment.Left
                    SliderImageNew.VerticalAlignment = VerticalAlignment.Top
                    Dim AnimNew As New DoubleAnimation With {.Duration = TimeSpan.FromSeconds(Me.EffectDuration), .From = 15, .To = 0}
                    SkewTransfNew.BeginAnimation(SkewTransform.AngleXProperty, AnimNew)
                    Dim WAnimNew As New DoubleAnimation With {.Duration = TimeSpan.FromSeconds(Me.EffectDuration), .From = 3 * Me.ActualHeight / 4, .To = Me.ActualHeight}
                    SliderImageNew.BeginAnimation(Image.HeightProperty, WAnimNew)

                End If

            End If

            'ALPHA
            If UseAlpha Then
                If SliderClass = "A" Then
                    TrancSlideShow.BeginAnimation(Grid.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(Me.EffectDuration)))
                End If
                If SliderClass = "B" Or SliderClass = "C" Then
                    SliderImageOld.BeginAnimation(Image.OpacityProperty, New DoubleAnimation(1, 0, TimeSpan.FromSeconds(Me.EffectDuration)))
                    SliderImageNew.BeginAnimation(Image.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(Me.EffectDuration)))
                End If
            End If

        End If

        prev_slide_index = CurrentSlide
        If ManualPlayback Then MainTimer.Stop()
    End Sub

End Class
