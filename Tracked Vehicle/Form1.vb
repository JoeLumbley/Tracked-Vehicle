

Imports System.Math
Imports System.Reflection.Metadata



Public Structure DeltaTimeStructure

    Public CurrentFrame As DateTime
    Public LastFrame As DateTime
    Public ElapsedTime As TimeSpan

    Public Sub New(currentFrame As Date, lastFrame As Date,
                       elapsedTime As TimeSpan)

        Me.CurrentFrame = currentFrame
        Me.LastFrame = lastFrame
        Me.ElapsedTime = elapsedTime
    End Sub

    Public Sub Update()

        ' Set the current frame's time to the current system time.
        CurrentFrame = Now

        ' Calculates the elapsed time ( delta time Δt ) between the
        ' current frame and the last frame.
        ElapsedTime = CurrentFrame - LastFrame

        ' Updates the last frame's time to the current frame's time for
        ' use in the next update.
        LastFrame = CurrentFrame

    End Sub

    Public Sub LastFrameNow()

        ' Set the current frame's time to the current system time.
        LastFrame = Now

    End Sub

End Structure




Public Structure ArrowVector

    Public Pen As Pen

    Public Center As PointF

    Public Velocity As Double

    Public VelocityVector As PointF

    Public MinVelocity As Integer

    Public MaxVelocity As Double

    Public Length As Integer

    Public MinLength As Integer

    Public MaxLength As Integer

    Public Width As Single

    Public MinWidth As Integer

    Public MaxWidth As Integer

    Public AngleInDegrees As Single

    Public AngleInRadians As Single

    Public EndPoint As PointF


    Public Sub New(pen As Pen,
                   center As PointF,
                   angleInDegrees As Single,
                   minLength As Double,
                   maxLength As Double,
                   minWidth As Double,
                   maxWidth As Double,
                   velocity As Double,
                   maxVelocity As Double)

        Me.Pen = pen

        Me.Center = center

        If angleInDegrees >= 0 AndAlso angleInDegrees <= 360 Then
            Me.AngleInDegrees = angleInDegrees
        Else
            Me.AngleInDegrees = 0
        End If

        ' Convert angle from degrees to radians.
        AngleInRadians = Me.AngleInDegrees * (PI / 180)

        Me.MinLength = minLength
        Me.MaxLength = maxLength

        Me.MinWidth = minWidth
        Me.MaxWidth = maxWidth

        Me.Velocity = velocity
        Me.MaxVelocity = maxVelocity

        ' Set velocity based on angle
        VelocityVector.X = Cos(AngleInRadians) * Me.Velocity
        VelocityVector.Y = Sin(AngleInRadians) * Me.Velocity

        Length = GetLength(Me.Velocity, Me.MaxVelocity, Me.MinLength, Me.MaxLength)

        Width = GetWidth(Me.Velocity, Me.MaxVelocity, Me.MinWidth, Me.MaxWidth)

    End Sub

    Public Sub Update(ByVal deltaTime As TimeSpan)

        Length = GetLength(Velocity, MaxVelocity, MinLength, MaxLength)

        Width = GetWidth(Velocity, MaxVelocity, MinWidth, MaxWidth)

        'Pen = New Pen() With {.EndCap = Drawing2D.LineCap.ArrowAnchor, .StartCap = Drawing2D.LineCap.Round}

        Pen.Width = Width

        Pen.StartCap = Drawing2D.LineCap.Round

        Pen.EndCap = Drawing2D.LineCap.ArrowAnchor

        ' Convert angle from degrees to radians.
        AngleInRadians = AngleInDegrees * (PI / 180)

        ' Set velocity based on angle
        VelocityVector.X = Cos(AngleInRadians) * Velocity
        VelocityVector.Y = Sin(AngleInRadians) * Velocity

        ' Calculate the endpoint of the line using trigonometry
        EndPoint = New PointF(Center.X + Length * Cos(AngleInRadians),
                              Center.Y + Length * Sin(AngleInRadians))

        UpdateMovement(deltaTime)

    End Sub

    Public Sub UpdateMovement(ByVal deltaTime As TimeSpan)

        'Move our arrow horizontally.
        Center.X += CSng(VelocityVector.X * deltaTime.TotalSeconds) 'Δs = V * Δt
        'Displacement = Velocity x Delta Time

        'Move our arrow vertically.
        Center.Y += CSng(VelocityVector.Y * deltaTime.TotalSeconds) 'Δs = V * Δt
        'Displacement = Velocity x Delta Time

    End Sub

    Public Sub Draw(g As Graphics)

        DrawLineFromCenterGivenLenghtAndAngle(g)

    End Sub

    Public Sub DrawLineFromCenterGivenLenghtAndAngle(g As Graphics)
        ' Draw a line of given length from the given center point at a given angle.

        ' Draw the line.
        g.DrawLine(Pen, Center, EndPoint)

    End Sub

    Function GetLength(velocity As Double, maxVelocity As Double, minlength As Double, maxlength As Double) As Double

        ' Normalize the velocity
        Dim NormalizedVelocity As Double = velocity / maxVelocity

        ' Interpolate the length
        Dim Length As Double = minlength + NormalizedVelocity * (maxlength - minlength)

        Return Length

    End Function

    Function GetWidth(velocity As Double, maxVelocity As Double, minWidth As Double, maxWidth As Double) As Double

        ' Normalize the velocity
        Dim NormalizedVelocity As Double = velocity / maxVelocity

        ' Interpolate the width
        Dim Width As Double = minWidth + NormalizedVelocity * (maxWidth - minWidth)

        Return Width

    End Function

