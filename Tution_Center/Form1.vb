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
End Class
