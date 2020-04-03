Imports System.Windows.Forms
Imports System.Drawing

Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As System.Windows.StartupEventArgs) Handles Me.Startup
        Dim splash As New System.Windows.SplashScreen("splash.jpg")
        splash.Show(False, True)
        splash.Close(TimeSpan.FromSeconds(3))
    End Sub

    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        Dim screens() As Screen = Screen.AllScreens
        If screens.Count = 2 Then
            'CREATE WIN-2 FOR SHOW SLIDES
            'MyBase.OnStartup(e)
            'Dim wnd2 As New Window2()
            'wnd2.WindowState = WindowState.Normal
            'wnd2.WindowStartupLocation = WindowStartupLocation.Manual
            'Dim r2 As Rectangle = screens(1).WorkingArea
            'wnd2.Top = r2.Top
            'wnd2.Left = r2.Left
            'wnd2.Show()
            'wnd2.WindowState = WindowState.Maximized
        End If
    End Sub
End Class
