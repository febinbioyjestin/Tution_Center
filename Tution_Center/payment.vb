Imports MySql.Data.MySqlClient
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing
Imports System.IO

Public Class payment
    Private stuId As Integer
    Private courseId As Integer

    ' Constructor to receive student & course IDs
    Public Sub New(ByVal studentId As Integer, ByVal courseId As Integer)
        InitializeComponent()
        Me.stuId = studentId
        Me.courseId = courseId
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Fetch student name and course name using the IDs
        Dim conn As New MySqlConnection("server=localhost;user=root;password=admin;database=coaching")
        Try
            conn.Open()

            ' Get student name
            Dim stuCmd As New MySqlCommand("SELECT stu_name FROM Students WHERE stu_id = @id", conn)
            stuCmd.Parameters.AddWithValue("@id", stuId)
            Dim stuName = stuCmd.ExecuteScalar()
            Label2.Text = If(stuName IsNot Nothing, stuName.ToString(), "Unknown Student")

            ' Get course name
            Dim courseCmd As New MySqlCommand("SELECT course_name FROM Course WHERE course_id = @id", conn)
            courseCmd.Parameters.AddWithValue("@id", courseId)
            Dim courseName = courseCmd.ExecuteScalar()
            Label5.Text = If(courseName IsNot Nothing, courseName.ToString(), "Unknown Course")

        Catch ex As Exception
            MessageBox.Show("Error loading names: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        If Guna2ComboBox2.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a mode of payment!", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim modeOfPayment As String = Guna2ComboBox2.SelectedItem.ToString()

        If modeOfPayment = "Card" Then
            Dim cardForm As New card()
            cardForm.ShowDialog()
        ElseIf modeOfPayment = "UPI" Then
            Dim upiForm As New upi()
            upiForm.ShowDialog()
        End If

        ' Save payment data after form closes
        Dim conn As New MySqlConnection("server=localhost;user=root;password=admin;database=coaching")
        Dim query As String = "INSERT INTO Payment (stu_id, course_id, mode_payment) VALUES (@stu_id, @course_id, @mode_payment)"
        Dim cmd As New MySqlCommand(query, conn)

        cmd.Parameters.AddWithValue("@stu_id", stuId)
        cmd.Parameters.AddWithValue("@course_id", courseId)
        cmd.Parameters.AddWithValue("@mode_payment", modeOfPayment)

        Try
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()

            ' Fetch latest payment details for receipt
            Dim paymentQuery As String = "SELECT p.pay_id, p.date, s.stu_name, s.stu_email, s.stu_contact, 
                                                 c.course_name, c.tutor_name, c.course_comp_time, c.course_fee, p.mode_payment 
                                          FROM Payment p
                                          JOIN Students s ON p.stu_id = s.stu_id
                                          JOIN Course c ON p.course_id = c.course_id
                                          WHERE p.stu_id = @stu_id AND p.course_id = @course_id
                                          ORDER BY p.pay_id DESC LIMIT 1"
            Dim paymentCmd As New MySqlCommand(paymentQuery, conn)
            paymentCmd.Parameters.AddWithValue("@stu_id", stuId)
            paymentCmd.Parameters.AddWithValue("@course_id", courseId)

            conn.Open()
            Dim reader As MySqlDataReader = paymentCmd.ExecuteReader()

            If reader.Read() Then
                Dim payId As Integer = reader("pay_id")
                Dim payDate As String = reader("date").ToString()
                Dim studentName As String = reader("stu_name").ToString()
                Dim studentEmail As String = reader("stu_email").ToString()
                Dim studentContact As String = reader("stu_contact").ToString()
                Dim courseName As String = reader("course_name").ToString()
                Dim tutorName As String = reader("tutor_name").ToString()
                Dim courseTime As String = reader("course_comp_time").ToString() & " months"
                Dim courseFee As String = "₹" & reader("course_fee").ToString()
                Dim paymentMode As String = reader("mode_payment").ToString()

                GeneratePDFReceipt(payId, payDate, studentName, studentEmail, studentContact, courseName, tutorName, courseTime, courseFee, paymentMode)

                MessageBox.Show("Payment Successful! Receipt generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub GeneratePDFReceipt(payId As Integer, payDate As String, studentName As String, studentEmail As String, studentContact As String, courseName As String, tutorName As String, courseTime As String, courseFee As String, paymentMode As String)
        Dim doc As New PdfDocument()
        doc.Info.Title = "Payment Receipt"
        Dim page As PdfPage = doc.AddPage()
        Dim gfx As XGraphics = XGraphics.FromPdfPage(page)
        Dim fontTitle As New XFont("Arial", 16)
        Dim fontNormal As New XFont("Arial", 12)

        Dim yPos As Double = 40

        gfx.DrawString("Payment Receipt", fontTitle, XBrushes.Black, New XPoint(200, yPos))
        yPos += 30

        gfx.DrawString("Payment ID: " & payId, fontNormal, XBrushes.Black, New XPoint(50, yPos))
        yPos += 20
        gfx.DrawString("Date: " & payDate, fontNormal, XBrushes.Black, New XPoint(50, yPos))
        yPos += 20
        gfx.DrawString("Payment Mode: " & paymentMode, fontNormal, XBrushes.Black, New XPoint(50, yPos))
        yPos += 30

        gfx.DrawString("Student Name: " & studentName, fontNormal, XBrushes.Black, New XPoint(50, yPos))
        yPos += 20
        gfx.DrawString("Email: " & studentEmail, fontNormal, XBrushes.Black, New XPoint(50, yPos))
        yPos += 20
        gfx.DrawString("Contact: " & studentContact, fontNormal, XBrushes.Black, New XPoint(50, yPos))
        yPos += 30

        gfx.DrawString("Course: " & courseName, fontNormal, XBrushes.Black, New XPoint(50, yPos))
        yPos += 20
        gfx.DrawString("Tutor: " & tutorName, fontNormal, XBrushes.Black, New XPoint(50, yPos))
        yPos += 20
        gfx.DrawString("Duration: " & courseTime, fontNormal, XBrushes.Black, New XPoint(50, yPos))
        yPos += 20
        gfx.DrawString("Course Fee: " & courseFee, fontNormal, XBrushes.Black, New XPoint(50, yPos))
        yPos += 30

        gfx.DrawString("Thank you for your payment!", fontNormal, XBrushes.Black, New XPoint(50, yPos))

        Dim filePath As String = "C:\Users\febin\OneDrive\Documents\Payment_Receipt_" & payId & ".pdf"
        doc.Save(filePath)
        doc.Close()

        MessageBox.Show("Receipt saved at: " & filePath, "Receipt Generated", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class
