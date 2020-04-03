Imports System.IO
Imports System.Xml

Public Class XmlhomeBlocks

    Public IsLoaded As Boolean = False

    Public Type() As String
    Public SheffClass() As String
    Public SheffIndex() As Integer
    Public SheffDelay() As Double
    Public SheffDuration() As Double
    Public Left() As Integer
    Public Top() As Integer
    Public Width() As Integer
    Public Height() As Integer
    Public Source() As String
    Public Link() As String

    Public ItemsCount As Integer = 0
    '<block type="sheff" class="A" effect="19" delay="3" duration="1" left="354" top="0" width="856" height="1080" source="slides_heads" link="1sub"/>
    Public Parameters() As String = {"type", "class", "effect", "delay", "duration", "left", "top", "width", "height", "source", "link"}

    Public set_value() As String

    Public Sub New(ByVal filename As String)
        Me.LoadTemplateData(filename)
    End Sub

    Public Sub LoadTemplateData(ByVal template_file As String)
        'READ XML
        Me.IsLoaded = False
        Dim xmlr As XmlTextReader
        'Try
        If File.Exists(template_file) Then
            xmlr = New XmlTextReader(template_file)
            xmlr.WhitespaceHandling = WhitespaceHandling.None
            While xmlr.Read()
                If xmlr.Name.Equals("blocks") Then
                    ItemsCount = 0
                    While xmlr.Read
                        If xmlr.Name.Equals("block") Then
                            ReDim Preserve Me.Type(ItemsCount)
                            Me.Type(ItemsCount) = xmlr.GetAttribute(Parameters(0))

                            ReDim Preserve Me.SheffClass(ItemsCount)
                            Me.SheffClass(ItemsCount) = xmlr.GetAttribute(Parameters(1))

                            ReDim Preserve Me.SheffIndex(ItemsCount)
                            Me.SheffIndex(ItemsCount) = -1
                            If IsNumeric(xmlr.GetAttribute(Parameters(2))) Then Me.SheffIndex(ItemsCount) = xmlr.GetAttribute(Parameters(2))

                            ReDim Preserve Me.SheffDelay(ItemsCount)
                            Me.SheffDelay(ItemsCount) = -1
                            If IsNumeric(xmlr.GetAttribute(Parameters(3))) Then Me.SheffDelay(ItemsCount) = xmlr.GetAttribute(Parameters(3))

                            ReDim Preserve Me.SheffDuration(ItemsCount)
                            Me.SheffDuration(ItemsCount) = -1
                            If IsNumeric(xmlr.GetAttribute(Parameters(4))) Then Me.SheffDuration(ItemsCount) = xmlr.GetAttribute(Parameters(4))

                            ReDim Preserve Me.Left(ItemsCount)
                            Me.Left(ItemsCount) = -1
                            If IsNumeric(xmlr.GetAttribute(Parameters(5))) Then Me.Left(ItemsCount) = xmlr.GetAttribute(Parameters(5))

                            ReDim Preserve Me.Top(ItemsCount)
                            Me.Top(ItemsCount) = -1
                            If IsNumeric(xmlr.GetAttribute(Parameters(6))) Then Me.Top(ItemsCount) = xmlr.GetAttribute(Parameters(6))

                            ReDim Preserve Me.Width(ItemsCount)
                            Me.Width(ItemsCount) = -1
                            If IsNumeric(xmlr.GetAttribute(Parameters(7))) Then Me.Width(ItemsCount) = xmlr.GetAttribute(Parameters(7))

                            ReDim Preserve Me.Height(ItemsCount)
                            Me.Height(ItemsCount) = -1
                            If IsNumeric(xmlr.GetAttribute(Parameters(8))) Then Me.Height(ItemsCount) = xmlr.GetAttribute(Parameters(8))

                            ReDim Preserve Me.Source(ItemsCount)
                            Me.Source(ItemsCount) = xmlr.GetAttribute(Parameters(9))

                            ReDim Preserve Me.Link(ItemsCount)
                            Me.Link(ItemsCount) = xmlr.GetAttribute(Parameters(10))

                            ItemsCount += 1
                        End If
                    End While
                End If
            End While
            xmlr.Close()
            xmlr = Nothing
            If ItemsCount <> 0 Then Me.IsLoaded = True
        Else
            'AddToLog("ERR: Missing " + template_file)
            Me.IsLoaded = False
        End If
        'Catch ex As Exception
        'AddToLog(template_file + " LOAD ERR: " + ex.ToString)
        'End Try
    End Sub

End Class
