Imports System.IO
Imports System.Math
Imports System.Runtime.InteropServices
Imports System.Text

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

    Public Acceleration As PointF

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
                   maxVelocity As Double, acceleration As Double)

        Me.Acceleration.X = acceleration
        Me.Acceleration.Y = acceleration

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

        If velocity > 0 Then

            ' Normalize the velocity
            Dim NormalizedVelocity As Double = velocity / maxVelocity

            ' Interpolate the width
            Return minWidth + NormalizedVelocity * (maxWidth - minWidth)

        Else

            Return minWidth

        End If

    End Function

End Structure

Public Structure Body
    ' Copy ArrowVector movement


    Public Brush As Brush

    Public Center As PointF

    Public Velocity As Double


    Public VelocityVector As PointF

    Public Acceleration As PointF

    Public MinVelocity As Integer

    Public MaxVelocity As Double


    Public AngleInDegrees As Single

    Public AngleInRadians As Single

    Public Width As Integer
    Public Height As Integer
    Dim HalfWidth As Integer
    Dim HalfHeight As Integer

    Private ReadOnly AlineCenterMiddle As StringFormat

    Dim KeyboardHints As PointF()

    Dim RotatedKeyboardHints As PointF()

    Dim Body As PointF()

    Dim RotatedBody As PointF()

    Public HitBox As Rectangle

    Public Min As PointF ' Minimum corner
    Public Max As PointF ' Maximum corner


    Public KeyboardHintsFont As Font

    Public Sub New(brush As Brush,
                   center As PointF,
                   width As Integer,
                   height As Integer,
                   angleInDegrees As Single, velocity As Double,
                   maxVelocity As Double, acceleration As Double)

        AlineCenterMiddle = New StringFormat With {.Alignment = StringAlignment.Center,
                                                   .LineAlignment = StringAlignment.Center}

        Me.Velocity = velocity

        Me.MaxVelocity = maxVelocity

        Me.Acceleration.X = acceleration
        Me.Acceleration.Y = acceleration

        Me.Center = center

        Me.Brush = brush

        Me.Width = width

        Me.Height = height

        HalfWidth = width / 2

        HalfHeight = height / 2

        Body = {
            New PointF(-HalfWidth, -HalfHeight),
            New PointF(HalfWidth, -HalfHeight),
            New PointF(HalfWidth, HalfHeight),
            New PointF(-HalfWidth, HalfHeight)
        }

        RotatedBody = New PointF(Body.Length - 1) {}

        KeyboardHintsFont = New Font("Segoe UI", 8)

        KeyboardHints = {
            New PointF(HalfWidth - 10, -HalfHeight + 10),
            New PointF(HalfWidth - 10, HalfHeight - 10)
        }

        RotatedKeyboardHints = New PointF(KeyboardHints.Length - 1) {}

        If angleInDegrees >= 0 AndAlso angleInDegrees <= 360 Then
            Me.AngleInDegrees = angleInDegrees
        Else
            Me.AngleInDegrees = 0
        End If

        AngleInRadians = DegreesToRadians(angleInDegrees)

        ' Set velocity based on angle
        VelocityVector.X = Cos(AngleInRadians) * Me.Velocity
        VelocityVector.Y = Sin(AngleInRadians) * Me.Velocity


    End Sub

    Public Sub Update(ByVal deltaTime As TimeSpan)

        AngleInRadians = DegreesToRadians(AngleInDegrees)

        RotatedBody = RotatePoints(Body, Center, AngleInRadians)

        RotatedKeyboardHints = RotatePoints(KeyboardHints, Center, AngleInRadians)

        ' Set velocity based on angle
        VelocityVector.X = Cos(AngleInRadians) * Velocity
        VelocityVector.Y = Sin(AngleInRadians) * Velocity


        UpdateMovement(deltaTime)

    End Sub




    Public Sub Draw(g As Graphics)

        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        g?.FillPolygon(Brush, RotatedBody)

        g?.DrawString("A", KeyboardHintsFont, Brushes.Black, RotatedKeyboardHints(0), AlineCenterMiddle)

        g?.DrawString("D", KeyboardHintsFont, Brushes.Black, RotatedKeyboardHints(1), AlineCenterMiddle)

    End Sub







    Public Sub UpdateMovement(ByVal deltaTime As TimeSpan)

        'Move horizontally.
        Center.X += CSng(VelocityVector.X * deltaTime.TotalSeconds) 'Δs = V * Δt
        'Displacement = Velocity x Delta Time

        'Move verticaly.
        Center.Y += CSng(VelocityVector.Y * deltaTime.TotalSeconds) 'Δs = V * Δt
        'Displacement = Velocity x Delta Time

    End Sub

    Public Sub CheckWallBounce(points As PointF(), clientWidth As Integer, clientHeight As Integer)
        ' Function to check for wall bounce and adjust position and velocity of the body.

        ' Check for collision with the left and right boundaries.
        If Center.X <= 0 Then

            Center.X = 0

            ' Reverse velocity for bounce back.
            Velocity = -Velocity

        ElseIf Center.X >= clientWidth Then

            Center.X = clientWidth

            ' Reverse velocity for bounce back.
            Velocity = -Velocity

        End If







        ' Check for collision with the top and bottom boundaries
        If Center.Y <= 0 Then

            Center.Y = 0

            ' Reverse velocity for bounce back.
            Velocity = -Velocity

        ElseIf Center.Y >= clientHeight Then

            Center.Y = clientHeight

            ' Reverse velocity for bounce back.
            Velocity = -Velocity

        End If




        'For i As Integer = 0 To points.Length - 1

        '    ' Check for collision with the left and right boundaries
        '    If points(i).X <= 0 Then

        '        'points(i).X = 0
        '        Center.X = 0 + 100

        '        VelocityVector.X = -VelocityVector.X ' Reverse the horizontal velocity

        '    ElseIf points(i).X >= clientWidth Then

        '        'points(i).X = clientWidth
        '        Center.X = clientWidth

        '        VelocityVector.X = -VelocityVector.X ' Reverse the horizontal velocity

        '    End If

        '    ' Check for collision with the top and bottom boundaries
        '    If points(i).Y <= 0 Then

        '        'points(i).Y = 0
        '        Center.X = 0

        '        VelocityVector.Y = -VelocityVector.Y ' Reverse the vertical velocity

        '    ElseIf points(i).Y >= clientHeight Then

        '        'points(i).Y = clientHeight
        '        Center.X = clientHeight

        '        VelocityVector.Y = -VelocityVector.Y ' Reverse the vertical velocity

        '    End If

        'Next

    End Sub




    Private Function RotatePoints(points As PointF(), center As PointF, angleInRadians As Single) As PointF()

        Dim RotatedPoints As PointF()

        RotatedPoints = New PointF(points.Length - 1) {}

        For i As Integer = 0 To points.Length - 1

            Dim x As Single = points(i).X * Cos(angleInRadians) - points(i).Y * Sin(angleInRadians)
            Dim y As Single = points(i).X * Sin(angleInRadians) + points(i).Y * Cos(angleInRadians)

            RotatedPoints(i) = New PointF(x + center.X, y + center.Y)

        Next

        Return RotatedPoints

    End Function

    Public Function DegreesToRadians(AngleInDegrees As Single)

        DegreesToRadians = AngleInDegrees * (PI / 180)

    End Function

    ' Method to check if a point is inside the AABB
    Public Function Contains(point As PointF) As Boolean
        Return point.X >= Min.X AndAlso point.X <= Max.X AndAlso
               point.Y >= Min.Y AndAlso point.Y <= Max.Y
    End Function

    Public Function Intersects(rectangle As Rectangle) As Boolean
        ' Function to check if the rectangle intersects with the points

        ' Get the minimum and maximum points of the body
        GetMinMax(Body)

        ' Return if there is an intersection
        Return Min.X <= rectangle.Right AndAlso Max.X >= rectangle.Left AndAlso
           Min.Y <= rectangle.Bottom AndAlso Max.Y >= rectangle.Top

    End Function

    ' Subroutine to get the minimum and maximum points
    Private Sub GetMinMax(points As PointF())

        ' Initialize Min and Max to the first point
        Min = points(0)
        Max = points(0)

        ' Iterate through the points to find the min and max
        For Each point As PointF In points
            If point.X < Min.X Then Min.X = point.X
            If point.Y < Min.Y Then Min.Y = point.Y
            If point.X > Max.X Then Max.X = point.X
            If point.Y > Max.Y Then Max.Y = point.Y
        Next

    End Sub