End Structure


Public Structure Body

    Public Brush As Brush

    Public Center As PointF

    Public AngleInDegrees As Single

    Public AngleInRadians As Single

    Public Width As Integer
    Public Height As Integer
    Dim HalfWidth As Integer
    Dim HalfHeight As Integer

    Dim Points As PointF()

    Dim RotatedPoints

    Public Sub New(brush As Brush,
                   center As PointF,
                   width As Integer,
                   height As Integer,
                   angleInDegrees As Single)

        Me.Center = center

        Me.Brush = brush

        Me.Width = width

        Me.Height = height

        HalfWidth = width / 2

        HalfHeight = height / 2


        Points = {
            New PointF(-HalfWidth, -HalfHeight),
            New PointF(HalfWidth, -HalfHeight),
            New PointF(HalfWidth, HalfHeight),
            New PointF(-HalfWidth, HalfHeight)
        }

        RotatedPoints = New PointF(Points.Length - 1) {}





        If angleInDegrees >= 0 AndAlso angleInDegrees <= 360 Then
            Me.AngleInDegrees = angleInDegrees
        Else
            Me.AngleInDegrees = 0
        End If

        ' Convert angle from degrees to radians.
        AngleInRadians = Me.AngleInDegrees * (PI / 180)



    End Sub

    Private Sub RotatePoints(points As PointF(), center As PointF, angleInRadians As Single)

        For i As Integer = 0 To points.Length - 1

            Dim x As Single = points(i).X * Cos(angleInRadians) - points(i).Y * Sin(angleInRadians)
            Dim y As Single = points(i).X * Sin(angleInRadians) + points(i).Y * Cos(angleInRadians)

            RotatedPoints(i) = New PointF(x + center.X, y + center.Y)

        Next

    End Sub



    Public Sub Update()

        AngleInRadians = DegreesToRadians(AngleInDegrees)

        RotatePoints(Points, Center, AngleInRadians)


    End Sub

    Public Function DegreesToRadians(AngleInDegrees As Single)

        DegreesToRadians = AngleInDegrees * (PI / 180)

    End Function



    Public Sub Draw(g As Graphics)

        '' Convert angle from degrees to radians.
        'AngleInRadians = AngleInDegrees * (PI / 180)


        'Dim width As Integer = 128
        'Dim height As Integer = 64
        'Dim halfWidth As Integer = width / 2
        'Dim halfHeight As Integer = height / 2

        'Dim points As PointF() = {
        '    New PointF(-halfWidth, -halfHeight),
        '    New PointF(halfWidth, -halfHeight),
        '    New PointF(halfWidth, halfHeight),
        '    New PointF(-halfWidth, halfHeight)
        '}

        'Dim transformedPoints As PointF() = New PointF(points.Length - 1) {}

        'For i As Integer = 0 To Points.Length - 1
        '    Dim x As Single = Points(i).X * Cos(AngleInRadians) - Points(i).Y * Sin(AngleInRadians)
        '    Dim y As Single = Points(i).X * Sin(AngleInRadians) + Points(i).Y * Cos(AngleInRadians)
        '    RotatedPoints(i) = New PointF(x + Center.X, y + Center.Y)
        'Next

        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        g.FillPolygon(Brush, RotatedPoints)

    End Sub


