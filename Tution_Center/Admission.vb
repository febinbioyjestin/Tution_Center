Imports MySql.Data.MySqlClient

Public Class Admission
    Dim connection As New MySqlConnection("server=localhost;user id=root;password=admin;database=coaching")

    ' SAVE STUDENT DATA
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Dim name As String = Guna2TextBox1.Text
        Dim age As String = Guna2TextBox2.Text
        Dim email As String = Guna2TextBox3.Text
        Dim contact As String = Guna2TextBox4.Text

        Try
            connection.Open()
            Dim query As String = "INSERT INTO Students (stu_name, stu_age, stu_email, stu_contact) VALUES (@name, @age, @email, @contact)"
            Dim cmd As New MySqlCommand(query, connection)

            cmd.Parameters.AddWithValue("@name", name)
            cmd.Parameters.AddWithValue("@age", age)
            cmd.Parameters.AddWithValue("@email", email)
            cmd.Parameters.AddWithValue("@contact", contact)

            cmd.ExecuteNonQuery()
            MessageBox.Show("Student Record Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
    End Sub

    ' SHOW EXISTING STUDENT DATA
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Try
            connection.Open()
            Dim query As String = "SELECT * FROM Students"
            Dim cmd As New MySqlCommand(query, connection)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim studentList As String = "Student Records:" & vbCrLf & "-------------------------" & vbCrLf

            While reader.Read()
                studentList &= "ID: " & reader("stu_id") & vbCrLf &
                               "Name: " & reader("stu_name") & vbCrLf &
                               "Age: " & reader("stu_age") & vbCrLf &
                               "Email: " & reader("stu_email") & vbCrLf &
                               "Contact: " & reader("stu_contact") & vbCrLf &
                               "-------------------------" & vbCrLf
            End While

            If studentList = "Student Records:" & vbCrLf & "-------------------------" & vbCrLf Then
                MessageBox.Show("No student records found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show(studentList, "Student Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
    End Sub

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Dim addcor As New add_course()
        addcor.Show()
        Me.Hide()
    End Sub

    Private Sub Guna2GradientButton4_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton4.Click
        Dim Enroll As New Form2()
        Enroll.Show()
        Me.Hide()
    End Sub
End Class
