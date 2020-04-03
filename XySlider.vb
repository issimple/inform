Imports System.IO
Imports System.Windows.Media.Animation
Imports System.Windows.Threading

Public Class XySlider : Inherits Grid

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

    Dim TransfOld As New TranslateTransform
    Dim TransfNew As New TranslateTransform

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

        Me.ClipToBounds = True

        'RENDER TRANS
        SliderImageOld.RenderTransform = TransfOld
        SliderImageNew.RenderTransform = TransfNew

        'FILES
        For i = 1 To 19
            If File.Exists(ContentDirectory + FilePrefix + CStr(i) + FileExtention) Then
                ReDim Preserve SlideImages(i - 1)
                SlideImages(i - 1) = New BitmapImage(New Uri(ContentDirectory + FilePrefix + CStr(i) + FileExtention, UriKind.Absolute))
            End If
        Next

        'INIT IMG
        SliderImageOld.Source = SlideImages(slide_ind)

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
    End Sub

    Public Sub SetPlay(ByVal _state As Boolean)
        Play = _state
        header_loop_timer_tick()
    End Sub

    Private Sub header_loop_timer_tick()
        If Play Then

            SliderImageOld.Source = SlideImages(slide_ind)
            If slide_ind + 1 = SlideImages.Length Then slide_ind = -1
            SliderImageNew.Source = SlideImages(slide_ind + 1)
            slide_ind += 1

            Dim AnimOld As New DoubleAnimation
            With AnimOld
                .From = 0
                Select Case Me.EffectIndex
                    Case 1 : .To = -Me.ActualWidth
                    Case 2 : .To = -Me.ActualHeight
                    Case 3 : .To = 2 * Me.ActualWidth
                    Case 4 : .To = 2 * Me.ActualHeight
                End Select
                .Duration = TimeSpan.FromSeconds(Me.EffectDuration)
                .AutoReverse = False
                .EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseInOut}
            End With
            If Me.EffectIndex = 1 Or Me.EffectIndex = 3 Then
                TransfOld.BeginAnimation(TranslateTransform.XProperty, AnimOld)
            Else
                TransfOld.BeginAnimation(TranslateTransform.YProperty, AnimOld)
            End If


            Dim AnimNew As New DoubleAnimation
            With AnimNew
                Select Case Me.EffectIndex
                    Case 1 : .From = -Me.ActualWidth
                    Case 2 : .From = -Me.ActualHeight
                    Case 3 : .From = 2 * Me.ActualWidth
                    Case 4 : .From = 2 * Me.ActualHeight
                End Select
                .To = 0
                .Duration = TimeSpan.FromSeconds(Me.EffectDuration)
                .AutoReverse = False
                .EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseInOut}
            End With
            If Me.EffectIndex = 1 Or Me.EffectIndex = 3 Then
                TransfNew.BeginAnimation(TranslateTransform.XProperty, AnimNew)
            Else
                TransfNew.BeginAnimation(TranslateTransform.YProperty, AnimNew)
            End If
        End If
    End Sub

End Class
