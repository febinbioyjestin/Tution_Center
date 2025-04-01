Imports MySql.Data.MySqlClient

Public Class add_course
    Dim connection As New MySqlConnection("server=localhost;user id=root;password=admin;database=coaching")

    ' SAVE COURSE DATA
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Dim courseName As String = Guna2TextBox1.Text
        Dim tutorName As String = Guna2ComboBox1.Text
        Dim courseDuration As String = Guna2TextBox2.Text
        Dim courseFee As String = Guna2TextBox4.Text

        Try
            connection.Open()
            Dim query As String = "INSERT INTO Course (course_name, tutor_name, course_comp_time, course_fee) VALUES (@courseName, @tutorName, @courseDuration, @courseFee)"
            Dim cmd As New MySqlCommand(query, connection)

            cmd.Parameters.AddWithValue("@courseName", courseName)
            cmd.Parameters.AddWithValue("@tutorName", tutorName)
            cmd.Parameters.AddWithValue("@courseDuration", courseDuration)
            cmd.Parameters.AddWithValue("@courseFee", courseFee)

            cmd.ExecuteNonQuery()
            MessageBox.Show("Course Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
    End Sub

    ' SHOW EXISTING COURSE DATA
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Try
            connection.Open()
            Dim query As String = "SELECT * FROM Course"
            Dim cmd As New MySqlCommand(query, connection)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim courseList As String = "Course List:" & vbCrLf & "-------------------------" & vbCrLf

            While reader.Read()
                courseList &= "ID: " & reader("course_id") & vbCrLf &
                              "Name: " & reader("course_name") & vbCrLf &
                              "Tutor: " & reader("tutor_name") & vbCrLf &
                              "Duration: " & reader("course_comp_time") & " months" & vbCrLf &
                              "Fee: ₹" & reader("course_fee") & vbCrLf &
                              "-------------------------" & vbCrLf
            End While

            If courseList = "Course List:" & vbCrLf & "-------------------------" & vbCrLf Then
                MessageBox.Show("No course records found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show(courseList, "Course Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
    End Sub

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Dim Enroll As New Form2()
        Enroll.Show()
        Me.Hide()
    End Sub

    Private Sub Guna2GradientButton4_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton4.Click
        Dim adm As New Admission()
        adm.Show()
        Me.Hide()
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub Guna2TextBox4_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox4.TextChanged

    End Sub
End Class
