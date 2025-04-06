Imports MySql.Data.MySqlClient

Public Class Form2
    Dim studentDict As New Dictionary(Of String, Integer)
    Dim courseDict As New Dictionary(Of String, Integer)

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadStudents()
        LoadCourses()
        Guna2DataGridView1.Visible = False
    End Sub

    Private Sub LoadStudents()
        Dim conn As New MySqlConnection("server=localhost;user=root;password=admin;database=coaching")
        Dim query As String = "SELECT stu_id, stu_name FROM Students"
        Dim cmd As New MySqlCommand(query, conn)

        Try
            conn.Open()
            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            Guna2ComboBox1.Items.Clear()
            studentDict.Clear()

            While reader.Read()
                Dim stuId As Integer = reader("stu_id")
                Dim stuName As String = reader("stu_name")
                studentDict(stuName) = stuId
                Guna2ComboBox1.Items.Add(stuName)
            End While

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

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
                courseDict(courseName) = courseId
                Guna2ComboBox2.Items.Add(courseName)
            End While

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub Guna2ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox2.SelectedIndexChanged
        If Guna2ComboBox2.SelectedItem Is Nothing Then Return

        Dim selectedCourse As String = Guna2ComboBox2.SelectedItem.ToString()
        Dim courseId As Integer = courseDict(selectedCourse)

        Dim conn As New MySqlConnection("server=localhost;user=root;password=admin;database=coaching")
        Dim query As String = "SELECT course_id, course_name, tutor_name, course_comp_time, course_fee FROM Course WHERE course_id = @id"
        Dim cmd As New MySqlCommand(query, conn)
        cmd.Parameters.AddWithValue("@id", courseId)

        Try
            conn.Open()
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)

            ' Bind and format DataGridView
            Guna2DataGridView1.DataSource = dt
            Guna2DataGridView1.Visible = True

            ' Hide course_id column
            If Guna2DataGridView1.Columns.Contains("course_id") Then
                Guna2DataGridView1.Columns("course_id").Visible = False
            End If

            ' Rename headers
            Guna2DataGridView1.Columns("course_name").HeaderText = "Course Name"
            Guna2DataGridView1.Columns("tutor_name").HeaderText = "Tutor"
            Guna2DataGridView1.Columns("course_comp_time").HeaderText = "Completion Time (Days)"
            Guna2DataGridView1.Columns("course_fee").HeaderText = "Course Fee"

            ' Auto-size columns
            Guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Guna2DataGridView1.AutoResizeColumns()

        Catch ex As Exception
            MessageBox.Show("Error loading course details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub


    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        If Guna2ComboBox1.SelectedItem Is Nothing Or Guna2ComboBox2.SelectedItem Is Nothing Then
            MessageBox.Show("Please select both a Student and a Course!", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim stuId As Integer = studentDict(Guna2ComboBox1.SelectedItem.ToString())
        Dim courseId As Integer = courseDict(Guna2ComboBox2.SelectedItem.ToString())

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

        Dim paymentForm As New payment(stuId, courseId)
        paymentForm.Show()
        Me.Hide()
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

        If enrollmentData = "Enrollment Details:" & vbNewLine & "-----------------------------" & vbNewLine Then
            MessageBox.Show("No Enrollment Records Found!", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

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

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Dim adm As New Admission()
        adm.Show()
        Me.Hide()
    End Sub
End Class