End Structure

Public Structure AudioPlayer

    <DllImport("winmm.dll", EntryPoint:="mciSendStringW")>
    Private Shared Function mciSendStringW(<MarshalAs(UnmanagedType.LPWStr)> ByVal lpszCommand As String,
                                           <MarshalAs(UnmanagedType.LPWStr)> ByVal lpszReturnString As StringBuilder,
                                           ByVal cchReturn As UInteger, ByVal hwndCallback As IntPtr) As Integer
    End Function

    Private Sounds() As String

    Public Function AddSound(SoundName As String, FilePath As String) As Boolean

        ' Do we have a name and does the file exist?
        If Not String.IsNullOrWhiteSpace(SoundName) AndAlso IO.File.Exists(FilePath) Then
            ' Yes, we have a name and the file exists.

            Dim CommandOpen As String = $"open ""{FilePath}"" alias {SoundName}"

            ' Do we have sounds?
            If Sounds Is Nothing Then
                ' No we do not have sounds.

                ' Did the sound file open?
                If SendMciCommand(CommandOpen, IntPtr.Zero) Then
                    ' Yes, the sound file did open.

                    ' Start the Sounds array with the sound.
                    ReDim Sounds(0)

                    Sounds(0) = SoundName

                    Return True ' The sound was added.

                End If

                ' Is the sound in the array already?
            ElseIf Not Sounds.Contains(SoundName) Then
                ' Yes we have sounds and no the sound is not in the array.

                ' Did the sound file open?
                If SendMciCommand(CommandOpen, IntPtr.Zero) Then
                    ' Yes, the sound file did open.

                    ' Add the sound to the Sounds array.
                    Array.Resize(Sounds, Sounds.Length + 1)

                    Sounds(Sounds.Length - 1) = SoundName

                    Return True ' The sound was added.

                End If

            End If

        End If

        Debug.Print($"{SoundName} not added to sounds.")

        Return False ' The sound was not added.

    End Function

    Public Function SetVolume(SoundName As String, Level As Integer) As Boolean

        ' Do we have sounds and is the sound in the array and is the level in the valid range?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) AndAlso Level >= 0 AndAlso Level <= 1000 Then
            ' We have sounds and the sound is in the array and the level is in range.

            Dim CommandVolume As String = $"setaudio {SoundName} volume to {Level}"

            Return SendMciCommand(CommandVolume, IntPtr.Zero) ' The volume was set.

        End If

        Debug.Print($"{SoundName} volume not set.")

        Return False ' The volume was not set.

    End Function

    Public Function LoopSound(SoundName As String) As Boolean

        ' Do we have sounds and is the sound in the array?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
            ' We have sounds and the sound is in the array.

            Dim CommandSeekToStart As String = $"seek {SoundName} to start"

            Dim CommandPlayRepeat As String = $"play {SoundName} repeat"

            Return SendMciCommand(CommandSeekToStart, IntPtr.Zero) AndAlso
                   SendMciCommand(CommandPlayRepeat, IntPtr.Zero) ' The sound is looping.

        End If

        Debug.Print($"{SoundName} not looping.")

        Return False ' The sound is not looping.

    End Function

    Public Function PlaySound(SoundName As String) As Boolean

        ' Do we have sounds and is the sound in the array?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
            ' We have sounds and the sound is in the array.

            Dim CommandSeekToStart As String = $"seek {SoundName} to start"

            Dim CommandPlay As String = $"play {SoundName} notify"

            Return SendMciCommand(CommandSeekToStart, IntPtr.Zero) AndAlso
                   SendMciCommand(CommandPlay, IntPtr.Zero) ' The sound is playing.

        End If

        Debug.Print($"{SoundName} not playing.")

        Return False ' The sound is not playing.

    End Function

    Public Function PauseSound(SoundName As String) As Boolean

        ' Do we have sounds and is the sound in the array?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
            ' We have sounds and the sound is in the array.

            Dim CommandPause As String = $"pause {SoundName} notify"

            Return SendMciCommand(CommandPause, IntPtr.Zero) ' The sound is paused.

        End If

        Debug.Print($"{SoundName} not paused.")

        Return False ' The sound is not paused.

    End Function

    Public Function IsPlaying(SoundName As String) As Boolean

        Return GetStatus(SoundName, "mode") = "playing"

    End Function

    Public Sub AddOverlapping(SoundName As String, FilePath As String)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"}

            AddSound(SoundName & Suffix, FilePath)

        Next

    End Sub

    Public Sub PlayOverlapping(SoundName As String)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"}

            If Not IsPlaying(SoundName & Suffix) Then

                PlaySound(SoundName & Suffix)

                Exit Sub

            End If

        Next

    End Sub

    Public Sub SetVolumeOverlapping(SoundName As String, Level As Integer)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"}

            SetVolume(SoundName & Suffix, Level)

        Next

    End Sub

    Private Function SendMciCommand(command As String, hwndCallback As IntPtr) As Boolean

        Dim ReturnString As New StringBuilder(128)

        Try

            Return mciSendStringW(command, ReturnString, 0, hwndCallback) = 0

        Catch ex As Exception

            Debug.Print($"Error sending MCI command: {command} | {ex.Message}")

            Return False

        End Try

    End Function

    Private Function GetStatus(SoundName As String, StatusType As String) As String

        Try

            ' Do we have sounds and is the sound in the array?
            If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
                ' We have sounds and the sound is in the array.

                Dim CommandStatus As String = $"status {SoundName} {StatusType}"

                Dim StatusReturn As New StringBuilder(128)

                mciSendStringW(CommandStatus, StatusReturn, 128, IntPtr.Zero)

                Return StatusReturn.ToString.Trim.ToLower

            End If

        Catch ex As Exception

            Debug.Print($"Error getting status: {SoundName} | {ex.Message}")

        End Try

        Return String.Empty

    End Function

    Public Sub CloseSounds()

        If Sounds IsNot Nothing Then

            For Each Sound In Sounds

                Dim CommandClose As String = $"close {Sound}"

                SendMciCommand(CommandClose, IntPtr.Zero)

            Next

            Sounds = Nothing

        End If

    End Sub

