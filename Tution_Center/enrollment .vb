Imports MySql.Data.MySqlClient
Public Class Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadStudents()
        LoadCourses()
    End Sub

    Dim studentDict As New Dictionary(Of String, Integer) ' Dictionary to store Student Name & ID

    Private Sub LoadStudents()
        Dim conn As New MySqlConnection("server=localhost;user=root;password=admin;database=coaching")
        Dim query As String = "SELECT stu_id, stu_name FROM Students"
        Dim cmd As New MySqlCommand(query, conn)

        Try
            conn.Open()
            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            Guna2ComboBox1.Items.Clear()
            studentDict.Clear() ' Clear previous data

            While reader.Read()
                Dim stuId As Integer = reader("stu_id")
                Dim stuName As String = reader("stu_name")
                studentDict(stuName) = stuId  ' Store in Dictionary
                Guna2ComboBox1.Items.Add(stuName) ' Add only Name in ComboBox
            End While

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    Dim courseDict As New Dictionary(Of String, Integer) ' Dictionary to store Course Name & ID

    Private Sub LoadCourses()
        Dim conn As New MySqlConnection("server=localhost;user=root;password=admin;database=coaching")
        Dim query As String = "SELECT course_id, course_name FROM Course"
        Dim cmd As New MySqlCommand(query, conn)

        Try
            conn.Open()
            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            Guna2ComboBox2.Items.Clear()
            courseDict.Clear()

            While reader.Read()
                Dim courseId As Integer = reader("course_id")
                Dim courseName As String = reader("course_name")
                courseDict(courseName) = courseId  ' Store in Dictionary
                Guna2ComboBox2.Items.Add(courseName) ' Add only Name in ComboBox
            End While

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub


    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        If Guna2ComboBox1.SelectedItem Is Nothing Or Guna2ComboBox2.SelectedItem Is Nothing Then
            MessageBox.Show("Please select both a Student and a Course!", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim stuName As String = Guna2ComboBox1.SelectedItem.ToString()
        Dim courseName As String = Guna2ComboBox2.SelectedItem.ToString()

        ' Get corresponding IDs
        Dim stuId As Integer = studentDict(stuName)
        Dim courseId As Integer = courseDict(courseName)

        Dim conn As New MySqlConnection("server=localhost;user=root;password=admin;database=coaching")
        Dim query As String = "INSERT INTO Enrollment (stu_id, course_id) VALUES (@stu_id, @course_id)"
        Dim cmd As New MySqlCommand(query, conn)

        cmd.Parameters.AddWithValue("@stu_id", stuId)
        cmd.Parameters.AddWithValue("@course_id", courseId)

        Try
            conn.Open()
            cmd.ExecuteNonQuery()
            MessageBox.Show("Enrollment Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Dim conn As New MySqlConnection("server=localhost;user=root;password=admin;database=coaching")
        Dim query As String = "SELECT e.enroll_id, e.date, s.stu_name, c.course_name 
                           FROM Enrollment e
                           JOIN Students s ON e.stu_id = s.stu_id
                           JOIN Course c ON e.course_id = c.course_id"
        Dim cmd As New MySqlCommand(query, conn)
        Dim enrollmentData As String = "Enrollment Details:" & vbNewLine & "-----------------------------" & vbNewLine

        Try
            conn.Open()
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            While reader.Read()
                enrollmentData &= "ID: " & reader("enroll_id").ToString() & vbNewLine
                enrollmentData &= "Date: " & reader("date").ToString() & vbNewLine
                enrollmentData &= "Student: " & reader("stu_name").ToString() & vbNewLine
                enrollmentData &= "Course: " & reader("course_name").ToString() & vbNewLine
                enrollmentData &= "-----------------------------" & vbNewLine
            End While

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            conn.Close()
        End Try

        ' If no data is found
        If enrollmentData = "Enrollment Details:" & vbNewLine & "-----------------------------" & vbNewLine Then
            MessageBox.Show("No Enrollment Records Found!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Show Data in a Scrollable MessageBox
        Dim enrollmentForm As New Form With {
            .Text = "Enrollment Records",
            .Width = 400,
            .Height = 400
        }

        Dim textBox As New TextBox With {
            .Multiline = True,
            .ReadOnly = True,
            .ScrollBars = ScrollBars.Vertical,
            .Dock = DockStyle.Fill,
            .Text = enrollmentData
        }

        enrollmentForm.Controls.Add(textBox)
        enrollmentForm.ShowDialog()
    End Sub

End Class
