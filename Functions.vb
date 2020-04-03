Module Functions

    Public Sub SetNavigationPosition(ByVal pos_value As String, ByVal nav_obj As Object)
        If pos_value.Contains("-") Then
            Dim vals() As String = pos_value.Split("-")
            Try
                Dim val1 As String = vals(0)
                Dim val2 As String = vals(1)
                If val1 = "left" Then nav_obj.HorizontalAlignment = HorizontalAlignment.Left
                If val1 = "center" Then nav_obj.HorizontalAlignment = HorizontalAlignment.Center
                If val1 = "right" Then nav_obj.HorizontalAlignment = HorizontalAlignment.Right
                If val2 = "top" Then nav_obj.VerticalAlignment = VerticalAlignment.Top
                If val2 = "middle" Then nav_obj.VerticalAlignment = VerticalAlignment.Center
                If val2 = "bottom" Then nav_obj.VerticalAlignment = VerticalAlignment.Bottom
            Catch ex As Exception
            End Try
        Else

        End If
    End Sub

End Module
