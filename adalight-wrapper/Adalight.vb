Imports System.IO.Ports

Public Class Adalight : Implements IDisposable
    Public LEDMatrix As ArrayList

    Private COMConn As SerialPort

    Private LEDS As Integer?
    Private serialData As Byte()
    Private Const magicWord As String = "Ada"

    Private _COMPort As String
    Public Property COMPort As String
        Set(value As String)
            _COMPort = value
        End Set
        Get
            Return _COMPort
        End Get
    End Property

    ''' <summary>
    ''' Initialze Instance
    ''' </summary>
    ''' <param name="Port">COM Port as String, e.g. "COM3"</param>
    ''' <param name="LEDCount">Total Count of LEDs on your Stripe</param>
    Public Sub New(ByVal Port As String, ByVal LEDCount As Integer)
        'Set Properties
        LEDS = LEDCount
        _COMPort = Port

        'Create connection object
        COMConn = New SerialPort With {
            .PortName = COMPort,
            .BaudRate = 115200,
            .Parity = Parity.None,
            .DataBits = 8,
            .StopBits = StopBits.One
            }

        'Create Matrix Array
        LEDMatrix = New ArrayList
        For i = 0 To LEDCount - 1
            LEDMatrix.Add({0, 0, 0})
        Next

        'Redefine ByteArray length on runtime of current LED count
        ReDim serialData(6 + (LEDS * 3))
    End Sub

    ''' <summary>
    ''' Tries to open connection
    ''' </summary>
    ''' <returns>Boolean value</returns>
    Public Function OpenConn() As String
        Try
            COMConn.Open()
            Return "Connection established!"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    ''' <summary>
    ''' Tries to close connection
    ''' </summary>
    ''' <returns>Boolean value</returns>
    Public Function CloseConn() As Boolean
        Try
            COMConn.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#Region "Helper routines"
    Private Sub WriteHeader()
        serialData(0) = Convert.ToByte(magicWord(0)) 'MagicWord
        serialData(1) = Convert.ToByte(magicWord(1))
        serialData(2) = Convert.ToByte(magicWord(2))
        serialData(3) = CByte((LEDS - 1) >> 8) 'LED count high byte
        serialData(4) = CByte(((LEDS - 1) And &HFF)) 'LED count low byte
        serialData(5) = CByte(serialData(3) Xor serialData(4) Xor &H55) 'Checksum
    End Sub

    Private Sub WriteMatrixToSerialData()
        Dim serialOffset = 6
        For i = 0 To LEDMatrix.Count - 1
            serialData(serialOffset) = LEDMatrix(i)(0) 'red
            serialOffset += 1
            serialData(serialOffset) = LEDMatrix(i)(1) 'blue
            serialOffset += 1
            serialData(serialOffset) = LEDMatrix(i)(2) 'green
            serialOffset += 1
        Next
    End Sub

    ''' <summary>
    ''' Helper Function
    ''' Returns a list of devices responded with the correct Adalight magicword
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetAdalightDevices() As List(Of String)
        Dim output As List(Of String) = New List(Of String)

        For Each dev In SerialPort.GetPortNames()
            Try
                Dim i As SerialPort = New SerialPort With {
                    .PortName = dev,
                    .BaudRate = 115200,
                    .Parity = Parity.None,
                    .DataBits = 8,
                    .StopBits = StopBits.One,
                    .ReadTimeout = 1500
                }
                i.Open()
                If i.ReadLine() = "Ada" Then
                    output.Add(dev)
                End If
                i.Close()
            Catch ex As Exception
            End Try
        Next
        Return output
    End Function
#End Region

    ''' <summary>
    ''' Tries to send data to Adalight device
    ''' </summary>
    ''' <returns>Boolean value</returns>
    Public Function Send() As Boolean
        Try
            WriteHeader()
            WriteMatrixToSerialData()
            COMConn.Write(serialData, 0, serialData.Length)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Try
                    COMConn.Close()
                Catch ex As Exception
                End Try
                COMConn.Dispose()
                LEDMatrix.Clear()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
