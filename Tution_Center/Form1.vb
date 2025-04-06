Imports MySql.Data.MySqlClient

Public Class Form1
    Dim connection As New MySqlConnection("server=localhost;user id=root;password=admin;database=coaching")

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Dim username As String = Guna2TextBox1.Text
        Dim password As String = Guna2TextBox2.Text

        Try
            connection.Open()
            Dim query As String = "SELECT * FROM Admin WHERE username = @username AND password = @password"
            Dim cmd As New MySqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@username", username)
            cmd.Parameters.AddWithValue("@password", password)

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            If reader.HasRows Then
                MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Dim dashboard As New Admission()
                dashboard.Show()
                Me.Hide()
            Else
                MessageBox.Show("Invalid Username or Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        ' Placeholder if needed later
    End Sub

    Private Sub Guna2PictureBox1_Click(sender As Object, e As EventArgs) Handles Guna2PictureBox1.Click
        If Guna2PictureBox1.Image IsNot Nothing Then
            Dim img As Bitmap = New Bitmap(Guna2PictureBox1.Image)
            Dim g As Graphics = Graphics.FromImage(img)

            Dim myFont As New Font("Arial", 14, FontStyle.Bold)
            Dim myBrush As New SolidBrush(Color.White)

            Dim x As Integer = 10
            Dim y As Integer = 10
            g.DrawString("Hello, World!", myFont, myBrush, x, y)

            Guna2PictureBox1.Image = img
            g.Dispose()
        End If
    End Sub

    Private Sub Guna2HtmlLabel1_Click(sender As Object, e As EventArgs) Handles Guna2HtmlLabel1.Click
        Dim username As String = InputBox("Enter your username:", "Forgot Password")
        If String.IsNullOrWhiteSpace(username) Then Exit Sub

        Dim newPassword As String = InputBox("Enter your new password:", "Reset Password")
        If String.IsNullOrWhiteSpace(newPassword) Then Exit Sub

        Try
            connection.Open()

            ' Check if the username exists
            Dim checkQuery As String = "SELECT COUNT(*) FROM Admin WHERE username = @username"
            Dim checkCmd As New MySqlCommand(checkQuery, connection)
            checkCmd.Parameters.AddWithValue("@username", username)
            Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If exists = 0 Then
                MessageBox.Show("Username not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Update password
            Dim updateQuery As String = "UPDATE Admin SET password = @password WHERE username = @username"
            Dim updateCmd As New MySqlCommand(updateQuery, connection)
            updateCmd.Parameters.AddWithValue("@password", newPassword)
            updateCmd.Parameters.AddWithValue("@username", username)

            updateCmd.ExecuteNonQuery()
            MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Guna2HtmlLabel1.AutoSize = True
        Guna2HtmlLabel1.Text = "Forgot Password?"
        Guna2HtmlLabel1.Refresh()

    End Sub
End Class
