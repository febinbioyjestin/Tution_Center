Public Class card
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        ' Get values from the textboxes
        Dim cardNumber As String = Guna2TextBox1.Text.Trim()
        Dim expiry As String = Guna2TextBox2.Text.Trim()
        Dim cvv As String = Guna2TextBox4.Text.Trim()

        ' Validate Card Number (14 digits)
        If cardNumber.Length <> 14 OrElse Not IsNumeric(cardNumber) Then
            MessageBox.Show("Card number must be exactly 14 digits.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Validate Expiry (MM/YY format)
        Dim expParts() As String = expiry.Split("/")
        If expParts.Length <> 2 OrElse expParts(0).Length <> 2 OrElse expParts(1).Length <> 2 _
           OrElse Not IsNumeric(expParts(0)) OrElse Not IsNumeric(expParts(1)) Then
            MessageBox.Show("Expiry must be in MM/YY format.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim month As Integer = Convert.ToInt32(expParts(0))
        If month < 1 OrElse month > 12 Then
            MessageBox.Show("Invalid month in expiry date.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Validate CVV (3 or 4 digits)
        If (cvv.Length <> 3 AndAlso cvv.Length <> 4) OrElse Not IsNumeric(cvv) Then
            MessageBox.Show("CVV must be 3 or 4 digits.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' All inputs are valid
        MessageBox.Show("Card details accepted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Me.Close()
    End Sub
End Class
