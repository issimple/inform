Imports System.IO
Imports System.Net.Mail
Imports System.Media
Imports System.Windows.Media.Animation
Imports System.ComponentModel

Public Class EmailForm : Inherits Grid

    Public ContentPath As String
    Dim deny_snd As SoundPlayer

    Dim gr_form As New Grid With {.HorizontalAlignment = HorizontalAlignment.Center, .VerticalAlignment = VerticalAlignment.Center,
                              .Background = Brushes.White, .Margin = New Thickness(5)}
    Dim lb_header As New Label With {.Content = "Send Email to", .FontSize = 24, .Margin = New Thickness(4)}
    Dim gr_addr As New Grid
    Dim lb_addr As New Label With {.Content = "", .FontSize = 32, .Margin = New Thickness(2), .HorizontalAlignment = HorizontalAlignment.Left}
    Dim lb_backsp As New Label With {.Content = "<- Backspace", .FontSize = 18, .HorizontalAlignment = HorizontalAlignment.Right,
                                     .VerticalContentAlignment = VerticalAlignment.Center, .Background = Brushes.LightGray,
                                     .Margin = New Thickness(2)}
    Dim lb_cancel As New Label With {.Content = "< Don't send", .FontSize = 18, .Width = 200, .VerticalContentAlignment = VerticalAlignment.Center,
                                     .Foreground = Brushes.OrangeRed, .HorizontalAlignment = HorizontalAlignment.Left, .Height = 60}
    Dim lb_ok As New Label With {.Content = "OK, Send >", .FontSize = 18, .Width = 300, .HorizontalContentAlignment = HorizontalAlignment.Right,
                                 .Background = Brushes.GreenYellow, .HorizontalAlignment = HorizontalAlignment.Right,
                                 .VerticalContentAlignment = VerticalAlignment.Center, .Height = 60}
    Dim gr_keyb As New Grid With {.Margin = New Thickness(0, 16, 0, 16)}
    Dim keys_wrap As New WrapPanel With {.Orientation = Orientation.Horizontal, .Width = 674}
    Dim gr_buttons As New Grid With {.Margin = New Thickness(2)}
    Dim v_stack As New StackPanel With {.Orientation = Orientation.Vertical}
    Dim lb_sending As New Label With {.VerticalAlignment = VerticalAlignment.Center, .HorizontalAlignment = HorizontalAlignment.Center,
                                      .Content = "Sending...", .FontSize = 32, .Visibility = Visibility.Hidden, .Foreground = Brushes.White}


    Dim keys_val() As String = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "",
                            "", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
                            "-", "A", "S", "D", "F", "G", "H", "J", "K", "L",
                            "@", "Z", "X", "C", "V", "B", "N", "M", ".", "_"}

    Dim bgworker As New BackgroundWorker

    Public Sub New(ByVal content_path As String)

        Me.ContentPath = content_path

        v_stack.Children.Add(lb_header)
        gr_addr.Children.Add(lb_addr)
        gr_addr.Children.Add(lb_backsp)
        v_stack.Children.Add(gr_addr)
        v_stack.Children.Add(gr_keyb)
        gr_buttons.Children.Add(lb_cancel)
        gr_buttons.Children.Add(lb_ok)
        v_stack.Children.Add(gr_buttons)

        For i = 0 To keys_val.Length - 1
            Dim lb_key As New Label With {.Height = 60, .Width = 60, .Content = keys_val(i), .FontSize = 24,
                                          .Margin = New Thickness(2), .Background = Brushes.Black, .Foreground = Brushes.White}
            If i = 10 Or i = 11 Then
                lb_key.Background = Brushes.White
                lb_key.Width = 30
            End If
            If i = 22 Then lb_key.Width = 60
            If i = 32 Then lb_key.Width = 94
            keys_wrap.Children.Add(lb_key)
        Next
        gr_keyb.Children.Add(keys_wrap)

        gr_form.Children.Add(v_stack)
        Me.Children.Add(gr_form)
        Me.Children.Add(lb_sending)

        Me.VerticalAlignment = VerticalAlignment.Stretch
        Me.HorizontalAlignment = HorizontalAlignment.Stretch
        Me.Background = New SolidColorBrush(ColorConverter.ConvertFromString("#ee000000"))

        AddHandler lb_cancel.MouseUp, AddressOf lb_cancel_MouseUp
        AddHandler lb_ok.MouseUp, AddressOf lb_ok_MouseUp
        AddHandler lb_ok.MouseDown, AddressOf lb_ok_MouseDown
        AddHandler keys_wrap.MouseUp, AddressOf keys_wrap_MouseUp
        AddHandler lb_backsp.MouseUp, AddressOf lb_backsp_MouseUp
        AddHandler lb_addr.LayoutUpdated, AddressOf lb_addr_LayoutUpdated

        bgworker.WorkerReportsProgress = True
        bgworker.WorkerSupportsCancellation = True
        AddHandler bgworker.DoWork, AddressOf bgworker_DoWork
        AddHandler bgworker.ProgressChanged, AddressOf bgworker_ProgressChanged
        AddHandler bgworker.RunWorkerCompleted, AddressOf bgworker_RunWorkerCompleted

        AddHandler lb_sending.MouseUp, AddressOf lb_sending_MouseUp
    End Sub

    Private Sub keys_wrap_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Dim lb As Label = TryCast(e.Source, Label)
        If Not IsNothing(lb) Then
            lb_addr.Content += lb.Content
            lb.BeginAnimation(Label.OpacityProperty, New DoubleAnimation(0.5, 1, TimeSpan.FromSeconds(0.25)))
        End If
    End Sub

    Private Sub lb_backsp_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Dim str As String = lb_addr.Content.ToString
        If str <> "" And str.Length <> 0 Then
            lb_addr.Content = str.Substring(0, str.Length - 1)
        End If
        lb_backsp.BeginAnimation(Label.OpacityProperty, New DoubleAnimation(0.5, 1, TimeSpan.FromSeconds(0.25)))
    End Sub

    Private Sub lb_cancel_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        lb_addr.Content = ""
        Me.Visibility = Visibility.Collapsed
    End Sub

    Private Sub lb_addr_LayoutUpdated(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim str As String = lb_addr.Content.ToString
        If str <> "" Then
            If str.Contains("@") And str.Contains(".") And str.Length > 5 And _
            str.IndexOf(".") > str.IndexOf("@") And _
            str.Chars(str.Length - 1) <> "@" And str.Chars(str.Length - 1) <> "." Then
                lb_ok.IsEnabled = True
                lb_ok.Opacity = 1
            Else
                lb_ok.IsEnabled = False
                lb_ok.Opacity = 0.5
            End If
        Else
            lb_ok.IsEnabled = False
            lb_ok.Opacity = 0.5
        End If
    End Sub

    Private Sub lb_ok_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        email_inp = lb_addr.Content
        work_done = False
        gr_form.Opacity = 0
        lb_sending.Visibility = Visibility.Visible
    End Sub

    Private Sub lb_ok_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        'If Not bgworker.IsBusy Then bgworker.RunWorkerAsync()
        SendEmail()
    End Sub

    Private Sub bgworker_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
        If worker.CancellationPending Then
            e.Cancel = True
        Else
            'SendEmail()

            If File.Exists(System.AppDomain.CurrentDomain.BaseDirectory() + "email.ini") Then
                Dim objIniFile As New IniFile(System.AppDomain.CurrentDomain.BaseDirectory() + "email.ini")
                Dim host As String = objIniFile.GetString("email", "host", "mail.issimple.co")
                Dim port As Integer = objIniFile.GetString("email", "port", 2626)
                Dim from_email As String = objIniFile.GetString("email", "from_email", "data@issimple.co")
                Dim from_pass As String = objIniFile.GetString("email", "from_pass", "issimple1data")
                Dim from_name As String = objIniFile.GetString("email", "from_name", "iSSimple iNFO")
                Dim subj As String = objIniFile.GetString("email", "subj", "Presentation Shared Content")
                Dim body As String = objIniFile.GetString("email", "body", "Hello, here is slide")
                If email_inp <> "" Then
                    'EMAIL INIT
                    Dim msg As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage
                    msg.To.Add(email_inp)
                    msg.From = New MailAddress(from_email, from_name, System.Text.Encoding.UTF8)
                    msg.Subject = subj
                    msg.Body = body
                    msg.SubjectEncoding = System.Text.Encoding.UTF8
                    msg.BodyEncoding = System.Text.Encoding.UTF8
                    msg.IsBodyHtml = False
                    msg.Priority = MailPriority.Normal
                    Dim client As SmtpClient = New SmtpClient
                    client.Credentials = New System.Net.NetworkCredential(from_email, from_pass)
                    client.Port = port
                    client.Host = host
                    client.EnableSsl = False
                    Dim data As Attachment = New Attachment(ContentPath)
                    msg.Attachments.Add(data)
                    Try
                        client.Send(msg)
                        'client.SendAsync(msg, compl_obj)
                    Catch ex As Exception
                    End Try
                End If
            End If

        End If
    End Sub
    Private Sub bgworker_ProgressChanged(ByVal sender As System.Object, ByVal e As ProgressChangedEventArgs)
        lb_sending.Content = (e.ProgressPercentage.ToString() + "%")
    End Sub
    Private Sub bgworker_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As RunWorkerCompletedEventArgs)
        If e.Cancelled = True Then
            lb_sending.Content = "Canceled!"
        ElseIf e.Error IsNot Nothing Then
            lb_sending.Content = "Error: " & e.Error.Message
        Else
            lb_sending.Content = "Done!"
        End If
        work_done = True
    End Sub


    Dim work_done As Boolean = False
    Private Sub lb_sending_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If work_done Then
            gr_form.Opacity = 1
            lb_sending.Visibility = Visibility.Hidden
            Me.Visibility = Visibility.Collapsed
            lb_sending.Background = Nothing
        End If
    End Sub


    Dim email_inp As String = ""
    Private Sub SendEmail()
        If File.Exists(System.AppDomain.CurrentDomain.BaseDirectory() + "email.ini") Then

            Dim objIniFile As New IniFile(System.AppDomain.CurrentDomain.BaseDirectory() + "email.ini")
            Dim host As String = objIniFile.GetString("email", "host", "mail.issimple.co")
            Dim port As Integer = objIniFile.GetString("email", "port", 2626)
            Dim from_email As String = objIniFile.GetString("email", "from_email", "data@issimple.co")
            Dim from_pass As String = objIniFile.GetString("email", "from_pass", "issimple1data")
            Dim from_name As String = objIniFile.GetString("email", "from_name", "iSSimple iNFO")
            Dim subj As String = objIniFile.GetString("email", "subj", "Presentation Shared Content")
            Dim body As String = objIniFile.GetString("email", "body", "Hello, here is slide")

            'TOUCH KEYB
            'Dim prc As Process
            'Try
            '    prc = Process.Start("C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe")
            'Catch ex As Exception
            'End Try

            'INPUT FIELD
            'email_inp = InputBox("Please enter receiver email address:")

            If email_inp <> "" Then

                'bgworker.RunWorkerAsync()

                'EMAIL INIT
                Dim msg As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage
                msg.To.Add(email_inp)
                msg.From = New MailAddress(from_email, from_name, System.Text.Encoding.UTF8)
                msg.Subject = subj
                msg.Body = body
                msg.SubjectEncoding = System.Text.Encoding.UTF8
                msg.BodyEncoding = System.Text.Encoding.UTF8
                msg.IsBodyHtml = False
                msg.Priority = MailPriority.Normal
                Dim client As SmtpClient = New SmtpClient
                AddHandler client.SendCompleted, AddressOf SendCompletedCallback
                client.Credentials = New System.Net.NetworkCredential(from_email, from_pass)
                client.Port = port
                client.Host = host
                client.EnableSsl = False
                Dim data As Attachment = New Attachment(ContentPath)
                msg.Attachments.Add(data)
                Try
                    'client.Send(msg)
                    'lb_sending.Content = "Done!"
                    'work_done = True
                    client.SendAsync(msg, Nothing)

                    'CLOSE KEYB
                    'If Not IsNothing(prc) Then
                    '    Try
                    '        prc.CloseMainWindow()
                    '        prc.Close()
                    '    Catch ex As Exception
                    '    End Try
                    'End If

                    'MsgBox("OK")
                    'gr_form.Opacity = 1
                    'lb_sending.Visibility = Visibility.Hidden
                    'Me.Visibility = Visibility.Collapsed
                Catch ex As Exception
                    lb_sending.Content = "Sorry, it's some error here..."
                    work_done = True
                    'gr_form.Opacity = 1
                    'lb_sending.Visibility = Visibility.Hidden
                    'Me.Visibility = Visibility.Collapsed
                End Try
            Else
                work_done = True
                lb_sending.Content = "ADDR ERR"
            End If
        Else
            work_done = True
            lb_sending.Content = "INI ERR"
        End If
    End Sub

    Private Sub SendCompletedCallback()
        work_done = True
        lb_sending.Content = "Done!"
        Dim brush As New SolidColorBrush
        brush.BeginAnimation(SolidColorBrush.ColorProperty, New ColorAnimation(Colors.Transparent, Colors.GreenYellow, TimeSpan.FromSeconds(0.75)))
        lb_sending.Background = brush
    End Sub


End Class
