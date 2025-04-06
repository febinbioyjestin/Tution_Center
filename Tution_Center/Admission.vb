Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient

Public Class Admission
    Dim connection As New MySqlConnection("server=localhost;user id=root;password=admin;database=coaching")

    ' SAVE STUDENT DATA


    ' SAVE STUDENT DATA
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Dim name As String = Guna2TextBox1.Text.Trim()
        Dim age As String = Guna2TextBox2.Text.Trim()
        Dim email As String = Guna2TextBox3.Text.Trim()
        Dim contact As String = Guna2TextBox4.Text.Trim()

        ' Clear previous validation messages
        Guna2HtmlLabel1.Text = ""
        Guna2HtmlLabel2.Text = ""
        Guna2HtmlLabel3.Text = ""

        Dim hasError As Boolean = False

        ' Age validation
        If Not Integer.TryParse(age, Nothing) Then
            Guna2HtmlLabel1.Text = "Age must be a number"
            hasError = True
        End If

        ' Email validation - must end with .com
        Dim emailPattern As String = "^[\w\.-]+@[\w\.-]+\.(com)$"
        If Not System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern) Then
            Guna2HtmlLabel2.Text = "Email must be valid and end with .com"
            hasError = True
        End If

        ' Contact validation - exactly 10 digits
        If Not System.Text.RegularExpressions.Regex.IsMatch(contact, "^\d{10}$") Then
            Guna2HtmlLabel3.Text = "Contact must be exactly 10 digits"
            hasError = True
        End If

        If hasError Then
            MessageBox.Show("Please correct the highlighted errors before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Save to database
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

            ' Clear fields
            Guna2TextBox1.Clear()
            Guna2TextBox2.Clear()
            Guna2TextBox3.Clear()
            Guna2TextBox4.Clear()
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

    ' NAVIGATE TO ADD COURSE
    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Dim addcor As New add_course()
        addcor.Show()
        Me.Hide()
    End Sub

    ' NAVIGATE TO ENROLL
    Private Sub Guna2GradientButton4_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton4.Click
        Dim Enroll As New Form2()
        Enroll.Show()
        Me.Hide()
    End Sub

    ' SHOW PANEL
    Private Sub Guna2GradientButton5_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton5.Click
        Guna2Panel1.Visible = True
    End Sub

    ' ON FORM LOAD
    Private Sub Admission_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Guna2Panel1.Visible = False
        Guna2HtmlLabel1.Text = ""
        Guna2HtmlLabel2.Text = ""
        Guna2HtmlLabel3.Text = ""
    End Sub

    ' LOGOUT
    Private Sub Guna2GradientButton6_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton6.Click
        Dim log As New Form1()
        log.Show()
        Me.Close()
    End Sub

End Class
