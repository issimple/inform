Imports System.Media
Imports System.Windows.Media.Animation
Imports System.IO
Imports System.Windows.Threading
'Imports System.Net.NetworkInformation
Imports Windows7.Multitouch.WPF
Imports Windows7.Multitouch.Manipulation
Imports System.ComponentModel
Imports WPFMediaKit.DirectShow.Controls

Class MainWindow
    Dim app_root As String = AppDomain.CurrentDomain.BaseDirectory()
    Dim project_root As String = app_root
    Dim data_root As String = app_root + "data\"
    Dim gui_dir As String = "interface\"
    Dim snd_dir As String = "sounds\"
    Dim slides_timer_preset As Integer = "10"
    Dim tick As Integer
    Dim bs_tick As Integer
    Dim pic_names(99) As String
    Dim bs_pic_names(99) As String
    Dim pic_count As Integer
    Dim bs_pic_count As Integer
    Dim ss_type As String
    Dim ind As Integer = 1
    Dim bs_ind As Integer = 1
    Dim ImageItem, ImageSubItem, ImagePic, IconSubItem, BackIcon As Image
    Dim item_snd As New SoundPlayer
    Dim subitem_snd As New SoundPlayer
    Dim pic_snd As New SoundPlayer
    Dim slide_snd As New SoundPlayer
    Dim deny_snd As New SoundPlayer
    Dim selected_mainmenuitm As Integer
    Dim selected_itm_mode As String
    Dim r_side_panel As Boolean = True
    Dim video_mode As Boolean = False
    Dim item_view As Boolean = False
    Dim sub_item As Boolean = False
    Dim demo_mode As Boolean = True
    Dim items_limit As Integer = 4
    Dim menu_x_eff As Boolean = False
    Dim sshow_bg As New SolidColorBrush(ColorConverter.ConvertFromString("#77FFFFFF"))
    Dim panel_bg As New SolidColorBrush(ColorConverter.ConvertFromString("#77FFFFFF"))
    Dim item_bg As New SolidColorBrush(ColorConverter.ConvertFromString("#FFFFFFFF"))
    Dim ExitButton As Image
    Dim SubslideButton As Image
    Dim NextButton As Image
    Dim PrevButton As Image
    'Dim subs1, subs2, subs3 As New Image
    Dim StPanSubs As New WrapPanel
    Dim StPanSubs2 As New StackPanel
    Dim me_w, me_h, me_x, me_y As Double
    Dim cur_img(4) As Integer
    Dim img_count(4) As Integer
    Dim level As Integer = 1
    Dim level_url(4) As String
    Dim preload_img_bg(99) As BitmapImage
    Dim preload_img_fg(99) As BitmapImage
    Dim preload_img_menu_uns(99) As BitmapImage
    Dim preload_img_menu_sel(99) As BitmapImage

    Dim use_home_timer As Boolean = False
    Dim home_screen As Boolean = False
    Dim help_opacity As Double = 1
    Dim ml1 As Boolean = False
    Dim ml2 As Boolean = False
    Dim touch_zoom_eff As Boolean = False
    Dim text_data(99) As String
    Dim text_lines As Integer = 0
    Dim show_date_time As Boolean = False
    Dim mpP As Windows7.Multitouch.Manipulation.ManipulationProcessor
    Dim multitouch_sw As Boolean = False
    'BG-V param
    Dim bgv_mode As Boolean = False
    Dim bgv_tr As New TranslateTransform
    Dim slides_transition_class As String = "A"
    Dim slides_transition_type As Integer
    Dim bg_transition_type As Integer
    Dim fg_transition_type As Integer
    Dim slides_noresize As Boolean = False
    Dim slides_hide_direction As String = "right"
    Dim slides_hide_alpha As Boolean = False
    Dim bg_toucheffect As Integer = 0

    Dim icon_border As New Thickness(2, 2, 2, 2)
    Dim icon_margin As New Thickness(2, 2, 2, 2)

    Dim exit_button_position As String
    Dim previous_button_position As String
    Dim next_button_position As String
    Dim subslides_button_position As String
    Dim toolbar_position As String

    Dim menu_touch_effect_type As String
    Dim menu_touch_effect_amount As String

    Dim dataselector_hidden As Boolean = False

    Dim nextitem_action_hocked As Boolean = False
    Dim previtem_action_hocked As Boolean = False

    'TIMERS
    Dim sstimer As DispatcherTimer = New DispatcherTimer()
    Dim anim_timer As DispatcherTimer = New DispatcherTimer()
    Dim demolabel_timer As DispatcherTimer = New DispatcherTimer()
    Dim home_timer As DispatcherTimer = New DispatcherTimer()
    Dim activity_timer As DispatcherTimer = New DispatcherTimer()
    Dim bgeff_timer As DispatcherTimer = New DispatcherTimer()
    Dim fgeff_timer As DispatcherTimer = New DispatcherTimer()
    Dim subslide_select_1st_timer As DispatcherTimer = New DispatcherTimer()

    'MULTI-SLIDERS
    Dim BgMuSli_set As MultiSliderSettings
    Dim BgMuSli As MultiSlider

    Dim HomeMuSli_set As MultiSliderSettings
    Dim HomeMuSli As MultiSlider

    Dim FgMuSli_set As MultiSliderSettings
    Dim FgMuSli As MultiSlider

    Public Function GetFileContent(ByVal FullPath As String, Optional ByRef ErrInfo As String = "") As String
        'Dim strContent As String
        'Dim objReader As StreamReader
        'Try
        '    objReader = New StreamReader(FullPath)
        '    strContent = objReader.ReadToEnd()
        '    objReader.Close()
        '    GetFileContent = strContent
        'Catch Ex As Exception
        'End Try
    End Function

    Public Shared Function Decrypt(ByVal pstrText As String, ByVal pstrDecrKey As String) As String
        'pstrText = pstrText.Replace(" ", "+")
        'Dim byKey As Byte() = {}
        'Dim IV As Byte() = {18, 52, 86, 120, 144, 171, 205, 239}
        'Dim inputByteArray As Byte() = New Byte(pstrText.Length - 1) {}
        'byKey = System.Text.Encoding.UTF8.GetBytes(pstrDecrKey.Substring(0, 8))
        'Dim des As New DESCryptoServiceProvider()
        'inputByteArray = Convert.FromBase64String(pstrText)
        'Dim ms As New MemoryStream()
        'Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)
        'cs.Write(inputByteArray, 0, inputByteArray.Length)
        'cs.FlushFinalBlock()
        'Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
        'Return encoding.GetString(ms.ToArray())
    End Function

    'DATA SELECTOR
    Dim selected_data As Integer = 1
    Private Sub StackPanelDataSel_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles StackPanelDataSel.MouseUp
        If StackPanelDataSel.Children.Count >= 2 Then
            Dim feSource As Image = TryCast(e.Source, Image)
            For i = 0 To StackPanelDataSel.Children.Count - 1
                Dim img As Image = StackPanelDataSel.Children(i)

                If File.Exists(project_root + "data_selector\" + CStr(i + 1) + ".png") Then
                    img.Source = New BitmapImage(New Uri(project_root + "data_selector\" + CStr(i + 1) + ".png"))
                End If

                If Not IsNothing(feSource) Then
                    If feSource.Name = img.Name Then
                        If File.Exists(item_snd.SoundLocation) Then item_snd.Play()
                        selected_data = i + 1
                        img.Visibility = Visibility.Visible
                        img.BeginAnimation(Image.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25)))
                        img.Source = New BitmapImage(New Uri(project_root + "data_selector\" + CStr(i + 1) + "_sel" + ".png"))
                        UpdateDataSource()
                        ReloadData()
                    End If
                End If

            Next
        End If
    End Sub

    Private Sub UpdateDataSource()
        If selected_data = 1 Then data_root = project_root + "data\"
        If selected_data >= 2 Then data_root = project_root + "data_" + CStr(selected_data) + "\"
    End Sub

    Private Sub Licensing()
        'LICENSE
        If File.Exists(app_root + "lic.dat") Or File.Exists("c:\lic.dat") Then
            'Dim lic As String = ""
            'If File.Exists(app_root + "lic.dat") Then lic = GetFileContent(app_root + "lic.dat")
            'If File.Exists("c:\lic.dat") Then lic = GetFileContent("c:\lic.dat")
            'Dim nic As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces.ElementAt(0)
            'Dim mac As String = nic.GetPhysicalAddress().ToString
            'lic = Decrypt(lic, "MCWzS3fegCDZpQWqSf4hEhWnhsxDJFJBBtqXbCmwGW6CRhj2542N2b5SPhv6rrE")
            'If lic.Substring(0, 12) = mac Then
            '    Select Case lic.Substring(12)
            '        Case "2HgsGaX7dQWmA8MaBJCj06yHUTVQD0gfuq1wRmM9DlzESpYLCPgjhFo42MGkH7C" 'iNFO Core
            '            demo_mode = False
            '            items_limit = 99
            '        Case "2HgsGaX7dQWmA8MaBJCj06yHUTVQD0gfuq1wRmM9DlzESpYLCPgjhFo42MGkML1" 'iNFO ML1
            '            demo_mode = False
            '            items_limit = 99
            '            ml1 = True
            '        Case "2HgsGaX7dQWmA8MaBJCj06yHUTVQD0gfuq1wRmM9DlzESpYLCPgjhFo42MGkML2" 'iNFO ML2
            '            demo_mode = False
            '            items_limit = 99
            '            ml1 = True
            '            ml2 = True
            '    End Select
            'End If
        End If

        'TMP LIC DISABLE !!!
        If File.Exists(app_root + "unlic.dat") Then
            demo_mode = False
            items_limit = 99
            ml1 = True
            ml2 = True
        End If

        If demo_mode Then
            Dim label_demo As New Label
            label_demo.Content = "LIC ERR - DEMO MODE"
            label_demo.HorizontalAlignment = HorizontalAlignment.Left
            label_demo.VerticalAlignment = VerticalAlignment.Top
            label_demo.Width = 150
            label_demo.Height = 25
            Dim anim_lb As New ColorAnimation(Colors.Black, Colors.White, TimeSpan.FromSeconds(2))
            anim_lb.AutoReverse = True
            anim_lb.RepeatBehavior = RepeatBehavior.Forever
            Dim col As New SolidColorBrush
            col.BeginAnimation(SolidColorBrush.ColorProperty, anim_lb)
            label_demo.Foreground = col
            GridBg.Children.Add(label_demo)
        End If
    End Sub

    Private Sub demolabel_timer_Tick()
        'TbDemoText.BeginAnimation(Label.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(5)) With {.AutoReverse = True})
    End Sub

    'INITIAL SETUP ---> WINDOW LOADED
    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        'Licensing()
        '...SKIP FOR PERMANENT DEMO
        demo_mode = False
        items_limit = 99
        ml1 = True
        ml2 = True

        'LICENSE - DEMO MODE
        'label with anim
        AddHandler demolabel_timer.Tick, AddressOf demolabel_timer_Tick
        demolabel_timer.Interval = TimeSpan.FromSeconds(30)
        demolabel_timer.Start()

        'SETUP.INI
        If File.Exists(app_root + "project.inform") Then
            Dim objIniFile As New IniFile(app_root + "setup.ini")
            objIniFile.WriteString("setup", "project", app_root)
        End If

        If Not File.Exists(app_root + "setup.ini") Then
            Dim filter_str As String = "inForm project (*.inform)|*.inform"
            Dim dlg As New Microsoft.Win32.OpenFileDialog() With {.Multiselect = False, .Filter = filter_str}
            If dlg.ShowDialog() Then
                Dim filepath As String = dlg.FileName
                Dim fileinfo As New FileInfo(filepath)

                Dim objIniFile As New IniFile(app_root + "setup.ini")
                objIniFile.WriteString("setup", "project", fileinfo.Directory.FullName)
            End If
        End If

        If Not File.Exists(app_root + "setup.ini") Then
            Exit Sub
        End If

        If File.Exists(app_root + "setup.ini") Then
            Dim objIniFile As New IniFile(app_root + "setup.ini")
            'PROJECT PATH
            Dim project_str As String
            project_str = objIniFile.GetString("setup", "project", "my_project")
            If project_str <> "" Then
                If project_str.Contains(":") Then
                    'ABS - C:\_issimple\INFORM\bin\Debug\project_demo_1base
                Else
                    'REL - project_demo_1base
                    project_str = app_root + project_str
                End If
            End If
            project_str += "\"
            project_root = project_str
            data_root = project_root + "data\"
        End If

        'DATA SELECTOR
        If Directory.Exists(project_root + "data_selector\") Then
            For i = 1 To 9
                If File.Exists(project_root + "data_selector\" + CStr(i) + ".png") Then
                    Dim ImageDataSel As New Image
                    Dim suff As String = ""
                    If i = selected_data Then suff = "_sel"
                    ImageDataSel.Source = New BitmapImage(New Uri(project_root + "data_selector\" + CStr(i) + suff + ".png"))
                    ImageDataSel.Name = "ImageDataSel" + CStr(i)
                    StackPanelDataSel.Children.Add(ImageDataSel)
                    StackPanelDataSel.RegisterName(ImageDataSel.Name, ImageDataSel)
                End If
            Next
        End If

        GridBg.UpdateLayout()

        UpdateDataSource()
        ReloadData()

        'VIDEO CONTENT
        With GridMediaContent
            .Visibility = Visibility.Hidden
            .Width = me_w
            .Height = me_h
            .Margin = New Thickness(me_x, me_y, 0, 0)
            .HorizontalAlignment = HorizontalAlignment.Left
            .VerticalAlignment = VerticalAlignment.Top
        End With

        'BACKGROUND WORKER SETUP
        AddHandler bgworker.DoWork, AddressOf bgworker_dowork

        'HOME LANDING
        AddHandler GridLanding.MouseUp, AddressOf GridLanding_MouseUp

        'TIMERS
        AddHandler subslide_select_1st_timer.Tick, AddressOf subslide_select_1st_timer_tick
        subslide_select_1st_timer.Interval = TimeSpan.FromSeconds(1.5)
    End Sub

    Private Sub ReadSettingsIni()

        'SETTINGS.INI
        If File.Exists(data_root + "settings.ini") Then
            Dim objIniFile As New IniFile(data_root + "settings.ini")

            menu_x_eff = objIniFile.GetBoolean("setup", "menu_x_eff", False)
            touch_zoom_eff = objIniFile.GetBoolean("setup", "touch_zoom_eff", False)
            ImgMainMenuLogo.Opacity = CDbl(objIniFile.GetString("setup", "logo_opacity", "1"))
            GridMainMenuItems.Opacity = CDbl(objIniFile.GetString("setup", "menu_opacity", "1"))
            'BorderCmenu.Opacity = CDbl(objIniFile.GetString("setup", "cmenu_opacity", "1"))
            help_opacity = CDbl(objIniFile.GetString("setup", "help_opacity", "1"))
            panel_bg = New SolidColorBrush(ColorConverter.ConvertFromString(objIniFile.GetString("setup", "panel_color", "#FFFFFFFF")))
            item_bg = New SolidColorBrush(ColorConverter.ConvertFromString(objIniFile.GetString("setup", "item_color", "#FFFFFFFF")))
            sshow_bg = New SolidColorBrush(ColorConverter.ConvertFromString(objIniFile.GetString("setup", "sshow_color", "#FFFFFFFF")))
            BorderHomeSlides.Background = sshow_bg
            'TEST MODE WxH
            If objIniFile.GetBoolean("setup", "test_mode", False) = True Then
                Dim test_width As Integer = objIniFile.GetInteger("setup", "test_width", "1920")
                Dim test_height As Integer = objIniFile.GetInteger("setup", "test_height", "1080")
                GridBg.Width = test_width
                GridBg.Height = test_height
                If Me.ActualWidth > test_width Or Me.ActualHeight > test_height Then ViewboxLayout.Stretch = Stretch.Uniform
                If Me.ActualWidth > test_width And Me.ActualHeight > test_height Then ViewboxLayout.Stretch = Stretch.None
                If Me.ActualWidth < test_width And Me.ActualHeight < test_height Then ViewboxLayout.Stretch = Stretch.Uniform

                'MULTI-MONITOR STRETCH MODE
                Dim screens() As System.Windows.Forms.Screen = System.Windows.Forms.Screen.AllScreens
                If screens.Count = 2 Then
                    MsgBox("Setup.ini W/H = " + test_width.ToString + ", " + test_height.ToString)
                    MsgBox("Screen 1 W/H = " + screens(0).WorkingArea.Width.ToString + ", " + screens(0).WorkingArea.Height.ToString)
                    MsgBox("Screen 2 W/H = " + screens(1).WorkingArea.Width.ToString + ", " + screens(1).WorkingArea.Height.ToString)
                    MsgBox("Screen 1 T/L = " + screens(0).WorkingArea.Top.ToString + ", " + screens(0).WorkingArea.Left.ToString)
                    MsgBox("Screen 2 T/L = " + screens(1).WorkingArea.Top.ToString + ", " + screens(1).WorkingArea.Left.ToString)

                    Dim wH As Integer = screens(0).WorkingArea.Height
                    Dim wW As Integer = screens(0).WorkingArea.Width
                    Dim dualmode As Boolean = False

                    If test_height = wH And test_width = 2 * wW Then dualmode = True
                    If test_height = 2 * wH And test_width = wW Then dualmode = True

                    If dualmode Then
                        Me.WindowState = Windows.WindowState.Normal
                        Me.WindowStartupLocation = Windows.WindowStartupLocation.Manual
                        Me.Top = screens(1).WorkingArea.Top
                        Me.Left = 0
                        Me.Width = screens(0).Bounds.Width
                        Me.Height = screens(0).Bounds.Height * 2
                        MsgBox("Dual Screen Stretch Mode")
                    End If
                End If

                Dim mask_col As New SolidColorBrush(ColorConverter.ConvertFromString(objIniFile.GetString("setup", "mask_color", "#FF000000")))
                Window1.Background = mask_col

                Dim MaskBrd1 As New Border
                With MaskBrd1
                    .BorderBrush = Brushes.Transparent
                    .Background = mask_col
                    .HorizontalAlignment = HorizontalAlignment.Stretch
                    .VerticalAlignment = VerticalAlignment.Top
                    .Height = Math.Abs((Window1.ActualHeight - GridBg.Height)) / 2
                    .Margin = New Thickness(0, -MaskBrd1.Height, 0, 0)
                End With

                Grid.SetColumnSpan(MaskBrd1, 2)
                GridBg.Children.Add(MaskBrd1)

                Dim MaskBrd2 As New Border
                With MaskBrd2
                    .BorderBrush = Brushes.Transparent
                    .Background = mask_col
                    .HorizontalAlignment = HorizontalAlignment.Stretch
                    .VerticalAlignment = VerticalAlignment.Bottom
                    .Height = Math.Abs((Window1.ActualHeight - GridBg.Height)) / 2
                    .Margin = New Thickness(0, 0, 0, -MaskBrd2.Height)
                End With

                Grid.SetColumnSpan(MaskBrd2, 2)
                GridBg.Children.Add(MaskBrd2)

                Dim MaskBrd3 As New Border
                With MaskBrd3
                    .BorderBrush = Brushes.Transparent
                    .Background = mask_col
                    .HorizontalAlignment = HorizontalAlignment.Left
                    .VerticalAlignment = VerticalAlignment.Stretch
                    .Width = Math.Abs((Window1.ActualWidth - GridBg.Width)) / 2
                    .Margin = New Thickness(-MaskBrd3.Width, 0, 0, 0)
                End With
                GridBg.Children.Add(MaskBrd3)

                Dim MaskBrd4 As New Border
                With MaskBrd4
                    .BorderBrush = Brushes.Transparent
                    .Background = mask_col
                    .HorizontalAlignment = HorizontalAlignment.Right
                    .VerticalAlignment = VerticalAlignment.Stretch
                    .Width = Math.Abs((Window1.ActualWidth - GridBg.Width)) / 2
                    .Margin = New Thickness(0, 0, -MaskBrd4.Width, 0)
                End With

                Grid.SetColumnSpan(MaskBrd4, 2)
                GridBg.Children.Add(MaskBrd4)
            Else
                GridBg.Width = System.Windows.SystemParameters.PrimaryScreenWidth
                GridBg.Height = System.Windows.SystemParameters.PrimaryScreenHeight
            End If
            GridBg.UpdateLayout()

            'ICON PANELS MARGIN
            BorderIcons.Margin = New Thickness(5)
            Dim mrg_str As String = objIniFile.GetString("setup", "iconspanel_margin", "5")
            If mrg_str.Contains(",") Then
                Dim mrgs() As String = mrg_str.Split(",")
                Try
                    Dim mrg1 As Integer = CInt(mrgs(0))
                    Dim mrg2 As Integer = CInt(mrgs(1))
                    Dim mrg3 As Integer = CInt(mrgs(2))
                    Dim mrg4 As Integer = CInt(mrgs(3))
                    BorderIcons.Margin = New Thickness(mrg1, mrg2, mrg3, mrg4)
                    BorderIcons.MaxWidth = GridBg.ActualWidth - mrg1 - mrg3
                    BorderIcons.MaxHeight = GridBg.ActualHeight - mrg2 - mrg4
                Catch ex As Exception
                End Try
            Else
                If IsNumeric(mrg_str) Then BorderIcons.Margin = New Thickness(CInt(mrg_str))
            End If

            'ICON MARGIN
            Dim ic_mrg_str As String = objIniFile.GetString("setup", "icon_margin", "5")
            If ic_mrg_str.Contains(",") Then
                Dim ic_mrgs() As String = ic_mrg_str.Split(",")
                Try
                    icon_margin = New Thickness(CInt(ic_mrgs(0)), CInt(ic_mrgs(1)), CInt(ic_mrgs(2)), CInt(ic_mrgs(3)))
                Catch ex As Exception
                End Try
            Else
                If IsNumeric(ic_mrg_str) Then icon_margin = New Thickness(CInt(ic_mrg_str), CInt(ic_mrg_str), CInt(ic_mrg_str), CInt(ic_mrg_str))
            End If

            'ICON BORDER
            Dim ic_brd_str As String = objIniFile.GetString("setup", "icon_border", "2")
            If ic_brd_str.Contains(",") Then
                Dim ic_brds() As String = ic_brd_str.Split(",")
                Try
                    icon_border = New Thickness(CInt(ic_brds(0)), CInt(ic_brds(1)), CInt(ic_brds(2)), CInt(ic_brds(3)))
                Catch ex As Exception
                End Try
            Else
                If IsNumeric(ic_brd_str) Then icon_border = New Thickness(CInt(ic_brd_str), CInt(ic_brd_str), CInt(ic_brd_str), CInt(ic_brd_str))
            End If

            'SLIDES PANEL MARGIN
            BorderHomeSlides.Margin = New Thickness(5)
            Dim mrg_str2 As String = objIniFile.GetString("setup", "slidespanel_margin", "5")
            If mrg_str2.Contains(",") Then
                Dim mrgs() As String = mrg_str2.Split(",")
                Try
                    Dim mrg1 As Integer = CInt(mrgs(0))
                    Dim mrg2 As Integer = CInt(mrgs(1))
                    Dim mrg3 As Integer = CInt(mrgs(2))
                    Dim mrg4 As Integer = CInt(mrgs(3))
                    BorderHomeSlides.Margin = New Thickness(mrg1, mrg2, mrg3, mrg4)
                Catch ex As Exception
                End Try
            Else
                If IsNumeric(mrg_str2) Then BorderHomeSlides.Margin = New Thickness(CInt(mrg_str2))
            End If

            'SLIDES NAVIGATION
            exit_button_position = objIniFile.GetString("slides_navigation", "exit_button_position", "center-top")
            previous_button_position = objIniFile.GetString("slides_navigation", "previous_button_position", "left-middle")
            next_button_position = objIniFile.GetString("slides_navigation", "next_button_position", "right-middle")
            subslides_button_position = objIniFile.GetString("slides_navigation", "subslides_button_position", "center-bottom")
            toolbar_position = objIniFile.GetString("slides_navigation", "toolbar_position", "left-bottom")

            'MENU TOUCH EFFECTS
            menu_touch_effect_type = objIniFile.GetString("menu_touch_effect", "type", "left")
            menu_touch_effect_amount = objIniFile.GetString("menu_touch_effect", "amount", "5")

            'BG TOUCH EFFECT
            bg_toucheffect = objIniFile.GetInteger("setup", "bg_toucheffect", "0")

            'CONTENT VIDEO POS
            me_w = objIniFile.GetInteger("setup", "me_w", System.Windows.SystemParameters.PrimaryScreenWidth)
            me_h = objIniFile.GetInteger("setup", "me_h", System.Windows.SystemParameters.PrimaryScreenHeight)
            me_x = objIniFile.GetInteger("setup", "me_x", 0)
            me_y = objIniFile.GetInteger("setup", "me_y", 0)

            'BACKGROUND VIDEO POS
            If objIniFile.GetBoolean("setup", "ManualVideoBackgroundPositionSetup", False) = True Then
                With MediaUriElementBg
                    .HorizontalAlignment = HorizontalAlignment.Left
                    .VerticalAlignment = VerticalAlignment.Top
                    .Width = objIniFile.GetInteger("setup", "vbg_w", 640)
                    .Height = objIniFile.GetInteger("setup", "vbg_h", 480)
                    .Margin = New Thickness(objIniFile.GetInteger("setup", "vbg_x", 0), objIniFile.GetInteger("setup", "vbg_y", 0), 0, 0)
                End With
                'With BorderHomeSlides
                '    .HorizontalAlignment = HorizontalAlignment.Left
                '    .VerticalAlignment = VerticalAlignment.Top
                '    .Width = objIniFile.GetInteger("setup", "vbg_w", 640)
                '    .Height = objIniFile.GetInteger("setup", "vbg_h", 480)
                '    .Margin = New Thickness(objIniFile.GetInteger("setup", "vbg_x", 0), objIniFile.GetInteger("setup", "vbg_y", 0), 0, 0)
                'End With
            End If

            'HOME TIMER
            AddHandler home_timer.Tick, AddressOf home_timer_tick
            AddHandler activity_timer.Tick, AddressOf activity_timer_tick
            home_timer.Interval = New TimeSpan(0, 0, 0, 500)
            If objIniFile.GetInteger("setup", "home_timer", 0) > 0 Then
                use_home_timer = True
                home_timer.Interval = New TimeSpan(0, 0, objIniFile.GetInteger("setup", "home_timer", 0))
                activity_timer.Start()
            End If
            home_screen = True

            'DELAYED ACTION - SHEFF (G-CODE)
            'AddHandler DelayedActionTimer.Tick, AddressOf DelayedActionTimerTick

            'SLIDES TIMER PRESET
            slides_timer_preset = objIniFile.GetInteger("setup", "slides_timer", 10)
            If slides_timer_preset <= 0 Then slides_timer_preset = 1
            'SLIDES TRANS CLASS
            slides_transition_class = objIniFile.GetString("setup", "slides_transition_class", "A")
            'SLIDES TRANS TYPE
            slides_transition_type = objIniFile.GetInteger("setup", "slides_transition_type", 10)
            'BG TRANS TYPE
            bg_transition_type = objIniFile.GetInteger("setup", "bg_transition_type", 13)
            'FG TRANS TYPE
            fg_transition_type = objIniFile.GetInteger("setup", "fg_transition_type", 17)
            'SLIDES NORESIZE
            slides_noresize = objIniFile.GetBoolean("setup", "slides_noresize", False)
            'SLIDES HIDE DIR
            slides_hide_direction = objIniFile.GetString("setup", "slides_hide_direction", "left").ToUpper
            'SLIDES HIDE APLHA
            slides_hide_alpha = objIniFile.GetBoolean("setup", "slides_hide_alpha", False)

            'GET MULTI-SLIDER SETTINGS
            With objIniFile
                'BG
                Dim locator As String = "bg-slides"
                BgMuSli_set = New MultiSliderSettings(.GetString(locator, "class", "B"),
                                                      .GetInteger(locator, "type", "13"),
                                                      .GetInteger(locator, "delay", "0"),
                                                      .GetInteger(locator, "eff_duration", "1"),
                                                      .GetString(locator, "alpha", "0"))
                'HOME
                locator = "home-slides"
                HomeMuSli_set = New MultiSliderSettings(.GetString(locator, "class", "B"),
                                                        .GetInteger(locator, "type", "13"),
                                                        .GetInteger(locator, "delay", "0"),
                                                        .GetInteger(locator, "eff_duration", "1"),
                                                        .GetString(locator, "alpha", "0"))
                'FG
                locator = "fg-slides"
                FgMuSli_set = New MultiSliderSettings(.GetString(locator, "class", "B"),
                                                      .GetInteger(locator, "type", "13"),
                                                      .GetInteger(locator, "delay", "0"),
                                                      .GetInteger(locator, "eff_duration", "1"),
                                                      .GetString(locator, "alpha", "0"))
            End With

            'SHOW DATE TIME
            If objIniFile.GetBoolean("setup", "show_date_time", 0) Then
                show_date_time = True
                TextBlockTime.Visibility = Visibility.Visible
                LabelDate.Visibility = Visibility.Visible
                Dim txt_eff As New Effects.DropShadowEffect
                txt_eff.BlurRadius = 3
                txt_eff.ShadowDepth = 0
                TextBlockTime.Effect = txt_eff
                LabelDate.Effect = txt_eff
                TextBlockTime.Text = DateTime.Now.ToShortTimeString()
                LabelDate.Content = DateTime.Now.ToShortDateString()
            End If
        End If

    End Sub

    'LANDING STUFF
    Private Sub ImageLandingBtn1_MouseUp() Handles ImageLandingBtn1.MouseUp
        If GridLanding.Visibility = Windows.Visibility.Collapsed Then
            GridLanding.Visibility = Windows.Visibility.Visible
            GridLanding.BeginAnimation(Grid.OpacityProperty, New DoubleAnimation(0.5, 1, TimeSpan.FromSeconds(0.5)))
            slide_snd.Play()
        End If
    End Sub
    Private Sub GridLanding_MouseUp()
        If GridLanding.Visibility = Windows.Visibility.Visible Then
            GridLanding.Visibility = Windows.Visibility.Collapsed
        End If
    End Sub

    'XML HOME SIGNAGE DIMs
    Dim XmlHomeBlocksData As XmlhomeBlocks
    Dim XmlHome As Boolean = False
    Dim XmlHomeHidden As Boolean = False

    Dim hitResultsList As New ArrayList

    Dim XmlHomeTimeDateTimer As DispatcherTimer

    'XML HOME - DATE TIME UPD
    Private Sub xmlTimeDateTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        Dim TbTime As TextBlock = FindName("XmlHomeTime")
        Dim TbDate As TextBlock = FindName("XmlHomeDate")

        TbTime.Text = DateTime.Now.ToShortTimeString()
        TbDate.Text = DateTime.Now.ToLongDateString() 'DateTime.Now.DayOfWeek.ToString + ", " + 

        If DateTime.Now.Minute.ToString <> prev_min Then
            Dim anim_time As New DoubleAnimation(0, -5, TimeSpan.FromSeconds(0.25))
            anim_time.AutoReverse = True
            Dim time_eff As New TextEffect
            time_eff.PositionStart = 3
            time_eff.PositionCount = 2
            Dim time_tr As New TranslateTransform
            time_eff.Transform = time_tr
            time_tr.BeginAnimation(TranslateTransform.YProperty, anim_time)
            TbTime.TextEffects.Add(time_eff)
        End If
        prev_min = DateTime.Now.Minute.ToString
    End Sub


    ' Return the result of the hit test to the callback.
    Public Function MyHitTestResult(ByVal result As HitTestResult) As HitTestResultBehavior
        ' Add the hit test result to the list that will be processed after the enumeration.
        Dim obj As Object = result.VisualHit
        Dim img As Image = TryCast(obj, Image)
        If Not IsNothing(img) Then hitResultsList.Add(img.Source.ToString)

        ' Set the behavior to return visuals at all z-order levels.
        Return HitTestResultBehavior.Continue
    End Function

    'XML HOME TOUCH LINKS TO OPEN CONTENT
    Private Sub XmlHomeItemHandler(ByVal sender As Object, ByVal e As MouseButtonEventArgs)

        If home_screen Then
            hitResultsList.Clear()
            Dim pt As Point = e.GetPosition(CType(sender, UIElement))
            VisualTreeHelper.HitTest(sender, Nothing, New HitTestResultCallback(AddressOf MyHitTestResult), New PointHitTestParameters(pt))

            If hitResultsList.Count > 0 Then
                For i = 0 To XmlHomeBlocksData.ItemsCount - 1
                    Dim grid_obj As Grid = TryCast(GridXmlhome.Children(i), Grid)
                    If Not IsNothing(grid_obj) Then

                        Dim img_obj As Image = TryCast(grid_obj.Children(grid_obj.Children.Count - 1), Image)
                        If Not IsNothing(img_obj) Then
                            If Not IsNothing(img_obj.Source) Then
                                If img_obj.Source.ToString = hitResultsList(0) Then

                                    Dim new_sender As Image = TryCast(VisualTreeHelper.GetChild(GridMainMenuItems, CInt(XmlHomeBlocksData.Link(i)) - 1), Image)
                                    If Not IsNothing(new_sender) Then ZImageItemHandler(new_sender, e)
                                    Exit Sub

                                End If
                            End If
                        End If
                    End If

                Next
            End If
        End If

        ImageItemHandler(sender, e)
        GridMainMenu_MouseDown(sender, e)
        If Not home_screen Then PngIconSubItemHandler(sender, e)

    End Sub


    'BACKGROUND WORKER WORK
    Dim bgworker As New BackgroundWorker
    Private Sub bgworker_dowork()
        MsgBox("Sending...")
    End Sub

    'GRID SLIDE
    Dim ImgSlide1 As New Image With {.Stretch = Stretch.Uniform}
    Dim ImgSlide_L2 As New Image With {.Stretch = Stretch.Uniform, .IsHitTestVisible = False}
    Dim ImgSlide_L3 As New Image With {.Stretch = Stretch.Uniform, .IsHitTestVisible = False}
    Dim ImgSlide2 As New Image With {.Stretch = Stretch.Uniform}
    Dim ImgSlide3 As New Image With {.Stretch = Stretch.Uniform}
    Dim ImageHelp2 As New Image
    Dim BrdSlideDots As New Border With {.VerticalAlignment = VerticalAlignment.Bottom, .Margin = New Thickness(5)}
    Dim StackPanelSlideDots As New StackPanel With {.Orientation = Orientation.Horizontal}
    Dim InkCanvasSlide As New InkCanvas With {.Visibility = Visibility.Hidden}
    Dim BrdSlideExtras As New Border With {.HorizontalAlignment = HorizontalAlignment.Left, .VerticalAlignment = VerticalAlignment.Bottom, .Margin = New Thickness(0)}
    Dim StackPanelSlideExtras As New StackPanel With {.Orientation = Orientation.Horizontal}
    Dim ImgSlideMultiTouch As New Image With {.Margin = New Thickness(0)}
    Dim ImgSlideNote As New Image With {.Margin = New Thickness(0)}
    Dim ImgSlideEmail As New Image With {.Margin = New Thickness(0)}
    Dim ImgSlideFacebook As New Image With {.Margin = New Thickness(0)}
    Dim ImageHelp As New Image
    Dim StackPanelEmail As New StackPanel With {.HorizontalAlignment = HorizontalAlignment.Left, .Margin = New Thickness(60), .Visibility = Visibility.Hidden,
                                                      .VerticalAlignment = VerticalAlignment.Bottom, .Orientation = Orientation.Horizontal}
    Dim TextBoxEmail As New TextBox With {.Width = 100, .Height = 48}
    Dim BtnEmailOK As New Button With {.Content = "OK"}

    Dim InkToolsPanel As New InkTools With {.Visibility = Visibility.Hidden}

    Private Sub ReloadData()

        ReadSettingsIni()

        ImageLandingBtn1.Visibility = Visibility.Hidden
        GridLanding.Visibility = Visibility.Collapsed
        GridLanding.Children.Clear()
        If Directory.Exists(data_root + "landing") Then

            Dim tr_type As String = "C"
            Dim tr_code As Integer = 1
            Dim tr_delay As Integer = 2
            Dim tr_time As Integer = 1

            If File.Exists(data_root + "landing\" + "\landing.ini") Then
                Dim objIniFile As New IniFile(data_root + "landing\" + "\landing.ini")
                tr_type = objIniFile.GetString("setup", "transition_type", "C")
                tr_code = objIniFile.GetInteger("setup", "transition_code", 1)
                tr_delay = objIniFile.GetInteger("setup", "transition_delay", 2)
                tr_time = objIniFile.GetInteger("setup", "transition_time", 1)
            End If

            If File.Exists(data_root + "landing\" + "\btn.png") Then
                ImageLandingBtn1.Source = New BitmapImage(New Uri(data_root + "landing\" + "\btn.png"))
                ImageLandingBtn1.Visibility = Visibility.Visible
                AddHandler ImageLandingBtn1.MouseUp, AddressOf ImageLandingBtn1_MouseUp
            End If

            If File.Exists(data_root + "landing\" + "\bg.png") Then
                Dim ImageLandingBg As New Image
                ImageLandingBg.Source = New BitmapImage(New Uri(data_root + "landing\" + "\bg.png"))
                GridLanding.Children.Add(ImageLandingBg)
            End If


            If tr_type = "B" Then
                Dim LandingSlider As New SheffSlider(data_root + "landing\", "", tr_code, tr_delay, tr_time, 0.1, ".png")
                GridLanding.Children.Add(LandingSlider)
            End If

            If tr_type = "C" Then
                Dim LandingSlider As New XySlider(data_root + "landing\", "", tr_code, tr_delay, tr_time, 0.1, ".png")
                GridLanding.Children.Add(LandingSlider)
            End If

            If File.Exists(data_root + "landing\" + "\fg.png") Then
                Dim ImageLandingFg As New Image
                ImageLandingFg.Source = New BitmapImage(New Uri(data_root + "landing\" + "\fg.png"))
                GridLanding.Children.Add(ImageLandingFg)
            End If
        End If

        'XML HOME SIGNAGE
        GridXmlhome.Visibility = Visibility.Hidden
        GridXmlhome.Children.Clear()
        If File.Exists(data_root + "xml_home\blocks.xml") Then
            XmlHomeBlocksData = New XmlhomeBlocks(data_root + "xml_home\blocks.xml")
            For i = 0 To XmlHomeBlocksData.ItemsCount - 1
                'GRID SUB-CONTAINER
                Dim BlockGrid As New Grid
                With BlockGrid
                    .Margin = New Thickness(XmlHomeBlocksData.Left(i), XmlHomeBlocksData.Top(i), 0, 0)
                    .Width = XmlHomeBlocksData.Width(i)
                    .Height = XmlHomeBlocksData.Height(i)
                    .HorizontalAlignment = HorizontalAlignment.Left
                    .VerticalAlignment = VerticalAlignment.Top
                End With

                'BG IMAGE
                Dim BlockGridBg As New Image
                If File.Exists(data_root + "xml_home\" + XmlHomeBlocksData.Source(i) + "\bg.png") Then _
                BlockGridBg.Source = New BitmapImage(New Uri(data_root + "xml_home\" + XmlHomeBlocksData.Source(i) + "\bg.png"))
                BlockGridBg.Stretch = Stretch.None
                BlockGrid.Children.Add(BlockGridBg)

                'SLIDES
                If XmlHomeBlocksData.Type(i) = "slides" Then
                    Dim BlockGridSheffSlider As New XySlider(data_root + "xml_home\" + XmlHomeBlocksData.Source(i) + "\", "",
                                                                XmlHomeBlocksData.SheffIndex(i), XmlHomeBlocksData.SheffDelay(i), XmlHomeBlocksData.SheffDuration(i), , ".png")
                    BlockGrid.Children.Add(BlockGridSheffSlider)
                End If

                'TIMEDATE
                If XmlHomeBlocksData.Type(i) = "timedate" Then
                    Dim StPanTimedate As New StackPanel With {.Orientation = Orientation.Horizontal, .Margin = New Thickness(20, 0, 10, 10)}
                    Dim TbTime As New TextBlock With {.Name = "XmlHomeTime", .FontSize = 72, .Foreground = Brushes.White}
                    Dim TbDate As New TextBlock With {.Name = "XmlHomeDate", .FontSize = 18, .Foreground = Brushes.White,
                                                      .VerticalAlignment = VerticalAlignment.Bottom, .Margin = New Thickness(20, 0, 0, 10)}
                    StPanTimedate.Children.Add(TbTime)
                    StPanTimedate.Children.Add(TbDate)
                    BlockGrid.Children.Add(StPanTimedate)

                    If IsNothing(GridXmlhome.FindName(TbTime.Name)) Then GridXmlhome.RegisterName(TbTime.Name, TbTime)
                    If IsNothing(GridXmlhome.FindName(TbDate.Name)) Then GridXmlhome.RegisterName(TbDate.Name, TbDate)

                    XmlHomeTimeDateTimer = New DispatcherTimer() With {.Interval = TimeSpan.FromSeconds(1)}
                    AddHandler XmlHomeTimeDateTimer.Tick, AddressOf xmlTimeDateTimer_Tick
                    XmlHomeTimeDateTimer.Start()
                End If

                'FG IMAGE
                Dim BlockGridFg As New Image
                If File.Exists(data_root + "xml_home\" + XmlHomeBlocksData.Source(i) + "\fg.png") Then _
                BlockGridFg.Source = New BitmapImage(New Uri(data_root + "xml_home\" + XmlHomeBlocksData.Source(i) + "\fg.png"))
                BlockGridFg.Stretch = Stretch.None
                BlockGrid.Children.Add(BlockGridFg)

                BlockGrid.BeginAnimation(Grid.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(1)) With {.BeginTime = TimeSpan.FromSeconds(0.5 * i)})

                GridXmlhome.Children.Add(BlockGrid)
            Next
            If GridXmlhome.Children.Count > 0 Then
                GridXmlhome.Visibility = Visibility.Visible
                XmlHome = True
            End If
        End If
        'SH EFF TST TMP
        If File.Exists(data_root + "fg_tmp.png") Then
            Dim sh_msk_obj As New Border With {.HorizontalAlignment = HorizontalAlignment.Stretch, .VerticalAlignment = VerticalAlignment.Stretch}
            sh_msk_obj.Effect = New ShaderEffectLibrary.RippleEffect
            sh_msk_obj.Background = New VisualBrush(GridXmlhome) With {.Stretch = Stretch.None, .AlignmentX = AlignmentX.Right}
            sh_msk_obj.OpacityMask = New ImageBrush() With {.ImageSource = New BitmapImage(New Uri(data_root + "fg_tmp.png"))}
            GridBg.Children.Add(sh_msk_obj)
        End If

        'STATIC PIC BG
        For i = 1 To items_limit
            If File.Exists(data_root + "" + CStr(i) + "sub/" + "bg.jpg") Then
                preload_img_bg(i) = New BitmapImage(New Uri(data_root + "" + CStr(i) + "sub/" + "bg.jpg"))
            End If
        Next i
        If File.Exists(data_root + "bg.jpg") Then
            preload_img_bg(0) = New BitmapImage(New Uri(data_root + "bg.jpg"))
        End If

        'STATIC PIC FG
        For i = 1 To items_limit
            If File.Exists(data_root + "" + CStr(i) + "sub/" + "fg.png") Then
                preload_img_fg(i) = New BitmapImage(New Uri(data_root + "" + CStr(i) + "sub/" + "fg.png"))
            End If
        Next i
        If File.Exists(data_root + "fg.png") Then
            preload_img_fg(0) = New BitmapImage(New Uri(data_root + "fg.png"))
        End If

        'VIDEO BG
        If File.Exists(data_root + "bg.avi") Then

            If File.Exists(data_root + "bg.jpg") Then
                If File.Exists(data_root + "interface\" + "hide.png") Then
                    ImageVBG.Source = New BitmapImage(New Uri(data_root + "interface\" + "hide.png"))
                    ImageVBG.Visibility = Visibility.Visible
                End If
            End If

            With MediaUriElementBg
                .Loop = True
                .HorizontalAlignment = HorizontalAlignment.Stretch
                .VerticalAlignment = VerticalAlignment.Stretch
                .BeginInit()
                'STRETCH MODES
                .Stretch = Stretch.UniformToFill
                .LoadedBehavior = WPFMediaKit.DirectShow.MediaPlayers.MediaState.Manual
                .VideoRenderer = WPFMediaKit.DirectShow.MediaPlayers.VideoRendererType.VideoMixingRenderer9
                If File.Exists(data_root + "bg.avi") Then
                    .Source = New Uri(data_root + "bg.avi")
                    .Volume = 1
                Else
                End If
                .EndInit()
            End With
            MediaUriElementBg.Play()
            MediaUriElementBg.Volume = 0.85
        End If

        'SLIDE GRID INIT
        GridSlide.Children.Clear()

        AddHandler ImgSlide1.MouseDown, AddressOf ImgSlide1_MouseDown
        'AddHandler BorderSlide.MouseUp, AddressOf ImgSlide1_MouseDown
        AddHandler ImgSlide1.MouseMove, AddressOf ImgSlide1_MouseMove
        'AddHandler BorderSlide.MouseMove, AddressOf ImgSlide1_MouseMove
        AddHandler ImgSlide1.MouseUp, AddressOf ImgSlide1_MouseUp
        'AddHandler BorderSlide.MouseUp, AddressOf ImgSlide1_MouseUp

        With GridSlide.Children
            .Add(ImgSlide3)
            .Add(ImgSlide2)
            .Add(ImgSlide1)
            .Add(ImgSlide_L2)
            .Add(ImgSlide_L3)
        End With

        With ImageHelp2
            .Width = 20
            .Height = 60
            .HorizontalAlignment = Windows.HorizontalAlignment.Right
            .VerticalAlignment = Windows.VerticalAlignment.Bottom
            .Stretch = Stretch.Uniform
            .Margin = New Thickness(10)
        End With
        GridSlide.Children.Add(ImageHelp2)

        BrdSlideDots.Child = StackPanelSlideDots
        GridSlide.Children.Add(BrdSlideDots)

        GridSlide.Children.Add(InkCanvasSlide)

        ImgSlideMultiTouch = New Image With {.Margin = New Thickness(0)}
        ImgSlideNote = New Image With {.Margin = New Thickness(0)}
        ImgSlideEmail = New Image With {.Margin = New Thickness(0)}
        ImgSlideFacebook = New Image With {.Margin = New Thickness(0)}

        AddHandler ImgSlideMultiTouch.MouseUp, AddressOf ImgSlideMultiTouch_MouseUp
        AddHandler ImgSlideNote.MouseUp, AddressOf ImgSlideNote_MouseUp
        AddHandler ImgSlideEmail.MouseUp, AddressOf ImgSlideEmail_MouseUp
        With StackPanelSlideExtras.Children
            .Clear()
            .Add(ImgSlideMultiTouch)
            .Add(ImgSlideNote)
            .Add(ImgSlideEmail)
            .Add(ImgSlideFacebook)
        End With
        BrdSlideExtras.Child = StackPanelSlideExtras
        SetNavigationPosition(toolbar_position, BrdSlideExtras)
        GridSlide.Children.Add(BrdSlideExtras)

        GridSlide.Children.Add(InkToolsPanel)

        With ImageHelp
            .Width = 20
            .Height = 60
            .HorizontalAlignment = Windows.HorizontalAlignment.Center
            .VerticalAlignment = Windows.VerticalAlignment.Bottom
            .Stretch = Stretch.Uniform
            .Margin = New Thickness(100)
        End With
        GridSlide.Children.Add(ImageHelp)

        StackPanelEmail.Children.Clear()
        StackPanelEmail.Children.Add(TextBoxEmail)
        StackPanelEmail.Children.Add(BtnEmailOK)
        GridSlide.Children.Add(StackPanelEmail)


        'JPG FS.SLIDES BG
        If File.Exists(data_root + "interface\" + "slide_bg.jpg") Then
            Dim slide_bg As New ImageBrush()
            slide_bg.ImageSource = New BitmapImage(New Uri(data_root + "interface\" + "slide_bg.jpg"))
            BorderSlide.Background = slide_bg
        End If

        'PNG FS.SLIDES BG
        SlidesBg.Source = Nothing
        If File.Exists(data_root + "interface\" + "slide_bg.png") Then
            BorderSlide.Background = Nothing
            SlidesBg.Source = New BitmapImage(New Uri(data_root + "interface\" + "slide_bg.png"))
        End If

        'PNG FS.SLIDES FG
        SlidesFg.Source = Nothing
        If File.Exists(data_root + "interface\" + "slide_fg.png") Then
            SlidesFg.Source = New BitmapImage(New Uri(data_root + "interface\" + "slide_fg.png"))
        End If

        'LOGO
        If File.Exists(data_root + "logo.png") Then
            ImgMainMenuLogo.Source = New BitmapImage(New Uri(data_root + "logo.png"))
            ImgMainMenuLogo.Width = ImgMainMenuLogo.Source.Width
            ImgMainMenuLogo.Height = ImgMainMenuLogo.Source.Height

            If File.Exists(data_root + "logo_mask.png") Then
                Dim mask As New ImageBrush()
                mask.ImageSource = New BitmapImage(New Uri(data_root + "logo_mask.png"))
                ImgMainMenuLogo.OpacityMask = mask
            End If

            'IMAGE LIC CHECK
            'Dim bmp As New BitmapImage()
            'Dim pix(4) As Byte
            'bmp = ImgLogo1.Source
            'Dim cbmp1 As New CroppedBitmap(bmp, New Int32Rect(100, 100, 1, 1))
            'Try
            '    cbmp1.CopyPixels(pix, 4, 0)
            '    If pix(2) <> 255 And pix(2) <> 255 And pix(2) <> 255 Then
            '        MsgBox("")
            '        Exit Sub
            '    Else
            '        Dim cbmp2 As New CroppedBitmap(bmp, New Int32Rect(20, 20, 1, 1))
            '        Try
            '            cbmp2.CopyPixels(pix, 4, 0)
            '            If pix(2) <> 85 And pix(2) <> 85 And pix(2) <> 85 Then
            '                MsgBox("")
            '                Exit Sub
            '            End If
            '        Catch ex As Exception
            '            MsgBox("")
            '            Exit Sub
            '        End Try
            '    End If
            'Catch ex As Exception
            '    MsgBox("")
            '    Exit Sub
            'End Try
        Else
        End If

        'UP/EXIT
        If File.Exists(data_root + gui_dir + "help_up.png") Then
            ExitButton = New Image
            With ExitButton
                .Source = New BitmapImage(New Uri(data_root + gui_dir + "help_up.png"))
                .Opacity = help_opacity
                .Stretch = Stretch.None
                .HorizontalAlignment = HorizontalAlignment.Center
                .VerticalAlignment = VerticalAlignment.Top
            End With
            SetNavigationPosition(exit_button_position, ExitButton)
            AddHandler ExitButton.MouseUp, AddressOf ExitSlides_Action
            GridSlide.Children.Add(ExitButton)
        End If

        'DN/ SINGLE SUBSLIDE
        If File.Exists(data_root + gui_dir + "help_dn.png") Then
            SubslideButton = New Image
            With SubslideButton
                .Source = New BitmapImage(New Uri(data_root + gui_dir + "help_dn.png"))
                .Opacity = help_opacity
                .Stretch = Stretch.None
                .HorizontalAlignment = HorizontalAlignment.Center
                .VerticalAlignment = VerticalAlignment.Bottom
            End With
            SetNavigationPosition(subslides_button_position, SubslideButton)
            AddHandler SubslideButton.MouseUp, AddressOf SubslideButton_Action
            GridSlide.Children.Add(SubslideButton)
        End If

        'DN/SUBSLIDES
        'With subs1
        '    .Name = "subs1"
        '    .Opacity = help_opacity
        '    .Width = 0
        '    .Stretch = Stretch.None
        '    .Margin = New Thickness(0)
        'End With
        'With subs2
        '    .Name = "subs2"
        '    .Opacity = help_opacity
        '    .Width = 0
        '    .Stretch = Stretch.None
        '    .Margin = New Thickness(0)
        'End With
        'With subs3
        '    .Name = "subs3"
        '    .Opacity = help_opacity
        '    .Width = 0
        '    .Stretch = Stretch.None
        '    .Margin = New Thickness(0)
        'End With
        With StPanSubs
            .HorizontalAlignment = HorizontalAlignment.Center
            .VerticalAlignment = VerticalAlignment.Bottom
            SetNavigationPosition(subslides_button_position, StPanSubs)
            .Orientation = Orientation.Horizontal
            .Visibility = Visibility.Hidden
            .Children.Clear()
            '.Children.Add(subs1)
            '.Children.Add(subs2)
            '.Children.Add(subs3)
            AddHandler .MouseUp, AddressOf StPanSubs_MouseUp
            GridSlide.Children.Add(StPanSubs)
            '.RegisterName("subs1", subs1)
            '.RegisterName("subs2", subs2)
            '.RegisterName("subs3", subs3)
        End With
        With StPanSubs2
            .HorizontalAlignment = HorizontalAlignment.Right
            .VerticalAlignment = VerticalAlignment.Top
            'SetNavigationPosition(subslides_button_position, StPanSubs)
            .Orientation = Orientation.Vertical
            .Visibility = Visibility.Hidden
            .Children.Clear()
            AddHandler .MouseUp, AddressOf StPanSubs2_MouseUp
            GridSlide.Children.Add(StPanSubs2)
        End With

        'LT/ PREV SLIDE
        If File.Exists(data_root + gui_dir + "help_lt.png") Then
            PrevButton = New Image
            With PrevButton
                .Source = New BitmapImage(New Uri(data_root + gui_dir + "help_lt.png"))
                .Opacity = help_opacity
                .Stretch = Stretch.None
                .HorizontalAlignment = HorizontalAlignment.Left
                .VerticalAlignment = VerticalAlignment.Center
            End With
            SetNavigationPosition(previous_button_position, PrevButton)
            'If Not previtem_action_hocked Then 
            AddHandler PrevButton.MouseUp, AddressOf PrevItem_Action
            GridSlide.Children.Add(PrevButton)
        End If
        'RT/ NEXT SLIDE
        If File.Exists(data_root + gui_dir + "help_rt.png") Then
            NextButton = New Image
            With NextButton
                .Source = New BitmapImage(New Uri(data_root + gui_dir + "help_rt.png"))
                .Opacity = help_opacity
                .Stretch = Stretch.None
                .HorizontalAlignment = HorizontalAlignment.Right
                .VerticalAlignment = VerticalAlignment.Center
            End With
            SetNavigationPosition(next_button_position, NextButton)
            'If Not nextitem_action_hocked Then 
            AddHandler NextButton.MouseUp, AddressOf NextItem_Action
            GridSlide.Children.Add(NextButton)
        End If

        'LOGO ANIM TIMER
        AddHandler anim_timer.Tick, AddressOf anim_timer_Tick
        anim_timer.Interval = TimeSpan.FromSeconds(0.5)

        'BG ANIM TIMER
        AddHandler bgeff_timer.Tick, AddressOf bgeff_timer_Tick
        bgeff_timer.Interval = TimeSpan.FromSeconds(0.25)
        'FG ANIM TIMER
        AddHandler fgeff_timer.Tick, AddressOf fgeff_timer_Tick
        fgeff_timer.Interval = TimeSpan.FromSeconds(0.75)

        'INIT TRANS SLIDE SHOW
        Dim trans_selector As New Transitionals.RandomTransitionSelector
        Dim ttrans(22) As Object
        ttrans(1) = New Transitionals.Transitions.CheckerboardTransition
        ttrans(2) = New Transitionals.Transitions.DiagonalWipeTransition
        ttrans(3) = New Transitionals.Transitions.DiamondsTransition
        ttrans(4) = New Transitionals.Transitions.DoorTransition
        ttrans(5) = New Transitionals.Transitions.DotsTransition
        ttrans(6) = New Transitionals.Transitions.DoubleRotateWipeTransition
        ttrans(7) = New Transitionals.Transitions.ExplosionTransition
        ttrans(8) = New Transitionals.Transitions.FadeAndBlurTransition
        ttrans(9) = New Transitionals.Transitions.FadeAndGrowTransition
        ttrans(10) = New Transitionals.Transitions.FadeTransition
        ttrans(11) = New Transitionals.Transitions.FlipTransition
        ttrans(12) = New Transitionals.Transitions.HorizontalBlindsTransition
        ttrans(13) = New Transitionals.Transitions.HorizontalWipeTransition
        ttrans(14) = New Transitionals.Transitions.MeltTransition
        ttrans(15) = New Transitionals.Transitions.PageTransition
        ttrans(16) = New Transitionals.Transitions.RollTransition
        ttrans(17) = New Transitionals.Transitions.RotateTransition
        ttrans(18) = New Transitionals.Transitions.RotateWipeTransition
        ttrans(19) = New Transitionals.Transitions.StarTransition
        ttrans(20) = New Transitionals.Transitions.TranslateTransition
        ttrans(21) = New Transitionals.Transitions.VerticalBlindsTransition
        ttrans(22) = New Transitionals.Transitions.VerticalWipeTransition
        For i = 1 To 22
            trans_selector.Transitions.Add(ttrans(i))
        Next i
        If slides_transition_class = "A" Then
            If slides_transition_type = 0 Then
                SlideShow.TransitionSelector = trans_selector
            Else
                SlideShow.Transition = ttrans(slides_transition_type)
            End If
        End If
        'CLASS B HOME SLIDES - SHEFF SLIDER
        If slides_transition_class = "B" Then
            Dim HomeSheffSlider As New SheffSlider(data_root + "slides\", "", slides_transition_type, slides_timer_preset, slides_timer_preset / 3, 0.1, ".png")
            'EFF 13 PIXELS NOT WORKING !!! 14,15 PROBLEM
            GridHomeMuSli.Children.Add(HomeSheffSlider)
            SlideShow.Visibility = Visibility.Hidden
            sstimer.Stop()
        End If

        'BG TRANS EFF
        'If bg_transition_type = 0 Then
        '    SlideShowBG.TransitionSelector = trans_selector
        'Else
        '    SlideShowBG.Transition = ttrans(bg_transition_type)
        'End If

        'FG TRANS EFF
        'If fg_transition_type = 0 Then
        '    SlideShowFG.TransitionSelector = trans_selector
        'Else
        '    SlideShowFG.Transition = ttrans(fg_transition_type)
        'End If

        'HOME PAGE SLIDES
        BorderHomeSlides.Visibility = Visibility.Hidden
        If Directory.Exists(data_root + "slides\") Then
            pic_count = 0
            pic_count = Directory.GetFiles(data_root + "slides\", "*.png").Count()
            If pic_count > 0 And Not File.Exists(data_root + "slides\video.avi") Then
                BorderHomeSlides.Visibility = Visibility.Visible
                Dim pic_recount As Integer = 0
                For i = 1 To pic_count
                    If File.Exists(data_root + "slides\" + CStr(i) + ".png") Then
                        pic_names(i) = data_root + "slides\" + CStr(i) + ".png"
                        pic_recount += 1
                    End If
                Next i
                tick = 0
                pic_count = pic_recount
                If pic_count <> 0 Then
                    For i = 0 To pic_count - 1
                        Dim ImageSlide As New Image
                        ImageSlide.Source = New BitmapImage(New Uri(pic_names(i + 1)))
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

                    sstimer.Interval = New TimeSpan(0, 0, 0, slides_timer_preset)
                    AddHandler sstimer.Tick, AddressOf sstimer_Tick
                    sstimer.Start()
                    SlideShow.TransitionNext()
                End If
            End If
            'VIDEO SLIDES
            If File.Exists(data_root + "slides\1.avi") Then
                BorderHomeSlides.Visibility = Visibility.Visible
                'MEDIA-KIT
                With MediaUriElement1
                    .Loop = False
                    If Not File.Exists(data_root + "slides\2.avi") Then .Loop = True
                    .HorizontalAlignment = HorizontalAlignment.Center
                    .VerticalAlignment = VerticalAlignment.Center
                    .BeginInit()
                    'STRETCH MODES
                    .Stretch = Stretch.Fill
                    .LoadedBehavior = WPFMediaKit.DirectShow.MediaPlayers.MediaState.Manual
                    .VideoRenderer = WPFMediaKit.DirectShow.MediaPlayers.VideoRendererType.VideoMixingRenderer9
                    If File.Exists(data_root + "slides\1.avi") Then
                        .Source = New Uri(data_root + "slides\1.avi")
                        .Volume = 1
                    Else
                    End If
                    .EndInit()
                End With
                cur_home_vdoslide = 1
                MediaUriElement1.Play()
                AddHandler MediaUriElement1.MediaEnded, AddressOf MediaUriElement1_MediaEnded
            End If
        End If

        'HOME PAGE BG SLIDES
        'If Directory.Exists(data_root + "bg_slides\") Then
        '    pic_count = 0
        '    pic_count = Directory.GetFiles(data_root + "bg_slides\", "*.png").Count()
        '    If pic_count > 0 Then
        '        For i = 1 To pic_count
        '            If File.Exists(data_root + "bg_slides\" + CStr(i) + ".png") Then
        '                pic_names(i) = data_root + "bg_slides\" + CStr(i) + ".png"
        '            End If
        '        Next i
        '        tick = 0
        '        For i = 1 To pic_count
        '            Dim ImageSlide As New Image
        '            ImageSlide.Source = New BitmapImage(New Uri(pic_names(i)))
        '            ImageSlide.Stretch = Stretch.Uniform
        '            Dim SlideShowItem As New Transitionals.Controls.SlideshowItem
        '            SlideShowItem.Content = ImageSlide
        '            SlideShowBG.Items.Add(SlideShowItem)
        '        Next i

        '        SlideShowBG.AutoAdvance = True
        '        SlideShowBG.AutoAdvanceDuration = TimeSpan.FromSeconds(0.5)
        '        SlideShowBG.TransitionNext()
        '    End If
        'End If

        'BG MULTI-SLIDER
        BgMuSli = New MultiSlider(BgMuSli_set.SliderClass, BgMuSli_set.EffectIndex)
        GridBgMuSli.Children.Clear()
        GridBgMuSli.Children.Add(BgMuSli)
        With BgMuSli
            .ReloadSource("MF", data_root + "", "", "sub", "bg", ".jpg",
                          BgMuSli_set.SlideDelay, BgMuSli_set.EffectDuration, 0.1, False)
            .UseAlpha = False
            .ManualPlayback = True

            .UpdateLayout()
            .CurrentSlide = -1
            .SetPlay(True)
        End With

        'FG MULTI-SLIDER
        FgMuSli = New MultiSlider(FgMuSli_set.SliderClass, FgMuSli_set.EffectIndex)
        GridFgMuSli.Children.Clear()
        GridFgMuSli.Children.Add(FgMuSli)
        With FgMuSli
            .ReloadSource("MF", data_root + "", "", "sub", "fg", ".png",
                          FgMuSli_set.SlideDelay, FgMuSli_set.EffectDuration, 0.1, False)
            .UseAlpha = False
            .ManualPlayback = True

            .UpdateLayout()
            .CurrentSlide = -1
            .SetPlay(True)
        End With

        'HOME MULTI-SLIDER
        SlideShow.Visibility = Visibility.Hidden
        BorderHomeSlides.Visibility = Visibility.Visible

        GridHomeMuSli.Children.Clear()
        HomeMuSli = New MultiSlider(HomeMuSli_set.SliderClass, HomeMuSli_set.EffectIndex)
        GridHomeMuSli.Children.Add(HomeMuSli)
        With HomeMuSli
            .ReloadSource("SF", data_root + "slides\", "", "", "", ".png",
                          HomeMuSli_set.SlideDelay, HomeMuSli_set.EffectDuration, 0.1, True)
            .UseAlpha = True
            .ManualPlayback = False

            .UpdateLayout()
            .CurrentSlide = -1
            .SetPlay(True)
        End With

        '-------------------------

        'SUB FG
        ImageSubFg.Visibility = Visibility.Hidden
        If File.Exists(data_root + "sub-fg.png") Then
            ImageSubFg.Source = New BitmapImage(New Uri(data_root + "sub-fg.png"))
            ImageSubFg.Visibility = Visibility.Visible
        End If

        'SOUNDS
        item_snd.SoundLocation = data_root + snd_dir + "item.wav"
        subitem_snd.SoundLocation = data_root + snd_dir + "subitem.wav"
        pic_snd.SoundLocation = data_root + snd_dir + "pic.wav"
        slide_snd.SoundLocation = data_root + snd_dir + "slide.wav"
        deny_snd.SoundLocation = data_root + snd_dir + "deny.wav"

        'TRACK
        If File.Exists(data_root + snd_dir + "track.mp3") Then
            MediaElementTrack.Source = New Uri(data_root + snd_dir + "track.mp3")
            If File.Exists(data_root + "interface\" + "mute.png") Then
                ImageSound.Source = New BitmapImage(New Uri(data_root + "interface\" + "mute.png"))
                ImageSound.Visibility = Visibility.Visible
            End If
            MediaElementTrack.Volume = 0.8
            MediaElementTrack.Play()
            MediaElementTrack.Loop = True
        End If

        'PRELOAD MENU IMG
        For i = 1 To items_limit
            If File.Exists(data_root + "" + CStr(i) + "btn.png") Then
                preload_img_menu_uns(i) = New BitmapImage(New Uri(data_root + "" + CStr(i) + "btn.png"))
            End If
            If File.Exists(data_root + "" + CStr(i) + "btn_sel.png") Then
                preload_img_menu_sel(i) = New BitmapImage(New Uri(data_root + "" + CStr(i) + "btn_sel.png"))
            End If
        Next i

        'REG MAIN MENU BTN
        GridMainMenuItems.Children.Clear()
        For i = 1 To items_limit
            If File.Exists(data_root + "" + CStr(i) + "btn.png") Then
                ImageItem = New Image() With {.Name = "ImageItem" + CStr(i), .Source = preload_img_menu_uns(i),
                                              .Width = .Source.Width, .Height = .Source.Height}
                GridMainMenuItems.Children.Add(ImageItem)
                If Not IsNothing(GridMainMenuItems.FindName("ImageItem" + CStr(i))) Then GridMainMenuItems.UnregisterName("ImageItem" + CStr(i))
                GridMainMenuItems.RegisterName("ImageItem" + CStr(i), ImageItem)
            End If
        Next i

        'REG SUBITEM BTN
        'For j = 1 To items_limit
        '    ImageSubItem = New Image()
        '    ImageSubItem.Name = "ImageSubItem" + CStr(j)
        '    Dim ScrollSubItem As New ScrollViewer
        '    ScrollSubItem.Name = "ScrollSubItem" + CStr(j)
        '    ScrollSubItem.Visibility = Visibility.Hidden
        '    ScrollSubItem.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden
        '    ScrollSubItem.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
        '    ScrollSubItem.Focusable = False
        '    ScrollSubItem.Content = ImageSubItem
        '    StackPanelContent.Children.Add(ScrollSubItem)
        '    StackPanelContent.RegisterName("ScrollSubItem" + CStr(j), ScrollSubItem)
        '    StackPanelContent.RegisterName("ImageSubItem" + CStr(j), ImageSubItem)
        'Next j

        BorderIcons.Background = panel_bg
        WrapPanelIcons.Children.Clear()

        'REG ICONS SUBITEM
        BackIcon = New Image() With {.Name = "BackIcon", .Margin = New Thickness(0)}
        Dim BackIconBorder = New Border
        With BackIconBorder
            .Child = BackIcon
            .Margin = icon_margin
            .BorderThickness = icon_border
            .BorderBrush = item_bg
            .Background = item_bg
            .Visibility = Visibility.Hidden
            .Width = 0
        End With
        WrapPanelIcons.Children.Add(BackIconBorder)
        If Not IsNothing(WrapPanelIcons.FindName("BackIconBorder")) Then WrapPanelIcons.UnregisterName("BackIconBorder")
        If Not IsNothing(WrapPanelIcons.FindName("BackIcon")) Then WrapPanelIcons.UnregisterName("BackIcon")
        WrapPanelIcons.RegisterName("BackIconBorder", BackIconBorder)
        WrapPanelIcons.RegisterName("BackIcon", BackIcon)

        For j = 1 To items_limit
            IconSubItem = New Image() With {.Name = "IconSubItem" + CStr(j), .Margin = New Thickness(0)}
            Dim IconSubItemBorder = New Border
            With IconSubItemBorder
                .Child = IconSubItem
                .Margin = icon_margin
                .BorderThickness = icon_border
                .BorderBrush = item_bg
                .Background = item_bg
                .Visibility = Visibility.Hidden
                .Width = 0
            End With
            WrapPanelIcons.Children.Add(IconSubItemBorder)
            If Not IsNothing(WrapPanelIcons.FindName("IconSubItemBorder" + CStr(j))) Then WrapPanelIcons.UnregisterName("IconSubItemBorder" + CStr(j))
            If Not IsNothing(WrapPanelIcons.FindName("IconSubItem" + CStr(j))) Then WrapPanelIcons.UnregisterName("IconSubItem" + CStr(j))
            WrapPanelIcons.RegisterName("IconSubItemBorder" + CStr(j), IconSubItemBorder)
            WrapPanelIcons.RegisterName("IconSubItem" + CStr(j), IconSubItem)
        Next j
        BorderSlide.Width = Double.NaN
        BorderSlide.Height = Double.NaN

        BorderIconsQV.BorderBrush = item_bg
        BorderMapQV.BorderBrush = item_bg

        'HEADER TEXT
        TextBlock1.Visibility = Visibility.Hidden
        If File.Exists(data_root + "header.txt") Then
            TextBlock1.Visibility = Visibility.Visible
            Dim oRead As System.IO.StreamReader = File.OpenText(data_root + "header.txt")
            Dim i As Integer = 0
            Do While oRead.Peek >= 0
                i += 1
                text_data(i) = oRead.ReadLine()
                If text_data(i) <> "" Then text_lines += 1
            Loop
            oRead.Close()
            Dim txt_eff As New Effects.DropShadowEffect
            txt_eff.BlurRadius = 3
            txt_eff.ShadowDepth = 0
            TextBlock1.Effect = txt_eff
            TextBlock1.Text = text_data(1)
            Dim anim_lb As New ColorAnimation(Colors.Black, Colors.White, TimeSpan.FromSeconds(5))
            anim_lb.AutoReverse = True
            anim_lb.RepeatBehavior = RepeatBehavior.Forever
            Dim col As New SolidColorBrush
            col.BeginAnimation(SolidColorBrush.ColorProperty, anim_lb)
            TextBlock1.Foreground = col
        End If

        'SLIDE EXTRAS

        'CHECK MULTITOUCH
        If Not Windows7.Multitouch.TouchHandler.DigitizerCapabilities.IsMultiTouchReady Then
        Else
            mpP = New Windows7.Multitouch.Manipulation.ManipulationProcessor(Windows7.Multitouch.Manipulation.ProcessorManipulations.ALL)
            Factory.EnableStylusEvents(Me)
            AddHandler mpP.ManipulationDelta, AddressOf ProcessManipulationDelta
            mpP.PivotRadius = 2
            mpP.MinimumScaleRotateRadius = 70
            If File.Exists(data_root + gui_dir + "multitouch.png") Then
                With ImgSlideMultiTouch
                    .Source = New BitmapImage(New Uri(data_root + gui_dir + "multitouch.png"))
                    .Width = .Source.Width
                    .Height = .Source.Height
                    .Opacity = 0.65
                End With
            End If
            multitouch_sw = False
        End If
        'TEMP                                                   --- !!!
        If File.Exists(data_root + gui_dir + "multitouch.png") Then
            With ImgSlideMultiTouch
                .Source = New BitmapImage(New Uri(data_root + gui_dir + "multitouch.png"))
                .Width = .Source.Width
                .Height = .Source.Height
                .Opacity = 0.65
            End With
        End If

        'EMAIL
        If File.Exists(data_root + gui_dir + "email.png") Then
            ImgSlideEmail.Source = New BitmapImage(New Uri(data_root + gui_dir + "email.png"))
            ImgSlideEmail.Width = ImgSlideEmail.Source.Width
            ImgSlideEmail.Height = ImgSlideEmail.Source.Height
            ImgSlideEmail.Opacity = 0.65
        End If

        'FACEBOOK
        If File.Exists(data_root + gui_dir + "facebook.png") Then
            ImgSlideFacebook.Source = New BitmapImage(New Uri(data_root + gui_dir + "facebook.png"))
            ImgSlideFacebook.Width = ImgSlideFacebook.Source.Width
            ImgSlideFacebook.Height = ImgSlideFacebook.Source.Height
            ImgSlideFacebook.Opacity = 0.65
        End If

        'NOTES
        If File.Exists(data_root + gui_dir + "note.png") Then
            ImgSlideNote.Source = New BitmapImage(New Uri(data_root + gui_dir + "note.png"))
            ImgSlideNote.Width = ImgSlideNote.Source.Width
            ImgSlideNote.Height = ImgSlideNote.Source.Height
            ImgSlideNote.Opacity = 0.65
        End If
    End Sub

    Dim cur_home_vdoslide As Integer = 0

    Private Sub MediaUriElement1_MediaEnded() '(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        MediaUriElement1.Source = Nothing
        GC.Collect()
        cur_home_vdoslide += 1
        Dim vdopath As String = data_root + "slides\" + CStr(cur_home_vdoslide) + ".avi"
        If File.Exists(vdopath) Then
            With MediaUriElement1
                .BeginInit()
                .Source = New Uri(vdopath)
                .EndInit()
                .Play()
            End With
        Else
            cur_home_vdoslide = 1
            vdopath = data_root + "slides\" + CStr(cur_home_vdoslide) + ".avi"
            With MediaUriElement1
                .BeginInit()
                .Source = New Uri(vdopath)
                .EndInit()
                .Play()
            End With
        End If
    End Sub

    Private Sub SlideHideNavigation(ByVal act As Boolean)
        Dim speed As Double = 0.25
        If act Then
            If BrdSlideDots.Visibility = Visibility.Visible Then
                'UI BTN HIDE ANIM
                BrdSlideDots.Visibility = Visibility.Hidden
                Dim btn_tr1 As New TranslateTransform
                ExitButton.RenderTransform = btn_tr1
                btn_tr1.BeginAnimation(TranslateTransform.YProperty, New DoubleAnimation(0, -ExitButton.ActualHeight, TimeSpan.FromSeconds(speed)))
                'SubslideButton.Visibility = Visibility.Hidden
                'Dim btn_tr2 As New TranslateTransform
                'help_dn.RenderTransform = btn_tr2
                'btn_tr2.BeginAnimation(TranslateTransform.YProperty, New DoubleAnimation(0, help_dn.ActualHeight, TimeSpan.FromSeconds(speed)))
                Dim btn_tr3 As New TranslateTransform
                PrevButton.RenderTransform = btn_tr3
                btn_tr3.BeginAnimation(TranslateTransform.XProperty, New DoubleAnimation(0, -PrevButton.ActualWidth, TimeSpan.FromSeconds(speed)))
                Dim btn_tr4 As New TranslateTransform
                NextButton.RenderTransform = btn_tr4
                btn_tr4.BeginAnimation(TranslateTransform.XProperty, New DoubleAnimation(0, NextButton.ActualWidth, TimeSpan.FromSeconds(speed)))
            End If
        Else
            'UI NAV BTN SHOW ANIM
            If BrdSlideDots.Visibility = Visibility.Hidden And Not multitouch_sw And Not note_sw Then
                BrdSlideDots.Visibility = Visibility.Visible
                Dim btn_tr1 As New TranslateTransform
                ExitButton.RenderTransform = btn_tr1
                btn_tr1.BeginAnimation(TranslateTransform.YProperty, New DoubleAnimation(-ExitButton.ActualHeight, 0, TimeSpan.FromSeconds(speed)))
                'SubslideButton.Visibility = Visibility.Visible
                'Dim btn_tr2 As New TranslateTransform
                'help_dn.RenderTransform = btn_tr2
                'btn_tr2.BeginAnimation(TranslateTransform.YProperty, New DoubleAnimation(help_dn.ActualHeight, 0, TimeSpan.FromSeconds(speed)))
                Dim btn_tr3 As New TranslateTransform
                PrevButton.RenderTransform = btn_tr3
                btn_tr3.BeginAnimation(TranslateTransform.XProperty, New DoubleAnimation(-PrevButton.ActualWidth, 0, TimeSpan.FromSeconds(speed)))
                Dim btn_tr4 As New TranslateTransform
                NextButton.RenderTransform = btn_tr4
                btn_tr4.BeginAnimation(TranslateTransform.XProperty, New DoubleAnimation(NextButton.ActualWidth, 0, TimeSpan.FromSeconds(speed)))
            End If
        End If
    End Sub

    'MULTITOUCH SWITCH
    Dim mt_trTranslate As New TranslateTransform
    Dim mt_trRotate As New RotateTransform
    Dim mt_trScale As New ScaleTransform
    Dim mt_sh_eff As New Effects.DropShadowEffect
    Private Sub ImgSlideMultiTouch_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        pic_snd.Play()
        multitouch_sw = Not multitouch_sw
        If multitouch_sw Then
            ShowHelp("help_multitouch", True, 1)
            ImgSlide1.Margin = New Thickness(0)
            ImgSlide2.Margin = New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)
            ImgSlide2.RenderTransform = Nothing
            ImgSlide1.RenderTransformOrigin = New Point(0.5, 0.5)
            Dim mt_transf_grp As New TransformGroup
            mt_sh_eff.ShadowDepth = 0
            ImgSlide1.Effect = mt_sh_eff
            mt_transf_grp.Children.Add(mt_trTranslate)
            mt_transf_grp.Children.Add(mt_trRotate)
            mt_transf_grp.Children.Add(mt_trScale)
            ImgSlide1.RenderTransform = mt_transf_grp
            Dim trS_anim As New DoubleAnimation(1, 0.95, TimeSpan.FromSeconds(0.25))
            Dim trA_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25))
            mt_trScale.BeginAnimation(ScaleTransform.ScaleXProperty, trS_anim)
            mt_trScale.BeginAnimation(ScaleTransform.ScaleYProperty, trS_anim)
            mt_trRotate.BeginAnimation(RotateTransform.AngleProperty, trA_anim)
            mt_trTranslate.X = 0
            mt_trTranslate.Y = 0
            ImgSlideMultiTouch.Opacity = 1
            SlideHideNavigation(True)
        Else
            ShowHelp("help_multitouch", False)
            Dim trTX_anim As New DoubleAnimation(mt_trTranslate.X, 0, TimeSpan.FromSeconds(0.5))
            Dim trTY_anim As New DoubleAnimation(mt_trTranslate.Y, 0, TimeSpan.FromSeconds(0.5))
            Dim trS_anim As New DoubleAnimation(mt_trScale.ScaleX, 1, TimeSpan.FromSeconds(0.5))
            Dim trA_anim As New DoubleAnimation(mt_trRotate.Angle, 0, TimeSpan.FromSeconds(0.5))
            mt_trTranslate.BeginAnimation(TranslateTransform.XProperty, trTX_anim)
            mt_trTranslate.BeginAnimation(TranslateTransform.YProperty, trTY_anim)
            mt_trScale.BeginAnimation(ScaleTransform.ScaleXProperty, trS_anim)
            mt_trScale.BeginAnimation(ScaleTransform.ScaleYProperty, trS_anim)
            mt_trRotate.BeginAnimation(RotateTransform.AngleProperty, trA_anim)
            ImgSlideMultiTouch.Opacity = 0.65
            ImgSlide1.Effect = Nothing
            SlideHideNavigation(False)
        End If
    End Sub
    'MULTITOUCH HANDLING
    Private Sub Window1_StylusDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.StylusDownEventArgs) Handles MyBase.StylusDown
        mpP.ProcessDown(CUInt(e.StylusDevice.Id), e.GetPosition(Me).ToDrawingPointF())
        If multitouch_sw Then
            ImgSlide1.Effect = Nothing
            mt_trTranslate.BeginAnimation(TranslateTransform.XProperty, Nothing)
            mt_trTranslate.BeginAnimation(TranslateTransform.YProperty, Nothing)
            mt_trScale.BeginAnimation(ScaleTransform.ScaleYProperty, Nothing)
            mt_trScale.BeginAnimation(ScaleTransform.ScaleXProperty, Nothing)
            mt_trRotate.BeginAnimation(RotateTransform.AngleProperty, Nothing)
        End If
    End Sub
    Private Sub Window1_StylusUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.StylusEventArgs) Handles MyBase.StylusUp
        mpP.ProcessUp(CUInt(e.StylusDevice.Id), e.GetPosition(Me).ToDrawingPointF())
        If multitouch_sw Or note_sw Then ImgSlide1.Effect = mt_sh_eff
    End Sub
    Private Sub Window1_StylusMove(ByVal sender As System.Object, ByVal e As System.Windows.Input.StylusEventArgs) Handles MyBase.StylusMove
        mpP.ProcessMove(CUInt(e.StylusDevice.Id), e.GetPosition(Me).ToDrawingPointF())
        If multitouch_sw Then ImgSlide1.Effect = Nothing
    End Sub
    'Private Sub ImgSlide1_Inertia(ByVal sender As System.Object, ByVal e As ManipulationInertiaStartingEventArgs) Handles ImgSlide1.ManipulationInertiaStarting
    '    e.TranslationBehavior.DesiredDeceleration = 10 * 96.0 / (1000.0 * 1000.0)
    '    e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / 1000.0 * 1000.0
    '    e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0)
    '    e.Handled = True
    'End Sub
    Private Sub ProcessManipulationDelta(ByVal sender As System.Object, ByVal e As ManipulationDeltaEventArgs)
        If multitouch_sw Then
            mt_trTranslate.X += e.TranslationDelta.Width * 2
            mt_trTranslate.Y += e.TranslationDelta.Height * 2
            mt_trRotate.Angle += e.RotationDelta * 180 / 3.14
            mt_trScale.ScaleX *= e.ScaleDelta
            mt_trScale.ScaleY *= e.ScaleDelta
        End If
    End Sub

    'ACTIVITY TIMER PROC
    Dim prev_min As String
    Dim activity_prev_x As Double = 0
    Dim activity_prev_y As Double = 0

    Private Sub activity_timer_tick(ByVal sender As Object, ByVal e As EventArgs)
        'DATE TIME UPD
        TextBlockTime.Text = DateTime.Now.ToShortTimeString()
        LabelDate.Content = DateTime.Now.ToShortDateString()
        If DateTime.Now.Minute.ToString <> prev_min Then
            Dim anim_time As New DoubleAnimation(0, -5, TimeSpan.FromSeconds(0.25))
            anim_time.AutoReverse = True
            Dim time_eff As New TextEffect
            time_eff.PositionStart = 3
            time_eff.PositionCount = 2
            Dim time_tr As New TranslateTransform
            time_eff.Transform = time_tr
            time_tr.BeginAnimation(TranslateTransform.YProperty, anim_time)
            TextBlockTime.TextEffects.Add(time_eff)
        End If
        prev_min = DateTime.Now.Minute.ToString

        'ACTIVITY CHECK
        If use_home_timer And Not home_screen Then
            home_timer.Start()
            If Mouse.GetPosition(Window1).X <> activity_prev_x Then
                home_timer.Stop()
            End If
        End If
        activity_prev_x = Mouse.GetPosition(Window1).X
        activity_prev_y = Mouse.GetPosition(Window1).Y
    End Sub

    'HOME TIMER PROC
    Private Sub home_timer_tick(ByVal sender As Object, ByVal e As EventArgs)
        If BorderSlide.Visibility = Visibility.Visible Then ExitSlides_Action()
        ImgMainMenuLogo_MouseDown(Nothing, Nothing)
        home_timer.Stop()
    End Sub

    'Private Sub MediaElementBg_MediaEnded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MediaElementBg.MediaEnded
    '    MediaElementBg.Position = TimeSpan.FromSeconds(0)
    '    MediaElementBg.Play()
    'End Sub

    'Private Sub MediaElementHome_MediaEnded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MediaElementHome.MediaEnded
    '    MediaElementHome.Position = TimeSpan.FromSeconds(0)
    '    MediaElementHome.Play()
    'End Sub

    Private Sub MediaElementTrack_MediaEnded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MediaElementTrack.MediaEnded
        MediaElementTrack.MediaPosition = 0
        MediaElementTrack.Play()
    End Sub

    Private Sub BgEffect(ByRef act As Integer, ByRef speed As Double)

        If bgv_mode Then
            Dim eleasy As New PowerEase
            eleasy.EasingMode = EasingMode.EaseInOut
            If act = 1 Then
                Dim Y_anim As New DoubleAnimation(bgv_tr.Y, -600 * selected_mainmenuitm, TimeSpan.FromSeconds(1))
                Y_anim.EasingFunction = eleasy
                bgv_tr.BeginAnimation(TranslateTransform.YProperty, Y_anim)
            End If
            If act = 0 Then
                Dim Y_anim As New DoubleAnimation(bgv_tr.Y, 0, TimeSpan.FromSeconds(1))
                Y_anim.EasingFunction = eleasy
                bgv_tr.BeginAnimation(TranslateTransform.YProperty, Y_anim)
            End If
        End If

        If Not bgv_mode Then

            SlideShowBg.Effect = Nothing

            If SlideShowBg.Items.Count > 2 Then
                For i = SlideShowBg.Items.Count - 1 To 2
                    SlideShowBg.Items.RemoveAt(i)
                Next
            End If
            SlideShowBg.AutoAdvance = False

            If act = 1 And Not item_view Then
                If File.Exists(data_root + "" + CStr(selected_mainmenuitm) + "sub\bg.jpg") Then

                    Dim ImageSlide As New Image With {.Source = preload_img_bg(selected_mainmenuitm), .Stretch = Stretch.Fill}
                    Dim SlideShowItem As New Transitionals.Controls.SlideshowItem With {.Content = ImageSlide}
                    If SlideShowBg.Items.Count = 2 Then SlideShowBg.Items(0) = SlideShowItem
                    If SlideShowBg.Items.Count = 1 Then SlideShowBg.Items.Add(SlideShowItem)

                    bgeff_timer.Start()
                End If
            End If

            If act = 1 And item_view Then
                If File.Exists(data_root + "" + CStr(selected_mainmenuitm) + "sub\bg.jpg") Then

                    Dim ImageSlide As New Image With {.Source = preload_img_bg(selected_mainmenuitm), .Stretch = Stretch.Fill}
                    Dim SlideShowItem As New Transitionals.Controls.SlideshowItem With {.Content = ImageSlide}
                    If SlideShowBg.Items.Count = 2 Then SlideShowBg.Items(0) = SlideShowItem
                    If SlideShowBg.Items.Count = 1 Then SlideShowBg.Items.Add(SlideShowItem)

                    bgeff_timer.Start()
                End If
            End If

            'HOME SCREEN
            If act = 0 Then
                If File.Exists(data_root + "bg.jpg") Then

                    Dim ImageSlide As New Image With {.Source = preload_img_bg(0), .Stretch = Stretch.Fill}
                    Dim SlideShowItem As New Transitionals.Controls.SlideshowItem With {.Content = ImageSlide}
                    If SlideShowBg.Items.Count = 2 Then SlideShowBg.Items(0) = SlideShowItem
                    If SlideShowBg.Items.Count = 1 Then SlideShowBg.Items.Add(SlideShowItem)
                    If SlideShowBg.Items.Count = 0 Then SlideShowBg.Items.Add(SlideShowItem)
                    SlideShowBg.AutoAdvance = False 'True for full slides bg

                    bgeff_timer.Start()
                End If
            End If

        End If
    End Sub

    'BG ANIM WITH DELAY
    Private Sub bgeff_timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        SlideShowBg.TransitionNext()
        bgeff_timer.Stop()
    End Sub
    'FG ANIM WITH DELAY
    Private Sub fgeff_timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        SlideShowFG.TransitionNext()
        fgeff_timer.Stop()
    End Sub


    Private Sub FgEffect(ByRef act As Integer, ByRef speed As Double)

        'SlideShowFG.Effect = Nothing

        If SlideShowFG.Items.Count > 2 Then
            For i = SlideShowFG.Items.Count - 1 To 2
                SlideShowFG.Items.RemoveAt(i)
            Next
        End If
        SlideShowFG.AutoAdvance = False

        If act = 1 And Not item_view Then
            If File.Exists(data_root + "" + CStr(selected_mainmenuitm) + "sub\fg.png") Then
                Dim ImageSlide As New Image With {.Source = preload_img_fg(selected_mainmenuitm), .Stretch = Stretch.Fill}
                Dim SlideShowItem As New Transitionals.Controls.SlideshowItem With {.Content = ImageSlide}
                If SlideShowFG.Items.Count = 2 Then SlideShowFG.Items(0) = SlideShowItem
                If SlideShowFG.Items.Count = 1 Then SlideShowFG.Items.Add(SlideShowItem)

                fgeff_timer.Start()
                'SlideShowFG.TransitionNext()
            End If
        End If

        If act = 1 And item_view Then
            If File.Exists(data_root + "" + CStr(selected_mainmenuitm) + "sub\fg.png") Then
                Dim ImageSlide As New Image With {.Source = preload_img_fg(selected_mainmenuitm), .Stretch = Stretch.Fill}
                Dim SlideShowItem As New Transitionals.Controls.SlideshowItem With {.Content = ImageSlide}
                If SlideShowFG.Items.Count = 2 Then SlideShowFG.Items(0) = SlideShowItem
                If SlideShowFG.Items.Count = 1 Then SlideShowFG.Items.Add(SlideShowItem)

                fgeff_timer.Start()
                'SlideShowFG.TransitionNext()
            End If
        End If

        'HOME SCREEN
        If act = 0 Then
            If File.Exists(data_root + "fg.png") Then
                Dim ImageSlide As New Image With {.Source = preload_img_fg(0), .Stretch = Stretch.Fill}
                Dim SlideShowItem As New Transitionals.Controls.SlideshowItem With {.Content = ImageSlide}
                If SlideShowFG.Items.Count = 2 Then SlideShowFG.Items(0) = SlideShowItem
                If SlideShowFG.Items.Count = 1 Then SlideShowFG.Items.Add(SlideShowItem)
                If SlideShowFG.Items.Count = 0 Then SlideShowFG.Items.Add(SlideShowItem)
                SlideShowFG.AutoAdvance = False

                fgeff_timer.Start()
                'SlideShowFG.TransitionNext()
            End If
        End If

    End Sub

    Private Sub sstimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        'HOME SLIDE DOTS
        StackPanelHomeSlideDots.Children.Clear()
        For i = 1 To pic_count
            Dim dot_img As New Image
            dot_img.Width = 8
            dot_img.Height = 8
            If File.Exists(pic_names(i)) Then
                If File.Exists(data_root + gui_dir + "dot.png") Then dot_img.Source = New BitmapImage(New Uri(data_root + gui_dir + "dot.png"))
                If i = ind Then If File.Exists(data_root + gui_dir + "dot_sel.png") Then dot_img.Source = New BitmapImage(New Uri(data_root + gui_dir + "dot_sel.png"))
                If Not IsNothing(dot_img.Source) Then StackPanelHomeSlideDots.Children.Add(dot_img)
            End If
        Next i

        SlideShow.TransitionNext()
        ind += 1
        If ind = pic_count + 1 Then ind = 1
        tick += 1
        If tick = 2 Then tick = 1
    End Sub

    Dim fl As String

    Private Sub anim_timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        BorderSlide.Visibility = Visibility.Hidden
        anim_timer.Stop()
    End Sub

    'Dim cmenu As Boolean = False
    'Dim cmenu_selected_itm As Integer
    ''CMENU ITEM CLICK
    'Private Sub CmenuImageItemHandler(ByVal sender As Object, ByVal e As RoutedEventArgs)
    '    cmenu = True
    '    Dim feSource As Image = TryCast(e.Source, Image)
    '    For i = 1 To items_limit
    '        Dim imgitm As Image = WrapPanelCmenuItems.FindName("CmenuImageItem" + CStr(i))
    '        If Not IsNothing(imgitm) Then
    '            If feSource.Name = imgitm.Name Then
    '                cmenu_selected_itm = i
    '                'CHECK FOLDER FOR EMPTY
    '                If Not Directory.Exists(data_root + "" + CStr(cmenu_selected_itm) + "sub\") Then Exit Sub
    '                'COMMON HANDLER
    '                ImageItemHandler(sender, e)
    '                BorderCmenu.Visibility = Visibility.Hidden
    '                'Dim anim As New DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5))
    '                'BorderCmenu.BeginAnimation(Border.OpacityProperty, anim)
    '                Dim transf As New TranslateTransform With {.X = -GridMainMenu.ActualWidth}
    '                Dim anim_transf As New DoubleAnimation(-GridMainMenu.ActualWidth, 0, TimeSpan.FromSeconds(1))
    '                anim_transf.EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseInOut}
    '                anim_transf.BeginTime = TimeSpan.FromSeconds(0.25)
    '                transf.BeginAnimation(TranslateTransform.XProperty, anim_transf)
    '                GridMainMenu.RenderTransform = transf
    '                Exit Sub
    '            End If
    '        End If
    '    Next i
    'End Sub

    'PRESSED CMENU BTN SEL STATE
    'Private Sub WrapPanelCmenuItems_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles WrapPanelCmenuItems.MouseDown
    '    Dim feSource As Image = TryCast(e.Source, Image)
    '    For i = 1 To items_limit
    '        Dim imgitm As Image = WrapPanelCmenuItems.FindName("CmenuImageItem" + CStr(i))
    '        If Not IsNothing(imgitm) Then
    '            If feSource.Name = imgitm.Name Then
    '                'SET ITM BTN SEL IMG
    '                If File.Exists(data_root + "cmenu/" + CStr(i) + "btn_sel.png") Then
    '                    imgitm.Source = New BitmapImage(New Uri(data_root + "cmenu/" + CStr(i) + "btn_sel.png"))
    '                End If
    '            End If
    '        End If
    '    Next i
    'End Sub
    'UN-PRESSED CMENU BTN UNSEL STATE
    'Private Sub WrapPanelCmenuItems_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles WrapPanelCmenuItems.MouseUp
    '    Dim feSource As Image = TryCast(e.Source, Image)
    '    For i = 1 To items_limit
    '        Dim imgitm As Image = WrapPanelCmenuItems.FindName("CmenuImageItem" + CStr(i))
    '        If Not IsNothing(imgitm) Then
    '            If feSource.Name = imgitm.Name Then
    '                'SET ITM BTN UNSEL IMG
    '                If File.Exists(data_root + "cmenu/" + CStr(i) + "btn.png") Then
    '                    imgitm.Source = New BitmapImage(New Uri(data_root + "cmenu/" + CStr(i) + "btn.png"))
    '                End If
    '            End If
    '        End If
    '    Next i
    'End Sub


    Dim oh_x_pos, oh_y_pos, oh_x_point, oh_y_point, oh_tot_dx, oh_tot_dy As Double
    Dim oh_scrolled As Boolean = False
    Dim zoom As New ScaleTransform

    'OVER-HEIGHT SCROLL INIT
    Private Sub ScrollViewerIcons_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ScrollViewerIcons.PreviewMouseDown
        If IsNothing(e.StylusDevice) Then
            '2DO: ENABLE 4 Mouse & DISABLE 4 Touch !
            oh_x_pos = e.GetPosition(Me).X
            oh_y_pos = e.GetPosition(Me).Y
            oh_x_point = oh_x_pos
            oh_y_point = oh_y_pos

            oh_tot_dx = 0
            oh_tot_dy = 0
            oh_scrolled = False


            'TOUCH ZOOM IN
            'If touch_zoom_eff Then
            '    ImgBS1.RenderTransform = zoom
            '    zoom.ScaleX = 1
            '    zoom.ScaleY = 1
            '    zoom.CenterX = GridBg.ActualWidth / 2
            '    zoom.CenterY = GridBg.ActualHeight / 2
            '    Dim z_anim As New DoubleAnimation(1, 1.05, TimeSpan.FromSeconds(0.5))
            '    zoom.BeginAnimation(ScaleTransform.ScaleXProperty, z_anim)
            '    zoom.BeginAnimation(ScaleTransform.ScaleYProperty, z_anim)
            'End If

            'Dim feSource As Image = TryCast(e.Source, Image)
            'If Not IsNothing(feSource) Then
            '    Dim zoom2 As New ScaleTransform
            '    feSource.RenderTransform = zoom2
            '    zoom2.ScaleX = 1
            '    zoom2.ScaleY = 1
            '    zoom2.CenterX = feSource.ActualWidth / 2
            '    zoom2.CenterY = feSource.ActualHeight / 2
            '    Dim z_anim2 As New DoubleAnimation(1, 0.98, TimeSpan.FromSeconds(0.25))
            '    zoom2.BeginAnimation(ScaleTransform.ScaleXProperty, z_anim2)
            '    zoom2.BeginAnimation(ScaleTransform.ScaleYProperty, z_anim2)
            'End If
        End If
    End Sub

    Dim source_changed As Boolean = False
    Dim preSource As Image = Nothing

    Private Sub ScrollViewerIcons_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles ScrollViewerIcons.MouseMove
        If IsNothing(e.StylusDevice) Then
            Dim feSource As Image = TryCast(e.Source, Image)
            If IsNothing(feSource) Then source_changed = True
            If Not IsNothing(feSource) And source_changed Then
                'HAVE TO FIX THAT HOVER EFF
                'Dim zoom2 As New ScaleTransform
                'feSource.RenderTransform = zoom2
                'zoom2.ScaleX = 1
                'zoom2.ScaleY = 1
                'zoom2.CenterX = feSource.ActualWidth / 2
                'zoom2.CenterY = feSource.ActualHeight / 2
                'Dim z_anim2 As New DoubleAnimation(1, 0.98, TimeSpan.FromSeconds(0.25))
                'zoom2.BeginAnimation(ScaleTransform.ScaleXProperty, z_anim2)
                'zoom2.BeginAnimation(ScaleTransform.ScaleYProperty, z_anim2)
                source_changed = False
                preSource = feSource
            End If
            If Not IsNothing(preSource) And source_changed Then preSource.RenderTransform = Nothing
            'If Not IsNothing(feSource) And Not source_changed Then feSource.RenderTransform = Nothing

            If e.LeftButton <> MouseButton.Left Then
                oh_tot_dx = e.GetPosition(Me).X - oh_x_pos
                oh_tot_dy = e.GetPosition(Me).Y - oh_y_pos

                Dim dx As Double = e.GetPosition(Me).X - oh_x_point
                Dim dy As Double = e.GetPosition(Me).Y - oh_y_point
                oh_x_point = e.GetPosition(Me).X
                oh_y_point = e.GetPosition(Me).Y
                ScrollViewerIcons.ScrollToHorizontalOffset(ScrollViewerIcons.HorizontalOffset - dx)
                ScrollViewerIcons.ScrollToVerticalOffset(ScrollViewerIcons.VerticalOffset - dy)

                If Math.Abs(oh_tot_dx) > 10 Or Math.Abs(oh_tot_dy) > 10 Then oh_scrolled = True

                'zoom.CenterX = ImgBS1.ActualWidth / 2 + oh_tot_dx
            End If
        End If
    End Sub

    'TOUCH ZOOM OUT
    Private Sub ScrollViewerIcons_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ScrollViewerIcons.MouseUp
        If IsNothing(e.StylusDevice) Then
            If touch_zoom_eff Then
                zoom.ScaleX = 1.05
                zoom.ScaleY = 1.05
                Dim z_anim As New DoubleAnimation(1.05, 1, TimeSpan.FromSeconds(0.5))
                zoom.BeginAnimation(ScaleTransform.ScaleXProperty, z_anim)
                zoom.BeginAnimation(ScaleTransform.ScaleYProperty, z_anim)
            End If

            'Dim feSource As Image = TryCast(e.Source, Image)
            'If Not IsNothing(feSource) Then feSource.RenderTransform = Nothing
        End If
    End Sub

    Dim gotonextslideonhomescreen As Boolean = False

    'LOGO CLICK --- HOME SCREEN
    Private Sub GridMainMenu_MouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs) Handles GridMainMenu.MouseDown
        Dim mouse_x As Integer = Mouse.GetPosition(GridMainMenuItems).X
        Dim mouse_y As Integer = Mouse.GetPosition(GridMainMenuItems).Y
        For i = 0 To VisualTreeHelper.GetChildrenCount(GridMainMenu) - 1
            Dim feSource0 As Image = TryCast(VisualTreeHelper.GetChild(GridMainMenu, i), Image)
            If Not IsNothing(feSource0) Then
                Dim bmp As New BitmapImage()
                Dim pix(4) As Byte
                bmp = feSource0.Source
                Dim cbmp As New CroppedBitmap(bmp, New Int32Rect(mouse_x, mouse_y, 1, 1))
                Try
                    cbmp.CopyPixels(pix, 4, 0)
                    If pix(3) <> 0 Then
                        ImgMainMenuLogo_MouseDown(feSource0, e) '-----> to logo tap action
                        Exit Sub
                    Else
                        'go to next slide, GGG
                        gotonextslideonhomescreen = True
                        Exit Sub
                        'If home_screen Then
                        '    HomeMuSli.MainTimer_Tick()
                        '    Exit Sub
                        'End If
                    End If
                Catch ex As Exception
                End Try
            End If
        Next
    End Sub

    Private Sub ImgMainMenuLogo_MouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)

        ImgLogo1_MouseUp(sender, e)

        'MP3 TRACK VOL UP
        If MediaElementTrack.IsPlaying Then
            MediaElementTrack.BeginAnimation(WPFMediaKit.DirectShow.Controls.MediaUriElement.VolumeProperty,
                                             New DoubleAnimation(0.7, 0.8, TimeSpan.FromSeconds(1)))
        End If

        'SEND STATUS TO WINDOW2
        If Application.Current.Windows.Count <> 1 Then
            Dim wnd2 As Window2 = Application.Current.Windows(0)
            wnd2.StateControl = 0
        End If

        TextBlock1.Visibility = Visibility.Hidden
        If File.Exists(data_root + "header.txt") Then TextBlock1.Visibility = Visibility.Visible
        If show_date_time Then
            TextBlockTime.Visibility = Visibility.Visible
            LabelDate.Visibility = Visibility.Visible
        End If

        'CLR INFORM GRID ICONS
        If Not home_screen Then
            For j = 0 To GridIcons.Children.Count - 1
                Dim iconsubitm As Image = GridIcons.Children(j)
                iconsubitm.BeginAnimation(Image.OpacityProperty, New DoubleAnimation(1, 0, TimeSpan.FromSeconds(j / (j * Rnd() + 5))) _
                                                           With {.BeginTime = TimeSpan.FromSeconds(0 + 0.1 * j)})
                GridIcons.IsHitTestVisible = False
            Next j
        End If

        home_screen = True
        If Not IsNothing(MediaUriElement1.Source) Then MediaUriElement1.Play()

        'ImageLandingBtn1.Visibility = Windows.Visibility.Visible

        BgMuSli.CurrentSlide = -1
        BgMuSli.SetPlay(True)

        FgMuSli.CurrentSlide = -1
        FgMuSli.SetPlay(True)

        'BgEffect(0, 1)
        'FgEffect(0, 1)

        BorderIcons.IsEnabled = False
        Dim anim_bi As New DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5))
        BorderIcons.BeginAnimation(Border.OpacityProperty, anim_bi)

        'CLEAR ICONS
        For j = 1 To items_limit
            Dim iconsubitm As Image = WrapPanelIcons.FindName("IconSubItem" + CStr(j))
            iconsubitm.Source = Nothing
            Dim iconsubitmbrd As Border = WrapPanelIcons.FindName("IconSubItemBorder" + CStr(j))
            iconsubitmbrd.Visibility = Visibility.Hidden
        Next j

        'ZOOM to 50x50
        Dim anim_pw As New DoubleAnimation(BorderIcons.ActualWidth, 50, TimeSpan.FromSeconds(0.5))
        BorderIcons.BeginAnimation(Border.WidthProperty, anim_pw)
        Dim anim_ph As New DoubleAnimation(BorderIcons.ActualHeight, 50, TimeSpan.FromSeconds(0.5))
        BorderIcons.BeginAnimation(Border.HeightProperty, anim_ph)

        For i = 1 To items_limit
            Dim imgitm As Image = GridMainMenuItems.FindName("ImageItem" + CStr(i))
            If File.Exists(data_root + "" + CStr(i) + "btn.png") Then
                imgitm.Source = New BitmapImage(New Uri(data_root + "" + CStr(i) + "btn.png"))
            End If
        Next i

        'SIDE SLIDE ANIM
        Dim transf2 = New TranslateTransform
        EventAnimatorSub(BorderHomeSlides, slides_hide_direction, False, slides_hide_alpha)
        r_side_panel = True

        ' LOGO ANIM
        If item_view Then
            fl = "logo.png"
            ImgMainMenuLogo.Source = New BitmapImage(New Uri(data_root + fl))
            'anim_timer.Start()
            item_view = False
            'ImgLogo1.Opacity = 1
            'Dim anim0 As New DoubleAnimation(0.75, 1, TimeSpan.FromSeconds(1))
            'ImgLogo1.BeginAnimation(Image.OpacityProperty, anim0)
        End If

        'If Directory.Exists(data_root + "cmenu") Then
        '    GridMainMenu.Visibility = Visibility.Hidden
        '    BorderCmenu.Visibility = Visibility.Visible
        '    Dim anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5))
        '    BorderCmenu.BeginAnimation(Border.OpacityProperty, anim)
        'End If

        'XML HOME - SHOW BLOCKS ON HOME SCREEN
        If XmlHome Then
            For ii = 0 To GridXmlhome.Children.Count - 1
                Dim grd As Grid = TryCast(GridXmlhome.Children(ii), Grid)
                grd.BeginAnimation(OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5)) With {.BeginTime = TimeSpan.FromSeconds(ii * 0.1)})
            Next
            'GridXmlhome.Visibility = Visibility.Visible
            XmlHomeHidden = False
        End If

        'DATA SELECTOR PANEL ANIMATION --- SHOW
        Dim dataselector_trans As New TranslateTransform With {.X = StackPanelSetup.ActualWidth * 2}
        StackPanelSetup.RenderTransform = dataselector_trans
        Dim dataselector_anim As New DoubleAnimation(dataselector_trans.X, 0, TimeSpan.FromSeconds(0.5)) _
            With {.BeginTime = TimeSpan.FromSeconds(1.5), .EasingFunction = New CubicEase}
        dataselector_trans.BeginAnimation(TranslateTransform.XProperty, dataselector_anim)
        dataselector_hidden = False

        'LANDING BTN SHOW
        Dim landbtn_trans As New TranslateTransform With {.X = ImageLandingBtn1.ActualWidth * 2}
        ImageLandingBtn1.RenderTransform = landbtn_trans
        Dim landbtn_anims As New DoubleAnimation(landbtn_trans.X, 0, TimeSpan.FromSeconds(0.5)) _
            With {.BeginTime = TimeSpan.FromSeconds(1.75), .EasingFunction = New CubicEase}
        landbtn_trans.BeginAnimation(TranslateTransform.XProperty, landbtn_anims)

        item_snd.Play()
    End Sub

    'MAIN MENU ITEM CLICK
    Dim pre_itm As Integer

    Private Sub ImageItemHandler(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim mouse_x As Integer = Mouse.GetPosition(GridMainMenuItems).X
        Dim mouse_y As Integer = Mouse.GetPosition(GridMainMenuItems).Y
        For i = 0 To VisualTreeHelper.GetChildrenCount(GridMainMenuItems) - 1
            Dim feSource0 As Image = TryCast(VisualTreeHelper.GetChild(GridMainMenuItems, i), Image)

            If Not IsNothing(feSource0) Then
                Dim bmp As New BitmapImage()
                Dim pix(4) As Byte
                bmp = feSource0.Source
                Dim cbmp As New CroppedBitmap(bmp, New Int32Rect(mouse_x, mouse_y, 1, 1))
                Try
                    cbmp.CopyPixels(pix, 4, 0)
                    If pix(3) <> 0 Then
                        ZImageItemHandler(feSource0, e) '----> to menu item tap action
                    End If
                Catch ex As Exception
                End Try
            End If
        Next

        If gotonextslideonhomescreen Then
            If home_screen Then HomeMuSli.MainTimer_Tick()
            gotonextslideonhomescreen = False
        End If

    End Sub

    Dim bypass_touchselection As Boolean = False
    Dim goto_itemindex As Integer = -1

    Private Sub ZImageItemHandler(ByVal sender As Object, ByVal e As RoutedEventArgs)
        exit_click_loc = 0
        home_screen = False
        TextBlock1.Visibility = Visibility.Hidden
        TextBlockTime.Visibility = Visibility.Hidden
        LabelDate.Visibility = Visibility.Hidden

        'ImageLandingBtn1.Visibility = Windows.Visibility.Hidden
        'LANDING BTN HIDE
        Dim landbtn_trans As New TranslateTransform With {.X = 0}
        ImageLandingBtn1.RenderTransform = landbtn_trans
        Dim landbtn_anims As New DoubleAnimation(landbtn_trans.X, ImageLandingBtn1.ActualWidth * 2, TimeSpan.FromSeconds(0.5)) _
            With {.BeginTime = TimeSpan.FromSeconds(0.25), .EasingFunction = New CubicEase}
        landbtn_trans.BeginAnimation(TranslateTransform.XProperty, landbtn_anims)

        Dim feSource As New Image
        If Not IsNothing(sender) Then
            feSource = sender 'TryCast(e.Source, Image)
        End If

        For i = 1 To GridMainMenuItems.Children.Count
            Dim imgitm As Image = GridMainMenuItems.FindName("ImageItem" + CStr(i))

            'SET ITMBTN UNSEL IMG
            If File.Exists(data_root + "" + CStr(i) + "btn.png") Then
                imgitm.Source = New BitmapImage(New Uri(data_root + "" + CStr(i) + "btn.png"))
            End If

            If Not IsNothing(imgitm) Then
                imgitm.Opacity = 1

                'skip touch selection for keyboard actions
                If i = goto_itemindex Then bypass_touchselection = True

                If feSource.Name = imgitm.Name Or bypass_touchselection Then

                    bypass_touchselection = False
                    goto_itemindex = -1
                    selected_mainmenuitm = i

                    If Application.Current.Windows.Count <> 1 Then
                        Dim wnd2 As Window2 = Application.Current.Windows(0)
                        wnd2.StateControl = CStr(selected_mainmenuitm)
                    End If

                    'SET ITM BTN SEL IMG
                    If File.Exists(data_root + "" + CStr(selected_mainmenuitm) + "btn_sel.png") Then
                        imgitm.Source = New BitmapImage(New Uri(data_root + "" + CStr(selected_mainmenuitm) + "btn_sel.png"))
                    End If

                    'SCROLL TO INIT
                    ScrollViewerIcons.ScrollToTop()
                    ScrollViewerIcons.ScrollToHome()

                    'CHECK FOLDER FOR EMPTY
                    If Not Directory.Exists(data_root + "" + CStr(selected_mainmenuitm) + "sub\") Then
                        Exit Sub
                    End If

                    BgMuSli.CurrentSlide = selected_mainmenuitm - 1
                    If File.Exists(data_root + "" + CStr(selected_mainmenuitm) + "sub\" + "bg.jpg") Then BgMuSli.SetPlay(True)

                    FgMuSli.CurrentSlide = selected_mainmenuitm - 1
                    FgMuSli.SetPlay(True)

                    Dim anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(1))

                    'LOGO IMG CHANGE
                    If Not item_view Then
                        Dim transf0 = New TranslateTransform
                        fl = "logo_sel.png"
                        If File.Exists(data_root + fl) Then ImgMainMenuLogo.Source = New BitmapImage(New Uri(data_root + fl))
                        'Dim anim0 As New DoubleAnimation(1, 0.75, TimeSpan.FromSeconds(1))
                        'ImgLogo1.BeginAnimation(Image.OpacityProperty, anim0)
                        item_view = True
                    End If

                    'XML HOME - HIDE BLOCKS
                    If XmlHome Then
                        If item_view And Not XmlHomeHidden Then
                            For ii = 0 To GridXmlhome.Children.Count - 1
                                Dim grd As Grid = TryCast(GridXmlhome.Children(ii), Grid)
                                grd.BeginAnimation(OpacityProperty, New DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5)) With {.BeginTime = TimeSpan.FromSeconds(ii * 0.1)})
                            Next
                            XmlHomeHidden = True
                        End If
                        'If item_view Then GridXmlhome.Visibility = Visibility.Hidden
                    End If

                    'DATA SELECTOR PANEL ANIMATION --- HIDE
                    If Not dataselector_hidden Then
                        Dim dataselector_trans As New TranslateTransform
                        StackPanelSetup.RenderTransform = dataselector_trans
                        Dim dataselector_anim As New DoubleAnimation(0, StackPanelSetup.ActualWidth * 2, TimeSpan.FromSeconds(0.5)) _
                            With {.BeginTime = TimeSpan.FromSeconds(1), .EasingFunction = New CubicEase}
                        dataselector_trans.BeginAnimation(TranslateTransform.XProperty, dataselector_anim)
                        dataselector_hidden = True
                    End If

                    'MP3 TRACK VOL DN
                    If MediaElementTrack.IsPlaying Then
                        MediaElementTrack.BeginAnimation(WPFMediaKit.DirectShow.Controls.MediaUriElement.VolumeProperty,
                                                         New DoubleAnimation(0.8, 0.7, TimeSpan.FromSeconds(1)))
                    End If

                    'HOME VIDEO PAUSE
                    'If Not IsNothing(MediaElementHome.Source) Then MediaElementHome.Stop()
                    If Not IsNothing(MediaUriElement1.Source) Then MediaUriElement1.Stop()

                    'INIT PANELS SET
                    BorderIcons.Visibility = Visibility.Hidden
                    ScrollViewerIcons.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
                    ScrollViewerIcons.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled
                    ScrollViewerIcons.MaxHeight = GridBg.ActualHeight

                    'USE ICONS MODE
                    level = 1
                    BorderIcons.IsEnabled = True
                    BorderIcons.BeginAnimation(Border.OpacityProperty, Nothing)
                    BorderIcons.Opacity = 1

                    'ZOOM ANIM INIT
                    BorderIcons.Width = Double.NaN
                    BorderIcons.BeginAnimation(WrapPanel.WidthProperty, Nothing)
                    BorderIcons.Height = Double.NaN
                    BorderIcons.BeginAnimation(WrapPanel.HeightProperty, Nothing)
                    Dim panel_width1 As Double = BorderIcons.ActualWidth
                    Dim panel_height1 As Double = BorderIcons.ActualHeight

                    'HIDE GO-BACK ICON
                    Dim backiconitm As Image = WrapPanelIcons.FindName("BackIcon")
                    Dim backiconitmborder As Border = WrapPanelIcons.FindName("BackIconBorder")
                    backiconitm.Source = Nothing
                    With backiconitmborder
                        .Visibility = Visibility.Hidden
                        .Opacity = 1
                        .Width = 0
                        .Height = 0
                        .Margin = New Thickness(0)
                    End With

                    'SHOW ICONS SUBITEMS
                    cur_img(level) = 0
                    img_count(level) = 0
                    Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"
                    Dim btn_level_url(4) As String

                    'CLR INFORM GRID ICONS
                    GridIcons.Children.Clear()
                    GridIcons.IsHitTestVisible = True

                    'GET ITEMS Q-TY
                    Dim items_qty As Integer = 0
                    For j = 1 To items_limit
                        Dim btn_sufx As String = "btn"
                        Dim btn_ext As String = ".jpg"
                        If File.Exists(base_url + CStr(j) + btn_sufx + ".png") Then btn_ext = ".png"
                        btn_level_url(1) = base_url + CStr(j) + btn_sufx + btn_ext
                        If File.Exists(btn_level_url(level)) Then items_qty += 1
                    Next

                    For j = 1 To items_limit
                        Dim iconsubitm As Image = WrapPanelIcons.FindName("IconSubItem" + CStr(j))
                        Dim iconsubitmbrd As Border = WrapPanelIcons.FindName("IconSubItemBorder" + CStr(j))
                        With iconsubitmbrd
                            .Visibility = Visibility.Hidden
                            .Width = 0
                            .Height = 0
                            .Margin = New Thickness(0) 'icon_margin
                        End With

                        Dim btn_sufx As String = "btn"
                        Dim btn_ext As String = ".jpg"

                        If File.Exists(base_url + CStr(j) + btn_sufx + ".png") Then btn_ext = ".png"

                        btn_level_url(1) = base_url + CStr(j) + btn_sufx + btn_ext
                        btn_level_url(2) = base_url + CStr(cur_img(1)) + "\" + CStr(j) + btn_sufx + btn_ext
                        btn_level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(j) + btn_sufx + btn_ext

                        If File.Exists(btn_level_url(level)) Then

                            'COMMON STACK ICONS
                            If btn_ext = ".jpg" Then
                                BorderIcons.Visibility = Visibility.Visible
                                iconsubitm.Source = New BitmapImage(New Uri(btn_level_url(level)))
                                With iconsubitmbrd
                                    .Visibility = Visibility.Visible
                                    .Opacity = 1
                                    .Width = Double.NaN
                                    .Height = Double.NaN
                                    .Margin = icon_margin
                                End With
                                iconsubitm.Width = iconsubitm.Source.Width
                                iconsubitm.Height = iconsubitm.Source.Height
                            End If

                            Dim multi_icon As Boolean = False
                            If File.Exists(base_url + CStr(j) + btn_sufx + "-1.png") Then multi_icon = True

                            'FULL SCREEN INFORM ICONS
                            If btn_ext = ".png" Then
                                Dim iconpic As New Image With {.Opacity = 0}
                                iconpic.Source = New BitmapImage(New Uri(btn_level_url(level)))
                                iconpic.BeginAnimation(Image.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(j / (j * Rnd() + items_qty))) _
                                                       With {.BeginTime = TimeSpan.FromSeconds(0.25 + 0.1 * j)})
                                If Not multi_icon Then GridIcons.Children.Add(iconpic)

                                'MULTI-IMAGE ICONS MODE
                                If multi_icon Then
                                    Dim multiiconpic As New MultiSlider("A", 10)
                                    With multiiconpic
                                        .ReloadSource("SF", base_url, CStr(j) + btn_sufx + "-", "", "", btn_ext, 0.5, 0.5, 0.1)
                                        .UseAlpha = False
                                        .ManualPlayback = False
                                    End With
                                    Dim multi_grid As New Grid
                                    multi_grid.Children.Add(iconpic)
                                    multi_grid.Children.Add(multiiconpic)
                                    GridIcons.Children.Add(multi_grid)
                                End If
                            End If

                            Dim file_ext As String = ".jpg"
                            If File.Exists(base_url + CStr(j) + ".png") Then file_ext = ".png"

                            level_url(1) = base_url + CStr(j) + file_ext
                            level_url(2) = base_url + CStr(cur_img(1)) + "\" + CStr(j) + file_ext
                            level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(j) + file_ext

                            If File.Exists(level_url(level)) Then img_count(level) += 1
                        End If

                    Next j
                    WrapPanelIcons.UpdateLayout()

                    'VIEW MODES
                    With BorderIconsQV
                        .Visibility = Visibility.Hidden
                        .Width = 0
                        .Margin = New Thickness(0)
                        .BorderThickness = New Thickness(0)
                        .BeginAnimation(Border.OpacityProperty, Nothing)
                    End With
                    ImageIconsQV.Source = Nothing

                    With BorderMapQV
                        .Visibility = Visibility.Hidden
                        .Width = Double.NaN
                        .Margin = New Thickness(0)
                        .BorderThickness = New Thickness(0)
                        .BeginAnimation(Border.OpacityProperty, Nothing)
                    End With
                    ScrollViewerMapQV.Height = 0
                    ImageMapQV.Source = Nothing

                    selected_itm_mode = "icon"

                    'PREVIEW QUICKVIEW
                    If File.Exists(base_url + "quick.view") Then
                        With BorderIconsQV
                            .Visibility = Visibility.Visible
                            .Width = ScrollViewerIcons.ActualWidth - icon_margin.Left * 2
                            .Height = Double.NaN
                            .Margin = New Thickness(icon_margin.Left, -icon_margin.Top * 2, icon_margin.Right, icon_margin.Bottom)
                            .BorderThickness = icon_border
                        End With
                        If File.Exists(base_url + "1.jpg") Then
                            ImageIconsQV.Source = New BitmapImage(New Uri(base_url + "1.jpg"))
                        End If
                        BorderIconsQV.Opacity = 0
                        Dim qv_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25))
                        qv_anim.BeginTime = TimeSpan.FromSeconds(1)
                        BorderIconsQV.BeginAnimation(Border.OpacityProperty, qv_anim)
                        selected_itm_mode = "preview_qv"
                    End If

                    'MAP QUICKVIEW
                    If File.Exists(base_url + "map.view") Then
                        With BorderMapQV
                            .Visibility = Visibility.Visible
                            .Width = ScrollViewerIcons.ActualWidth - 16
                            .Margin = New Thickness(8, 0, 8, 8)
                            .BorderThickness = New Thickness(5)
                        End With
                        If File.Exists(base_url + "1.jpg") Then
                            ImageMapQV.Source = New BitmapImage(New Uri(base_url + "1.jpg"))
                            ImageMapQV.Stretch = Stretch.None
                            If GridBg.ActualHeight - StackPanelIcons.ActualHeight - 32 > 0 Then _
                                ScrollViewerMapQV.Height = GridBg.ActualHeight - StackPanelIcons.ActualHeight - 32 _
                                Else _
                                ScrollViewerMapQV.Height = Double.NaN
                        End If

                        BorderMapQV.Opacity = 0
                        Dim qv_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25))
                        qv_anim.BeginTime = TimeSpan.FromSeconds(1)
                        BorderMapQV.BeginAnimation(Border.OpacityProperty, qv_anim)
                        selected_itm_mode = "map_qv"
                    End If

                    'ZOOM ANIM
                    WrapPanelIcons.UpdateLayout()

                    'CHECK OVER-WIDTH
                    If WrapPanelIcons.ActualHeight > BorderIcons.ActualHeight And GridBg.ActualHeight < GridBg.ActualWidth Then
                        ScrollViewerIcons.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden
                        ScrollViewerIcons.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled
                        WrapPanelIcons.UpdateLayout()
                    End If

                    'CHECK OVER-HEIGHT
                    If WrapPanelIcons.ActualHeight > BorderIcons.ActualHeight And GridBg.ActualHeight > GridBg.ActualWidth Then
                        ScrollViewerIcons.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
                        ScrollViewerIcons.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
                        WrapPanelIcons.UpdateLayout()
                    End If

                    Dim panel_width2 As Double = BorderIcons.ActualWidth
                    Dim panel_height2 As Double = BorderIcons.ActualHeight

                    'HIDE ICONS FOR ANIM
                    If panel_width1 <> panel_width2 Or panel_height1 <> panel_height2 Or selected_mainmenuitm <> pre_itm Then
                        pre_itm = selected_mainmenuitm
                        For j = 1 To items_limit
                            Dim iconsubitm As Image = WrapPanelIcons.FindName("IconSubItem" + CStr(j))
                            Dim iconsubitmbrd As Border = WrapPanelIcons.FindName("IconSubItemBorder" + CStr(j))
                            With iconsubitmbrd
                                .Visibility = Visibility.Hidden
                                .Width = 0
                                .Height = 0
                                .Margin = icon_margin
                            End With
                        Next j
                        'ANIM W AND H
                        Dim anim_pw As New DoubleAnimation(panel_width1, panel_width2, TimeSpan.FromSeconds(0.5)) With {.BeginTime = TimeSpan.FromSeconds(0)}
                        BorderIcons.BeginAnimation(Border.WidthProperty, anim_pw)
                        If selected_itm_mode = "preview_qv" Then
                            panel_height2 += icon_margin.Top * 2
                        End If
                        Dim anim_ph As New DoubleAnimation(panel_height1, panel_height2, TimeSpan.FromSeconds(0.5)) With {.BeginTime = TimeSpan.FromSeconds(0)}
                        BorderIcons.BeginAnimation(Border.HeightProperty, anim_ph)

                        'SHOW ICONS BACK AFTER DELAY
                        If selected_itm_mode = "icon" Then cur_img(level) = 0
                        If selected_itm_mode = "preview_qv" Or selected_itm_mode = "map_qv" Then cur_img(level) = 1
                        For j = 1 To items_limit
                            Dim iconsubitm As Image = WrapPanelIcons.FindName("IconSubItem" + CStr(j))
                            Dim iconsubitmbrd As Border = WrapPanelIcons.FindName("IconSubItemBorder" + CStr(j))
                            With iconsubitmbrd
                                .Visibility = Visibility.Hidden
                                .Width = 0
                                .Height = 0
                                .Margin = icon_margin
                                .Opacity = 0
                            End With
                            iconsubitm.Opacity = 0
                            iconsubitm.BeginAnimation(Image.OpacityProperty, Nothing)
                            iconsubitmbrd.BeginAnimation(Border.OpacityProperty, Nothing)

                            If File.Exists(data_root + "" + CStr(selected_mainmenuitm) + "sub\" + CStr(j) + "btn.jpg") Then
                                iconsubitm.Source = New BitmapImage(New Uri(data_root + "" + CStr(selected_mainmenuitm) + "sub\" + CStr(j) + "btn.jpg"))
                                With iconsubitmbrd
                                    .Visibility = Visibility.Visible
                                    .Width = Double.NaN
                                    .Height = Double.NaN
                                    .Margin = icon_margin
                                End With
                                iconsubitm.Width = iconsubitm.Source.Width
                                iconsubitm.Height = iconsubitm.Source.Height
                                Dim m_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(j / (j * Rnd() + 5))) With {.BeginTime = TimeSpan.FromSeconds(0.5)}
                                iconsubitm.BeginAnimation(Image.OpacityProperty, m_anim)
                                iconsubitmbrd.BeginAnimation(Border.OpacityProperty, m_anim)
                            End If

                        Next j
                    End If

                    'SHOW SUBITEMS
                    'Grid.SetColumnSpan(StackPanelContent, 1)
                    'sub_item = False
                    'For j = 1 To items_limit
                    '    Dim imgsubitm As Image = StackPanelContent.FindName("ImageSubItem" + CStr(j))
                    '    Dim srcsubitm As ScrollViewer = StackPanelContent.FindName("ScrollSubItem" + CStr(j))
                    '    imgsubitm.Visibility = Visibility.Hidden
                    '    srcsubitm.Visibility = Visibility.Hidden
                    '    imgsubitm.Width = 0
                    '    srcsubitm.Width = 0
                    '    srcsubitm.BeginAnimation(ScrollViewer.WidthProperty, Nothing)
                    '    If File.Exists(base_dir + "item" + CStr(selected_itm) + "sub\" + CStr(j) + ".jpg") Then
                    '        If Not IsNothing(imgsubitm) Then
                    '            Grid.SetColumnSpan(StackPanelContent, 2)
                    '            imgsubitm.Source = New BitmapImage(New Uri(base_dir + "item" + CStr(selected_itm) + "sub\" + CStr(j) + ".jpg"))
                    '            imgsubitm.Width = imgsubitm.Source.Width
                    '            imgsubitm.Height = imgsubitm.Source.Height
                    '            imgsubitm.Stretch = Stretch.None
                    '            imgsubitm.Visibility = Visibility.Visible
                    '            srcsubitm.Visibility = Visibility.Visible
                    '            Dim eff As New Effects.DropShadowEffect
                    '            eff.ShadowDepth = 0
                    '            srcsubitm.Effect = eff
                    '            srcsubitm.Width = 100
                    '            If j = 1 Then
                    '                Dim w_anim1 As New DoubleAnimation(100, imgsubitm.Source.Width, TimeSpan.FromSeconds(0.5))
                    '                srcsubitm.BeginAnimation(ScrollViewer.WidthProperty, w_anim1)
                    '                imgsubitm.Opacity = 1
                    '            Else
                    '                If srcsubitm.Width <> 100 Then
                    '                    Dim w_anim2 As New DoubleAnimation(imgsubitm.Source.Width, 100, TimeSpan.FromSeconds(0.5))
                    '                    srcsubitm.BeginAnimation(ScrollViewer.WidthProperty, w_anim2)
                    '                End If
                    '                imgsubitm.Opacity = 0.75
                    '            End If
                    '            srcsubitm.ScrollToRightEnd()
                    '            sub_item = True
                    '            selected_subitem = 1
                    '        End If
                    '    End If
                    'Next j

                    'MENU BTN ANIM - TOUCH EFF
                    If menu_touch_effect_type = "left" Then
                        Dim eff_transf = New TranslateTransform
                        imgitm.RenderTransform = eff_transf
                        Dim eff_anim As New DoubleAnimation(-menu_touch_effect_amount, 0, TimeSpan.FromSeconds(0.25)) With
                            {.EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseOut}}
                        eff_transf.BeginAnimation(TranslateTransform.XProperty, eff_anim)
                    End If
                    If menu_touch_effect_type = "right" Then
                        Dim eff_transf = New TranslateTransform
                        imgitm.RenderTransform = eff_transf
                        Dim eff_anim As New DoubleAnimation(menu_touch_effect_amount, 0, TimeSpan.FromSeconds(0.25)) With
                            {.EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseOut}}
                        eff_transf.BeginAnimation(TranslateTransform.XProperty, eff_anim)
                    End If
                    If menu_touch_effect_type = "top" Then
                        Dim eff_transf = New TranslateTransform
                        imgitm.RenderTransform = eff_transf
                        Dim eff_anim As New DoubleAnimation(-menu_touch_effect_amount, 0, TimeSpan.FromSeconds(0.25)) With
                            {.EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseOut}}
                        eff_transf.BeginAnimation(TranslateTransform.YProperty, eff_anim)
                    End If
                    If menu_touch_effect_type = "bottom" Then
                        Dim eff_transf = New TranslateTransform
                        imgitm.RenderTransform = eff_transf
                        Dim eff_anim As New DoubleAnimation(menu_touch_effect_amount, 0, TimeSpan.FromSeconds(0.25)) With
                            {.EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseOut}}
                        eff_transf.BeginAnimation(TranslateTransform.YProperty, eff_anim)
                    End If
                    If menu_touch_effect_type = "scale-up" Then
                        Dim eff_transf = New ScaleTransform With {.CenterX = imgitm.ActualWidth / 2, .CenterY = imgitm.ActualHeight / 2}
                        imgitm.RenderTransform = eff_transf
                        Dim eff_anim As New DoubleAnimation(1 + menu_touch_effect_amount / 100, 1, TimeSpan.FromSeconds(0.25)) With
                            {.EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseOut}}
                        eff_transf.BeginAnimation(ScaleTransform.ScaleXProperty, eff_anim)
                        eff_transf.BeginAnimation(ScaleTransform.ScaleYProperty, eff_anim)
                    End If
                    If menu_touch_effect_type = "scale-down" Then
                        Dim eff_transf = New ScaleTransform With {.CenterX = imgitm.ActualWidth / 2, .CenterY = imgitm.ActualHeight / 2}
                        imgitm.RenderTransform = eff_transf
                        Dim eff_anim As New DoubleAnimation(1 - menu_touch_effect_amount / 100, 1, TimeSpan.FromSeconds(0.25)) With
                            {.EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseOut}}
                        eff_transf.BeginAnimation(ScaleTransform.ScaleXProperty, eff_anim)
                        eff_transf.BeginAnimation(ScaleTransform.ScaleYProperty, eff_anim)
                    End If

                    'OLD:
                    If menu_x_eff Then
                        Dim transf = New TranslateTransform
                        imgitm.RenderTransform = transf
                        Dim anim1 As New DoubleAnimation(-5, 0, TimeSpan.FromSeconds(0.25)) With {.EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseOut}}
                        transf.BeginAnimation(TranslateTransform.XProperty, anim1)
                    End If

                    'SIDE SLIDE ANIM
                    If r_side_panel Then
                        EventAnimatorSub(BorderHomeSlides, slides_hide_direction, True, slides_hide_alpha)
                        r_side_panel = False
                    End If

                    'SND
                    item_snd.Play()
                End If
            End If
        Next i
    End Sub

    'MENU MOUSE MOVE ANIM 
    Private Sub StackPanel2_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles GridMainMenu.MouseMove
        'Dim feSource As Image = TryCast(e.Source, Image)
        'For i = 1 To items_limit
        '    Dim imgitm As Image = StackPanel1.FindName("ImageItem" + CStr(i))
        '    'SET ITMBTN UNSEL IMG
        '    If Not IsNothing(preload_img_menu_uns(i)) Then imgitm.Source = preload_img_menu_uns(i)
        '    'SET ITMBTN SEL IMG
        '    If Not IsNothing(imgitm) Then
        '        If feSource.Name = imgitm.Name Then imgitm.Source = preload_img_menu_sel(i)
        '    End If
        '    'CUR ITM SEL
        '    If i = selected_itm And item_view Then imgitm.Source = preload_img_menu_sel(i)
        '    'LOGO IMG CHANGE
        '    If item_view Then
        '        If File.Exists(base_dir + "logo_sel.png") Then ImgLogo1.Source = New BitmapImage(New Uri(base_dir + "logo_sel.png"))
        '        If feSource.Name = ImgLogo1.Name Then If File.Exists(base_dir + "logo.png") Then ImgLogo1.Source = New BitmapImage(New Uri(base_dir + "logo.png"))
        '    End If
        'Next i
    End Sub

    Private Sub StackPanel2_MouseLeave(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles GridMainMenu.MouseLeave
        'For i = 1 To items_limit
        '    Dim imgitm As Image = StackPanel1.FindName("ImageItem" + CStr(i))
        '    'SET ITMBTN UNSEL IMG
        '    If Not IsNothing(preload_img_menu_uns(i)) Then imgitm.Source = preload_img_menu_uns(i)
        '    'CUR ITM SEL
        '    If i = selected_itm And item_view Then imgitm.Source = preload_img_menu_sel(i)
        '    'LOGO IMG CHANGE
        '    If item_view Then
        '        If File.Exists(base_dir + "logo_sel.jpg") Then ImgLogo1.Source = New BitmapImage(New Uri(base_dir + "logo_sel.jpg"))
        '    End If
        'Next i
    End Sub

    'LOAD ICONS
    Private Sub LoadIcons(ByRef j As Integer, ByRef show_backicon As Boolean)
        Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"

        'USE ICONS MODE
        'StackPanelContent.Visibility = Visibility.Hidden
        BorderIcons.Visibility = Visibility.Visible
        BorderIcons.IsEnabled = True
        BorderIcons.BeginAnimation(Border.OpacityProperty, Nothing)
        BorderIcons.Opacity = 1

        'ZOOM ANIM INIT
        BorderIcons.Width = Double.NaN
        BorderIcons.BeginAnimation(WrapPanel.WidthProperty, Nothing)
        BorderIcons.Height = Double.NaN
        BorderIcons.BeginAnimation(WrapPanel.HeightProperty, Nothing)
        Dim panel_width1 As Double = BorderIcons.ActualWidth
        Dim panel_height1 As Double = BorderIcons.ActualHeight

        'HIDE BACKICON
        Dim backiconitm As Image = WrapPanelIcons.FindName("BackIcon")
        Dim backiconitmborder As Border = WrapPanelIcons.FindName("BackIconBorder")
        backiconitm.Source = Nothing
        With backiconitmborder
            .Visibility = Visibility.Hidden
            .Opacity = 0
            .Width = 0
            .Height = 0
            .Margin = New Thickness(0)
        End With

        'SHOW BACKICON
        If show_backicon Or level > 1 Then
            If level = 2 Then _
                If File.Exists(base_url + CStr(cur_img(1)) + "\" + "0btn.jpg") Then _
                    backiconitm.Source = New BitmapImage(New Uri(base_url + CStr(cur_img(1)) + "\" + "0btn.jpg"))
            If level = 3 Then _
                If File.Exists(base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + "0btn.jpg") Then _
                backiconitm.Source = New BitmapImage(New Uri(base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + "0btn.jpg"))
            If Not IsNothing(backiconitm.Source) Then
                With backiconitmborder
                    .Visibility = Visibility.Visible
                    .Opacity = 1
                    .Width = Double.NaN
                    .Height = Double.NaN
                    .Margin = icon_margin
                End With
                backiconitm.Width = backiconitm.Source.Width
                backiconitm.Height = backiconitm.Source.Height
            End If
        End If

        'SHOW ICONS SUBITEMS
        cur_img(level) = j
        img_count(level) = 0
        Dim btn_level_url(4) As String
        For jj = 1 To items_limit
            Dim iconsubitm As Image = WrapPanelIcons.FindName("IconSubItem" + CStr(jj))
            Dim iconsubitmbrd As Border = WrapPanelIcons.FindName("IconSubItemBorder" + CStr(jj))
            With iconsubitmbrd
                .Visibility = Visibility.Hidden
                .Width = 0
                .Height = 0
                .Margin = New Thickness(0)
            End With
            Dim file_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\" + CStr(cur_img(level)) + "\" + CStr(jj) + "btn.jpg"

            btn_level_url(1) = base_url + CStr(jj) + "btn.jpg"
            If ml1 Or demo_mode Then btn_level_url(2) = base_url + CStr(cur_img(1)) + "\" + CStr(jj) + "btn.jpg"
            If ml2 Or demo_mode Then btn_level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(jj) + "btn.jpg"

            level_url(1) = base_url + CStr(jj) + ".jpg"
            If ml1 Or demo_mode Then level_url(2) = base_url + CStr(cur_img(1)) + "\" + CStr(jj) + ".jpg"
            If ml2 Or demo_mode Then level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(jj) + ".jpg"

            If File.Exists(btn_level_url(level)) Then
                Dim b_img As New BitmapImage(New Uri(btn_level_url(level)))
                b_img.CacheOption = BitmapCacheOption.OnLoad
                iconsubitm.Source = b_img
                With iconsubitmbrd
                    .Visibility = Visibility.Visible
                    .Opacity = 1
                    .Width = Double.NaN
                    .Height = Double.NaN
                    .Margin = icon_margin
                End With
                iconsubitm.Width = iconsubitm.Source.Width
                iconsubitm.Height = iconsubitm.Source.Height
                Dim m_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(jj / 5))
                iconsubitm.BeginAnimation(Image.OpacityProperty, m_anim)
                If File.Exists(level_url(level)) Then img_count(level) += 1
            End If
        Next jj

        'ZOOM ANIM
        WrapPanelIcons.UpdateLayout()
        Dim panel_width2 As Double = BorderIcons.ActualWidth
        Dim panel_height2 As Double = BorderIcons.ActualHeight
        'HIDE ICONS FOR ANIM
        If panel_width1 <> panel_width2 Or panel_height1 <> panel_height2 Then
            If show_backicon Or level > 1 Then
                'BACKICON
                With backiconitmborder
                    .Visibility = Visibility.Hidden
                    .Width = 0
                    .Height = 0
                    .Margin = New Thickness(0)
                End With
            End If
            'ALL ICONS
            For j = 1 To items_limit
                Dim iconsubitm As Image = WrapPanelIcons.FindName("IconSubItem" + CStr(j))
                Dim iconsubitmbrd As Border = WrapPanelIcons.FindName("IconSubItemBorder" + CStr(j))
                With iconsubitmbrd
                    .Visibility = Visibility.Hidden
                    .Width = 0
                    .Height = 0
                    .Margin = New Thickness(0)
                End With
            Next j
            'ANIM W AND H
            Dim anim_pw As New DoubleAnimation(panel_width1, panel_width2, TimeSpan.FromSeconds(0.5))
            BorderIcons.BeginAnimation(Border.WidthProperty, anim_pw)
            Dim anim_ph As New DoubleAnimation(panel_height1, panel_height2, TimeSpan.FromSeconds(0.5))
            BorderIcons.BeginAnimation(Border.HeightProperty, anim_ph)
            'SHOW ICONS BACK AFTER DELAY
            cur_img(level) = j

            If show_backicon Or level > 1 Then
                'BACKICON
                If Not IsNothing(backiconitm.Source) Then
                    backiconitm.Opacity = 0
                    backiconitm.BeginAnimation(Image.OpacityProperty, Nothing)
                    With backiconitmborder
                        .Opacity = 0
                        .BeginAnimation(Border.OpacityProperty, Nothing)
                        .Visibility = Visibility.Visible
                        .Width = Double.NaN
                        .Height = Double.NaN
                        .Margin = icon_margin
                    End With
                    backiconitm.Width = backiconitm.Source.Width
                    backiconitm.Height = backiconitm.Source.Height
                    Dim bi_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5))
                    bi_anim.BeginTime = TimeSpan.FromSeconds(0.5)
                    backiconitm.BeginAnimation(Image.OpacityProperty, bi_anim)
                    backiconitmborder.BeginAnimation(Border.OpacityProperty, bi_anim)
                End If
            End If

            For jj = 1 To items_limit
                Dim iconsubitm As Image = WrapPanelIcons.FindName("IconSubItem" + CStr(jj))
                Dim iconsubitmbrd As Border = WrapPanelIcons.FindName("IconSubItemBorder" + CStr(jj))
                With iconsubitmbrd
                    .Visibility = Visibility.Hidden
                    .Width = 0
                    .Height = 0
                    .Margin = New Thickness(0)
                    .Opacity = 0
                End With
                iconsubitm.Opacity = 0
                iconsubitm.BeginAnimation(Image.OpacityProperty, Nothing)
                iconsubitmbrd.BeginAnimation(Border.OpacityProperty, Nothing)
                Dim file_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\" + CStr(cur_img(level)) + "\" + CStr(jj) + "btn.jpg"

                btn_level_url(1) = base_url + CStr(jj) + "btn.jpg"
                btn_level_url(2) = base_url + CStr(cur_img(1)) + "\" + CStr(jj) + "btn.jpg"
                btn_level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(jj) + "btn.jpg"

                level_url(1) = base_url + CStr(j) + ".jpg"
                level_url(2) = base_url + CStr(cur_img(1)) + "\" + CStr(j) + ".jpg"
                level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(j) + ".jpg"

                If File.Exists(btn_level_url(level)) Then
                    iconsubitm.Source = New BitmapImage(New Uri(btn_level_url(level)))
                    With iconsubitmbrd
                        .Visibility = Visibility.Visible
                        .Width = Double.NaN
                        .Height = Double.NaN
                        .Margin = icon_margin
                    End With
                    iconsubitm.Width = iconsubitm.Source.Width
                    iconsubitm.Height = iconsubitm.Source.Height
                    Dim m_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(jj / 5))
                    m_anim.BeginTime = TimeSpan.FromSeconds(0.5)
                    iconsubitm.BeginAnimation(Image.OpacityProperty, m_anim)
                    iconsubitmbrd.BeginAnimation(Border.OpacityProperty, m_anim)
                End If
            Next jj

        End If

        subitem_snd.Play()
    End Sub

    'SHOW HELP
    Private Sub ShowHelp(ByVal param As String, ByVal show As Boolean, Optional ByVal time As Integer = 3)
        Dim transf_help = New TranslateTransform
        ImageHelp.RenderTransform = transf_help
        transf_help.Y = GridSlide.ActualHeight / 2
        If show Then
            If File.Exists(data_root + param + ".jpg") Then
                ImageHelp.Source = New BitmapImage(New Uri(data_root + param + ".jpg"))
                ImageHelp.Width = ImageHelp.Source.Width
                ImageHelp.Height = ImageHelp.Source.Height
                ImageHelp.Opacity = help_opacity
                If File.Exists(data_root + param + ".jpg") Then
                    Dim mask As New ImageBrush()
                    mask.ImageSource = New BitmapImage(New Uri(data_root + param + ".jpg"))
                    ImageHelp.OpacityMask = mask
                End If
            Else
                ImageHelp.Source = Nothing
            End If
            Dim anim_help1 As New DoubleAnimationUsingKeyFrames
            'anim_help1.Duration = TimeSpan.FromSeconds(6)
            anim_help1.AutoReverse = True
            anim_help1.KeyFrames.Add(New LinearDoubleKeyFrame(0, TimeSpan.FromSeconds(0.5)))
            anim_help1.KeyFrames.Add(New LinearDoubleKeyFrame(0, TimeSpan.FromSeconds(time)))
            'anim_help1.KeyFrames.Add(New LinearDoubleKeyFrame(GridSlide.ActualHeight / 2, TimeSpan.FromSeconds(0.5)))
            transf_help.BeginAnimation(TranslateTransform.YProperty, anim_help1)
        End If
    End Sub

    'SHOW SLIDES
    Private Sub ShowSlides(ByRef url As String, ByRef expand_item As Boolean)
        If File.Exists(url) And Not expand_item Then

            BorderSlide.Visibility = Visibility.Visible
            ImgSlide2.Source = Nothing
            ImgSlide1.Source = New BitmapImage(New Uri(url))

            Dim ease As New BackEase
            ease.EasingMode = EasingMode.EaseInOut
            ease.Amplitude = 0.25

            Dim anim0 As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5))
            BorderSlide.BeginAnimation(Border.OpacityProperty, anim0)
            BorderSlide.IsEnabled = True

            Dim transf1 = New TranslateTransform
            ImgSlide1.RenderTransform = transf1
            ImgSlide1.Margin = New Thickness(0)
            ImgSlide2.Margin = New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)
            Dim anim1 As New DoubleAnimation(-GridSlide.ActualWidth, 0, TimeSpan.FromSeconds(1))
            anim1.EasingFunction = ease
            transf1.BeginAnimation(TranslateTransform.XProperty, anim1)

            ShowHelp("help", True, 3)

            UpdateMultiLayerSlides("next")
            UpdateSubslideButtonVisibility()
            UpdateNavigationVisual()
            slide_snd.Play()
        End If
    End Sub

    'INFORM ICON CLICK
    Private Sub PngIconSubItemHandler(ByVal sender As Object, ByVal e As RoutedEventArgs)

        Dim mouse_x As Integer = Mouse.GetPosition(GridIcons).X
        Dim mouse_y As Integer = Mouse.GetPosition(GridIcons).Y
        For i = 0 To VisualTreeHelper.GetChildrenCount(GridIcons) - 1
            Dim feSource0 As Image = TryCast(VisualTreeHelper.GetChild(GridIcons, i), Image)

            If Not IsNothing(feSource0) Then
                Dim bmp As New BitmapImage()
                Dim pix(4) As Byte
                bmp = feSource0.Source
                Dim cbmp As New CroppedBitmap(bmp, New Int32Rect(mouse_x, mouse_y, 1, 1))
                Try
                    cbmp.CopyPixels(pix, 4, 0)
                    If pix(3) <> 0 Then
                        ZPngIconSubItemHandler(feSource0, e)
                        Exit Sub
                    End If
                Catch ex As Exception
                End Try
            End If
        Next

        ImageItemHandler(sender, e)
        GridMainMenu_MouseDown(sender, e)

    End Sub

    Private Sub ZPngIconSubItemHandler(ByVal sender As Object, ByVal e As RoutedEventArgs)

        Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"
        'Dim feSource As Image = TryCast(e.Source, Image)
        Dim feSource As Image = sender
        Dim expand_item As Boolean = False

        feSource.BeginAnimation(Image.OpacityProperty, New DoubleAnimation(0.75, 1, TimeSpan.FromSeconds(0.5)))

        For j = 1 To GridIcons.Children.Count
            Dim imgitm As Image = TryCast(GridIcons.Children(j - 1), Image)
            If Not IsNothing(imgitm) And Not IsNothing(feSource) And selected_itm_mode = "icon" Then
                'If feSource.Name = "BackIcon" Then
                '    level -= 1
                '    LoadIcons(j, False)
                '    Exit Sub
                'End If

                If feSource.Source.ToString = imgitm.Source.ToString Then
                    cur_img(level) = j

                    'MULTILEVEL SHOW SUB ICONS
                    'LEVEL 2
                    If My.Computer.FileSystem.DirectoryExists(data_root + "" + CStr(selected_mainmenuitm) + "sub\" + CStr(j)) And level = 1 Then
                        expand_item = True
                        level = 2
                        LoadIcons(j, True)
                    End If
                    'LEVEL 3
                    If My.Computer.FileSystem.DirectoryExists(data_root + "" + CStr(selected_mainmenuitm) + "sub\" + CStr(cur_img(1)) + "\" + CStr(j)) And level = 2 Then
                        expand_item = True
                        level = 3
                        LoadIcons(j, True)
                    End If

                    Dim slides_ext As String = ".jpg"
                    If File.Exists(base_url + CStr(j) + ".png") Then
                        slides_ext = ".png"
                    End If

                    level_url(1) = base_url + CStr(j) + slides_ext
                    level_url(2) = base_url + CStr(cur_img(1)) + "\" + CStr(j) + slides_ext
                    level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(j) + slides_ext

                    'PNG FS.SLIDES BG
                    SlidesBg.Source = Nothing
                    If File.Exists(data_root + "interface\" + "slide_bg.png") Then
                        BorderSlide.Background = Nothing
                        SlidesBg.Source = New BitmapImage(New Uri(data_root + "interface\" + "slide_bg.png"))
                    End If
                    If File.Exists(base_url + "slide_bg.png") Then
                        BorderSlide.Background = Nothing
                        SlidesBg.Source = New BitmapImage(New Uri(base_url + "slide_bg.png"))
                    End If
                    Dim subsub_path As String = base_url + CStr(cur_img(1)) + "sub" + "\"
                    If File.Exists(subsub_path + "slide_bg.png") Then
                        BorderSlide.Background = Nothing
                        SlidesBg.Source = New BitmapImage(New Uri(subsub_path + "slide_bg.png"))
                    End If

                    'PNG FS.SLIDES FG
                    SlidesFg.Source = Nothing
                    If File.Exists(data_root + "interface\" + "slide_fg.png") Then
                        SlidesFg.Source = New BitmapImage(New Uri(data_root + "interface\" + "slide_fg.png"))
                    End If
                    If File.Exists(base_url + "slide_fg.png") Then
                        SlidesFg.Source = New BitmapImage(New Uri(base_url + "slide_fg.png"))
                    End If
                    If File.Exists(subsub_path + "slide_fg.png") Then
                        SlidesFg.Source = New BitmapImage(New Uri(subsub_path + "slide_fg.png"))
                    End If

                    '3D CONTENT TEST
                    'If File.Exists(level_url(level).Replace("jpg", "3ds")) And Not expand_item Then
                    '    Grid3D.Visibility = Visibility.Visible
                    '    Grid3D.Width = Double.NaN
                    '    Grid3D.Height = Double.NaN

                    '    Dim obj As New HelixToolkit.StudioVisual3D
                    '    obj.Source = level_url(level).Replace("jpg", "3ds")
                    '    Dim light As New HelixToolkit.DefaultLightsVisual3D
                    '    HelixView3D1.Add(obj)
                    '    HelixView3D1.Add(light)
                    '    Dim vec As New Media3D.Vector3D
                    '    vec.X = 1
                    '    vec.Y = 1
                    '    vec.Z = -1
                    '    HelixView3D1.Camera.LookDirection = vec
                    '    HelixView3D1.CameraController.Rotate(0, 0)
                    '    HelixView3D1.ZoomToFit()
                    'End If

                    'VIDEO CONTENT TEST
                    VideoContent(level_url(level).Replace(slides_ext, ".avi"), expand_item)

                    'LEVEL ICON HANDLER - SHOW CONTENT
                    ShowSlides(level_url(level), expand_item)

                    If Not File.Exists(level_url(level)) And Not expand_item Then deny_snd.Play()
                End If
            End If

            'QUICK VIEW MODE
            '    If Not IsNothing(imgitm) And Not IsNothing(feSource) Then
            '        If feSource.Name = imgitm.Name Then
            '            If selected_itm_mode = "preview_qv" Or selected_itm_mode = "map_qv" Then
            '                If File.Exists(base_url + CStr(j) + ".jpg") Then
            '                    cur_img(1) = j
            '                    If selected_itm_mode = "preview_qv" Then
            '                        ImageIconsQV.Source = New BitmapImage(New Uri(base_url + CStr(j) + ".jpg"))
            '                        BorderIconsQV.Opacity = 0
            '                        Dim qv_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25))
            '                        BorderIconsQV.BeginAnimation(Border.OpacityProperty, qv_anim)
            '                    End If
            '                    If selected_itm_mode = "map_qv" Then
            '                        ImageMapQV.Source = New BitmapImage(New Uri(base_url + CStr(j) + ".jpg"))
            '                        BorderMapQV.Opacity = 0
            '                        Dim qv_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25))
            '                        BorderMapQV.BeginAnimation(Border.OpacityProperty, qv_anim)
            '                    End If
            '                    pic_snd.Play()
            '                Else
            '                    ImageIconsQV.Source = Nothing
            '                    ImageMapQV.Source = Nothing
            '                    deny_snd.Play()
            '                End If
            '            End If
            '        End If
            '    End If

        Next j

    End Sub

    'ICON CLICK
    Private Sub IconSubItemHandler(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If Not oh_scrolled Then
            Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"
            Dim feSource As Image = TryCast(e.Source, Image)
            Dim expand_item As Boolean = False
            BorderSlide.Visibility = Visibility.Hidden
            For j = 1 To items_limit
                Dim imgitm As Image = WrapPanelIcons.FindName("IconSubItem" + CStr(j))
                If Not IsNothing(imgitm) And Not IsNothing(feSource) And selected_itm_mode = "icon" Then
                    If feSource.Name = "BackIcon" Then
                        level -= 1
                        LoadIcons(j, False)
                        Exit Sub
                    End If

                    If feSource.Name = imgitm.Name Then
                        cur_img(level) = j

                        'MULTILEVEL SHOW SUB ICONS
                        'LEVEL 2
                        If My.Computer.FileSystem.DirectoryExists(data_root + "" + CStr(selected_mainmenuitm) + "sub\" + CStr(j)) And level = 1 Then
                            expand_item = True
                            level = 2
                            LoadIcons(j, True)
                        End If
                        'LEVEL 3
                        If My.Computer.FileSystem.DirectoryExists(data_root + "" + CStr(selected_mainmenuitm) + "sub\" + CStr(cur_img(1)) + "\" + CStr(j)) And level = 2 Then
                            expand_item = True
                            level = 3
                            LoadIcons(j, True)
                        End If

                        Dim slides_ext As String = ".jpg"
                        If File.Exists(base_url + CStr(j) + ".png") Then
                            slides_ext = ".png"
                        End If

                        level_url(1) = base_url + CStr(j) + slides_ext
                        level_url(2) = base_url + CStr(cur_img(1)) + "\" + CStr(j) + slides_ext
                        level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(j) + slides_ext

                        'PNG FS.SLIDES BG
                        SlidesBg.Source = Nothing
                        If File.Exists(data_root + "interface\" + "slide_bg.png") Then
                            BorderSlide.Background = Nothing
                            SlidesBg.Source = New BitmapImage(New Uri(data_root + "interface\" + "slide_bg.png"))
                        End If
                        If File.Exists(base_url + "slide_bg.png") Then
                            BorderSlide.Background = Nothing
                            SlidesBg.Source = New BitmapImage(New Uri(base_url + "slide_bg.png"))
                        End If
                        Dim subsub_path As String = base_url + CStr(cur_img(1)) + "sub" + "\"
                        If File.Exists(subsub_path + "slide_bg.png") Then
                            BorderSlide.Background = Nothing
                            SlidesBg.Source = New BitmapImage(New Uri(subsub_path + "slide_bg.png"))
                        End If
                        'PNG FS.SLIDES FG
                        SlidesFg.Source = Nothing
                        If File.Exists(data_root + "interface\" + "slide_fg.png") Then
                            SlidesFg.Source = New BitmapImage(New Uri(data_root + "interface\" + "slide_fg.png"))
                        End If
                        If File.Exists(base_url + "slide_fg.png") Then
                            SlidesFg.Source = New BitmapImage(New Uri(base_url + "slide_fg.png"))
                        End If

                        '3D CONTENT TEST
                        If File.Exists(level_url(level).Replace(slides_ext, "3ds")) And Not expand_item Then
                            Grid3D.Visibility = Visibility.Visible
                            Grid3D.Width = Double.NaN
                            Grid3D.Height = Double.NaN

                            Dim obj As New HelixToolkit.StudioVisual3D
                            obj.Source = level_url(level).Replace(slides_ext, "3ds")
                            Dim light As New HelixToolkit.DefaultLightsVisual3D
                            HelixView3D1.Add(obj)
                            HelixView3D1.Add(light)
                            Dim vec As New Media3D.Vector3D
                            vec.X = 1
                            vec.Y = 1
                            vec.Z = -1
                            HelixView3D1.Camera.LookDirection = vec
                            HelixView3D1.CameraController.Rotate(0, 0)
                            HelixView3D1.ZoomToFit()
                        End If

                        'VIDEO CONTENT TEST
                        'Dim slides_ext As String = ".jpg" 'TMP 2FIX !!!
                        VideoContent(level_url(level).Replace(slides_ext, ".avi"), expand_item)

                        'LEVEL ICON HANDLER - SHOW CONTENT
                        ShowSlides(level_url(level), expand_item)

                        If Not File.Exists(level_url(level)) And Not expand_item Then deny_snd.Play()
                    End If
                End If

                'QUICK VIEW MODE
                If Not IsNothing(imgitm) And Not IsNothing(feSource) Then
                    If feSource.Name = imgitm.Name Then
                        If selected_itm_mode = "preview_qv" Or selected_itm_mode = "map_qv" Then
                            If File.Exists(base_url + CStr(j) + ".jpg") Then
                                cur_img(1) = j
                                If selected_itm_mode = "preview_qv" Then
                                    ImageIconsQV.Source = New BitmapImage(New Uri(base_url + CStr(j) + ".jpg"))
                                    BorderIconsQV.Opacity = 0
                                    Dim qv_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25))
                                    BorderIconsQV.BeginAnimation(Border.OpacityProperty, qv_anim)
                                End If
                                If selected_itm_mode = "map_qv" Then
                                    ImageMapQV.Source = New BitmapImage(New Uri(base_url + CStr(j) + ".jpg"))
                                    BorderMapQV.Opacity = 0
                                    Dim qv_anim As New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25))
                                    BorderMapQV.BeginAnimation(Border.OpacityProperty, qv_anim)
                                End If
                                pic_snd.Play()
                            Else
                                ImageIconsQV.Source = Nothing
                                ImageMapQV.Source = Nothing
                                deny_snd.Play()
                            End If
                        End If
                    End If
                End If
            Next j
        End If

    End Sub

    'PREVIEW QUICKVIEW
    Private Sub ImageIconsQV_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ImageIconsQV.MouseUp
        Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"
        ShowSlides(base_url + CStr(cur_img(1)) + ".jpg", False) 'ImageIconsQV.Source.ToString.Substring(8)
    End Sub

    'MAP QUICKVIEW
    Dim mqv_x_pos, mqv_y_pos, mqv_x_point, mqv_y_point As Double
    Private Sub ImageMapQV_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ImageMapQV.MouseDown
        mqv_x_pos = e.GetPosition(Me).X
        mqv_y_pos = e.GetPosition(Me).Y
        mqv_x_point = mqv_x_pos
        mqv_y_point = mqv_y_pos
    End Sub
    Private Sub ImageMapQV_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles ImageMapQV.MouseMove
        If e.LeftButton <> MouseButton.Left Then
            Dim mqv_dx As Double = e.GetPosition(Me).X - mqv_x_point
            Dim mqv_dy As Double = e.GetPosition(Me).Y - mqv_y_point
            mqv_x_point = e.GetPosition(Me).X
            mqv_y_point = e.GetPosition(Me).Y
            ScrollViewerMapQV.ScrollToHorizontalOffset(ScrollViewerMapQV.HorizontalOffset - mqv_dx)
            ScrollViewerMapQV.ScrollToVerticalOffset(ScrollViewerMapQV.VerticalOffset - mqv_dy)
        End If
    End Sub

    'VDO CONTENT
    Private Sub VideoContent(ByRef url As String, ByRef expand_item As Boolean)
        With GridMediaContent
            .Visibility = Visibility.Hidden
            .Opacity = 0
            .BeginAnimation(MediaUriElement.OpacityProperty, Nothing)
        End With
        MediaUriElementContent.Stretch = Stretch.UniformToFill
        MediaUriElementContent.Source = Nothing
        MediaUriElementContent.Stop()
        LbStopBtn.Content = "Stop"
        LbFullScreenBtn.Content = "Full Screen"

        If File.Exists(url) And Not expand_item And url.EndsWith(".avi") Then
            GridMediaContent.Visibility = Visibility.Visible
            GridMediaContent.BeginAnimation(MediaUriElement.OpacityProperty,
                            New DoubleAnimation(0, 1, TimeSpan.FromSeconds(1)) _
                            With {.BeginTime = TimeSpan.FromSeconds(1)})
            MediaUriElementContent.Source = New Uri(url)
            MediaUriElementContent.Play()
        End If
    End Sub

    'SHOW NAV BTN FOR VIDEO CONTENT
    Private Sub MediaUriElementContent_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles MediaUriElementContent.MouseUp
        Dim kf_anim As New DoubleAnimationUsingKeyFrames
        kf_anim.KeyFrames.Add(New SplineDoubleKeyFrame(0, TimeSpan.FromSeconds(0)))
        kf_anim.KeyFrames.Add(New SplineDoubleKeyFrame(1, TimeSpan.FromSeconds(0.5)))
        kf_anim.KeyFrames.Add(New SplineDoubleKeyFrame(1, TimeSpan.FromSeconds(3)))
        kf_anim.KeyFrames.Add(New SplineDoubleKeyFrame(0, TimeSpan.FromSeconds(3.5)))
        StPanMediaContentControls.BeginAnimation(StackPanel.OpacityProperty, kf_anim)
    End Sub

    'VIDEO CONTROLS
    Private Sub PauseBtn_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles PauseBtn.MouseUp
        With MediaUriElementContent
            If .IsPlaying Then .Pause() Else .Play()
        End With
        StPanMediaContentControls.BeginAnimation(StackPanel.OpacityProperty, New DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5)))
    End Sub
    Private Sub StopBtn_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles StopBtn.MouseUp
        With MediaUriElementContent
            If .IsPlaying Then
                .Stop()
                .MediaPosition = 0
                LbStopBtn.Content = "Play"
            Else
                .Play()
                LbStopBtn.Content = "Stop"
            End If
        End With
        StPanMediaContentControls.BeginAnimation(StackPanel.OpacityProperty, New DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5)))
    End Sub
    Private Sub FullScreenBtn_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles FullScreenBtn.MouseUp
        If LbFullScreenBtn.Content = "Full Screen" Then
            With GridMediaContent
                .Width = GridSlide.ActualWidth
                .Height = GridSlide.ActualHeight
                .Margin = New Thickness(0)
            End With
            LbFullScreenBtn.Content = "Exit"
        Else
            With GridMediaContent
                .Width = me_w
                .Height = me_h
                .Margin = New Thickness(me_x, me_y, 0, 0)
            End With
            LbFullScreenBtn.Content = "Full Screen"
        End If
        StPanMediaContentControls.BeginAnimation(StackPanel.OpacityProperty, New DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5)))
    End Sub

    Dim view_mode As String
    Dim track_click As Integer = 0
    Dim fs_ind As Integer = 1
    Dim slide_x_point, slide_y_point As Double
    Dim transf = New TranslateTransform
    Dim touch As Boolean = False
    Dim dx, dy As Double
    Dim lim As Integer = 50
    Dim zoom_mode As Boolean = False
    Dim zoom_click As Integer = 0
    Dim zoom_x0, zoom_y0, zoom_x1, zoom_y1 As Double
    Dim next_index As Integer
    Dim next_transf = New TranslateTransform

    'SLIDES GESTURES
    Private Sub ImgSlide1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        slide_x_point = e.GetPosition(Me).X
        slide_y_point = e.GetPosition(Me).Y
        If Not zoom_mode Then touch = True
    End Sub

    Private Sub ImgSlide1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)

        If e.LeftButton <> MouseButton.Left And touch And Not multitouch_sw And Not note_sw Then
            dx = e.GetPosition(Me).X - slide_x_point
            dy = e.GetPosition(Me).Y - slide_y_point
            If ImgSlide2.Margin.Equals(New Thickness(0)) Then transf.X = GridSlide.ActualWidth + dx / 2
            If ImgSlide2.Margin.Equals(New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)) Then transf.X = dx / 2
            If Math.Abs(dx) < 10 Then transf.Y = dy / 2 Else transf.Y = 0
            If Not subitemview And Not zoomed Then ImgSlide1.RenderTransform = transf
            If Not subitemview And zoomed And dx >= 10 Then ImgSlide1.RenderTransform = transf

            'EXIT / MOVE UP
            If dy < -lim And Math.Abs(dx) < lim / 2 Then
                ExitSlides_Action()
                Exit Sub
            End If

            'SLIDE <---
            If dx > lim And Math.Abs(dy) < lim / 2 Then
                PrevItem_Action()
                Exit Sub
            End If

            'SLIDE --->
            If dx < -lim And Math.Abs(dy) < lim / 2 Then
                NextItem_Action()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub ImgSlide1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If touch And dx <> 0 Then
            Dim transf1 = New TranslateTransform
            If Not subitemview Then ImgSlide1.RenderTransform = transf1
            Dim anim1 As New DoubleAnimation(GridSlide.ActualWidth + dx / 2, GridSlide.ActualWidth, TimeSpan.FromSeconds(0.25))
            Dim anim2 As New DoubleAnimation(dx / 2, 0, TimeSpan.FromSeconds(0.25))
            If ImgSlide2.Margin.Equals(New Thickness(0)) Then transf1.BeginAnimation(TranslateTransform.XProperty, anim1)
            If ImgSlide2.Margin.Equals(New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)) Then transf1.BeginAnimation(TranslateTransform.XProperty, anim2)
            Dim anim3 As New DoubleAnimation(dy / 2, 0, TimeSpan.FromSeconds(0.25))
            transf1.BeginAnimation(TranslateTransform.YProperty, anim3)
            dx = 0
            dy = 0
            touch = False
        End If
    End Sub

    'MAIN PAGE SLIDES SEL
    Dim border1_click As Boolean = False
    Private Sub Border1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles BorderHomeSlides.MouseUp
        sstimer.Stop()
        sstimer.Start()
        'If Not IsNothing(ImgRS1.Source) Then
        If SlideShow.Items.Count <> 0 Then sstimer_Tick(Nothing, Nothing)
        If Not IsNothing(MediaUriElement1.Source) Then
            If border1_click Then MediaUriElement1.Pause() Else MediaUriElement1.Play()
            border1_click = Not border1_click
        End If
        slide_snd.Play()
    End Sub

    '======================================= 3D =================================
    Dim init_x, init_y As Double
    'PAN 3D
    Private Sub HelixView3D1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles HelixView3D1.MouseDown
        init_x = e.GetPosition(Me).X
        init_y = e.GetPosition(Me).Y
    End Sub
    Private Sub HelixView3D1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles HelixView3D1.MouseMove
        If Mouse.LeftButton = MouseButtonState.Pressed Then
            HelixView3D1.CameraController.AddRotateForce(-(init_x - e.GetPosition(Me).X) / 500, -(init_y - e.GetPosition(Me).Y) / 500)
        End If
    End Sub
    'ZOOM 3D
    Dim zoomed_3d As Boolean = False
    Private Sub HelixView3D1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles HelixView3D1.MouseDoubleClick
        If Not zoomed_3d Then
            HelixView3D1.CameraController.AddZoomForce(-0.5)
        Else
            HelixView3D1.CameraController.AddZoomForce(0.5)
        End If
        zoomed_3d = Not zoomed_3d
    End Sub
    'EXIT 3D
    Private Sub Label1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Label1.MouseDown
        Grid3D.Visibility = Visibility.Hidden
        HelixView3D1.Camera.LookDirection = Nothing
        Dim vec As New Media3D.Vector3D
        vec.X = 1
        vec.Y = 1
        vec.Z = -1
        HelixView3D1.Camera.LookDirection = vec
        HelixView3D1.CameraController.Rotate(0, 0)
        Grid3D.Width = 10
        Grid3D.Height = 10
        HelixView3D1.Children.Clear()
    End Sub

    '========================================================================

    'EXIT SLIDES AAA
    Private Sub ExitSlides_Action()
        touch = False
        subitemview = False
        selected_subs = 0
        dx = 0
        dy = 0
        item_snd.Play()
        'TRANSP ANIM
        Dim anim0 As New DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5))
        BorderSlide.BeginAnimation(Border.OpacityProperty, anim0)
        BorderSlide.IsEnabled = False
        anim_timer.Start()
        VideoContent("", True)
    End Sub

    Dim subitemview As Boolean = False
    'SHOW SUBSLIDE VVV
    Private Sub SubslideButton_Action()
        Dim anim0 As New DoubleAnimation(dy / 2, 0, TimeSpan.FromSeconds(0.25))
        transf.BeginAnimation(TranslateTransform.YProperty, anim0)
        dx = 0
        dy = 0
        touch = False

        'HAVE ICON SUBITEM
        Dim subcontent_level_url(4) As String
        Dim subbase_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"
        Dim file_ext As String = ".jpg"
        If File.Exists(subbase_url + CStr(cur_img(1)) + "sub.png") Then file_ext = ".png"
        subcontent_level_url(1) = subbase_url + CStr(cur_img(1)) + "sub" + file_ext
        subcontent_level_url(2) = subbase_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "sub" + file_ext
        subcontent_level_url(3) = subbase_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(cur_img(3)) + "sub" + file_ext

        If File.Exists(subcontent_level_url(level)) Then
            VideoContent("", True)

            ImgSlide2.Source = Nothing
            If ImgSlide1.Margin.Equals(New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)) Then
                ImgSlide2.Margin = New Thickness(0)
            End If
            If ImgSlide1.Margin.Equals(New Thickness(0)) Then
                ImgSlide2.Margin = New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)
            End If

            If Not subitemview Then
                'SUB-ITEM NOT SHOWN
                ImgSlide3.Source = ImgSlide1.Source
                ImgSlide1.Source = New BitmapImage(New Uri(subcontent_level_url(level)))

                Dim ease As New BackEase With {.EasingMode = EasingMode.EaseOut, .Amplitude = 0.25}
                Dim transf2 = New TranslateTransform
                ImgSlide3.RenderTransform = transf2
                ImgSlide3.Margin = New Thickness(0, -GridSlide.ActualHeight, 0, GridSlide.ActualHeight)
                Dim anim2 As New DoubleAnimation(GridSlide.ActualHeight, 0, TimeSpan.FromSeconds(1))
                anim2.EasingFunction = ease
                transf2.BeginAnimation(TranslateTransform.YProperty, anim2)

                Dim transf1 = New TranslateTransform
                ImgSlide1.RenderTransform = transf1
                ImgSlide1.Margin = New Thickness(0)
                Dim anim1 As New DoubleAnimation(GridSlide.ActualHeight, 0, TimeSpan.FromSeconds(1))
                anim1.EasingFunction = ease
                transf1.BeginAnimation(TranslateTransform.YProperty, anim1)

                'CHANGE BTN PIC
                If File.Exists(data_root + gui_dir + "help_dn_sel.png") Then
                    SubslideButton.Source = New BitmapImage(New Uri(data_root + gui_dir + "help_dn_sel.png"))
                End If

                subitemview = True
                UpdateMultiLayerSlides("next")
                subitem_snd.Play()
                Exit Sub
            Else
                'SUB-ITEM SHOWN ALREADY
                Dim backcontent_level_url(4) As String
                Dim backbase_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"
                Dim backfile_ext As String = ".jpg"
                If File.Exists(backbase_url + CStr(cur_img(1)) + ".png") Then backfile_ext = ".png"
                backcontent_level_url(1) = backbase_url + CStr(cur_img(1)) + backfile_ext
                backcontent_level_url(2) = backbase_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + backfile_ext
                backcontent_level_url(3) = backbase_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(cur_img(3)) + backfile_ext

                ImgSlide3.Source = ImgSlide1.Source
                ImgSlide1.Source = New BitmapImage(New Uri(backcontent_level_url(level)))

                Dim ease As New BackEase With {.EasingMode = EasingMode.EaseOut, .Amplitude = 0.25}
                Dim transf2 = New TranslateTransform
                ImgSlide3.RenderTransform = transf2
                ImgSlide3.Margin = New Thickness(0, -GridSlide.ActualHeight, 0, GridSlide.ActualHeight)
                Dim anim2 As New DoubleAnimation(GridSlide.ActualHeight, 0, TimeSpan.FromSeconds(1))
                anim2.EasingFunction = ease
                transf2.BeginAnimation(TranslateTransform.YProperty, anim2)

                Dim transf1 = New TranslateTransform
                ImgSlide1.RenderTransform = transf1
                ImgSlide1.Margin = New Thickness(0)
                Dim anim1 As New DoubleAnimation(GridSlide.ActualHeight, 0, TimeSpan.FromSeconds(1))
                anim1.EasingFunction = ease
                transf1.BeginAnimation(TranslateTransform.YProperty, anim1)

                'CHANGE BACK BTN PIC
                SubslideButton.Source = New BitmapImage(New Uri(data_root + gui_dir + "help_dn.png"))

                Dim slides_ext As String = ".jpg" 'TMP! --- HAVE TO FIX FOR PNG
                VideoContent(level_url(level).Replace(slides_ext, ".avi"), False)

                subitemview = False
                UpdateMultiLayerSlides("next")
                subitem_snd.Play()
                Exit Sub
            End If
        End If

    End Sub

    Dim selected_subs As Integer = 0
    Dim have_subs As Boolean = False
    Dim subs_obj As Image
    Dim subs_count As Integer = 0

    Dim have_multisubs As Boolean = False
    Dim multisubs_count As Integer = 0
    Dim selected_multisub As Integer = -1

    'SHOW SUB-SLIDES
    Private Sub StPanSubs_MouseUp(ByVal sender As Object, ByVal e As RoutedEventArgs)
        'MULTI SUBS  - - - NEW  / data -> 1sub -> 2sub -> 1.jpg + 1btn.jpg + 1btn_sel.jpg, ...  // option: select.1st file
        If have_multisubs Then
            Dim src_obj As Image = TryCast(e.Source, Image)
            'Dim base_url As String = data_root + "" + CStr(selected_itm) + "sub\"
            If Not IsNothing(src_obj) Then
                For i = 0 To StPanSubs.Children.Count - 1
                    'SEL ITM
                    If src_obj.Equals(StPanSubs.Children(i)) Then
                        SelectSubSlide(i)
                    End If
                Next
            End If
        End If
    End Sub

    'SHOW SUB-SUB-SLIDES 
    Private Sub StPanSubs2_MouseUp(ByVal sender As Object, ByVal e As RoutedEventArgs)
        'MULTI SUB-SUBS  - - - NEW  / data -> 1sub -> 2sub ->  1sub -> 1.jpg + 1btn.jpg + 1btn_sel.jpg, ...  // option: select.1st file
        If have_multisubs Then '!!!
            Dim src_obj As Image = TryCast(e.Source, Image)
            If Not IsNothing(src_obj) Then
                For i = 0 To StPanSubs2.Children.Count - 1
                    'SEL ITM
                    If src_obj.Equals(StPanSubs2.Children(i)) Then
                        SelectSub2Slide(i)
                    End If
                Next
            End If
        End If
    End Sub


    Sub SelectSubSlide(i As Integer)

        If i > StPanSubs.Children.Count - 1 Then i = 0
        If i < 0 Then i = StPanSubs.Children.Count - 1

        'UNSEL ITM IMG
        For j = 0 To StPanSubs.Children.Count - 1
            Dim unsel_img As Image = TryCast(StPanSubs.Children(j), Image)
            If Not IsNothing(unsel_img) Then
                If File.Exists(multisubs_btn(j)) Then
                    unsel_img.Source = New BitmapImage(New Uri(multisubs_btn(j)))
                End If
            End If
        Next j

        Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"

        'SELECTED ITM
        selected_multisub = i + 1
        VideoContent("", True) 'clear vdo area

        'SEL ITM IMG
        If StPanSubs.Children.Count <> 0 Then
            Dim sel_img As Image = TryCast(StPanSubs.Children(i), Image)
            If Not IsNothing(sel_img) Then
                If File.Exists(multisubs_btn_sel(i)) Then
                    sel_img.Source = New BitmapImage(New Uri(multisubs_btn_sel(i)))
                End If
            End If
        End If

        'SHOW SUB SLIDE
        Dim subsub_ext As String = ".jpg"
        Dim subsub_path As String = base_url + CStr(cur_img(1)) + "sub" + "\"
        If File.Exists(subsub_path + "png.ext") Then subsub_ext = ".png"
        Dim multisubs_slide As String = subsub_path + CStr(i + 1) + subsub_ext

        If File.Exists(multisubs_slide) Then
            ImgSlide2.Source = Nothing
            If ImgSlide1.Margin.Equals(New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)) Then
                ImgSlide2.Margin = New Thickness(0)
            End If
            If ImgSlide1.Margin.Equals(New Thickness(0)) Then
                ImgSlide2.Margin = New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)
            End If

            'VDO
            VideoContent(multisubs_slide.Replace(subsub_ext, ".avi"), False)

            'SUBITEM NOT SHOWN
            ImgSlide3.Source = ImgSlide1.Source
            ImgSlide1.Source = New BitmapImage(New Uri(multisubs_slide))

            Dim ease As New BackEase With {.EasingMode = EasingMode.EaseInOut, .Amplitude = 0.25}
            Dim transf2 = New TranslateTransform
            ImgSlide3.RenderTransform = transf2
            ImgSlide3.Margin = New Thickness(0, -GridSlide.ActualHeight, 0, GridSlide.ActualHeight)
            Dim anim2 As New DoubleAnimation(GridSlide.ActualHeight, 0, TimeSpan.FromSeconds(1))
            anim2.EasingFunction = ease
            transf2.BeginAnimation(TranslateTransform.YProperty, anim2)

            Dim transf1 = New TranslateTransform
            ImgSlide1.RenderTransform = transf1
            ImgSlide1.Margin = New Thickness(0)
            Dim anim1 As New DoubleAnimation(GridSlide.ActualHeight, 0, TimeSpan.FromSeconds(1))
            anim1.EasingFunction = ease
            transf1.BeginAnimation(TranslateTransform.YProperty, anim1)

            subitem_snd.Play()

            'CHECK SUB-SUB-SLIDES
            UpdateSub2slideButtonVisibility()
        End If
    End Sub

    Sub SelectSub2Slide(i As Integer)
        'UNSEL ITM IMG - BTNS
        For j = 0 To StPanSubs2.Children.Count - 1
            Dim unsel_img As Image = TryCast(StPanSubs2.Children(j), Image)
            If Not IsNothing(unsel_img) Then
                If File.Exists(multisubs2_btn(j)) Then
                    unsel_img.Source = New BitmapImage(New Uri(multisubs2_btn(j)))
                End If
            End If
        Next j

        Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"

        'SELECTED ITM
        'selected_multisub = i + 1
        'VideoContent("", True) 'clear vdo area

        'SEL ITM IMG - BTNS
        Dim sel_img As Image = TryCast(StPanSubs2.Children(i), Image)
        If Not IsNothing(sel_img) Then
            If File.Exists(multisubs2_btn_sel(i)) Then
                sel_img.Source = New BitmapImage(New Uri(multisubs2_btn_sel(i)))
            End If
        End If

        'SHOW SUB SLIDE
        'Dim subsub_ext As String = ".jpg"
        'Dim subsub_path As String = base_url + CStr(cur_img(1)) + "sub" + "\"
        'If File.Exists(subsub_path + "png.ext") Then subsub_ext = ".png"
        'Dim multisubs_slide As String = subsub_path + CStr(i + 1) + subsub_ext

        If File.Exists(multisubs2_slide(i)) Then
            ImgSlide2.Source = Nothing
            If ImgSlide1.Margin.Equals(New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)) Then
                ImgSlide2.Margin = New Thickness(0)
            End If
            If ImgSlide1.Margin.Equals(New Thickness(0)) Then
                ImgSlide2.Margin = New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)
            End If

            'VDO
            'VideoContent(multisubs_slide.Replace(subsub_ext, ".avi"), False)

            'SUBITEM NOT SHOWN
            ImgSlide3.Source = ImgSlide1.Source
            ImgSlide1.Source = New BitmapImage(New Uri(multisubs2_slide(i)))

            Dim ease As New BackEase With {.EasingMode = EasingMode.EaseInOut, .Amplitude = 0.25}
            Dim transf2 = New TranslateTransform
            ImgSlide3.RenderTransform = transf2
            ImgSlide3.Margin = New Thickness(0, -GridSlide.ActualHeight, 0, GridSlide.ActualHeight)
            Dim anim2 As New DoubleAnimation(GridSlide.ActualHeight, 0, TimeSpan.FromSeconds(1))
            anim2.EasingFunction = ease
            transf2.BeginAnimation(TranslateTransform.YProperty, anim2)

            Dim transf1 = New TranslateTransform
            ImgSlide1.RenderTransform = transf1
            ImgSlide1.Margin = New Thickness(0)
            Dim anim1 As New DoubleAnimation(GridSlide.ActualHeight, 0, TimeSpan.FromSeconds(1))
            anim1.EasingFunction = ease
            transf1.BeginAnimation(TranslateTransform.YProperty, anim1)

            subitem_snd.Play()
        End If
    End Sub

    Dim subslide_panel_visible As Boolean = False
    Dim multisubs_btn() As String
    Dim multisubs_btn_sel() As String

    'SUB-SLIDES
    Private Sub UpdateSubslideButtonVisibility()
        Dim btn_transf = New TranslateTransform With {.Y = SubslideButton.ActualHeight * 2}
        SubslideButton.RenderTransform = btn_transf
        Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"

        'MULTI SUBS - - - NEW
        have_multisubs = False
        StPanSubs.Visibility = Visibility.Hidden
        StPanSubs.Children.Clear()
        StPanSubs.RenderTransform = Nothing
        Dim multisubs_slide() As String
        multisubs_count = 0

        Dim subsub_ext As String = ".jpg"
        Dim subsub_btn_ext As String = ".jpg"
        Dim subsub_path As String = base_url + CStr(cur_img(1)) + "sub" + "\"
        If File.Exists(subsub_path + "png.ext") Then subsub_ext = ".png"

        For j = 1 To 99
            If File.Exists(subsub_path + CStr(j) + subsub_ext) Then
                have_multisubs = True
                multisubs_count += 1

                ReDim Preserve multisubs_slide(j - 1)
                multisubs_slide(j - 1) = subsub_path + CStr(j) + subsub_ext
                ReDim Preserve multisubs_btn(j - 1)
                multisubs_btn(j - 1) = subsub_path + CStr(j) + "btn" + subsub_btn_ext

                'If File.Exists(subsub_path + "_sel" + subsub_ext) Then
                ReDim Preserve multisubs_btn_sel(j - 1)
                multisubs_btn_sel(j - 1) = subsub_path + CStr(j) + "btn" + "_sel" + subsub_btn_ext
                'End If

                If File.Exists(multisubs_btn(j - 1)) Then
                    Dim sub_img As New Image With {.Source = New BitmapImage(New Uri(multisubs_btn(j - 1))), .Stretch = Stretch.None}
                    StPanSubs.Children.Add(sub_img)
                End If
                StPanSubs.UpdateLayout()

                StPanSubs.Visibility = Visibility.Visible
                Dim transf_subs_panel = New TranslateTransform(0, StPanSubs.ActualHeight * 2)
                StPanSubs.RenderTransform = transf_subs_panel
                Dim anim_subs_panel As New DoubleAnimation(StPanSubs.ActualHeight, 0, TimeSpan.FromSeconds(0.5))
                anim_subs_panel.BeginTime = TimeSpan.FromSeconds(0.5)
                transf_subs_panel.BeginAnimation(TranslateTransform.YProperty, anim_subs_panel)

                subslide_panel_visible = True
                'selected_multisub = j - 1
            End If
        Next j
        selected_multisub = -1

        'SELECT 1ST SUB-SLIDE IF SELECT.1ST FILE EXISTS
        If File.Exists(subsub_path + "select.1st") Then
            subslide_select_1st_timer.Interval = TimeSpan.FromSeconds(0.75)
            subslide_select_1st_timer.Start()
        End If

        'PNG FS.SLIDES BG
        SlidesBg.Source = Nothing
        If File.Exists(data_root + "interface\" + "slide_bg.png") Then
            BorderSlide.Background = Nothing
            SlidesBg.Source = New BitmapImage(New Uri(data_root + "interface\" + "slide_bg.png"))
        End If
        If File.Exists(base_url + "slide_bg.png") Then
            BorderSlide.Background = Nothing
            SlidesBg.Source = New BitmapImage(New Uri(base_url + "slide_bg.png"))
        End If
        If File.Exists(subsub_path + "slide_bg.png") Then
            BorderSlide.Background = Nothing
            SlidesBg.Source = New BitmapImage(New Uri(subsub_path + "slide_bg.png"))
        End If

        'PNG FS.SLIDES FG
        SlidesFg.Source = Nothing
        If File.Exists(data_root + "interface\" + "slide_fg.png") Then
            SlidesFg.Source = New BitmapImage(New Uri(data_root + "interface\" + "slide_fg.png"))
        End If
        If File.Exists(base_url + "slide_fg.png") Then
            SlidesFg.Source = New BitmapImage(New Uri(base_url + "slide_fg.png"))
        End If
        If File.Exists(subsub_path + "slide_fg.png") Then
            SlidesFg.Source = New BitmapImage(New Uri(subsub_path + "slide_fg.png"))
        End If

        UpdateSub2slideButtonVisibility()
    End Sub

    'Dim subslide2_panel_visible As Boolean = False
    Dim multisubs2_slide() As String
    Dim multisubs2_btn() As String
    Dim multisubs2_btn_sel() As String

    'SUB-SUB-SLIDES
    Private Sub UpdateSub2slideButtonVisibility()
        'Dim btn_transf = New TranslateTransform With {.Y = SubslideButton.ActualHeight * 2}
        'SubslideButton.RenderTransform = btn_transf

        'MULTI SUBS - - - NEW
        'have_multisubs = False
        StPanSubs2.Visibility = Visibility.Hidden
        StPanSubs2.Children.Clear()
        StPanSubs2.RenderTransform = Nothing
        'Dim multisubs_slide() As String
        'multisubs_count = 0

        Dim slide_ext As String = ".jpg"
        Dim btn_ext As String = ".jpg"
        Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"   'data\1sub\
        Dim subsub_path As String = base_url + CStr(cur_img(1)) + "sub" + "\"   'data\1sub\2sub\
        Dim subsubsub_path As String = subsub_path + CStr(selected_multisub) + "sub" + "\"   'data\1sub\2sub\3sub\
        If File.Exists(subsubsub_path + "png.ext") Then slide_ext = ".png"

        For j = 1 To 9
            If File.Exists(subsubsub_path + CStr(j) + slide_ext) Then 'data\1sub\2sub\3sub\1.jpg 
                'have_multisubs = True
                'multisubs_count += 1

                ReDim Preserve multisubs2_slide(j - 1)
                multisubs2_slide(j - 1) = subsubsub_path + CStr(j) + slide_ext

                ReDim Preserve multisubs2_btn(j - 1)
                multisubs2_btn(j - 1) = subsubsub_path + CStr(j) + "btn" + btn_ext

                'If File.Exists(subsub_path + "_sel" + subsub_ext) Then
                ReDim Preserve multisubs2_btn_sel(j - 1)
                multisubs2_btn_sel(j - 1) = subsubsub_path + CStr(j) + "btn" + "_sel" + btn_ext
                'End If

                StPanSubs2.Visibility = Visibility.Visible

                If File.Exists(subsubsub_path + CStr(j) + "btn" + btn_ext) Then 'multisubs_btn(j - 1)
                    Dim btn_img As New Image With {.Source = New BitmapImage(New Uri(subsubsub_path + CStr(j) + "btn" + btn_ext)), .Stretch = Stretch.None, .Opacity = 0}
                    StPanSubs2.Children.Add(btn_img)
                    btn_img.BeginAnimation(Image.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25)) With {.BeginTime = TimeSpan.FromSeconds(0.5 + 0.25 * j)})
                End If
                StPanSubs2.UpdateLayout()

                'Dim transf_subs_panel = New TranslateTransform(StPanSubs2.ActualWidth * 2.5, 0)
                'StPanSubs2.RenderTransform = transf_subs_panel
                'Dim anim_subs_panel As New DoubleAnimation(StPanSubs2.ActualWidth * 2.5, 0, TimeSpan.FromSeconds(0.5)) With
                '    {.BeginTime = TimeSpan.FromSeconds(0.75), .EasingFunction = New CubicEase}
                'transf_subs_panel.BeginAnimation(TranslateTransform.XProperty, anim_subs_panel)
            End If
        Next j

        'selected_multisub = -1

        'SELECT 1ST SUB-SLIDE IF SELECT.1ST FILE EXISTS
        'If File.Exists(subsub_path + "select.1st") Then
        '    subslide_select_1st_timer.Interval = TimeSpan.FromSeconds(0.75)
        '    subslide_select_1st_timer.Start()
        'End If

        'PNG FS.SLIDES BG
        'SlidesBg.Source = Nothing
        'If File.Exists(data_root + "interface\" + "slide_bg.png") Then
        '    BorderSlide.Background = Nothing
        '    SlidesBg.Source = New BitmapImage(New Uri(data_root + "interface\" + "slide_bg.png"))
        'End If
        'If File.Exists(base_url + "slide_bg.png") Then
        '    BorderSlide.Background = Nothing
        '    SlidesBg.Source = New BitmapImage(New Uri(base_url + "slide_bg.png"))
        'End If
        'If File.Exists(subsub_path + "slide_bg.png") Then
        '    BorderSlide.Background = Nothing
        '    SlidesBg.Source = New BitmapImage(New Uri(subsub_path + "slide_bg.png"))
        'End If

        'PNG FS.SLIDES FG
        'SlidesFg.Source = Nothing
        'If File.Exists(data_root + "interface\" + "slide_fg.png") Then
        '    SlidesFg.Source = New BitmapImage(New Uri(data_root + "interface\" + "slide_fg.png"))
        'End If
        'If File.Exists(base_url + "slide_fg.png") Then
        '    SlidesFg.Source = New BitmapImage(New Uri(base_url + "slide_fg.png"))
        'End If
        'If File.Exists(subsub_path + "slide_fg.png") Then
        '    SlidesFg.Source = New BitmapImage(New Uri(subsub_path + "slide_fg.png"))
        'End If
    End Sub

    Sub subslide_select_1st_timer_tick()
        SelectSubSlide(0)
        subslide_select_1st_timer.Stop()
    End Sub

    Dim through_subs_nav As Boolean = False 'TMP - 2 FIX

    'PREV SLIDE <<<
    Private Sub PrevItem_Action()
        previtem_action_hocked = True

        'NAVIGATION THROUGH SUB SLIDES
        If through_subs_nav Then
            If have_multisubs And selected_multisub <> -1 Then
                If selected_multisub <> 0 Then
                    SelectSubSlide(selected_multisub - 1)
                    Exit Sub
                End If
            End If
        End If

        Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"
        Dim content_level_url(4) As String

        If cur_img(level) <= 1 Then cur_img(level) = img_count(level) + 1
        cur_img(level) -= 1
        touch = False
        subitemview = False
        dx = 0
        dy = 0
        selected_subs = 0
        UpdateSubslideButtonVisibility()

        Dim file_ext As String = ".jpg"
        If File.Exists(base_url + CStr(cur_img(1)) + ".png") Then file_ext = ".png"

        content_level_url(1) = base_url + CStr(cur_img(1)) + file_ext
        content_level_url(2) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + file_ext
        content_level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(cur_img(3)) + file_ext

        VideoContent(content_level_url(level).Replace(file_ext, ".avi"), False)

        ImgSlide2.Source = ImgSlide1.Source
        If File.Exists(content_level_url(level)) Then ImgSlide1.Source = New BitmapImage(New Uri(content_level_url(level)))

        UpdateMultiLayerSlides("prev")

        'MAIN SLIDE ANIMATION
        Dim ease As New BackEase With {.EasingMode = EasingMode.EaseInOut, .Amplitude = 0.25}

        Dim transf2 = New TranslateTransform
        ImgSlide2.RenderTransform = transf2
        ImgSlide2.Margin = New Thickness(0)
        Dim anim2 As New DoubleAnimation(0, GridSlide.ActualWidth, TimeSpan.FromSeconds(1)) With {.EasingFunction = ease}
        transf2.BeginAnimation(TranslateTransform.XProperty, anim2)

        Dim transf1 = New TranslateTransform
        ImgSlide1.RenderTransform = transf1
        ImgSlide1.Margin = New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)
        Dim anim1 As New DoubleAnimation(0, GridSlide.ActualWidth, TimeSpan.FromSeconds(1)) With {.EasingFunction = ease}
        transf1.BeginAnimation(TranslateTransform.XProperty, anim1)

        UpdateNavigationVisual()

        slide_snd.Play()
    End Sub

    'NEXT SLIDE >>>
    Private Sub NextItem_Action()
        nextitem_action_hocked = True

        'NAVIGATION THROUGH SUB SLIDES
        If through_subs_nav Then
            If have_multisubs And selected_multisub <> -1 Then
                If selected_multisub <> multisubs_count - 1 Then
                    SelectSubSlide(selected_multisub + 1)
                    UpdateNavigationVisual()
                    Exit Sub
                End If
            End If
        End If

        Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"
        Dim content_level_url(4) As String

        If cur_img(level) >= img_count(level) Then cur_img(level) = 0
        cur_img(level) += 1
        touch = False
        subitemview = False
        dx = 0
        dy = 0

        selected_subs = 0
        UpdateSubslideButtonVisibility()

        Dim file_ext As String = ".jpg"
        If File.Exists(base_url + CStr(cur_img(1)) + ".png") Then file_ext = ".png"

        content_level_url(1) = base_url + CStr(cur_img(1)) + file_ext
        content_level_url(2) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + file_ext
        content_level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\" + CStr(cur_img(3)) + file_ext

        VideoContent(content_level_url(level).Replace(file_ext, ".avi"), False)

        ImgSlide2.Source = ImgSlide1.Source
        If File.Exists(content_level_url(level)) Then ImgSlide1.Source = New BitmapImage(New Uri(content_level_url(level)))

        UpdateMultiLayerSlides("next")

        'MAIN SLIDE ANIMATION
        Dim ease As New BackEase With {.EasingMode = EasingMode.EaseInOut, .Amplitude = 0.25}
        Dim transf2 = New TranslateTransform
        ImgSlide2.RenderTransform = transf2
        ImgSlide2.Margin = New Thickness(-GridSlide.ActualWidth, 0, GridSlide.ActualWidth, 0)
        Dim anim2 As New DoubleAnimation(GridSlide.ActualWidth, 0, TimeSpan.FromSeconds(1)) With {.EasingFunction = ease}
        transf2.BeginAnimation(TranslateTransform.XProperty, anim2)

        Dim transf1 = New TranslateTransform
        ImgSlide1.RenderTransform = transf1
        ImgSlide1.Margin = New Thickness(0)
        Dim anim1 As New DoubleAnimation(GridSlide.ActualWidth, 0, TimeSpan.FromSeconds(1)) With {.EasingFunction = ease}
        transf1.BeginAnimation(TranslateTransform.XProperty, anim1)

        UpdateNavigationVisual()

        slide_snd.Play()
    End Sub

    'MULTI-LAYER CONTENT
    Private Sub UpdateMultiLayerSlides(ByVal direction As String)
        Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"
        Dim layer2_url = base_url + CStr(cur_img(1)) + "L2.png"
        Dim layer3_url = base_url + CStr(cur_img(1)) + "L3.png"
        ImgSlide_L2.Source = Nothing
        ImgSlide_L3.Source = Nothing

        Dim x_value As Integer
        If direction = "next" Then x_value = 10
        If direction = "prev" Then x_value = -10

        'LAYER 2 ANIM
        If File.Exists(layer2_url) And Not subitemview Then
            ImgSlide_L2.Source = New BitmapImage(New Uri(layer2_url))

            ImgSlide_L2.Opacity = 0
            ImgSlide_L2.BeginAnimation(Image.OpacityProperty, Nothing)
            ImgSlide_L2.BeginAnimation(Image.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5)) _
                                       With {.BeginTime = TimeSpan.FromSeconds(1)})
            Dim imgslide_l2_transf As New TranslateTransform With {.X = x_value}
            ImgSlide_L2.RenderTransform = imgslide_l2_transf
            imgslide_l2_transf.BeginAnimation(TranslateTransform.XProperty, New DoubleAnimation(imgslide_l2_transf.X, 0, TimeSpan.FromSeconds(0.5)) _
                                              With {.BeginTime = TimeSpan.FromSeconds(1)})
        End If

        'LAYER 3 ANIM
        If File.Exists(layer3_url) And Not subitemview Then
            ImgSlide_L3.Source = New BitmapImage(New Uri(layer3_url))

            ImgSlide_L3.Opacity = 0
            ImgSlide_L3.BeginAnimation(Image.OpacityProperty, Nothing)
            ImgSlide_L3.BeginAnimation(Image.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5)) _
                                       With {.BeginTime = TimeSpan.FromSeconds(1.25)})
            Dim imgslide_l3_transf As New TranslateTransform With {.X = x_value}
            ImgSlide_L3.RenderTransform = imgslide_l3_transf
            imgslide_l3_transf.BeginAnimation(TranslateTransform.XProperty, New DoubleAnimation(imgslide_l3_transf.X, 0, TimeSpan.FromSeconds(0.5)) _
                                              With {.BeginTime = TimeSpan.FromSeconds(1.25)})
        End If

    End Sub

    Dim nextbtn_hidden As Boolean = False
    Dim prevbtn_hidden As Boolean = False

    'NEXT/PREV PIC HIDE
    Private Sub UpdateNavigationVisual()

        Dim nextbtn_transf As New TranslateTransform With {.X = 0}
        Dim prevbtn_transf As New TranslateTransform With {.X = 0}
        NextButton.RenderTransform = nextbtn_transf
        PrevButton.RenderTransform = prevbtn_transf

        If nextbtn_hidden Then
            nextbtn_transf.BeginAnimation(TranslateTransform.XProperty,
                                          New DoubleAnimation(2 * NextButton.ActualWidth, 0, TimeSpan.FromSeconds(0.5)))
            nextbtn_hidden = False
        End If
        If prevbtn_hidden Then
            prevbtn_transf.BeginAnimation(TranslateTransform.XProperty,
                                          New DoubleAnimation(-PrevButton.ActualWidth, 0, TimeSpan.FromSeconds(0.5)))
            prevbtn_hidden = False
        End If
        If img_count(level) = 1 Then
            nextbtn_transf.X = 2 * NextButton.ActualWidth
            prevbtn_transf.X = -PrevButton.ActualWidth
        End If
        If cur_img(level) = img_count(level) Then
            nextbtn_transf.BeginAnimation(TranslateTransform.XProperty,
                                          New DoubleAnimation(0, 2 * NextButton.ActualWidth, TimeSpan.FromSeconds(0.5)))
            nextbtn_hidden = True
        End If
        If cur_img(level) = 1 Then
            prevbtn_transf.BeginAnimation(TranslateTransform.XProperty,
                                          New DoubleAnimation(0, -PrevButton.ActualWidth, TimeSpan.FromSeconds(0.5)))
            prevbtn_hidden = True
        End If

        'NAVIGATION THROUGH SUB SLIDES
        'If through_subs_nav Then
        '    If have_multisubs And selected_multisub <> -1 Then
        '        If selected_multisub <= multisubs_count And selected_multisub >= 0 Then
        '            nextbtn_transf.BeginAnimation(TranslateTransform.XProperty,
        '                                  New DoubleAnimation(0, 2 * NextButton.ActualWidth, TimeSpan.FromSeconds(0.5)))
        '            nextbtn_transf.BeginAnimation(TranslateTransform.XProperty,
        '                                  New DoubleAnimation(2 * NextButton.ActualWidth, 0, TimeSpan.FromSeconds(0.5)))
        '            nextbtn_hidden = False
        '            prevbtn_transf.BeginAnimation(TranslateTransform.XProperty,
        '                      New DoubleAnimation(-PrevButton.ActualWidth, 0, TimeSpan.FromSeconds(0.5)))
        '            prevbtn_hidden = False
        '        End If
        '    End If
        'End If

        'SLIDE DOTS
        StackPanelSlideDots.Children.Clear()
        For i = 1 To 99
            Dim dot_img As New Image
            dot_img.Width = 8
            dot_img.Height = 8
            Dim content_level_url(4) As String
            Dim base_url As String = data_root + "" + CStr(selected_mainmenuitm) + "sub\"
            content_level_url(1) = base_url
            content_level_url(2) = base_url + CStr(cur_img(1)) + "\"
            content_level_url(3) = base_url + CStr(cur_img(1)) + "\" + CStr(cur_img(2)) + "\"
            If File.Exists(content_level_url(level) + CStr(i) + ".jpg") Then
                If File.Exists(data_root + "dot.png") Then dot_img.Source = New BitmapImage(New Uri(data_root + "dot.png"))
                If i = cur_img(level) Then If File.Exists(data_root + "dot_sel.png") Then dot_img.Source = New BitmapImage(New Uri(data_root + "dot_sel.png"))
                If Not IsNothing(dot_img.Source) Then StackPanelSlideDots.Children.Add(dot_img)
            End If
        Next i
    End Sub

    'ZOOMING
    Dim z_source As String = ""
    Dim zoomed As Boolean = False
    Dim z_x, z_y As Double
    Dim click As Integer = 0
    Dim z_factor As Double
    'Private Sub ImgSlide1_MouseRightButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ImgSlide1.MouseRightButtonDown
    '    If Not multitouch_sw Then
    '        ImgSlide1.Margin = New Thickness(0)
    '        click += 1
    '        Dim zoom As New ScaleTransform
    '        ImgSlide1.RenderTransform = zoom
    '        ImgSlide1.RenderTransformOrigin = Nothing
    '        If click = 1 Then
    '            z_factor = 1.5
    '            zoom.ScaleX = 1
    '            zoom.ScaleY = 1
    '            zoom.CenterX = e.GetPosition(ImgSlide1).X
    '            zoom.CenterY = e.GetPosition(ImgSlide1).Y
    '            z_x = zoom.CenterX
    '            z_y = zoom.CenterY
    '            Dim z_anim As New DoubleAnimation(1, z_factor, TimeSpan.FromSeconds(0.5))
    '            zoom.BeginAnimation(ScaleTransform.ScaleXProperty, z_anim)
    '            zoom.BeginAnimation(ScaleTransform.ScaleYProperty, z_anim)
    '            z_source = ImgSlide1.Source.ToString
    '            zoomed = True
    '        End If
    '        If click = 2 Then
    '            z_factor = 2.5
    '            zoom.ScaleX = 1.5
    '            zoom.ScaleY = 1.5
    '            Dim x_anim As New DoubleAnimation(z_x, e.GetPosition(ImgSlide1).X, TimeSpan.FromSeconds(0.5))
    '            Dim y_anim As New DoubleAnimation(z_y, e.GetPosition(ImgSlide1).Y, TimeSpan.FromSeconds(0.5))
    '            zoom.BeginAnimation(ScaleTransform.CenterXProperty, x_anim)
    '            zoom.BeginAnimation(ScaleTransform.CenterYProperty, y_anim)
    '            zoom.CenterX = e.GetPosition(ImgSlide1).X
    '            zoom.CenterY = e.GetPosition(ImgSlide1).Y
    '            z_x = zoom.CenterX
    '            z_y = zoom.CenterY
    '            Dim z_anim1 As New DoubleAnimation(1.5, z_factor, TimeSpan.FromSeconds(0.5))
    '            z_anim1.BeginTime = TimeSpan.FromSeconds(0.5)
    '            zoom.BeginAnimation(ScaleTransform.ScaleXProperty, z_anim1)
    '            zoom.BeginAnimation(ScaleTransform.ScaleYProperty, z_anim1)
    '            z_source = ImgSlide1.Source.ToString
    '            zoomed = True
    '        End If
    '        If click = 3 Then
    '            z_factor = 1
    '            zoom.CenterX = z_x
    '            zoom.CenterY = z_y
    '            Dim z_anim As New DoubleAnimation(2.5, z_factor, TimeSpan.FromSeconds(0.5))
    '            zoom.BeginAnimation(ScaleTransform.ScaleXProperty, z_anim)
    '            zoom.BeginAnimation(ScaleTransform.ScaleYProperty, z_anim)
    '            z_source = ImgSlide1.Source.ToString
    '            zoomed = False
    '            click = 0
    '        End If
    '    End If
    'End Sub
    'Private Sub ImgSlide1_LayoutUpdated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImgSlide1.LayoutUpdated
    '    If Not IsNothing(ImgSlide1.Source) Then
    '        If ImgSlide1.Source.ToString <> z_source And zoomed Then
    '            z_source = ""
    '            zoomed = False
    '            'Dim zoom As New ScaleTransform
    '            'zoom.CenterX = z_x
    '            'zoom.CenterY = z_y
    '            'ImgSlide1.RenderTransform = zoom
    '            'zoom.ScaleX = z_factor
    '            'zoom.ScaleY = z_factor
    '            'Dim z_anim As New DoubleAnimation(z_factor, 1, TimeSpan.FromSeconds(0.5))
    '            'zoom.BeginAnimation(ScaleTransform.ScaleXProperty, z_anim)
    '            'zoom.BeginAnimation(ScaleTransform.ScaleYProperty, z_anim)
    '            click = 0
    '        End If
    '    End If
    'End Sub

    'SND MUTE
    Dim mute As Boolean = False
    Private Sub ImageSound_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ImageSound.MouseUp
        If Not mute Then
            MediaElementTrack.Pause()
            If File.Exists(data_root + "interface\" + "play.png") Then
                ImageSound.Source = New BitmapImage(New Uri(data_root + "interface\" + "play.png"))
                'Dim mask As New ImageBrush()
                'mask.ImageSource = New BitmapImage(New Uri(data_dir + "interface\" + "play.png"))
                'mask.Stretch = Stretch.Fill
                'mask.Viewport = New Rect(0, 0, ImageSound.Width, ImageSound.Height)
                'mask.ViewportUnits = BrushMappingMode.Absolute
                'ImageSound.OpacityMask = mask
            End If
        Else
            MediaElementTrack.Play()
            If File.Exists(data_root + "interface\" + "mute.png") Then
                ImageSound.Source = New BitmapImage(New Uri(data_root + "interface\" + "mute.png"))
                'Dim mask As New ImageBrush()
                'mask.ImageSource = New BitmapImage(New Uri(data_dir + "interface\" + "mute.png"))
                'mask.Stretch = Stretch.Fill
                'mask.Viewport = New Rect(0, 0, ImageSound.Width, ImageSound.Height)
                'mask.ViewportUnits = BrushMappingMode.Absolute
                'ImageSound.OpacityMask = mask
            End If
        End If
        mute = Not mute
        deny_snd.Play()
    End Sub

    'VDO BG
    Dim hide_vbg As Boolean = False
    Private Sub ImageVBG_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ImageVBG.MouseUp
        If Not hide_vbg Then
            MediaUriElementBg.Source = Nothing
            MediaUriElementBg.Visibility = Visibility.Hidden
            MediaUriElementBg.Stop()
            If File.Exists(data_root + "interface\" + "show.png") Then
                ImageVBG.Source = New BitmapImage(New Uri(data_root + "interface\" + "show.png"))
                'Dim mask As New ImageBrush()
                'mask.ImageSource = New BitmapImage(New Uri(data_dir + "interface\" + "show.png"))
                'mask.Stretch = Stretch.Fill
                'mask.Viewport = New Rect(0, 0, ImageSound.Width, ImageSound.Height)
                'mask.ViewportUnits = BrushMappingMode.Absolute
                'ImageVBG.OpacityMask = mask
            End If
        Else
            MediaUriElementBg.Source = New Uri(data_root + "bg.avi")
            MediaUriElementBg.Visibility = Visibility.Visible
            MediaUriElementBg.Play()

            If File.Exists(data_root + "interface\" + "hide.png") Then
                ImageVBG.Source = New BitmapImage(New Uri(data_root + "interface\" + "hide.png"))
                'Dim mask As New ImageBrush()
                'mask.ImageSource = New BitmapImage(New Uri(data_dir + "interface\" + "hide.png"))
                'mask.Stretch = Stretch.Fill
                'mask.Viewport = New Rect(0, 0, ImageSound.Width, ImageSound.Height)
                'mask.ViewportUnits = BrushMappingMode.Absolute
                'ImageVBG.OpacityMask = mask
            End If
        End If
        hide_vbg = Not hide_vbg
        deny_snd.Play()
    End Sub

    'TOUCH EXIT
    Dim exit_click_loc As Integer = 0
    Dim exit_click_x As Double = 0
    Dim exit_click_y As Double = 0
    Private Sub ImgLogo1_MouseUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs) Handles ImgMainMenuLogo.MouseUp
        If Not IsNothing(e) Then
            Dim cur_x As Double = e.GetPosition(Me).X
            Dim cur_y As Double = e.GetPosition(Me).Y
            If cur_x <> 0 And cur_y <> 0 Then
                If Math.Abs(cur_x - exit_click_x) < 25 Then
                    If Math.Abs(cur_y - exit_click_y) < 25 Then
                        exit_click_loc += 1
                    Else
                        exit_click_loc = 0
                    End If
                Else
                    exit_click_loc = 0
                End If
                exit_click_x = e.GetPosition(Me).X
                exit_click_y = e.GetPosition(Me).Y
                If exit_click_loc = 5 Then

                    GridHiddenMenu.Visibility = Visibility.Visible
                    Dim menu_trans As New TranslateTransform With {.X = -25}
                    GridHiddenMenu.RenderTransform = menu_trans
                    menu_trans.BeginAnimation(TranslateTransform.XProperty, New DoubleAnimation(menu_trans.X, 0, TimeSpan.FromSeconds(0.25)))
                    GridHiddenMenu.BeginAnimation(Grid.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5)))

                    exit_click_loc = 0
                End If
            End If
        End If
    End Sub

    'NOTES
    'Dim inkDA As New DrawingAttributes
    Dim note_sw As Boolean = False
    Declare Function keyboardevent Lib "user32" Alias "keybd_event" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer) As Integer
    Const KeyEventF_Keydown As Integer = &H0 ' Press key		

    Const KeyEventF_Keyup As Integer = &H2 ' Release key		
    Const VK_Snapshot As Integer = &H2C

    Private Sub ImgSlideNote_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        pic_snd.Play()
        note_sw = Not note_sw
        If note_sw Then
            ShowHelp("help_note", True, 1)
            multitouch_sw = False
            InkCanvasSlide.Visibility = Visibility.Visible
            InkCanvasSlide.Background = Nothing

            InkToolsPanel.Visibility = Visibility.Visible
            InkToolsPanel.ICanvas = InkCanvasSlide

            'inkDA.Color = Colors.Black
            'inkDA.Height = 3
            'inkDA.Width = 7
            'inkDA.FitToCurve = True
            'InkCanvasSlide.DefaultDrawingAttributes = inkDA
            'InkCanvasSlide.EditingMode = InkCanvasEditingMode.Ink
            ImgSlideNote.Opacity = 1
            SlideHideNavigation(True)
        Else

            InkToolsPanel.Visibility = Visibility.Hidden
            InkToolsPanel.ICanvas = Nothing

            ShowHelp("help_note", False)
            If ImgSlideMultiTouch.Opacity = 1 Then multitouch_sw = True
            InkCanvasSlide.Visibility = Visibility.Hidden
            InkCanvasSlide.Strokes.Clear()
            ImgSlideNote.Opacity = 0.65
            SlideHideNavigation(False)
        End If
    End Sub

    Private Sub ImgSlideEmail_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        pic_snd.Play()

        'SAVE IMG
        Dim rtb = New RenderTargetBitmap(InkCanvasSlide.ActualWidth + 0, InkCanvasSlide.ActualHeight + 0, 96D, 96D, PixelFormats.Default)
        BrdSlideExtras.Visibility = Visibility.Hidden
        rtb.Render(BorderSlide)
        BrdSlideExtras.Visibility = Visibility.Visible
        Dim encoder = New JpegBitmapEncoder
        encoder.Frames.Add(BitmapFrame.Create(rtb))
        Dim fs_path As String = ""
        Try
            fs_path = app_root + "note_" + DateTime.Now.ToString("HHMMss") + "_" + DateTime.Now.ToString("ddMMyyyy") + ".jpg"
            Dim fs = File.Open(fs_path, FileMode.Create)
            encoder.Save(fs)
            fs.Close()
        Catch ex As Exception
        End Try

        If IsNothing(GridSlide.FindName("EmailForm")) Then
            Dim NewEmailForm As New EmailForm(fs_path) With {.Name = "EmailForm"}
            GridSlide.Children.Add(NewEmailForm)
            GridSlide.RegisterName(NewEmailForm.Name, NewEmailForm)
        Else
            Dim NewEmailForm As EmailForm = GridSlide.FindName("EmailForm")
            NewEmailForm.ContentPath = fs_path
            NewEmailForm.Visibility = Visibility.Visible
        End If

        'If File.Exists(System.AppDomain.CurrentDomain.BaseDirectory() + "email.ini") Then
        '    Dim objIniFile As New IniFile(System.AppDomain.CurrentDomain.BaseDirectory() + "email.ini")
        '    Dim host As String = objIniFile.GetString("email", "host", "mail.issimple.co")
        '    Dim port As Integer = objIniFile.GetString("email", "port", 2626)
        '    Dim from_email As String = objIniFile.GetString("email", "from_email", "data@issimple.co")
        '    Dim from_pass As String = objIniFile.GetString("email", "from_pass", "issimple1data")
        '    Dim from_name As String = objIniFile.GetString("email", "from_name", "iSSimple iNFO")
        '    Dim subj As String = objIniFile.GetString("email", "subj", "Presentation Shared Content")
        '    Dim body As String = objIniFile.GetString("email", "body", "Hello, here is slide")

        '    'TOUCH KEYB
        '    Dim prc As Process
        '    Try
        '        prc = Process.Start("C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe")
        '    Catch ex As Exception
        '    End Try

        '    'INPUT FIELD
        '    Dim email_inp As String = ""
        '    email_inp = InputBox("Please enter receiver email address:")

        '    If email_inp <> "" Then

        '        bgworker.RunWorkerAsync()
        '        'EMAIL INIT
        '        Dim msg As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage
        '        msg.To.Add(email_inp)
        '        msg.From = New MailAddress(from_email, from_name, System.Text.Encoding.UTF8)
        '        msg.Subject = subj
        '        msg.Body = body
        '        msg.SubjectEncoding = System.Text.Encoding.UTF8
        '        msg.BodyEncoding = System.Text.Encoding.UTF8
        '        msg.IsBodyHtml = False
        '        msg.Priority = MailPriority.Normal
        '        Dim client As SmtpClient = New SmtpClient
        '        client.Credentials = New System.Net.NetworkCredential(from_email, from_pass)
        '        client.Port = port
        '        client.Host = host
        '        client.EnableSsl = False
        '        Dim data As Attachment = New Attachment(fs_path)
        '        msg.Attachments.Add(data)
        '        Try
        '            client.Send(msg)

        '            'CLOSE KEYB
        '            If Not IsNothing(prc) Then
        '                Try
        '                    prc.CloseMainWindow()
        '                    prc.Close()
        '                Catch ex As Exception
        '                End Try
        '            End If

        '            MsgBox("OK")
        '        Catch ex As Exception
        '            deny_snd.Play()
        '            MsgBox("ERROR")
        '        End Try
        '        'bgworker.CancelAsync()
        '    End If
        'End If

    End Sub


    'SH EFF -- JUST FOR FUN
    Dim sheff0 As New ShaderEffectLibrary.MagnifyEffect
    Dim sheff1 As New ShaderEffectLibrary.SmoothMagnifyEffect
    Dim sheff2 As New ShaderEffectLibrary.RippleEffect
    Dim sheff4 As New ShaderEffectLibrary.BrightExtractEffect
    Dim sheff5 As New ShaderEffectLibrary.PixelateEffect
    Dim sheff6 As New ShaderEffectLibrary.ZoomBlurEffect
    Dim sheff_active As Boolean = False
    Private Sub SlideShowBS_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles SlideShowBg.MouseDown
        If Not sheff_active And bg_toucheffect = 1 Then
            SlideShowBg.Effect = sheff2
            sheff2.Center = New Point(Mouse.GetPosition(Window1).X / SlideShowBg.ActualWidth, Mouse.GetPosition(Window1).Y / SlideShowBg.ActualHeight)
            sheff2.BeginAnimation(ShaderEffectLibrary.RippleEffect.AmplitudeProperty, New DoubleAnimation(0, 0.05, TimeSpan.FromSeconds(1)))

            Dim coord As New Point(Mouse.GetPosition(Window1).X / SlideShowBg.ActualWidth, Mouse.GetPosition(Window1).Y / SlideShowBg.ActualHeight)
            Dim easexy As New CubicEase
            Dim animxy As New PointAnimation(coord, TimeSpan.FromSeconds(1))
            animxy.EasingFunction = easexy
            sheff2.BeginAnimation(ShaderEffectLibrary.RippleEffect.CenterProperty, animxy)

            sheff_active = True
        End If
    End Sub
    Private Sub SlideShowBS_MouseMove(sender As Object, e As MouseEventArgs) Handles SlideShowBg.MouseMove
        If sheff_active Then
            sheff2.BeginAnimation(ShaderEffectLibrary.RippleEffect.CenterProperty, Nothing)
            sheff2.Center = New Point(Mouse.GetPosition(Window1).X / SlideShowBg.ActualWidth, Mouse.GetPosition(Window1).Y / SlideShowBg.ActualHeight)
        End If
    End Sub
    Private Sub SlideShowBS_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles SlideShowBg.MouseUp
        If sheff_active Then
            sheff2.BeginAnimation(ShaderEffectLibrary.RippleEffect.AmplitudeProperty,
                              New DoubleAnimation(0.05, 0, TimeSpan.FromSeconds(1)))
            sheff_active = False
        End If
    End Sub
    Private Sub SlideShowBS_TouchDown(sender As Object, e As Input.TouchEventArgs) Handles SlideShowBg.TouchDown
        SlideShowBS_MouseDown(sender, Nothing)
    End Sub

    Private Sub AltF4Termination() Handles Me.Closing
        'Application.Current.Shutdown()
    End Sub

    'FOR MULTI-LAYER PNG PROCESSING
    Private Sub GridBg_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles GridBg.MouseDown
        Dim pt As Point = e.GetPosition(CType(sender, UIElement))
        hitResultsList.Clear()
        VisualTreeHelper.HitTest(GridBg, Nothing, New HitTestResultCallback(AddressOf ExtraHitTestResult), New PointHitTestParameters(pt))
        If hitResultsList.Count > 0 Then
            Dim str As String = ""
            For i = 0 To hitResultsList.Count - 1
                Dim obj As Object = hitResultsList.Item(i)
                If Not IsNothing(obj) Then
                    'tmp fix to gelix 3d obj
                    Try
                        str += " / " + obj.name.ToString
                    Catch ex As Exception

                    End Try
                End If
            Next
            ' MsgBox(str)
        End If
    End Sub

    Public Function ExtraHitTestResult(ByVal result As HitTestResult) As HitTestResultBehavior
        hitResultsList.Add(result.VisualHit)
        Return HitTestResultBehavior.Continue
    End Function

    Private Sub LbLoadOtherProject_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles LbLoadOtherProject.MouseUp
        Dim filter_str As String = "inForm project (*.inform)|*.inform"
        Dim dlg As New Microsoft.Win32.OpenFileDialog() With {.Multiselect = False, .Filter = filter_str}
        If dlg.ShowDialog() Then
            Dim filepath As String = dlg.FileName
            Dim fileinfo As New FileInfo(filepath)

            Dim objIniFile As New IniFile(app_root + "setup.ini")
            objIniFile.WriteString("setup", "project", fileinfo.Directory.FullName)

            MsgBox("Please start application again...")
            Application.Current.Shutdown()
        End If
    End Sub

    Private Sub LbMinimize_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles LbMinimize.MouseUp
        GridHiddenMenu.Visibility = Windows.Visibility.Collapsed
        Window1.WindowState = WindowState.Minimized
    End Sub

    Private Sub LbExit_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles LbExit.MouseUp
        Application.Current.Shutdown()
    End Sub

    Private Sub LbCloseMenu_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles LbCloseMenu.MouseUp
        GridHiddenMenu.Visibility = Windows.Visibility.Collapsed
    End Sub

    '-------------------------------------- K E Y B O A R D   E V E N TS -------------------------------------- 

    Private Sub Window1_KeyUp(sender As Object, e As KeyEventArgs) Handles Window1.KeyUp
        'NEXT SLIDE
        If e.Key = Key.Right Then
            If BorderSlide.Visibility = Visibility.Visible Then
                NextItem_Action()
            Else
                goto_itemindex = selected_mainmenuitm + 1
                If goto_itemindex > GridMainMenuItems.Children.Count Then goto_itemindex = 1
                ZImageItemHandler(Nothing, Nothing)
            End If
        End If
        'PREV SLIDE
        If e.Key = Key.Left Then
            If BorderSlide.Visibility = Visibility.Visible Then
                PrevItem_Action()
            Else
                goto_itemindex = selected_mainmenuitm - 1
                If goto_itemindex <= 0 Then goto_itemindex = GridMainMenuItems.Children.Count
                ZImageItemHandler(Nothing, Nothing)
            End If
        End If
        'NEXT SUB-SLIDE
        If e.Key = Key.Up Then
            If BorderSlide.Visibility = Visibility.Visible Then
                SelectSubSlide(selected_multisub - 1 - 1)
            End If
        End If
        'PREV SUB-SLIDE
        If e.Key = Key.Down Then
            If BorderSlide.Visibility = Visibility.Visible Then
                SelectSubSlide(selected_multisub - 1 + 1)
            End If
        End If
        'EXIT
        If e.Key = Key.Escape Then
            If BorderSlide.Visibility = Visibility.Visible Then
                ExitSlides_Action()
            Else
                ImgMainMenuLogo_MouseDown(Nothing, Nothing)
                selected_mainmenuitm = 0
            End If
        End If

    End Sub

End Class