End Structure

Public Class Form1

    Private ClientCenter As Point = New Point(ClientSize.Width / 2, ClientSize.Height / 2)

    Dim myArrow As New ArrowVector(New Pen(Color.Black, 10), New PointF(640, 360), 0, 60, 70, 10, 15, 0, 100, 30)

    Private MyBody As New Body(Brushes.Gray, New PointF(0, 0), 128, 64, 0, 0, 400, 30)

    Private DeltaTime As New DeltaTimeStructure(Now, Now, TimeSpan.Zero)

    Private ADown As Boolean

    Private DDown As Boolean

    Private WDown As Boolean

    Private SDown As Boolean

    Private InstructionsFont As New Font("Segoe UI", 12)

    Private InstructionsLocation As New PointF(0, 0)

    Private InstructionsText As New String("Use A or D keys to rotate the vehicle" &
                                           Environment.NewLine &
                                           "W for forward and S for reverse.")
    Private Player As AudioPlayer


    ' Constructor for the form.
    Public Sub New()
        InitializeComponent()

        InitializeForm()

        InitializeTimer()

        CreateSoundFiles()

        Dim FilePath As String = Path.Combine(Application.StartupPath, "tracknoise.mp3")

        Player.AddSound("tracknoise", FilePath)

        Player.SetVolume("tracknoise", 300)

        Player.LoopSound("tracknoise")

        FilePath = Path.Combine(Application.StartupPath, "ambientnoise.mp3")

        Player.AddSound("ambientnoise", FilePath)

        Player.SetVolume("ambientnoise", 10)

        Player.LoopSound("ambientnoise")

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


        'MyBody.Center = myArrow.Center
        myArrow.Center = MyBody.Center

        'MyBody.AngleInDegrees = myArrow.AngleInDegrees
        myArrow.AngleInDegrees = MyBody.AngleInDegrees


        myArrow.Velocity = MyBody.Velocity



        myArrow.Update(DeltaTime.ElapsedTime)

        MyBody.Update(DeltaTime.ElapsedTime)



        'Wall Bounce


        MyBody.CheckWallBounce(MyBody.Body, ClientSize.Width, ClientSize.Height)





        If Not Player.IsPlaying("tracknoise") Then

            Player.LoopSound("tracknoise")

        End If

        Invalidate()

    End Sub















    'Private Sub CheckForWallBounce()

    '    'Did the ball hit the top wall?
    '    If Ball.Rect.Y < ClientRectangle.Top Then
    '        'Yes, the ball hit the top wall.

    '        Dim TempV As Single = Ball.Velocity.Y

    '        Ball.Velocity.Y = 0

    '        'Push the ball to the walls top edge.
    '        Ball.Rect.Y = ClientRectangle.Top + 5

    '        Ball.Position.Y = Ball.Rect.Y

    '        PlayBounceSound()

    '        'Reverse direction on the y-axis
    '        Ball.Velocity.Y = TempV * -1

    '    End If

    '    'Did the ball hit the bottom wall?
    '    If Ball.Rect.Y + Ball.Rect.Height > ClientRectangle.Bottom Then
    '        'Yes, the ball hit the bottom wall.

    '        Dim TempV As Single = Ball.Velocity.Y

    '        Ball.Velocity.Y = 0

    '        'Push the ball to the walls bottom edge.
    '        Ball.Rect.Y = ClientRectangle.Bottom - Ball.Rect.Height - 5

    '        Ball.Position.Y = Ball.Rect.Y

    '        PlayBounceSound()

    '        'Reverse direction on the y-axis
    '        Ball.Velocity.Y = TempV * -1

    '    End If

    'End Sub

    'Private Sub CheckForWallBounceXaxis()

    '    'Did the ball hit the left edge of the wall?
    '    If Ball.Rect.X < ClientRectangle.Left Then
    '        'Yes, the ball hit the left edge of the wall.

    '        Dim TempV As Single = Ball.Velocity.X

    '        Ball.Velocity.X = 0

    '        'Push the ball to the walls left edge.
    '        Ball.Rect.X = ClientRectangle.Left + 5

    '        Ball.Position.X = Ball.Rect.X

    '        PlayBounceSound()

    '        'Reverse direction on the y-axis
    '        Ball.Velocity.X = TempV * -1

    '    End If

    '    'Did the ball hit the bottom wall?
    '    If Ball.Rect.X + Ball.Rect.Width > ClientRectangle.Right Then
    '        'Yes, the ball hit the bottom wall.

    '        Dim TempV As Single = Ball.Velocity.X

    '        Ball.Velocity.X = 0

    '        'Push the ball to the walls right edge.
    '        Ball.Rect.X = ClientRectangle.Right - Ball.Rect.Height - 5

    '        Ball.Position.X = Ball.Rect.X

    '        PlayBounceSound()

    '        'Reverse direction on the y-axis
    '        Ball.Velocity.X = TempV * -1

    '    End If

    'End Sub



















    Private Sub HandleKeyPresses()
        ' Handle key presses to rotate the turret or fire projectiles.

        If ADown Then

            If MyBody.AngleInDegrees > 0 Then

                MyBody.AngleInDegrees -= 1 ' Rotate counterclockwise

            Else

                MyBody.AngleInDegrees = 360

            End If

        End If

        If DDown Then

            If MyBody.AngleInDegrees < 360 Then

                MyBody.AngleInDegrees += 1 ' Rotate clockwise

            Else

                MyBody.AngleInDegrees = 0

            End If

        End If

        If WDown Then

            If MyBody.Velocity < MyBody.MaxVelocity Then

                MyBody.Velocity += 1

            Else

                MyBody.Velocity = MyBody.MaxVelocity

            End If



        ElseIf SDown Then

            If MyBody.Velocity > -MyBody.MaxVelocity Then

                MyBody.Velocity += -1

            Else

                MyBody.Velocity = -MyBody.MaxVelocity

            End If

        Else

            Decelerate()

        End If

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        e.Graphics.CompositingMode = Drawing2D.CompositingMode.SourceOver

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        e.Graphics.DrawString(InstructionsText, InstructionsFont, Brushes.Black, InstructionsLocation)

        MyBody.Draw(e.Graphics)

        myArrow.Draw(e.Graphics)

    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If e.KeyCode = Keys.D Then

            DDown = True

        End If

        If e.KeyCode = Keys.A Then

            ADown = True

        End If

        If e.KeyCode = Keys.W Then

            WDown = True

        End If

        If e.KeyCode = Keys.S Then

            SDown = True

        End If

    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)

        If e.KeyCode = Keys.D Then

            DDown = False

        End If

        If e.KeyCode = Keys.A Then

            ADown = False

        End If

        If e.KeyCode = Keys.W Then

            WDown = False

        End If

        If e.KeyCode = Keys.S Then

            SDown = False

        End If

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        ClientCenter = New Point(ClientSize.Width / 2, ClientSize.Height / 2)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        ClientCenter = New Point(ClientSize.Width / 2, ClientSize.Height / 2)

    End Sub

    Private Sub CreateSoundFiles()

        Dim FilePath As String = Path.Combine(Application.StartupPath, "tracknoise.mp3")

        CreateFileFromResource(FilePath, My.Resources.Resource1.tracknoise6)

        FilePath = Path.Combine(Application.StartupPath, "ambientnoise.mp3")

        CreateFileFromResource(FilePath, My.Resources.Resource1.ambientnoise)

    End Sub

    Private Sub CreateFileFromResource(filepath As String, resource As Byte())

        Try

            If Not IO.File.Exists(filepath) Then

                IO.File.WriteAllBytes(filepath, resource)

            End If

        Catch ex As Exception

            Debug.Print($"Error creating file: {ex.Message}")

        End Try

    End Sub


    Private Sub Decelerate()

        If MyBody.Velocity < 0 Then

            ' Calculate potential new velocity
            Dim newVelocity As Double = MyBody.Velocity + (MyBody.Acceleration.Y * DeltaTime.ElapsedTime.TotalSeconds)

            If newVelocity > 0 Then

                MyBody.Velocity = 0

            Else

                MyBody.Velocity = newVelocity

            End If

        End If

        If MyBody.Velocity > 0 Then

            ' Calculate potential new velocity
            Dim newVelocity As Double = MyBody.Velocity + (-MyBody.Acceleration.Y * DeltaTime.ElapsedTime.TotalSeconds)

            If newVelocity < 0 Then

                MyBody.Velocity = 0

            Else

                MyBody.Velocity = newVelocity

            End If

        End If

    End Sub

End Class