End Structure

Public Class Form1

    Private ClientCenter As Point = New Point(ClientSize.Width / 2, ClientSize.Height / 2)

    Dim myArrow As New ArrowVector(New Pen(Color.Black, 10), New PointF(640, 360), 0, 60, 80, 10, 20, 0, 100)

    Private MyBody As New Body(Brushes.Gray, New PointF(0, 0), 128, 64, 0)

    Private DeltaTime As New DeltaTimeStructure(Now, Now, TimeSpan.Zero)

    Private LeftArrowDown As Boolean

    Private RightArrowDown As Boolean

    Private UpArrowDown As Boolean

    Private DownArrowDown As Boolean

    ' Constructor for the form.
    Public Sub New()
        InitializeComponent()

        InitializeForm()

        InitializeTimer()

    End Sub

    Private Sub InitializeForm()

        CenterToScreen()

        SetStyle(ControlStyles.UserPaint, True)

        ' Enable double buffering to reduce flickering
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)

        Text = "Tracked Vehicle - Code with Joe"

        WindowState = FormWindowState.Maximized

    End Sub


    Private Sub InitializeTimer()

        Timer1.Interval = 15

        Timer1.Start()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        DeltaTime.Update()

        HandleKeyPresses()

        myArrow.Update(DeltaTime.ElapsedTime)

        MyBody.Center = myArrow.Center

        MyBody.AngleInDegrees = myArrow.AngleInDegrees

        MyBody.Update()


        Invalidate()

    End Sub

    Private Sub HandleKeyPresses()
        ' Handle key presses to rotate the turret or fire projectiles.

        If LeftArrowDown Then

            If myArrow.AngleInDegrees > 0 Then

                myArrow.AngleInDegrees -= 1 ' Rotate counterclockwise

            Else

                myArrow.AngleInDegrees = 360

            End If

        End If

        If RightArrowDown Then

            If myArrow.AngleInDegrees < 360 Then

                myArrow.AngleInDegrees += 1 ' Rotate clockwise

            Else

                myArrow.AngleInDegrees = 0

            End If

        End If

        If UpArrowDown Then

            If myArrow.Velocity < myArrow.MaxVelocity Then

                myArrow.Velocity += 1

            Else

                myArrow.Velocity = myArrow.MaxVelocity

            End If

        End If

        If DownArrowDown Then

            If myArrow.Velocity > myArrow.MinVelocity Then

                myArrow.Velocity -= 1

            Else

                myArrow.Velocity = myArrow.MinVelocity

            End If

        End If

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.CompositingMode = Drawing2D.CompositingMode.SourceOver

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        MyBody.Draw(e.Graphics)


        myArrow.Draw(e.Graphics)

    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If e.KeyCode = Keys.Right Then

            RightArrowDown = True

        End If

        If e.KeyCode = Keys.Left Then

            LeftArrowDown = True

        End If

        If e.KeyCode = Keys.Up Then

            UpArrowDown = True

        End If

        If e.KeyCode = Keys.Down Then

            DownArrowDown = True

        End If

    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)

        If e.KeyCode = Keys.Right Then

            RightArrowDown = False

        End If

        If e.KeyCode = Keys.Left Then

            LeftArrowDown = False

        End If

        If e.KeyCode = Keys.Up Then

            UpArrowDown = False

        End If

        If e.KeyCode = Keys.Down Then

            DownArrowDown = False

        End If

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        ClientCenter = New Point(ClientSize.Width / 2, ClientSize.Height / 2)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        ClientCenter = New Point(ClientSize.Width / 2, ClientSize.Height / 2)
        'MyBody.Center = ClientCenter

    End Sub

End Class
