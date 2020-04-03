Imports System.Windows.Media.Animation

Module EventAnimator

    Public Sub EventAnimatorSub(ByRef Obj As Object, ByVal EventAnimatorEffect As String, ByVal ObjState As Boolean, ByVal UseAlpha As Boolean)

        Dim event_duration As Double = 1

        Dim TrTransf = New TranslateTransform
        Obj.RenderTransform = TrTransf
        Dim CuEase As New CubicEase With {.EasingMode = EasingMode.EaseInOut}
        Dim Anim As New DoubleAnimation(0, 0, TimeSpan.FromSeconds(event_duration)) With {.EasingFunction = CuEase}
        If Not ObjState Then
            Anim.BeginTime = TimeSpan.FromSeconds(0.5)
        End If

        If EventAnimatorEffect = "LEFT" Or EventAnimatorEffect = "RIGHT" Then
            If Not ObjState Then
                TrTransf.X = Obj.ActualWidth * 2
                If EventAnimatorEffect = "RIGHT" Then TrTransf.X = -Obj.ActualWidth * 2
                Anim.From = TrTransf.X
            Else
                TrTransf.X = 0
                Anim.To = Obj.ActualWidth * 2
                If EventAnimatorEffect = "RIGHT" Then Anim.To = -Obj.ActualWidth * 2
            End If
            TrTransf.BeginAnimation(TranslateTransform.XProperty, Anim)
        End If
        If EventAnimatorEffect = "BOTTOM" Or EventAnimatorEffect = "TOP" Then
            If Not ObjState Then
                TrTransf.Y = Obj.ActualHeight * 2
                Anim.From = TrTransf.Y
            Else
                TrTransf.Y = 0
                Anim.To = Obj.ActualHeight * 2
            End If
            TrTransf.BeginAnimation(TranslateTransform.YProperty, Anim)
        End If


        If EventAnimatorEffect = "SCALE" Then
            Dim eff_transf = New ScaleTransform With {.CenterX = Obj.ActualWidth / 2, .CenterY = Obj.ActualHeight / 2}
            Obj.RenderTransform = eff_transf
            Dim fromz, toz As Double
            If Not ObjState Then
                fromz = 0.95
                toz = 1
            Else
                fromz = 1
                toz = 0.95
            End If
            Dim eff_anim As New DoubleAnimation(fromz, toz, TimeSpan.FromSeconds(event_duration)) With _
                {.EasingFunction = New CubicEase With {.EasingMode = EasingMode.EaseOut}}

            eff_transf.BeginAnimation(ScaleTransform.ScaleXProperty, eff_anim)
            eff_transf.BeginAnimation(ScaleTransform.ScaleYProperty, eff_anim)
        End If

        If UseAlpha Then
            If Not ObjState Then
                Obj.BeginAnimation(Grid.OpacityProperty, New DoubleAnimation(0, 1, TimeSpan.FromSeconds(event_duration / 2)) With {.BeginTime = TimeSpan.FromSeconds(0.5)})
            Else
                Obj.BeginAnimation(Grid.OpacityProperty, New DoubleAnimation(1, 0, TimeSpan.FromSeconds(event_duration / 2)))
            End If
        End If

    End Sub

End Module
