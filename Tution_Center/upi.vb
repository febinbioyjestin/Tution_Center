Public Class upi
    Private WithEvents closeTimer As New Timer()

    Private Sub upi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the interval to 20 seconds (20,000 milliseconds)
        closeTimer.Interval = 20000
        closeTimer.Start()
    End Sub

    Private Sub closeTimer_Tick(sender As Object, e As EventArgs) Handles closeTimer.Tick
        closeTimer.Stop() ' Stop the timer so it doesn’t tick again
        Me.Close()        ' Close the form
    End Sub
End Class
