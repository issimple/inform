Imports System.Windows.Ink
Imports System.Media
Imports System.Windows.Media.Animation
Imports System.IO

Public Class InkTools : Inherits Grid

    Dim base_dir As String = System.AppDomain.CurrentDomain.BaseDirectory()
    Dim res As String = "inktools\"

    Public ICanvas As New InkCanvas
    Dim ToolsStack As New StackPanel With {.VerticalAlignment = VerticalAlignment.Center, .HorizontalAlignment = HorizontalAlignment.Left,
                                           .Margin = New Thickness(0)}
    Dim Picker As New Image With {.Visibility = Visibility.Hidden, .Stretch = Stretch.None, .Margin = New Thickness(70, 0, 0, 0),
                                  .VerticalAlignment = VerticalAlignment.Center, .HorizontalAlignment = HorizontalAlignment.Left}
    Dim BrushGrid As New Grid With {.Height = 55, .Width = 55, .Background = Brushes.White}
    Dim BrushEllipse As New Ellipse With {.Height = 10, .Width = 5, .Stroke = Brushes.Black, .Fill = Brushes.Black}

    Dim Btn_BrushUp As New Image With {.Stretch = Stretch.None}
    Dim Btn_BrushDn As New Image With {.Stretch = Stretch.None}
    Dim Btn_Picker As New Image With {.Stretch = Stretch.None}
    Dim Btn_DrawFg As New Image With {.Stretch = Stretch.None}
    Dim Btn_DrawBg As New Image With {.Stretch = Stretch.None}
    Dim Btn_Erase As New Image With {.Stretch = Stretch.None}
    Dim Btn_BlockErase As New Image With {.Stretch = Stretch.None}
    Dim Btn_Clear As New Image With {.Stretch = Stretch.None}

    Dim inkDA As New DrawingAttributes With {.Color = Colors.Black, .FitToCurve = True, .Width = 5, .Height = 10}

    Public Sub New()
        SetSource("picker.bmp", Picker)
        SetSource("btn_ink_plus.png", Btn_BrushUp)
        SetSource("btn_ink_min.png", Btn_BrushDn)
        SetSource("btn_ink_picker.png", Btn_Picker)
        SetSource("btn_ink_pen.png", Btn_DrawFg)
        SetSource("btn_ink_hl.png", Btn_DrawBg)
        SetSource("btn_ink_erase.png", Btn_Erase)
        SetSource("btn_ink_erase2.png", Btn_BlockErase)
        SetSource("btn_ink_del.png", Btn_Clear)

        'SetSource("btn_ink_pen_sel.png", Btn_DrawFg)

        'Me.Children.Add(ICanvas)

        With ToolsStack.Children
            .Add(Btn_BrushUp)
            .Add(BrushGrid)
            .Add(Btn_BrushDn)
            .Add(Btn_Picker)
            .Add(Btn_DrawFg)
            .Add(Btn_DrawBg)
            .Add(Btn_Erase)
            .Add(Btn_BlockErase)
            .Add(Btn_Clear)
        End With

        Me.Children.Add(Picker)
        Me.Children.Add(ToolsStack)

        AddHandler Btn_Picker.MouseUp, AddressOf Btn_Picker_Click
        AddHandler Btn_DrawFg.MouseUp, AddressOf Btn_DrawFg_Click
        AddHandler Btn_DrawBg.MouseUp, AddressOf Btn_DrawBg_Click
        AddHandler Btn_Erase.MouseUp, AddressOf Btn_Erase_Click
        AddHandler Btn_BlockErase.MouseUp, AddressOf Btn_BlockErase_Click
        AddHandler Btn_Clear.MouseUp, AddressOf Btn_Clear_Click

        AddHandler Btn_BrushUp.MouseUp, AddressOf Btn_BrushUp_MouseUp
        AddHandler Btn_BrushUp.MouseDown, AddressOf Btn_BrushUp_MouseDown
        AddHandler Btn_BrushDn.MouseUp, AddressOf Btn_BrushDn_MouseUp
        AddHandler Btn_BrushDn.MouseDown, AddressOf Btn_BrushDn_MouseDown

        AddHandler Picker.MouseDown, AddressOf Picker_MouseDown
        AddHandler Picker.MouseUp, AddressOf Picker_MouseUp
        AddHandler Picker.MouseMove, AddressOf Picker_MouseMove

        AddHandler BrushGrid.MouseUp, AddressOf BrushGrid_Click

        BrushEllipse.HorizontalAlignment = HorizontalAlignment.Center
        BrushGrid.Children.Add(BrushEllipse)

        ICanvas.DefaultDrawingAttributes = inkDA

        Btn_DrawFg_Click(Nothing, Nothing)
    End Sub


    Public Sub SetSource(ByVal filename As String, ByRef imageobj As Image)
        If File.Exists(base_dir + res + filename) Then imageobj.Source = New BitmapImage(New Uri(base_dir + res + filename))
    End Sub

    Private Function SetBGBrush(ByVal bg_filename As String, ByVal bg_width As Integer, ByVal bg_height As Integer) As ImageBrush
        Dim bgbrush As New ImageBrush()
        With bgbrush
            .ImageSource = New BitmapImage(New Uri(base_dir + "res\" + bg_filename))
            .Stretch = Stretch.UniformToFill
            .TileMode = TileMode.None
            .Viewport = New Rect(0, 0, bg_width, bg_height)
            .ViewportUnits = BrushMappingMode.Absolute
        End With
        SetBGBrush = bgbrush
    End Function

    Dim pick_vis = False

    Private Sub Pick_Show()
        Picker.Visibility = Visibility.Visible
        Dim anim_show As New DoubleAnimation(-Picker.ActualWidth, 0, TimeSpan.FromSeconds(0.5))
        Dim transf = New TranslateTransform
        Picker.RenderTransform = transf
        Dim ease As New BackEase
        ease.EasingMode = EasingMode.EaseOut
        ease.Amplitude = 0.2
        anim_show.EasingFunction = ease
        transf.BeginAnimation(TranslateTransform.XProperty, anim_show)
        SetSource("btn_ink_picker_sel.png", Btn_Picker)
        pick_vis = True
    End Sub

    Private Sub Pick_Hide()
        Dim anim_show As New DoubleAnimation(0, -Picker.ActualWidth - 100, TimeSpan.FromSeconds(0.5))
        Dim transf = New TranslateTransform
        Picker.RenderTransform = transf
        Dim ease As New BackEase
        ease.EasingMode = EasingMode.EaseIn
        ease.Amplitude = 0.2
        anim_show.EasingFunction = ease
        transf.BeginAnimation(TranslateTransform.XProperty, anim_show)
        SetSource("btn_ink_picker.png", Btn_Picker)
        pick_vis = False
    End Sub

    Private Sub Btn_Picker_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If pick_vis Then Pick_Hide() Else Pick_Show()
    End Sub

    Private Sub Btn_DrawFg_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        With inkDA
            .IsHighlighter = False
            .Width = BrushEllipse.Width
            .Height = BrushEllipse.Height
        End With
        ICanvas.DefaultDrawingAttributes = inkDA
        ICanvas.EditingMode = InkCanvasEditingMode.Ink

        SetSource("btn_ink_pen_sel.png", Btn_DrawFg)
        SetSource("btn_ink_hl.png", Btn_DrawBg)
        SetSource("btn_ink_erase.png", Btn_Erase)
        SetSource("btn_ink_erase2.png", Btn_BlockErase)
    End Sub

    Private Sub Btn_DrawBg_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        With inkDA
            .IsHighlighter = True
            .Width = BrushEllipse.Width
            .Height = BrushEllipse.Height
        End With
        ICanvas.DefaultDrawingAttributes = inkDA
        ICanvas.EditingMode = InkCanvasEditingMode.Ink

        SetSource("btn_ink_pen.png", Btn_DrawFg)
        SetSource("btn_ink_hl_sel.png", Btn_DrawBg)
        SetSource("btn_ink_erase.png", Btn_Erase)
        SetSource("btn_ink_erase2.png", Btn_BlockErase)
    End Sub

    Private Sub Btn_Erase_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ICanvas.EditingMode = InkCanvasEditingMode.EraseByPoint
        SetSource("btn_ink_pen.png", Btn_DrawFg)
        SetSource("btn_ink_hl.png", Btn_DrawBg)
        SetSource("btn_ink_erase_sel.png", Btn_Erase)
        SetSource("btn_ink_erase2.png", Btn_BlockErase)
    End Sub

    Private Sub Btn_BlockErase_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ICanvas.EditingMode = InkCanvasEditingMode.EraseByStroke
        SetSource("btn_ink_pen.png", Btn_DrawFg)
        SetSource("btn_ink_hl.png", Btn_DrawBg)
        SetSource("btn_ink_erase.png", Btn_Erase)
        SetSource("btn_ink_erase2_sel.png", Btn_BlockErase)
    End Sub

    Private Sub Btn_Clear_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ICanvas.Strokes.Clear()
    End Sub

    Dim brush_size_anim As New DoubleAnimation
    Dim size_animation As Boolean = False

    Private Sub Btn_BrushUp_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not size_animation Then
            Dim wx As Integer = BrushEllipse.Width
            brush_size_anim.From = wx
            brush_size_anim.To = 40
            brush_size_anim.Duration = TimeSpan.FromSeconds(1)
            BrushEllipse.BeginAnimation(WidthProperty, brush_size_anim)
            size_animation = True
        End If
    End Sub

    Private Sub Btn_BrushUp_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If size_animation Then
            Dim wx As Double = BrushEllipse.ActualWidth
            BrushEllipse.BeginAnimation(WidthProperty, Nothing)
            BrushEllipse.Width = wx
            size_animation = False
            inkDA.Height = BrushEllipse.Height
            inkDA.Width = BrushEllipse.Width
            ICanvas.DefaultDrawingAttributes = inkDA
        End If

        'Dim wx As Integer = BrushEllipse.Width
        'Dim wy As Integer = BrushEllipse.Height
        'If wx <= 40 And wy <= 40 Then
        '    BrushEllipse.Width = wx + 2
        'End If
        'inkDA.Height = BrushEllipse.Height
        'inkDA.Width = BrushEllipse.Width
        'ICanvas.DefaultDrawingAttributes = inkDA
    End Sub

    Private Sub Btn_BrushDn_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not size_animation Then
            Dim wx As Integer = BrushEllipse.Width
            brush_size_anim.From = wx
            brush_size_anim.To = 2
            brush_size_anim.Duration = TimeSpan.FromSeconds(1)
            BrushEllipse.BeginAnimation(WidthProperty, brush_size_anim)
            size_animation = True
        End If
    End Sub

    Private Sub Btn_BrushDn_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If size_animation Then
            Dim wx As Double = BrushEllipse.ActualWidth
            BrushEllipse.BeginAnimation(WidthProperty, Nothing)
            BrushEllipse.Width = wx
            size_animation = False
            inkDA.Height = BrushEllipse.Height
            inkDA.Width = BrushEllipse.Width
            ICanvas.DefaultDrawingAttributes = inkDA
        End If

        'Dim wx As Integer = BrushEllipse.Width
        'Dim wy As Integer = BrushEllipse.Height
        'If wx <> 2 And wy <> 2 Then
        '    BrushEllipse.Width = wx - 2
        '    inkDA.Height = BrushEllipse.Height
        '    inkDA.Width = BrushEllipse.Width
        'End If
        'ICanvas.DefaultDrawingAttributes = inkDA
    End Sub

    Private Sub BrushGrid_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim wx As Integer = BrushEllipse.Width
        Dim wy As Integer = BrushEllipse.Height
        BrushEllipse.Width = wy
        BrushEllipse.Height = wx
        If wx <> 2 And wy <> 2 Then
            inkDA.Height = BrushEllipse.Height
            inkDA.Width = BrushEllipse.Width
        End If
        ICanvas.DefaultDrawingAttributes = inkDA
    End Sub

    Dim picket_mouse_down As Boolean = False
    Private Sub Picker_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        picket_mouse_down = True
        Picker_MouseMove(Nothing, Nothing)
    End Sub

    Private Sub Picker_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Pick_Hide()
        picket_mouse_down = False
    End Sub

    Private Sub Picker_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        If picket_mouse_down = True Then
            Dim bmp As New BitmapImage()
            Dim pix(4) As Byte
            bmp = Picker.Source
            Dim cbmp As New CroppedBitmap(bmp, New Int32Rect(Mouse.GetPosition(Picker).X, Mouse.GetPosition(Picker).Y, 1, 1))
            Try
                cbmp.CopyPixels(pix, 4, 0)
                BrushEllipse.Fill = New SolidColorBrush(Color.FromRgb(pix(2), pix(1), pix(0)))
                BrushGrid.Background = New SolidColorBrush(Color.FromArgb(128, pix(2), pix(1), pix(0)))
                inkDA.Color = Color.FromRgb(pix(2), pix(1), pix(0))
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub BtnInkSave()
        'def_snd.Play()
        'Dim rtb = New RenderTargetBitmap(InkCanvas1.ActualWidth + 7, InkCanvas1.ActualHeight + 8, 96D, 96D, PixelFormats.Default)
        'rtb.Render(InkCanvas1)
        'Dim encoder = New JpegBitmapEncoder
        'encoder.Frames.Add(BitmapFrame.Create(rtb))
        'Try
        '    Dim fs = File.Open(base_dir + "is_" + _
        '        DateTime.Now.ToString("HHMMss") + "_" + _
        '        DateTime.Now.ToString("ddMMyyyy") + ".jpg", FileMode.Create)
        '    encoder.Save(fs)
        '    fs.Close()

        '    BorderMsg.Visibility = Visibility.Visible
        '    Dim anim_show As New DoubleAnimation(BorderMsg.ActualWidth + 20, 0, TimeSpan.FromSeconds(1))
        '    Dim transf = New TranslateTransform
        '    BorderMsg.RenderTransform = transf
        '    Dim ease As New BackEase
        '    ease.EasingMode = EasingMode.EaseOut
        '    ease.Amplitude = 0.2
        '    anim_show.EasingFunction = ease
        '    anim_show.AutoReverse = True
        '    transf.BeginAnimation(TranslateTransform.XProperty, anim_show)
        '    pick_vis = True
        'Catch ex As Exception
        'End Try
    End Sub


End Class
